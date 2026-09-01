using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Presentation;
using Microsoft.VisualBasic;
using System.Globalization;
using Domain;
using Presentation.Util;
using Common.Cache;

namespace Presentation.InfGeográfica
{
#pragma warning disable CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
    public partial class frmTransf : Form
#pragma warning restore CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
    {
        //internal static short ID;
        internal static short ZOOM;
        private DateTime originalValue;
        internal static bool ONPANEL;

        TransfModel transfModel = new TransfModel();
        CadModel cadModel = new CadModel();            

#pragma warning disable CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
        public frmTransf()
#pragma warning restore CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
        {
            InitializeComponent();
            listas_combobox();
            cadModel.ZOOM = ZOOM;
        }

        private void frmEdTransf_Load(object sender, EventArgs e)
        {
            //tabTransf.SelectedIndex = V_TRANSF;
            Cargar(TransfModel.ID.ToString());
        }

#pragma warning disable CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
        public void Cargar(string id_transf)
#pragma warning restore CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
        {
            System.Data.DataTable dt_fila = transfModel.dt_transf(id_transf);
            if (dt_fila.Rows.Count.Equals(0))
            {
                MessageBox.Show("Id fuera de rango", "Notificación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            txtId.Text = id_transf;
            txtCodigo.Text = dt_fila.Rows[0]["codigo"].ToString();
            txtX.Text = Convert.ToString(dt_fila.Rows[0]["x"]);
            txtY.Text = Convert.ToString(dt_fila.Rows[0]["y"]);
            txtGrados.Text = Convert.ToString(dt_fila.Rows[0]["grados"]);
            txtEscala.Text = Convert.ToString(dt_fila.Rows[0]["escala"]);

            if (txtX.Text == "0")
            {
                btnCoordMaps.Enabled = false;
                tsbCadMostrar.Enabled = false;
            }
            else
            {
                btnCoordMaps.Enabled = true;
                tsbCadMostrar.Enabled = true;
            }

            txtNombre.Text = dt_fila.Rows[0]["transformador"].ToString() + " - S/E " + dt_fila.Rows[0]["subestacion"].ToString();
            cbxkV1.Text = dt_fila.Rows[0]["kv_alta"].ToString();
            cbxkV2.Text = dt_fila.Rows[0]["kv_baja"].ToString();

            cbxMarca.Text = dt_fila.Rows[0]["marca"].ToString();

            txtMvaInst.Text = dt_fila.Rows[0]["mva_inst"].ToString();
            txtZ.Text = dt_fila.Rows[0]["z"].ToString();

            txtAmpMax.Text = dt_fila.Rows[0]["amp_max"].ToString();
            txtPvar.Text = dt_fila.Rows[0]["pvar"].ToString();
            originalValue = System.Convert.ToDateTime(dt_fila.Rows[0]["f_max"]);
            dtpFechaDemanda.Value = originalValue;
          
            txtFecAct.Text = Convert.ToDateTime(dt_fila.Rows[0]["f_act"]).ToString("dd-MM-yy HH:mm");
            txtUsuario.Text = dt_fila.Rows[0]["usuario"].ToString();

            chkTension.Checked = (Boolean)dt_fila.Rows[0]["b_p_voltaje"];
            chkProteccion.Checked = (Boolean)dt_fila.Rows[0]["b_p_proteccion"];
            chkScada.Checked = (Boolean)dt_fila.Rows[0]["b_p_scada"];
            chkForzada.Checked = (Boolean)dt_fila.Rows[0]["b_p_ventilacion"];
            chkNoOperativo.Checked = (Boolean)dt_fila.Rows[0]["b_p_operativo"];
            chkMovil.Checked = (Boolean)dt_fila.Rows[0]["b_movil"];


            cbxCondInter.Text = dt_fila.Rows[0]["t_condicion"].ToString();
           

            txtObs.Text = dt_fila.Rows[0]["observacion"].ToString();

            calcular();

        }
                
       
        private void listas_combobox()
        {
            //cbxkV1.Items.Add("2.4");
            //cbxkV1.Items.Add("13.8");
            //cbxkV1.Items.Add("34.5");
            //cbxkV1.Items.Add("115");
            //cbxkV1.Items.Add("230");
            //cbxkV1.Items.Add("400");
            //cbxkV1.Items.Add("765");

            //cbxkV2.Items.Add("2.4");
            //cbxkV2.Items.Add("13.8");
            //cbxkV2.Items.Add("34.5");
            //cbxkV2.Items.Add("115");
            //cbxkV2.Items.Add("230");
            //cbxkV2.Items.Add("400");
            //cbxkV2.Items.Add("765");

            cbxkV1.DataSource = transfModel.dt_lista("esq_red.lta_kv");
            cbxkV1.DisplayMember = "nombre";
            cbxkV1.ValueMember = "id";

            cbxkV2.DataSource = transfModel.dt_lista("esq_red.lta_kv");
            cbxkV2.DisplayMember = "nombre";
            cbxkV2.ValueMember = "id";

            cbxMarca.DataSource = transfModel.dt_lista("esq_red.lta_marcas");
            cbxMarca.DisplayMember = "nombre";
            cbxMarca.ValueMember = "id";

            cbxCondInter.DataSource = transfModel.dt_lista("esq_red.lta_cond_inter");
            cbxCondInter.DisplayMember = "nombre";
            cbxCondInter.ValueMember = "id";
                       
        }

        private void tsbGuardar_Click(object sender, EventArgs e)
        {
            if (this.btnGuardar.Tag == null)
            {
                this.btnGuardar.Tag = 0;
                this.btnGuardar.Image = global::Presentation.Properties.Resources.page_save;
                this.btnGuardar.Text = "Guardar";

                cbxkV1.DropDownStyle = ComboBoxStyle.DropDown;
                cbxkV2.DropDownStyle = ComboBoxStyle.DropDown;
                cbxMarca.DropDownStyle = ComboBoxStyle.DropDown;
                cbxCondInter.DropDownStyle = ComboBoxStyle.DropDown;

                dtpFechaDemanda.ValueChanged -= dtpFechaDemanda_ValueChanged;

                txtMvaInst.ReadOnly = false;
                txtZ.ReadOnly = false;
                txtAmpMax.ReadOnly = false;
                txtPvar.ReadOnly = false;
                txtObs.ReadOnly = false;

                chkTension.AutoCheck = true;
                chkForzada.AutoCheck = true;
                chkProteccion.AutoCheck = true;
                chkScada.AutoCheck = true;
                chkMovil.AutoCheck = true;
                chkNoOperativo.AutoCheck = true;
            }
            else
            {
                if (!Verificar())
                    return;

                this.btnGuardar.Tag = null;
                this.btnGuardar.Image = global::Presentation.Properties.Resources.page_edit;
                this.btnGuardar.Text = "Editar";

                cbxkV1.DropDownStyle = ComboBoxStyle.Simple;
                cbxkV2.DropDownStyle = ComboBoxStyle.Simple;
                cbxMarca.DropDownStyle = ComboBoxStyle.Simple;
                cbxCondInter.DropDownStyle = ComboBoxStyle.Simple;

                originalValue = dtpFechaDemanda.Value;
                dtpFechaDemanda.ValueChanged += dtpFechaDemanda_ValueChanged;

                txtMvaInst.ReadOnly = true;
                txtZ.ReadOnly = true;
                txtAmpMax.ReadOnly = true;
                txtPvar.ReadOnly = true;
                txtObs.ReadOnly = true;

                chkTension.AutoCheck = false;
                chkForzada.AutoCheck = false;
                chkProteccion.AutoCheck = false;
                chkScada.AutoCheck = false;
                chkMovil.AutoCheck = false;
                chkNoOperativo.AutoCheck = false;

                Guardar();
            }
        }

        private bool Verificar()
        {
            errorProvider1.Clear();

            if (cbxkV1.SelectedIndex.Equals(-1))
            {
                errorProvider1.SetError(cbxkV1, "Favor Asignar valor de la lista");
                return false;
            }
            if (cbxkV2.SelectedIndex.Equals(-1))
            {
                errorProvider1.SetError(cbxkV2, "Favor Asignar valor de la lista");
                return false;
            }
            if (cbxMarca.SelectedIndex.Equals(-1))
            {
                errorProvider1.SetError(cbxMarca, "Favor Asignar valor de la lista");
                return false;
            }
            if (cbxCondInter.SelectedIndex.Equals(-1))
            {
                errorProvider1.SetError(cbxCondInter, "Favor Asignar valor de la lista");
                return false;
            }

            if (!Fun.isShort(txtAmpMax.Text))
            {
                errorProvider1.SetError(txtAmpMax, "Favor corregir valor asignado");
                return false;
            }
            if (!Fun.isShort(txtPvar.Text))
            {
                errorProvider1.SetError(txtPvar, "Favor corregir valor asignado");
                return false;
            }
            if (!Fun.isSingle(txtMvaInst.Text))
            {
                errorProvider1.SetError(txtMvaInst, "Favor corregir valor asignado");
                return false;
            }
           if (!Fun.isSingle(txtZ.Text))
            {
                errorProvider1.SetError(txtZ, "Favor corregir valor asignado");
                return false;
            }

            return true;
        }

        private void Guardar()
        {
            NumberFormatInfo provider = new NumberFormatInfo();
            provider.NumberDecimalSeparator = ".";

            TransfModel.F_MAX = Convert.ToDateTime(dtpFechaDemanda.Value);
            TransfModel.MVA = Convert.ToDecimal(txtMvaInst.Text.Replace(",", "."), provider);
            TransfModel.AMP = Convert.ToInt16(txtAmpMax.Text);
            TransfModel.PVAR = Convert.ToInt16(txtPvar.Text);
            TransfModel.KV1 = Convert.ToDecimal(cbxkV1.Text, provider);
            TransfModel.KV2 = Convert.ToDecimal(cbxkV2.Text, provider);
            TransfModel.Z = Convert.ToDecimal(txtZ.Text.Replace(",","."), provider);
            TransfModel.MARCA=cbxMarca.Text;
            TransfModel.B_MOVIL=chkMovil.Checked;
            TransfModel.B_VOLTAJE=chkTension.Checked;
            TransfModel.B_PROCT=chkProteccion.Checked;
            TransfModel.B_SCADA=chkScada.Checked;
            TransfModel.B_VENT=chkForzada.Checked;
            TransfModel.B_OPER=chkNoOperativo.Checked;
            TransfModel.ID_COND = short.Parse(cbxCondInter.SelectedValue.ToString());
            TransfModel.OBS= txtObs.Text;
           TransfModel.USER= UserCache.LoginUser;

           if (transfModel.update_tbl_transf())
           {
               if (!ONPANEL)
                   this.Close();// txtFecAct.Text = Convert.ToDateTime(DateTime.Now).ToString("ddd dd-MM-yy HH:mm");              
           }
           else
               MessageBox.Show("Datos No guardados", "Notificación", MessageBoxButtons.OK, MessageBoxIcon.Information);

          
        }
        private void calcular()
        {
            double mva_max = Fun.funMVA(txtAmpMax.Text, cbxkV2.Text);
            txtMvaMax.Text = mva_max.ToString();
            txtFu.Text = Fun.funFU(txtMvaMax.Text, txtMvaInst.Text).ToString();
        }

        private void btnCalcular_Click(object sender, EventArgs e)
        {
            calcular();
        }
               
            

        private void btnCoordMaps_Click(object sender, EventArgs e)
        {
            double longi = 0, lati = 0;// xi = Convert.ToDouble(txtX.Text), yi = Convert.ToDouble(txtY.Text);

            CadModel cadModel = new CadModel();

            cadModel.X = Convert.ToDouble(txtX.Text);
            cadModel.Y = Convert.ToDouble(txtY.Text);

            cadModel.CambioUTM_DG(out lati, out longi);

            string dirección = Convert.ToString(longi).Replace(",", ".") + "," + Convert.ToString(lati).Replace(",", ".");
            System.Diagnostics.Process.Start(string.Format("http://www.google.co.ve/maps?q={0}", dirección)); 
        }

        
        private void btnObtXY_Click(object sender, EventArgs e)
        {
            string msg = "Datos No guardados";

            if (chkOrden.Checked == false)
                this.WindowState = FormWindowState.Minimized;

            if (cadModel.BloqueLeer())
            {
                
                if (cadModel.BLOQUE.Equals("ECADTR02"))//|| cadModel.CAPA.Equals("RED")
                {
                    TransfModel.X = (int)cadModel.X;
                    TransfModel.Y = (int)cadModel.Y;
                    TransfModel.ESCALA = (float)cadModel.ESC;
                    TransfModel.GRADOS = cadModel.GRADOS;
                    TransfModel.CODIGO = cadModel.CODIGO;

                    SubModel.USER = UserCache.LoginUser;

                    

                    if (transfModel.update_tbl_transf_xy())
                    {
                        txtX.Text = TransfModel.X.ToString();
                        txtY.Text = TransfModel.Y.ToString();
                        txtEscala.Text = TransfModel.ESCALA.ToString();
                        txtGrados.Text = TransfModel.GRADOS.ToString();
                        txtCodigo.Text = TransfModel.CODIGO.ToString();
                        txtUsuario.Text = TransfModel.USER;

                        txtFecAct.Text = Convert.ToDateTime(DateTime.Now).ToString("ddd dd-MM-yy HH:mm");
                        msg = "Datos guardados con Éxitos";
                    }
                    MessageBox.Show(msg, "Notificación", MessageBoxButtons.OK, MessageBoxIcon.Information);   

                }
                else
                {
                    msg = "No corresponde el Bloque: 'ECADTR02' <> " + cadModel.BLOQUE;
                    MessageBox.Show(msg, "Error de Selección", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            else
            {
                msg = cadModel.MENSAJE;
                MessageBox.Show(msg, "Error de Selección", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            if (chkOrden.Checked == false)
            {
                this.WindowState = FormWindowState.Normal;
                this.TopMost = true;
            }
            

        }

        private void tsbOrden_Click(object sender, EventArgs e)
        {
            if (chkOrden.Checked == true)
            {
                chkOrden.Checked = false;
                this.TopMost = false;
            }
            else
            {
                chkOrden.Checked = true;
                this.TopMost = true;
            }
        }

        private void cbxkVAlta_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void cbxkVbaja_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void cbxMarca_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void cbxCondición_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void dtpFechaDemanda_ValueChanged(object sender, EventArgs e)
        {
            dtpFechaDemanda.Value = originalValue;
        }

        private void tsbCadMostrar_Click(object sender, EventArgs e)
        {
            string msg;
            double x = Convert.ToDouble(txtX.Text);
            double y = Convert.ToDouble(txtY.Text);
            if (x.Equals(0))
            {
                msg = ConfCache.MSG_XY_NULL;
                MessageBox.Show(msg, "Notificación", MessageBoxButtons.OK, MessageBoxIcon.Information);   
                return;
            }
            if (!cadModel.CoordZoom(x, y, 0))
            {
                msg = this.cadModel.MENSAJE;
                MessageBox.Show(msg, "Notificación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
                        
                //this.WindowState = FormWindowState.Minimized;
        }

        

       

        

        

      






    }
}