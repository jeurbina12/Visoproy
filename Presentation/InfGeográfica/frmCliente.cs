using System;
using System.Collections.Generic;
using System.ComponentModel;
//using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Microsoft.VisualBasic;
using System.Globalization;
using System.Xml;
using System.Net;
using System.Net.NetworkInformation;
using Common.Cache;
using Domain;
using Presentation.Util;

namespace Presentation.InfGeográfica
{
#pragma warning disable CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
    public partial class frmCliente : Form
#pragma warning restore CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
    {
        CadModel cadModel = new CadModel();
        ClienteModel clienteModel = new ClienteModel();

        internal static Int32 ID;
        internal static Int16 ZOOM;
        //internal static bool B_GUARDO;
    

#pragma warning disable CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
        public frmCliente()
#pragma warning restore CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
        {
            InitializeComponent();
            cadModel.CAPA_TEMP = ConfCache.CAD_LAYER_TEM_CLIENTES;
            
        }

        private void frmCliente_Load(object sender, EventArgs e)
        {
            //string _id = Convert.ToString(ID);
            ConfiguracionFormLectura();
            CargarCliente(ID.ToString());
            cadModel.ZOOM = ZOOM;
        }

#pragma warning disable CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
        public void CargarCliente(string id)
#pragma warning restore CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
        {

            System.Data.DataTable dt_fila =  clienteModel.dt_clientes("a.id", id,false);
            if (dt_fila.Rows.Count.Equals(0))
            {
                lblObs.Text = "Id fuera de rango";
                return;
            }

            txtId.Text = System.Convert.ToString(id);
                txtNombre.Text = dt_fila.Rows[0]["nombre"].ToString();
                txtSerie.Text = dt_fila.Rows[0]["serie"].ToString();
                txtNic.Text = dt_fila.Rows[0]["nic"].ToString();
                txtNis.Text = dt_fila.Rows[0]["nis"].ToString();
                txtCuenta.Text = dt_fila.Rows[0]["cuenta"].ToString();
                txtInst.Text = dt_fila.Rows[0]["instalacion"].ToString();
                txtDocId.Text = dt_fila.Rows[0]["doc_id"].ToString();
                txtTipo.Text = dt_fila.Rows[0]["actividad"].ToString();

                txtTelef.Text = dt_fila.Rows[0]["tfno_cli"].ToString();
                txtAlta.Text = System.Convert.ToDateTime(dt_fila.Rows[0]["f_alta_cont"]).ToString("dd-MM-yy");
                txtFInst.Text = System.Convert.ToDateTime(dt_fila.Rows[0]["f_inst"]).ToString("dd-MM-yy");
                txtCalibración.Text = System.Convert.ToDateTime(dt_fila.Rows[0]["f_p_calibracion"]).ToString("dd-MM-yy");
                txtkWh.Text = dt_fila.Rows[0]["kwh"].ToString();

                txtCiau.Text = dt_fila.Rows[0]["ciau"].ToString(); 
                txtUnicom.Text = dt_fila.Rows[0]["unicom_lect"].ToString();
                txtRuta.Text = dt_fila.Rows[0]["ruta"].ToString();
                txtInin.Text = dt_fila.Rows[0]["itin"].ToString();
                txtAol.Text = dt_fila.Rows[0]["aol_finc"].ToString();
                txtCodExt.Text = dt_fila.Rows[0]["cod_ext"].ToString();
                txtTramo.Text = dt_fila.Rows[0]["tramo"].ToString();


                txtMunicipio.Text = dt_fila.Rows[0]["municipio"].ToString();
                txtParroquia.Text = dt_fila.Rows[0]["parroquia"].ToString();
                txtLocalidad.Text = dt_fila.Rows[0]["localidad"].ToString();
                txtIdLoc.Text = dt_fila.Rows[0]["id_localidad"].ToString();
                txtViaTipo.Text = dt_fila.Rows[0]["tip_vía"].ToString();
                txtViaNombre.Text = dt_fila.Rows[0]["nom_vía"].ToString();
                txtObjCon.Text = dt_fila.Rows[0]["obj_conexion"].ToString();
                txtReferencia.Text = dt_fila.Rows[0]["ref_dir"].ToString();
                txtPosteId.Text = dt_fila.Rows[0]["id_poste"].ToString();
                txtPoste.Text = dt_fila.Rows[0]["poste"].ToString();
                txtDuplicador.Text = dt_fila.Rows[0]["duplicador"].ToString();
                txtTFinca.Text = dt_fila.Rows[0]["t_finca"].ToString();
                txtPostal.Text = dt_fila.Rows[0]["id_postal"].ToString();

                txtNif.Text = dt_fila.Rows[0]["nif"].ToString(); 
                txtPuerta.Text = dt_fila.Rows[0]["puerta"].ToString();
                txtInm1.Text = dt_fila.Rows[0]["inm1"].ToString();
                txtInm2.Text = dt_fila.Rows[0]["inm2"].ToString(); 
                txtX.Text = dt_fila.Rows[0]["x"].ToString(); 
                txtY.Text = dt_fila.Rows[0]["y"].ToString(); 
                txtGrado.Text = dt_fila.Rows[0]["grados"].ToString(); 
                txtEsc.Text = dt_fila.Rows[0]["escala"].ToString();

                btnAsociar.Enabled = true;
                if (txtPoste.Text.Length == 0)
                {
                    btnCoordMaps.Enabled = false;
                    btnCoordCad.Enabled = false;
                }
                else
                {
                    btnCoordMaps.Enabled = true;
                    btnCoordCad.Enabled = true;
                }

                if (txtX.Text.Length == 0)
                {
                    btnCoordMaps2.Enabled = false;
                    btnCoordCad2.Enabled = false;
                    btnAgregarCad.Enabled = false;
                }
                else
                {
                    btnCoordMaps2.Enabled = true;
                    btnCoordCad2.Enabled = true;
                    btnAgregarCad.Enabled = true;
                }



        }      

        private void frmCliente_FormClosing(object sender, FormClosingEventArgs e)
        {
            ConfiguracionFormGuarda();
        }

        /// <summary>
        /// Almacena la información del formulario actual en fichero XML
        /// </summary>
        private void ConfiguracionFormGuarda()
        {  
            try
            {
                string nombreForm = this.Name;
                string ficheroXML = Path.Combine(ConfCache.RUTA_XML, nombreForm + ".xml");
                XmlNode nodoPrincipal;

                // Intancia del documento XML que permitirá guardar la configuración
                XmlDocument doc = new XmlDocument();
                // Se comprueba que exista el fichero
                if (!File.Exists(ficheroXML))
                {
                    // No existe el fichero. Se añaden los nodos iniciales
                    doc.AppendChild(doc.CreateComment(ConfCache.MSG_CONF_FRM));

                    // Creación del nodo principal
                    nodoPrincipal = doc.CreateNode(XmlNodeType.Element, "Configuracion", null);

                    // Se añade el nodo principal al documento
                    doc.AppendChild(nodoPrincipal);
                }
                else
                {
                    // El fichero existe previamente. Se recupera la información
                    doc.Load(ficheroXML);

                    // Lectura del nodo principal
                    nodoPrincipal = doc.SelectSingleNode("Configuracion");
                }

                // Se comprueba si existe un nodo con el nombre del form actual
                XmlNode nodoFormActual = nodoPrincipal.SelectSingleNode(nombreForm);
                if (nodoFormActual == null)
                    nodoFormActual =
                          nodoPrincipal.AppendChild(
                            doc.CreateNode(XmlNodeType.Element, nombreForm, null));

                // Almacenamiento de los valores
                Fun.Instance.NodoValor(doc, nodoFormActual, "WindowState", this.WindowState);
                Fun.Instance.NodoValor(doc, nodoFormActual, "Top", this.Top);
                Fun.Instance.NodoValor(doc, nodoFormActual, "Left", this.Left);
                Fun.Instance.NodoValor(doc, nodoFormActual, "TopMost", this.TopMost);
                Fun.Instance.NodoValor(doc, nodoFormActual, "Ficha", tabControl.SelectedIndex);

                // Se guarda el fichero de configuración en disco
                XmlTextWriter tw = new XmlTextWriter(ficheroXML, Encoding.UTF8);
                tw.Indentation = 4;
                tw.IndentChar = " "[0];
                tw.Formatting = Formatting.Indented;

                doc.Save(tw);
                doc = null;

                tw.Flush();
                tw.Close();
            }
            catch { }
        }
        /// <summary>
        /// Realiza una lectura del fichero de configuración y aplica los cambios al form
        /// </summary>
        private void ConfiguracionFormLectura()
        {
            // No se controlan errores. En caso de existir cualquier error
            // no se modifica nada en el formulario actual
            try
            {
                string nombreForm = this.Name;
                string ficheroXML = Path.Combine(ConfCache.RUTA_XML, nombreForm + ".xml");
                XmlNode nodoPrincipal;

                XmlDocument doc = new XmlDocument();
                doc.Load(ficheroXML);

                // Lectura del nodo principal
                nodoPrincipal = doc.SelectSingleNode("Configuracion");

                // Lectura del nodo del formulario actual
                XmlNode nodoActual = nodoPrincipal.SelectSingleNode(nombreForm);

                // Lectura del modo de presentación de la ventana
                this.WindowState = (FormWindowState)Enum.Parse(typeof(FormWindowState), Fun.Instance.GetValor(nodoActual, "WindowState").ToString(), false);

                tabControl.SelectedIndex = Convert.ToInt16(Fun.Instance.GetValor(nodoActual, "Ficha").ToString());
                //ColocarDias();
                if (this.WindowState == FormWindowState.Normal)
                {
                    this.Top = Convert.ToInt32(Fun.Instance.GetValor(nodoActual, "Top"));
                    this.Left = Convert.ToInt32(Fun.Instance.GetValor(nodoActual, "Left"));                   
                    tsbOrden.Checked = Convert.ToBoolean(Fun.Instance.GetValor(nodoActual, "TopMost"));

                    if (tsbOrden.Checked == true)                   
                        this.TopMost = true;                    
                }
                else                
                    this.TopMost = false;                
            }
            catch
            {
                tabControl.SelectedIndex = 0;
                this.TopMost = false;
            }
        }

        
        private void cmnAnterior_Click(object sender, EventArgs e)
        {
            int i = Convert.ToInt32(txtId.Text) - 1;
            CargarCliente(Convert.ToString(i));
        }

        private void cmnSiguiente_Click(object sender, EventArgs e)
        {
            int i = Convert.ToInt32(txtId.Text) + 1;
            CargarCliente(Convert.ToString(i));
        }

        private void btnVerMap_Click(object sender, EventArgs e)
        {

        }

        private void btnCoordMaps_Click(object sender, EventArgs e)
        {
            
            //double longi = 0, lati = 0;
            
            string codigo = txtPoste.Text;
           cadModel.Coordenadas(codigo);


           //cadModel.CambioUTM_DG(out lati, out longi);

           // string dirección = Convert.ToString(longi).Replace(",", ".") + "," + Convert.ToString(lati).Replace(",", ".");
           // System.Diagnostics.Process.Start(string.Format("http://www.google.co.ve/maps?q={0}", dirección));

           // cadModel.X = Convert.ToDouble(txtX.Text);
           // cadModel.Y = Convert.ToDouble(txtY.Text);

            cadModel.CoordMap();
        }

        private void btnCoordCad_Click(object sender, EventArgs e)
        {
            if (txtPoste.Text.Length.Equals(0))
            {
                lblObs.Text = ConfCache.MSG_XY_NULL;
                return;
            }
            cadModel.ZoomGeo(txtPoste.Text, 1);
                lblObs.Text = cadModel.MENSAJE;
        }

        private void btnCoordMaps2_Click(object sender, EventArgs e)
        {           
            double longi = 0, lati = 0;
            cadModel.X = Convert.ToDouble(txtX.Text);
            cadModel.Y = Convert.ToDouble(txtY.Text);

            cadModel.CambioUTM_DG(out lati, out longi);

            string dirección = Convert.ToString(longi).Replace(",", ".") + "," + Convert.ToString(lati).Replace(",", ".");
            System.Diagnostics.Process.Start(string.Format("http://www.google.co.ve/maps?q={0}", dirección));
        }

        private void btnCoordCad2_Click(object sender, EventArgs e)
        {
            
            double xi = Convert.ToDouble(txtX.Text);
            double yi = Convert.ToDouble(txtY.Text);

            cadModel.CoordZoom(xi, yi, 0);
        }

        private void btnAgregarCad_Click(object sender, EventArgs e)
        {
            double x = Convert.ToDouble(txtX.Text);
            double y = Convert.ToDouble(txtY.Text);

            if (x.Equals(0))
            {
                //errorProvider1.SetError(txtX, "Favor asignar coordenadas X e Y");
                lblObs.Text = ConfCache.MSG_XY_NULL;
                return;
            }
            NumberFormatInfo provider = new NumberFormatInfo();
            provider.NumberDecimalSeparator = ".";

            cadModel.X = x;
            cadModel.Y = y;
            cadModel.ESC = Convert.ToDouble(txtEsc.Text.Replace(",", "."), provider);         
            cadModel.GRADOS =Int16.Parse(txtGrado.Text);
            cadModel.CODIGO = txtId.Text;
            cadModel.BLOQUE = "vsg_cliente";

            string nif =txtNif.Text;
            string inm1 = txtInm1.Text;
            string inm2 = txtInm2.Text;
            string puerta = txtPuerta.Text;


            if (!cadModel.BloqueEscribirCliente(nif, inm1, inm2, puerta))
                lblObs.Text = ConfCache.MSG_CAD_ER_DIBUJAR;

           

        }

        private void tsbOrden_Click(object sender, EventArgs e)
        {
            if (tsbOrden.Checked == true)
            {
                tsbOrden.Checked = false;
                this.TopMost = false;
            }
            else
            {
                tsbOrden.Checked = true;
                this.TopMost = true;
            }
        }

        private void mnuFavoritos_Click(object sender, EventArgs e)
        {
            //frmFavoritos frm_favoritos = new frmFavoritos();

            //this.TopMost = false;
            //frm_favoritos.TopLevel = true;
            //frm_favoritos.ID_CLI = txtId.Text;

            //frm_favoritos.ShowDialog();
            //frm_favoritos.Dispose();

            //this.TopLevel = true;


        }
        
       

        private void btnAsociar_Click(object sender, EventArgs e)
        {
            try
            {
                this.TopMost = false;
                this.TopLevel = false;
                
                //string text1 = null, text2 = null;
                string bloque = null;

                if (!txtPoste.Text.Length.Equals(0))
                {
                    MsgBoxStyle style = MsgBoxStyle.YesNo;
                    string msg = "Esta seguro de realizar el cambio de CÓDIGO\ndel transformador asociado?";
                    string title = "Notificación de cambio de Código";

                    MsgBoxResult result = Interaction.MsgBox(msg, style, title);

                    if (result == MsgBoxResult.No)
                    {
                        if (tsbOrden.Checked == true)
                        {
                            this.TopMost = true;

                        }
                        this.TopLevel = true;
                        return;
                    }
                }

                if (!cadModel.BloqueLeer())
                {
                    lblObs.Text = cadModel.MENSAJE;
                    return;
                }
                bloque = cadModel.BLOQUE;
                   //abre solo bloques de transformador
                    if (bloque.Equals("ECADTR01") || bloque.Equals("ECADCT01") || bloque.Equals("ECADST01") || bloque.Equals("ECADTR03") || bloque.Equals("ECADTR04") || bloque.Equals("ECADTR05"))
                    {
                        //string sql;
                        //DateTime Hoy = DateTime.Now;

                        //txtPoste.Text = text1 + text2;
                        if (cadModel.CODIGO.Length <8 || cadModel.CODIGO.Length > 10)
                        {
                            lblObs.Text = cadModel.MENSAJE;
                            MessageBox.Show("Banco de transformador sin Código Geográfico", "Notificación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                           
                            if (txtPosteId.Text.Length.Equals(0))
                            {//registro nuevo de asociación cliente transformador
                                int id_poste=clienteModel.insert_tbl_postes(txtSerie.Text,Convert.ToInt32(txtNis.Text),Convert.ToInt32(txtNif.Text),cadModel.CODIGO,UserCache.LoginUser);
                                if (id_poste.Equals(0))
                                {
                                    lblObs.Text = "No se logró guardar los datos";
                                    return;
                                }
                                else
                                {
                                    txtPosteId.Text = Convert.ToString(id_poste);
                                    txtPoste.Text = cadModel.CODIGO;
                                }
                            }
                            else
                            {//Actualización de un registro
                                if (!clienteModel.update_tbl_postes(int.Parse(txtPosteId.Text), cadModel.CODIGO, UserCache.LoginUser))
                                {
                                    lblObs.Text = "No se logró guardar los datos";
                                    return;
                                }
                                txtPoste.Text = cadModel.CODIGO;
                            }
                                                      
                                                           
                    }
                    else
                    {
                        MessageBox.Show("Favor seleccione Bloques de Transformador", "Notificación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
               


                if (tsbOrden.Checked == true)
                {
                    this.TopMost = true;

                }
                this.TopLevel = true;
            }
            catch (Exception f)
            {
                if (tsbOrden.Checked == true)
                {
                    this.TopMost = true;

                }
                this.TopLevel = true;
                MessageBox.Show("Error: " + f.Message);
            }
        }

        private void cmnBorrarLayer_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            if (!cadModel.LayerBorrar())
                lblObs.Text = cadModel.MENSAJE;

            this.Cursor = Cursors.Default;
        }

        private void tsbVerMap_Click(object sender, EventArgs e)
        {
            //double longi = 0, lati = 0;
            cadModel.X = Convert.ToDouble(txtX.Text);
            cadModel.Y = Convert.ToDouble(txtY.Text);

            cadModel.CoordMap();
            //Class_ModCad MCad = new Class_ModCad();

            //cadModel.CambioUTM_DG(out lati, out longi);
            //string dirección = Convert.ToString(longi).Replace(",", ".") + "," + Convert.ToString(lati).Replace(",", ".");
            //System.Diagnostics.Process.Start(string.Format("http://www.google.co.ve/maps?q={0}", dirección));
        }

        private void tsbCadMostrar_Click(object sender, EventArgs e)
        {
            if (txtX.Text.Equals(""))
            {
                btnCoordCad.PerformClick();
                //lblObs.Text = ConfCache.MSG_XY_NULL;
                return;
            }
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
        
    }
}
