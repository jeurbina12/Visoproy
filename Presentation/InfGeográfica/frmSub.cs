using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using System.IO;
using Domain;
using Common.Cache;
using Presentation.Util;

namespace Presentation.InfGeográfica
{
#pragma warning disable CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
    public partial class frmSub : Form
#pragma warning restore CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
    {
        internal static short ZOOM;
        internal static bool ONPANEL;

        SubModel subModel = new SubModel();
        CadModel cadModel = new CadModel();
        //internal Int16 V_SUB;     

#pragma warning disable CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
        public frmSub()
#pragma warning restore CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
        {
            InitializeComponent();
            listas_combobox();
            cadModel.ZOOM = ZOOM;
        }

        private void frmSub_Load(object sender, EventArgs e)
        {
            CargarSE(SubModel.ID.ToString());
            //    tabSSEE.SelectedIndex = V_SUB;            
        }

        private void listas_combobox()
        {
            cbxGerencia.Items.Add("Generación");
            cbxGerencia.Items.Add("Transmisión");
            cbxGerencia.Items.Add("Distribución");
            cbxGerencia.Items.Add("Tercero");

            cbxTensiones.Items.Add("115/13.8");
            cbxTensiones.Items.Add("115/34.5");
            cbxTensiones.Items.Add("115/34.5/13.8");
            cbxTensiones.Items.Add("13.8/2.4");
            cbxTensiones.Items.Add("230/115");
            cbxTensiones.Items.Add("230/115/13.8");
            cbxTensiones.Items.Add("34.5/13.8");
            cbxTensiones.Items.Add("34.5/13.8/2.4");
            cbxTensiones.Items.Add("400/230/115/13.8");
            cbxTensiones.Items.Add("765/400/230/115");

            //cbxGerencia.DropDownStyle = ComboBoxStyle.DropDownList;
            //cbxGerencia.FlatStyle = FlatStyle.Flat;
            //cbxGerencia.BackColor = Color.FromArgb(235, 235, 235);
            //cbxGerencia.DrawMode = DrawMode.OwnerDrawFixed;
            //cbxGerencia.DrawItem += field_DrawItem;
        }

        
        internal void CargarSE(string id_sub)
        {
            System.Data.DataTable dt_fila = subModel.dt_sub(id_sub);
            if (dt_fila.Rows.Count.Equals(0))
            {
                MessageBox.Show("Id fuera de rango", "Notificación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            txtId.Text = id_sub;
            txtNombre.Text = dt_fila.Rows[0]["subestacion"].ToString() + " - Eje " + dt_fila.Rows[0]["sistema"].ToString();
            cbxTensiones.Text = dt_fila.Rows[0]["tensiones_kv"].ToString();
            cbxGerencia.Text = dt_fila.Rows[0]["gerencia"].ToString();
            txtYear.Text = dt_fila.Rows[0]["year_construccion"].ToString();
            txtCantMva.Text = dt_fila.Rows[0]["cantidad_mva"].ToString();

            chkTroncal.Checked = (Boolean)dt_fila.Rows[0]["b_troncal"];
            chkTension.Checked = (Boolean)dt_fila.Rows[0]["b_p_tension"];
            chkScada.Checked = (Boolean)dt_fila.Rows[0]["b_scada"];
            chkResguardo.Checked = (Boolean)dt_fila.Rows[0]["b_p_resguardo"];
            chkObsolecencia.Checked = (Boolean)dt_fila.Rows[0]["b_p_obsolecencia"];
            chkCapFirme.Checked = (Boolean)dt_fila.Rows[0]["b_cap_firme"];
            chkOperadores.Checked = (Boolean)dt_fila.Rows[0]["b_operadores"];
            chkPersonal.Checked = (Boolean)dt_fila.Rows[0]["b_personal"];

            txtFecAct.Text = Convert.ToDateTime(dt_fila.Rows[0]["f_act"]).ToString("dd-MM-yy HH:mm");
            txtUsuario.Text = Convert.ToString(dt_fila.Rows[0]["usuario"]);
            txtObs.Text = Convert.ToString(dt_fila.Rows[0]["observacion"]);

            //txtCodigo.Text = System.Convert.ToString(dt_fila.Rows[0]["codigo"]).TrimEnd();
            txtX.Text = Convert.ToString(dt_fila.Rows[0]["x"]);
            txtY.Text = Convert.ToString(dt_fila.Rows[0]["y"]);
            txtGrados.Text = Convert.ToString(dt_fila.Rows[0]["grados"]);
            txtEscala.Text = Convert.ToString(dt_fila.Rows[0]["escala"]);


            if (txtX.Text == "0")            
                btnCoordMaps.Enabled = false;               
            

            //DATOS DEL SUMINISTRO
            txtEstado.Text = Convert.ToString(dt_fila.Rows[0]["estado"]);
            txtMunicipio.Text = Convert.ToString(dt_fila.Rows[0]["municipio"]);
            txtParroquia.Text = Convert.ToString(dt_fila.Rows[0]["parroquia"]);
            txtReferencia.Text = Convert.ToString(dt_fila.Rows[0]["direccion"]);

            //ID_MUNICIPIO = System.Convert.ToString(dt_fila.Rows[0]["id_municipio"]).TrimEnd();


        }


        private void tsbGuardar_Click(object sender, EventArgs e)
        {

            if (this.btnGuardar.Tag == null)
            {
                this.btnGuardar.Tag = 0;
                this.btnGuardar.Image = global::Presentation.Properties.Resources.page_save;
                              
                cbxTensiones.DropDownStyle = ComboBoxStyle.DropDown;
                cbxGerencia.DropDownStyle = ComboBoxStyle.DropDown;
                                
                txtYear.ReadOnly = false;
                txtCantMva.ReadOnly = false;
                txtReferencia.ReadOnly = false;
                txtObs.ReadOnly = false;

                chkTension.AutoCheck = true;
                chkResguardo.AutoCheck = true;
                chkObsolecencia.AutoCheck = true;
                chkScada.AutoCheck = true;
                chkOperadores.AutoCheck = true;
                chkCapFirme.AutoCheck = true;
                chkTroncal.AutoCheck = true;
                chkPersonal.AutoCheck = true;
            }
            else
            {
                if (!Verificar())
                    return;
                //msgb
                this.btnGuardar.Tag = null;
                this.btnGuardar.Image = global::Presentation.Properties.Resources.page_edit;

                cbxTensiones.DropDownStyle = ComboBoxStyle.Simple;
                cbxGerencia.DropDownStyle = ComboBoxStyle.Simple;

                txtYear.ReadOnly = true;
                txtCantMva.ReadOnly = true;
                txtReferencia.ReadOnly = true;
                txtObs.ReadOnly = true;

                chkTension.AutoCheck = false;
                chkResguardo.AutoCheck = false;
                chkObsolecencia.AutoCheck = false;
                chkScada.AutoCheck = false;
                chkOperadores.AutoCheck = false;
                chkCapFirme.AutoCheck = false;
                chkTroncal.AutoCheck = false;
                chkPersonal.AutoCheck = false;

                Guardar();
            }


        }

        private bool Verificar()
        {
            errorProvider1.Clear();

            if (cbxTensiones.SelectedIndex.Equals(-1))
            {
                errorProvider1.SetError(cbxTensiones, "Favor Asignar valor de la lista");
                return false;
            }
            if (cbxGerencia.SelectedIndex.Equals(-1))
            {
                errorProvider1.SetError(cbxGerencia, "Favor Asignar valor de la lista");
                return false;
            }
            if (!Fun.isShort(txtYear.Text))
            {
                errorProvider1.SetError(txtYear, "Favor corregir valor asignado");
                return false;
            }
            if (txtCantMva.Text.Length<4)
            {
                errorProvider1.SetError(txtCantMva, "Favor asignar Capacidad");
                return false;
            }

            return true;
        }

        private void Guardar()
        {          
            SubModel.YEAR = Int16.Parse(txtYear.Text);

            SubModel.CAPACIDAD = txtCantMva.Text;
            SubModel.KV = cbxTensiones.SelectedItem.ToString();
            SubModel.GERENCIA = cbxGerencia.SelectedItem.ToString();
            SubModel.OBS = txtObs.Text;

            SubModel.B_OPER = chkOperadores.Checked;
            SubModel.B_PERS = chkPersonal.Checked;
            SubModel.B_RESG = chkResguardo.Checked;
            SubModel.B_OBSO = chkObsolecencia.Checked;
            SubModel.B_FIRM = chkCapFirme.Checked;
            SubModel.B_KV = chkTension.Checked;
            SubModel.B_TRONC = chkTroncal.Checked;
            SubModel.B_SCADA = chkScada.Checked;
            SubModel.USER = UserCache.LoginUser;

            if (subModel.update_tbl_Sub())
            {
                if (!ONPANEL)
                    this.Close();//   txtFecAct.Text = Convert.ToDateTime(DateTime.Now).ToString("ddd dd-MM-yy HH:mm"); 
            }
            else
                MessageBox.Show("Datos No guardados", "Notificación", MessageBoxButtons.OK, MessageBoxIcon.Information);                 
        }

        private void tsbCadMostrar_Click(object sender, EventArgs e)
        {           
            double x = Convert.ToDouble(txtX.Text);
            double y = Convert.ToDouble(txtY.Text);
            if (x.Equals(0))
            {               
                MessageBox.Show(ConfCache.MSG_XY_NULL, "Notificación", MessageBoxButtons.OK, MessageBoxIcon.Information);   
                return;
            }
            cadModel.CoordZoom(x, y, 0);           
            MessageBox.Show(this.cadModel.MENSAJE, "Notificación", MessageBoxButtons.OK, MessageBoxIcon.Information);   
        }

       
        private void btnEdDirección_Click(object sender, EventArgs e)
        {
            //frmEdDirSE frm_EdDirSE = new frmEdDirSE();
            //this.TopMost = false;
            //frm_EdDirSE.TopLevel = true;

            //frm_EdDirSE.ID = txtId.Text;
            //frm_EdDirSE.ID_ESTADO_SE = 8;
            //frm_EdDirSE.ID_MUNICIPIO = ID_MUNICIPIO;
            //frm_EdDirSE.ID_PARROQUIA = ID_PARROQUIA;
            //frm_EdDirSE.MUNICIPIO = txtMunicipio.Text;
            //frm_EdDirSE.PARROQUIA = txtParroquia.Text;           
            //frm_EdDirSE.DIRECCION = txtReferencia.Text;

            //frm_EdDirSE.txtPostal.Text = txtPostal.Text;

            //frm_EdDirSE.PC_USUARIO = PC_USUARIO;


            //frm_EdDirSE.Cargar();

            //frm_EdDirSE.ShowDialog();
            //frm_EdDirSE.Dispose();

            //if (frm_EdDirSE.GUARDO)
            //{               
            //    ID_MUNICIPIO = frm_EdDirSE.ID_MUNICIPIO;
            //    ID_PARROQUIA = frm_EdDirSE.ID_PARROQUIA;
            //    txtMunicipio.Text = frm_EdDirSE.MUNICIPIO;
            //    txtParroquia.Text = frm_EdDirSE.PARROQUIA;

            //    txtReferencia.Text = frm_EdDirSE.DIRECCION;              
            //    txtPostal.Text = frm_EdDirSE.txtPostal.Text;

            //}
            //this.TopLevel = true;
        }

       

        private void frmSub_FormClosing(object sender, FormClosingEventArgs e)
        {
            //V_SUB = (Int16)tabSSEE.SelectedIndex;
        }

        private void cbxTensiones_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void cbxGerencia_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void btnObtXY_Click(object sender, EventArgs e)
        {
            string msg;
            if (chkOrden.Checked == false)
                this.WindowState = FormWindowState.Minimized;

            if (cadModel.BloqueLeer())
            {
                if (cadModel.BLOQUE.Equals("ECADSU01") || cadModel.CAPA.Equals("RED"))
                {
                    SubModel.X = (int)cadModel.X;
                    SubModel.Y = (int) cadModel.Y;
                    SubModel.ESCALA =(float) cadModel.ESC;
                    SubModel.GRADOS = cadModel.GRADOS;

                    SubModel.USER = UserCache.LoginUser;

                    if (subModel.update_tbl_sub_xy())
                    {
                        txtX.Text =SubModel.X.ToString();
                        txtY.Text = SubModel.Y.ToString();
                        txtEscala.Text = SubModel.ESCALA.ToString();
                        txtGrados.Text = SubModel.GRADOS.ToString();
                        txtUsuario.Text = SubModel.USER;

                        txtFecAct.Text = Convert.ToDateTime(DateTime.Now).ToString("ddd dd-MM-yy HH:mm");
                        msg = "Datos guardados con Éxitos";
                    }
                    else
                        msg = "Datos No guardados";

                    MessageBox.Show(msg, "Notificación", MessageBoxButtons.OK, MessageBoxIcon.Information);   

                }
                else
                {
                    msg = "No corresponde a la Capa o Bloque: RED,ECADSU01 <>" + cadModel.CAPA + ", " + cadModel.BLOQUE;
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

        private void chkOrden_Click(object sender, EventArgs e)
        {
            chkOrden.Checked = !chkOrden.Checked;
            this.TopMost = chkOrden.Checked; 
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

       
    }
}
