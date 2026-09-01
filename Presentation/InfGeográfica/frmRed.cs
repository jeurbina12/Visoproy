using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Presentation;
using Domain;
using Presentation.Util;
using Presentation.InfGeográfica;

namespace Presentation.InfGeográfica
{
#pragma warning disable CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
    public partial class frmRed : Form
#pragma warning restore CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
    {
        RedModel redModel = new RedModel();

#pragma warning disable CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
        public frmRed()
#pragma warning restore CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
        {
            InitializeComponent();
            conexión_Eje();
        }

        private void frmRed_Load(object sender, EventArgs e)
        {
           
            
        }
        private void conexión_Eje()
        {
            string sql = "SELECT id, RTRIM(nombre) AS \"Eje Eléctrico\",root,f_act AS \"Actualización\",RTRIM(usuario) AS  \"Usuario\" FROM esq_red.lta_sistema ORDER BY nombre ASC;";
           
            this.bindingSource1.DataSource = redModel.dt_lista(sql);
            this.bindingNavigator1.BindingSource = this.bindingSource1;
            this.dgv1.DataSource = this.bindingSource1; 
        }

        private void dgvEjes_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            tabEditorArbol.SelectedTab = tabPgSE;
            //int fila = int.Parse(BindingNavigatorPositionItem.Text) - 1;
            conexión_SE(int.Parse(txtIdEje.Text));
        }
        private void conexión_SE(int nPadre)
        {
            string sql;
            // Making connection with Npgsql provider
            //NpgsqlConnection conn = new NpgsqlConnection(conn_string);
            //sql = "SELECT * FROM esq_red.tbl_ctos";
            sql = "SELECT id, RTRIM(nombre) AS \"Subestación\",root,f_act AS \"Actualización\",RTRIM(usuario) AS  \"Usuario\" FROM esq_red.tbl_sub ";
            if (nPadre >= 0)
            {
                sql += "WHERE root='" + nPadre + "' ";
            }

            sql += "ORDER BY nombre ASC;";

            // Assign the DataSet as the DataSource for the BindingSource.           
            this.bindingSource2.DataSource = redModel.dt_lista(sql);
            this.bindingNavigator2.BindingSource = this.bindingSource2;
            this.dgv2.DataSource = this.bindingSource2;
        }

        private void conexión_Barra(int nPadre)
        {
            string sql;
            // Making connection with Npgsql provider
            //NpgsqlConnection conn = new NpgsqlConnection(conn_string);
            //sql = "SELECT * FROM esq_red.tbl_ctos";
            sql = "SELECT id, RTRIM(nombre) AS \"Barra\",root,f_act AS \"Actualización\",RTRIM(usuario) AS  \"Usuario\" FROM esq_red.tbl_transf ";
            if (nPadre >= 0)
            {
                sql += "WHERE root='" + nPadre + "' ";
            }
            sql += "ORDER BY nombre ASC;";


            // Assign the DataSet as the DataSource for the BindingSource.           
            this.bindingSource3.DataSource = redModel.dt_lista(sql);
            this.bindingNavigator3.BindingSource = this.bindingSource3;
            this.dgv3.DataSource = this.bindingSource3;          
        }

        private void conexión_Cto(int nPadre)
        {
            string sql;
            // Making connection with Npgsql provider
            //NpgsqlConnection conn = new NpgsqlConnection(conn_string);
            //sql = "SELECT * FROM esq_red.tbl_ctos";
            sql = "SELECT id, RTRIM(nombre) AS \"Alimentador\",root,f_act AS \"Actualización\",RTRIM(usuario) AS  \"Usuario\" FROM esq_red.tbl_ctos ";
            if (nPadre >= 0)
            {
                sql += "WHERE root='" + nPadre + "' ";
            }
            sql += "ORDER BY nombre ASC;";

            // Assign the DataSet as the DataSource for the BindingSource.           
            this.bindingSource4.DataSource = redModel.dt_lista(sql);
            this.bindingNavigator4.BindingSource = this.bindingSource4;
            this.dgv4.DataSource = this.bindingSource4;

           
            //dgv4.RowTemplate.Height = 17;
            //dgv4.AlternatingRowsDefaultCellStyle.BackColor = Color.WhiteSmoke;
            //dgv4.BorderStyle = BorderStyle.None;
            //dgv4.CellBorderStyle = DataGridViewCellBorderStyle.None;
            //dgv4.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            //dgv4.AllowUserToResizeRows = false;
            //dgv4.RowHeadersVisible = true;
            //dgv4.RowHeadersWidth = 30;
            //dgv4.AllowUserToAddRows = false;
            //dgv4.ReadOnly = true;

            //dgv4.Columns["nombre"].HeaderText = "Alimentador";
            //dgv4.Columns["id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //dgv4.Columns["id"].Width = 30;
            ////dgvCto.Columns["nodo"].Width = 40;
            //dgv4.Columns["nombre"].Width = 150;
            ////dgvCto.Columns["orden"].Width = 40;
            ////dgvCto.Columns["orden"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //dgv4.Columns["root"].Width = 40;
        }

        private void dgvEjes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //int fila = int.Parse(BindingNavigatorPositionItem.Text) - 1;
            txtIdEje.Text = Convert.ToString(dgv1.Rows[dgv1.SelectedCells[0].RowIndex].Cells["id"].Value);           
            txtNombreEje.Text = Convert.ToString(dgv1.Rows[dgv1.SelectedCells[0].RowIndex].Cells[1].Value);
            txtRootEje.Text = Convert.ToString(dgv1.Rows[dgv1.SelectedCells[0].RowIndex].Cells["root"].Value);
            tsbVerSE.Checked = false;
        }

        private void btnEjeGuardar_Click(object sender, EventArgs e)
        {
             if (!Fun.isShort(txtRootEje.Text)){
                 lblObs1.Text="Número de ID Estado incorrecto";
                 return;
             }

            Int16 id=Convert.ToInt16(txtIdEje.Text);
            Int16 root=Convert.ToInt16(txtRootEje.Text);
            string nombre=txtNombreEje.Text;
            string usuario=Common.Cache.UserCache.LoginUser;

            if (redModel.update_tbl("lta_sistema", id, root, nombre, usuario))
             {
                 int fila = int.Parse(txtNFilaP1.Text)-1;

                 dgv1.Rows[fila].Cells[1].Value = nombre;
                 dgv1.Rows[fila].Cells["root"].Value = root;
                 dgv1.Rows[fila].Cells["Usuario"].Value = usuario;

                 lblObs1.Text = "Datos Guardados con Éxito";

             }
             else
                 lblObs1.Text = "Datos no Guardados";   
        }

        private void txtEjeId_TextChanged(object sender, EventArgs e)
        {
            if (txtIdEje.Text == "")
            {
                btnEjeGuardar.Enabled = false;
            }
            else
            {
                btnEjeGuardar.Enabled = true;
            }
        }

        private void btnSEGuardar_Click(object sender, EventArgs e)
        {
            if (!Fun.isShort(txtRootSE.Text))
            {
                lblObs2.Text = "Número de Id Eje incorrecto";
                return;
            }

            Int16 id = Convert.ToInt16(txtIdSE.Text);
            string nombre = txtNombreSE.Text;
            Int16 root = Convert.ToInt16(txtRootSE.Text);
            
            string usuario = Common.Cache.UserCache.LoginUser;

            if (redModel.update_tbl("tbl_sub", id, root, nombre, usuario))
            {
                int fila = int.Parse(txtNFilaP2.Text) - 1;

                dgv2.Rows[fila].Cells[1].Value = nombre;
                dgv2.Rows[fila].Cells["root"].Value = root;
                dgv2.Rows[fila].Cells["Usuario"].Value = usuario;

                lblObs2.Text = "Datos Guardados con Éxito";

            }
            else
                lblObs2.Text = "Datos no Guardados"; 
        }

        private void dgvSE_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtIdSE.Text = Convert.ToString(dgv2.Rows[dgv2.SelectedCells[0].RowIndex].Cells["id"].Value);
            txtNombreSE.Text = Convert.ToString(dgv2.Rows[dgv2.SelectedCells[0].RowIndex].Cells[1].Value);
           txtRootSE.Text = Convert.ToString(dgv2.Rows[dgv2.SelectedCells[0].RowIndex].Cells["root"].Value);
            tsbVerTransf.Checked = false;
        }
       
        private void dgvSE_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            tabEditorArbol.SelectedTab = tabPgBarra;
            //int fila = int.Parse(BindingNavigatorPositionItem.Text) - 1;
            conexión_Barra(int.Parse(txtIdSE.Text));
            
        }

        private void dgvBarras_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtIdBarra.Text = Convert.ToString(dgv3.Rows[dgv3.SelectedCells[0].RowIndex].Cells["id"].Value);
            txtNombreBarra.Text = Convert.ToString(dgv3.Rows[dgv3.SelectedCells[0].RowIndex].Cells[1].Value);
            txtRootBarra.Text = Convert.ToString(dgv3.Rows[dgv3.SelectedCells[0].RowIndex].Cells["root"].Value);
            tsbVerCtos.Checked = false;
        }

        

        private void btnBarraGuardar_Click(object sender, EventArgs e)
        {
            if (!Fun.isShort(txtRootBarra.Text))
            {
                lblObs3.Text = "Número de Id S/E incorrecto";
                return;
            }

            Int16 id = Convert.ToInt16(txtIdBarra.Text);
            string nombre = txtNombreBarra.Text;
            Int16 root = Convert.ToInt16(txtRootBarra.Text);

            string usuario = Common.Cache.UserCache.LoginUser;

            if (redModel.update_tbl("tbl_transf", id, root, nombre, usuario))
            {
                int fila = int.Parse(txtNFilaP3.Text) - 1;

                dgv3.Rows[fila].Cells[1].Value = nombre;
                dgv3.Rows[fila].Cells["root"].Value = root;
                dgv3.Rows[fila].Cells["Usuario"].Value = usuario;

                lblObs3.Text = "Datos Guardados con Éxito";

            }
            else
                lblObs3.Text = "Datos no Guardados";             
          
        }

        private void txtSEId_TextChanged(object sender, EventArgs e)
        {
            if (txtIdSE.Text == "")
            {
                btnSEGuardar.Enabled = false;
                //chkFiltroSE.Enabled = false;
            }
            else
            {
                btnSEGuardar.Enabled = true;
                //chkFiltroSE.Enabled = true;
            }
        }

        private void txtBarraId_TextChanged(object sender, EventArgs e)
        {
            if (txtIdBarra.Text == "")
            {
                btnBarraGuardar.Enabled = false;
                //chkFiltroBarra.Enabled = false;
            }
            else
            {
                btnBarraGuardar.Enabled = true;
                //chkFiltroBarra.Enabled = true;
            }
        }

        private void txtCtoId_TextChanged(object sender, EventArgs e)
        {
            if (txtIdCto.Text == "")
            {
                cmnGuardarCto.Enabled = false;
                //chk_Filtro_Cto.Enabled = false;
            }
            else
            {
                cmnGuardarCto.Enabled = true;
                //chk_Filtro_Cto.Enabled = true;
            }
        }

        private void dgvBarras_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {         

              tabEditorArbol.SelectedTab = tabPgCto;
            //int fila = int.Parse(BindingNavigatorPositionItem.Text) - 1;
            conexión_Cto(int.Parse(txtIdBarra.Text));
        }

      

        private void dgvCto_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtIdCto.Text = Convert.ToString(dgv4.Rows[dgv4.SelectedCells[0].RowIndex].Cells["id"].Value);
            txtNombreCto.Text = Convert.ToString(dgv4.Rows[dgv4.SelectedCells[0].RowIndex].Cells[1].Value);
            txtRootCto.Text = Convert.ToString(dgv4.Rows[dgv4.SelectedCells[0].RowIndex].Cells["root"].Value);

        }

        

        private void cmnGuardarCto_Click(object sender, EventArgs e)
        {
            if (!Fun.isShort(txtRootCto.Text))
            {
                lblObs3.Text = "Número de Id Transf incorrecto";
                return;
            }

            Int16 id = Convert.ToInt16(txtIdCto.Text);
            string nombre = txtNombreCto.Text;
            Int16 root = Convert.ToInt16(txtRootCto.Text);

            string usuario = Common.Cache.UserCache.LoginUser;

            if (redModel.update_tbl("tbl_ctos", id, root, nombre, usuario))
            {
                int fila = int.Parse(txtNFilaP4.Text) - 1;

                dgv4.Rows[fila].Cells[1].Value = nombre;
                dgv4.Rows[fila].Cells["root"].Value = root;
                dgv4.Rows[fila].Cells["usuario"].Value = usuario;

                lblObs4.Text = "Datos Guardados con Éxito";

            }
            else
                lblObs4.Text = "Datos no Guardados";    


        }              

        private void tsbVerCtos_Click(object sender, EventArgs e)
        {
            if (tsbVerCtos.Checked == false)
            {
                conexión_Cto(-1);
                tsbVerCtos.Checked = true;
            }
            else
            {
                tsbVerCtos.Checked = false;
                if (txtIdBarra.Text != "")
                {
                    conexión_Cto(int.Parse(txtIdBarra.Text));
                }
            }
        }

        private void tsbVerTransf_Click(object sender, EventArgs e)
        {
            if (tsbVerTransf.Checked == false)
            {
                conexión_Barra(-1);
                tsbVerTransf.Checked = true;
            }
            else
            {
                if (int.Parse(txtIdSE.Text) > 0)
                {
                    tsbVerTransf.Checked = false;
                    conexión_Barra(int.Parse(txtIdSE.Text));
                }
            }
        }

        private void tsbVerSE_Click(object sender, EventArgs e)
        {
            if (tsbVerSE.Checked == false)
            {
                conexión_SE(-1);
                tsbVerSE.Checked = true;
            }
            else
            {
                if (int.Parse(txtIdEje.Text) > 0)
                {
                    conexión_SE(int.Parse(txtIdEje.Text));
                    tsbVerSE.Checked = false;
                }
            }
        }

        private void btnEdSE_Click(object sender, EventArgs e)
        {    
            if (!Fun.isShort(txtIdSE.Text))
            {
                lblObs2.Text = "Favor incorporar un valor ID S/E adecuado";
                return;
            }
            frmSub.ZOOM = 20;
            frmSub.ONPANEL = false;
            SubModel.ID = Int16.Parse(txtIdSE.Text);            
            this.TopLevel = false;

            frmSub f_sub = (frmSub)Fun.AbrirFormulario(typeof(frmSub), true);
            //f_sub.CargarSE(txtIdSE.Text);
            this.TopLevel = true;              
            
        }

        private void btnEdTransf_Click(object sender, EventArgs e)
        {
            if (!Fun.isShort(txtIdBarra.Text))
            {
                lblObs3.Text = "Favor incorporar un valor ID Barra adecuado";
                return;
            }
            frmTransf.ZOOM = 10;
            frmSub.ONPANEL = false;
            TransfModel.ID = Int16.Parse(txtIdBarra.Text);
            this.TopLevel = false;

            frmTransf f_transf = (frmTransf)Fun.AbrirFormulario(typeof(frmTransf), true);
            //f_transf.Cargar(txtIdBarra.Text);
            this.TopLevel = true;
            //if (txtIdBarra.Text == "")
            //{
            //    MessageBox.Show("No hay Transformador asignado", "proyecto sin transformador", MessageBoxButtons.OK, MessageBoxIcon.Stop);

            //}
            //else
            //{
                //String nombre;
                //frmEdTransf frm_EditorTransf = new frmEdTransf();
                //this.TopLevel = false;
                //frm_EditorTransf.TopLevel = true;
                //nombre = System.Convert.ToString(txtBarraId.Text);

                //frm_EditorTransf.Cargar_id_transf("0", txtBarraId.Text);
                ////frm_EditorTransf.btnCalcular.PerformClick();
                ////frm_EditorTransf.Cargar_id_transf(System.Convert.ToInt16(nombre));
                ////frm_plan. = true;
                //frm_EditorTransf.ShowDialog();
                //frm_EditorTransf.Dispose();
                //this.TopLevel = true;
            //}
        }

        private void btnEdCto_Click(object sender, EventArgs e)
        {
            if (!Fun.isShort(txtIdCto.Text))
            {
                lblObs4.Text = "Favor incorporar un valor ID Barra adecuado";
                return;
            }
            frmCto.ZOOM = 5;
            frmSub.ONPANEL = false;
            CtoModel.ID = Int16.Parse(txtIdCto.Text);
            this.TopLevel = false;
            frmCto f_cto = (frmCto)Fun.AbrirFormulario(typeof(frmCto), true); 
            this.TopLevel = true;

            //if (txtIdCto.Text == "")
            //{
            //    MessageBox.Show("No hay alimentador asignado", "proyecto sin alimentador", MessageBoxButtons.OK, MessageBoxIcon.Stop);

            //}
            //else
            //{
            //   // //Int16 Id;
            //   //frmEdCto frm_EditorCto = new frmEdCto();
            //   // this.TopLevel = false;
            //   // frm_EditorCto.TopLevel = true;
            //   // //Id = System.Convert.ToInt16(txtCtoId.Text);

            //   // frm_EditorCto.Cargar(txtCtoId.Text);
            //   // //frm_plan. = true;
            //   // frm_EditorCto.ShowDialog();
            //   // frm_EditorCto.Dispose();
            //   // this.TopLevel = true;
            //}
        }

        private void frmRed_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Fun.FormularioCerrar(typeof(frmSub));
            //Fun.FormularioCerrar(typeof(frmTransf));
            //Fun.FormularioCerrar(typeof(frmCto));
        //    if (!frmSub.Equals(null))
        //    frmSub.ActiveForm.Dispose();

        //    if (!frmTransf.ActiveForm.IsDisposed)
        //    frmTransf.ActiveForm.Dispose();
        //    if (!frmCto.ActiveForm.IsDisposed)
        //    frmCto.ActiveForm.Dispose();

        }
           
       
    }
}