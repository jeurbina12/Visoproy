using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Presentation.Util;
using Microsoft.VisualBasic;
using Common.Cache;
using Domain;
using System.Globalization;


namespace Presentation.InfGeográfica
{
#pragma warning disable CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
    public partial class frmLocalidad : Form
#pragma warning restore CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
    {
        CadModel cadModel = new CadModel();
        LocModel locModel = new LocModel();

#pragma warning disable CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
        public static Int16 ZOOM;
#pragma warning restore CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
#pragma warning disable CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
        public static bool B_GUARDO;
#pragma warning restore CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente

#pragma warning disable CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
        public frmLocalidad()
#pragma warning restore CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
        {
            InitializeComponent();
            
            cadModel.CAPA_TEMP = ConfCache.CAD_LAYER_TEM_LOCALIDAD;
            //string S_bloque = "vsg_zona";
            cadModel.GRADOS = 0;
            cadModel.BLOQUE = "vsg_zona";
           
        }

        private void frmLocalidad_Load(object sender, EventArgs e)
        {
            
            B_GUARDO = false;
            cadModel.ZOOM = ZOOM;

            txtId.Text = (string)LocModel.ID.ToString();
            txtNombre.Text = LocModel.TIPO.ToString() + " " + LocModel.LOCALIDAD.ToString();
            txtMunicipio.Text = LocModel.MUNICIPIO.ToString();
            txtParroquia.Text = LocModel.PARROQUIA.ToString();
            txtX.Text = LocModel.X.ToString();
            txtY.Text = LocModel.Y.ToString();
            txtEscala.Text = LocModel.ESCALA.ToString();
            txtArea.Text = (string)LocModel.AREA.ToString();
            txtClientes.Text = (string)LocModel.CLIENTES.ToString();
            txtFecAct.Text = Convert.ToDateTime(LocModel.ACTUALIZACION).ToString("ddd dd-MM-yy HH:mm");//System.Convert.ToDateTime(
            txtUsuario.Text = LocModel.USUARIO.ToString();

            if (txtX.Text.Equals("0"))
                tsbXYObt.Enabled=true;
            else
                tsbXYObt.Enabled = false;
           
        }

#pragma warning disable CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
        public void Cargar_id_zona(string id_zona,bool modificar)
#pragma warning restore CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
        {
            //tsbObtenerXY.Enabled = modificar;
            //txtArea.ReadOnly = !modificar;

            //conexión_id_zona(id_zona);
            //if (this.ds_datos.Tables[0].Rows.Count == 1)
            //{
            //    txtId.Text = System.Convert.ToString(this.ds_datos.Tables[0].Rows[0]["id"]);
            //    txtNombre.Text =System.Convert.ToString(this.ds_datos.Tables[0].Rows[0]["tipo"])+" "+ System.Convert.ToString(this.ds_datos.Tables[0].Rows[0]["zona"]);
            //    txtParroquia.Text = System.Convert.ToString(this.ds_datos.Tables[0].Rows[0]["parroquia"]);
            //    txtMunicipio.Text = System.Convert.ToString(this.ds_datos.Tables[0].Rows[0]["municipio"]);
            //    txtClientes.Text = System.Convert.ToString(this.ds_datos.Tables[0].Rows[0]["num_clientes"]);

            //    txtX.Text = System.Convert.ToString(this.ds_datos.Tables[0].Rows[0]["x"]);
            //    txtY.Text = System.Convert.ToString(this.ds_datos.Tables[0].Rows[0]["y"]);
            //    txtArea.Text = System.Convert.ToString(this.ds_datos.Tables[0].Rows[0]["area"]);
            //    tsbGuardar.Enabled = false;

            //    dtpFecha.Value = System.Convert.ToDateTime(this.ds_datos.Tables[0].Rows[0]["f_act"]);
            //    txtUsuario.Text = System.Convert.ToString(this.ds_datos.Tables[0].Rows[0]["usuario"]).TrimEnd();

            //    if (txtX.Text == "0")
            //    {
            //        tsbVerCAD.Enabled = false;
            //        tsbVerMap.Enabled = false;
            //    }

            //}
        }

       

        private void tsbVerCAD_Click(object sender, EventArgs e)
        {
            double x = Convert.ToDouble(txtX.Text);
            double y = Convert.ToDouble(txtY.Text);
            if (x.Equals(0))
            {
                lblObs.Text = ConfCache.MSG_XY_NULL;
                return;
            }
            cadModel.CoordZoom(x, y, 0);
           
                lblObs.Text = this.cadModel.MENSAJE;
         }

        private void tsbObtenerXY_Click(object sender, EventArgs e)
        {
              try
            {

                if (!txtX.Text.Equals("0"))
                {
                    MsgBoxStyle style = MsgBoxStyle.YesNo;
                    string msg = "Esta seguro de realizar el cambio de las coordenadas Geográficas?";
                    string title = "Notificación de cambio de coordenadas";

                    MsgBoxResult result = Interaction.MsgBox(msg, style, title);

                    if (result == MsgBoxResult.No)
                        return;

                }
                if (chkOrden.Checked)
                    this.TopMost = false;

                this.TopLevel = false;              
                  
                if(cadModel.CoordObtener())
                {
                    txtX.Text =Convert.ToString(Math.Round(cadModel.X,0));
                    txtY.Text = Convert.ToString(Math.Round(cadModel.Y,0));                  
                }          

                if (chkOrden.Checked)               
                    this.TopMost = true;               
                this.TopLevel = true;
            }
              catch (Exception f)
              {
                  if (chkOrden.Checked == true)
                  {
                      this.TopMost = true;
                  }
                  this.TopLevel = true;
                  MessageBox.Show("Error: " + f.Message);
              }
        }

        private void tsbGuardar_Click(object sender, EventArgs e)
        {
            NumberFormatInfo provider = new NumberFormatInfo();
            provider.NumberDecimalSeparator = ".";

            //if (!cadModel.bloq_esc(Convert.ToDouble(tsbCadEsc.Text.Replace(",", "."), provider)))

            errorProvider1.Clear();
            if (!Fun.isSingle(txtEscala.Text))
            {
                errorProvider1.SetError(txtEscala, "Verifique el valor decimal");
                return;
            }
            int id=int.Parse(txtId.Text);
            Single escala = Convert.ToSingle(txtEscala.Text.Replace(",", "."), provider);
            int x=int.Parse(txtX.Text);
            int y=int.Parse(txtY.Text);
            int area=int.Parse(txtArea.Text);
            string usuario=UserCache.LoginUser;
            if (locModel.update_tbl_zona(id, escala, x, y, area, usuario))
            {
                B_GUARDO = true;
                LocModel.ESCALA = escala;
                LocModel.X = x;
                LocModel.Y = y;
                LocModel.ESCALA = escala;
                LocModel.AREA = area;
                LocModel.USUARIO = usuario;
                LocModel.ACTUALIZACION = DateTime.Now;
                //lblObs.Text = "Datos guardados con éxitos";
                this.Close();
            }
            else
            {
                B_GUARDO = false;
                System.Media.SystemSounds.Beep.Play();
                lblObs.Text = "No se logró guardar los datos";

            }
            //string sql;
            //string area = txtArea.Text;

            //Class_Funciones func = new Class_Funciones();

            ////if (func.isInt16(area))
            ////{
            //sql = "UPDATE esq_open.tbl_zona set " +
            //     "area=@area " +
            //    //"area='" + area + "' " +
            //       ",usuario=@usuario " +
            //    "WHERE id='" + txtId.Text + "'";

            //NpgsqlCommand cmd = new NpgsqlCommand(sql, Pool.traerConexion());

            //cmd.Parameters.Add("@area", NpgsqlDbType.Bigint).Value = System.Convert.ToInt32(area);
            //cmd.Parameters.Add("@usuario", NpgsqlDbType.Varchar, 15).Value = PC_USUARIO;
            //cmd.Parameters.Add("@id", NpgsqlDbType.Integer).Value = System.Convert.ToInt16(txtId.Text);

            //Pool.openConnection();

            //int res = cmd.ExecuteNonQuery();

            //if (res == 0)
            //{
            //    //    MessageBox.Show("Datos Ingresados con Éxitos")};
            //    //else {
            //    MessageBox.Show("No se Ingresaron los Datos");
            //}

            //Pool.terminaPool();
            //cmd.Parameters.Clear();

            //this.Close();

        }

        private void txtArea_TextChanged(object sender, EventArgs e)
        {
            //string area = txtArea.Text;

            //Class_Funciones func = new Class_Funciones();

            //if (func.isInt32(area) && Convert.ToInt32(txtArea.Text) > 0)
            //{
            //    tsbGuardar.Enabled = true;
            //}
            //else
            //{
            //    tsbGuardar.Enabled = false;
            //}
        }

        private void tsbVerMap_Click(object sender, EventArgs e)
        {
            double longi = 0, lati = 0;
            cadModel.X = Convert.ToDouble(txtX.Text);
            cadModel.Y = Convert.ToDouble(txtY.Text);

            //Class_ModCad MCad = new Class_ModCad();
           
            cadModel.CambioUTM_DG(out lati, out longi);
            string dirección = Convert.ToString(longi).Replace(",", ".") + "," + Convert.ToString(lati).Replace(",", ".");
            System.Diagnostics.Process.Start(string.Format("http://www.google.co.ve/maps?q={0}", dirección));
           
        }

        private void btnDibujarCAD_Click(object sender, EventArgs e)
        {
            double x = Convert.ToDouble(txtX.Text);
            double y = Convert.ToDouble(txtY.Text);
            
            if (x.Equals(0))
            {
                errorProvider1.SetError(txtX,"Favor asignar coordenadas X e Y");
                lblObs.Text = ConfCache.MSG_XY_NULL;
                return;
            }
               
            
            cadModel.X = x;
            cadModel.Y = y;
            NumberFormatInfo provider = new NumberFormatInfo();
            provider.NumberDecimalSeparator = ".";
            
            cadModel.ESC = System.Convert.ToDouble(txtEscala.Text.Replace(",", "."), provider);
            
            cadModel.CODIGO =txtId.Text;
            cadModel.CAPACIDAD = LocModel.TIPO.ToString();
            cadModel.SIVPLUS = LocModel.LOCALIDAD.ToString();
            if (!cadModel.BloqueEscribir())
                lblObs.Text = ConfCache.MSG_CAD_ER_DIBUJAR;
            //tsbVerCAD.PerformClick();

            //Class_ModCad MCad = new Class_ModCad();
            //double[] insertionPnt;
            ////double altura=15;
            ////bool malo = false;
            //bool abierto = true;
            //string nombre=System.Convert.ToString(this.ds_datos.Tables[0].Rows[0]["zona"]);
            //string codigo = txtId.Text;
            //string capacidad=System.Convert.ToString(this.ds_datos.Tables[0].Rows[0]["tipo"]);
            //string bloque = "vsg_zona";
            //int marcar = 0;
            //string LAYER = "v_zonas";
            //insertionPnt = new double[3];
            //insertionPnt[0] = Convert.ToDouble(txtX.Text);
            //insertionPnt[1] = Convert.ToDouble(txtY.Text);
            //insertionPnt[2] = 0;

            ////MCad.DibujarTEX(txtNombre.Text, ref insertionPnt, ref altura);
            //MCad.LayerNuevo(LAYER);
            //MCad.DibujarBloque(insertionPnt, codigo, nombre, capacidad, ref abierto, bloque);
            //MCad.CoordZoom(insertionPnt[0], insertionPnt[1], 1, marcar);
        }

       

        private void chkOrden_Click(object sender, EventArgs e)
        {
            chkOrden.Checked = !chkOrden.Checked;
            this.TopMost = chkOrden.Checked;           
        }

        private void tsbCerrar_Click(object sender, EventArgs e)
        {
            
        }

        private void tsbAreaObt_Click(object sender, EventArgs e)
        {
            if (chkOrden.Checked)
                this.TopMost = false;
            this.TopLevel = false;
            string area = cadModel.PolylineObtArea().ToString();
            if (area.Equals("0")){
                System.Media.SystemSounds.Beep.Play();
                lblObs.Text = "No se logró seleccionar la Polylinea en CAD";
            }else             
            txtArea.Text = area;

            if (chkOrden.Checked)
                this.TopMost = true;
            this.TopLevel = true;
           
        }

        private void tsbAtrObt_Click(object sender, EventArgs e)
        {
            if (chkOrden.Checked)
                this.TopMost = false;
            this.TopLevel = false;

            if (cadModel.BloqueLeer())
            {
                if (cadModel.BLOQUE.Equals("vsg_zona"))
                {
                    if (cadModel.CODIGO.Equals(txtId.Text))
                    {
                        txtEscala.Text = cadModel.ESC.ToString();                      
                        txtX.Text = Convert.ToString(Math.Round(cadModel.X, 0));
                        txtY.Text = Convert.ToString(Math.Round(cadModel.Y, 0)); 
                    }
                    else
                    {
                        System.Media.SystemSounds.Beep.Play();
                        lblObs.Text = "Favor selecionar el ID Localidad = " + txtId.Text;
                    }
                }
                else
                {
                    System.Media.SystemSounds.Beep.Play();
                    lblObs.Text = "Favor selecionar el Bloque: 'vsg_zona.dwg'";
                }
            }
            else           
                lblObs.Text = cadModel.MENSAJE;

            if (chkOrden.Checked)
                this.TopMost = true;
            this.TopLevel = true;

        }

       

     

       

     
    }
}
