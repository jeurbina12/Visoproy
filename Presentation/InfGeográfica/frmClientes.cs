using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
using System.Text;
using System.Windows.Forms;
using Domain;
using Common.Cache;
using System.IO;
using Presentation.Util;
using System.Xml;
using System.Collections;
using System.Globalization;
using Presentation.InfGeográfica;


namespace Presentation.InfGeográfica
{
    public partial class frmClientes : Form
    {
        private string MTS_VEC = "100";

        ClienteModel clientesModel = new ClienteModel();
        CadModel cadModel = new CadModel();

        BindingSource customersBindingSource = new BindingSource();     

        //private static SortedList formInstances = new SortedList(); // Para guardar las referencias de las instancias de los formularios

#pragma warning disable CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
        public frmClientes()
#pragma warning restore CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
        {
            InitializeComponent();
            ListaClientes();
            cadModel.CAPA_TEMP = ConfCache.CAD_LAYER_TEM_CLIENTES;
           
        }

        private void frmLocalidades_Load(object sender, EventArgs e)
        {
            ConfiguracionFormLectura();

        }

        private void frmLocalidades_FormClosing(object sender, FormClosingEventArgs e)
        {
            ConfiguracionFormGuarda();
        }

        /// <summary>
        /// Realiza una lectura del fichero de configuración y aplica los cambios al form
        /// </summary>
        private void ConfiguracionFormLectura()
        {
            string nombreForm = this.Name;            
            string ficheroXML = Path.Combine(ConfCache.RUTA_XML, nombreForm + ".xml");
            XmlNode nodoPrincipal;

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(ficheroXML);

                // Lectura del nodo principal
                nodoPrincipal = doc.SelectSingleNode("Configuracion");

                // Lectura del nodo del formulario actual
                XmlNode nodoActual = nodoPrincipal.SelectSingleNode(nombreForm);

                // Lectura del modo de presentación de la ventana
                this.WindowState = (FormWindowState)Enum.Parse(typeof(FormWindowState), Fun.Instance.GetValor(nodoActual, "WindowState").ToString(), false);

                if (this.WindowState == FormWindowState.Normal)
                {
                    this.Top = Convert.ToInt32(Fun.Instance.GetValor(nodoActual, "Top"));
                    this.Left = Convert.ToInt32(Fun.Instance.GetValor(nodoActual, "Left"));
                    this.Width = Convert.ToInt32(Fun.Instance.GetValor(nodoActual, "Width"));
                    this.Height = Convert.ToInt32(Fun.Instance.GetValor(nodoActual, "Height"));

                    if (Convert.ToBoolean(Fun.Instance.GetValor(nodoActual, "Orden").ToString()))
                        this.mnuOrden.PerformClick();

                    //}
                    //else
                    //{
                    //    tsbOrden.Checked = false;
                }
                //this.mnuVerArbol.Checked = Convert.ToBoolean(Fun.Instance.GetValor(nodoActual, "VerArbol").ToString());
                if (Convert.ToBoolean(Fun.Instance.GetValor(nodoActual, "VerInmuebles").ToString()))
                    mnuVerInmuebles.PerformClick();

                if (!Convert.ToBoolean(Fun.Instance.GetValor(nodoActual, "mnuVerBarFiltro").ToString()))
                    mnuVerBarFiltro.PerformClick();
                if (!Convert.ToBoolean(Fun.Instance.GetValor(nodoActual, "mnuVerBarCad").ToString()))
                    mnuVerBarCad.PerformClick();
                //if (!Convert.ToBoolean(Fun.Instance.GetValor(nodoActual, "mnuVerAbrev").ToString()))
                //    mnuVerInmuebles.PerformClick();


                cadModel.ZOOM = Convert.ToInt16(Fun.Instance.GetValor(nodoActual, "Zoom"));
                this.tsbCadMostrar.Text = cadModel.ZOOM + "x";

                cadModel.MARCAR = Convert.ToInt16(Fun.Instance.GetValor(nodoActual, "Marcar"));
                this.tsbCadMarcar.Text = cadModel.MARCAR + " m";


                this.txtCodigo.Text = Convert.ToString(Fun.Instance.GetValor(nodoActual, "Codigo"));

                this.cbxFiltro.Text = Convert.ToString(Fun.Instance.GetValor(nodoActual, "cbxFiltro"));
                this.cbx_ltaVariable.Text = Convert.ToString(Fun.Instance.GetValor(nodoActual, "cbxVariable"));
                this.txtId.Text = Convert.ToString(Fun.Instance.GetValor(nodoActual, "id"));
                this.MTS_VEC = Convert.ToString(Fun.Instance.GetValor(nodoActual, "MTS_VEC"));
            }
            //catch (Exception f)
            catch
            {
                XmlDocument doc = new XmlDocument();
                //XmlNode nodoPrincipal;
                // No existe el fichero. Se añaden los nodos iniciales
                doc.AppendChild(doc.CreateComment(ConfCache.MSG_CONF_FRM));

                // Creación del nodo principal
                nodoPrincipal = doc.CreateNode(XmlNodeType.Element, "Configuracion", null);

                // Se añade el nodo principal al documento
                doc.AppendChild(nodoPrincipal);

                cadModel.ZOOM = 1;
                cadModel.MARCAR = 5;
                tsbCadMarcar.Text = cadModel.MARCAR + " m";

                //MessageBox.Show("Error: " + f.Message);
            }
        }

        private void ConfiguracionFormGuarda()
        {
            string nombreForm = this.Name;
            string ficheroXML = Path.Combine(ConfCache.RUTA_XML, nombreForm + ".xml");
            XmlNode nodoPrincipal;

            try
            {
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
                Fun.Instance.NodoValor(doc, nodoFormActual, "Width", this.Width);
                Fun.Instance.NodoValor(doc, nodoFormActual, "Height", this.Height);
                Fun.Instance.NodoValor(doc, nodoFormActual, "Orden", this.mnuOrden.Checked);
                Fun.Instance.NodoValor(doc, nodoFormActual, "VerInmuebles", this.mnuVerInmuebles.Checked);
                Fun.Instance.NodoValor(doc, nodoFormActual, "mnuVerBarFiltro", this.mnuVerBarFiltro.Checked);
                Fun.Instance.NodoValor(doc, nodoFormActual, "mnuVerBarCad", this.mnuVerBarCad.Checked);
                //Fun.Instance.NodoValor(doc, nodoFormActual, "mnuVerAbrev", this.mnuVerInmuebles.Checked);

                Fun.Instance.NodoValor(doc, nodoFormActual, "Zoom", cadModel.ZOOM);
                Fun.Instance.NodoValor(doc, nodoFormActual, "Marcar", cadModel.MARCAR);
                Fun.Instance.NodoValor(doc, nodoFormActual, "Codigo", txtCodigo.Text);

                Fun.Instance.NodoValor(doc, nodoFormActual, "cbxFiltro", cbxFiltro.Text);
                Fun.Instance.NodoValor(doc, nodoFormActual, "cbxVariable", cbx_ltaVariable.Text);
                Fun.Instance.NodoValor(doc, nodoFormActual, "id", txtId.Text);
                Fun.Instance.NodoValor(doc, nodoFormActual, "MTS_VEC", this.MTS_VEC);

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
            catch (Exception f)
            {

                MessageBox.Show("Error: " + f.Message);

            }


        }

        private void ListaClientes()
        {
            cbxFiltro.Items.Clear();
            cbxFiltro.Items.Add("Nif");
            cbxFiltro.Items.Add("Nic");
            cbxFiltro.Items.Add("Nis");
            cbxFiltro.Items.Add("Poste");
            cbxFiltro.Items.Add("Documento");
            cbxFiltro.Items.Add("Nombre");
            cbxFiltro.Items.Add("id_localidad");
            cbxFiltro.Items.Add("cod_ext");
            cbxFiltro.Items.Add("puerta");
            cbxFiltro.Items.Add("Contrato Sap");
            cbxFiltro.Items.Add("Instalación Sap");
            cbxFiltro.Items.Add("obj_conexion");
            //cbxFiltro.Items.Add("obj_conexion Palabra");
            cbxFiltro.Items.Add("Itinerario");
            //cbxFiltro.SelectedIndex = 1;
        }         
        
      

        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            if (cbx_ltaVariable.Text.Length.Equals(0))
            {
                MessageBox.Show("Favor seleccionar un elemento de la lista", "Notificación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            this.Cursor = Cursors.WaitCursor;

            this.customersBindingSource.DataSource = clientesModel.dt_clientes(cbxFiltro.Text.ToLower(), cbx_ltaVariable.Text, mnuVerInmuebles.Checked);
            this.tspBindingNavigatorCad.BindingSource = this.customersBindingSource;
            this.dgvDatos.DataSource = this.customersBindingSource;


            this.Cursor = Cursors.Default;
        }

        //public static Form AbrirVentana(Type type)
        //{
        //    return AbrirVentana(type, false);
        //}

        //public static Form AbrirVentana(Type type, bool dialog)//static
        //{
        //    Form formulario;
        //    if ((formulario = (Form)formInstances[type.ToString()]) == null || formulario.IsDisposed)
        //    {
        //        formulario = (Form)Activator.CreateInstance(type);
        //        formInstances[type.ToString()] = formulario;
        //    }

        //    formulario.Activate();
        //    //formulario.WindowState = FormWindowState.Normal;
        //    //formulario.MdiParent = this;
        //    //formulario.Text = formulario.Name + "Ventana " + childFormNumber++;
        //    //formulario.Text = formulario.Text;

        //    if (dialog)
        //        formulario.ShowDialog();
        //    else
        //        formulario.Show();

        //    return formulario;
        //}

        private void dgvDatos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            frmCliente.ZOOM = cadModel.ZOOM;

            this.TopLevel = false;

            DataGridViewRow dgv_selecc = dgvDatos.Rows[e.RowIndex];

            frmCliente.ID = Int32.Parse(dgv_selecc.Cells["id"].Value.ToString());
            //LocCache.LOCALIDAD = dgv_selecc.Cells["localidad"].Value.ToString();
            //LocCache.TIPO = dgv_selecc.Cells["tipo"].Value.ToString();
            //LocCache.MUNICIPIO = dgv_selecc.Cells["municipio"].Value.ToString();
            //LocCache.PARROQUIA = dgv_selecc.Cells["parroquia"].Value.ToString();
            //LocCache.X = Int32.Parse(dgv_selecc.Cells["x"].Value.ToString());
            //LocCache.Y = Int32.Parse(dgv_selecc.Cells["y"].Value.ToString());
            //LocCache.ESCALA = Single.Parse(dgv_selecc.Cells["escala"].Value.ToString());
            //LocCache.AREA = Int32.Parse(dgv_selecc.Cells["área"].Value.ToString());
            //LocCache.CLIENTES = Int16.Parse(dgv_selecc.Cells["clientes"].Value.ToString());
            //LocCache.ACTUALIZACION = System.Convert.ToDateTime(dgv_selecc.Cells["actualización"].Value);//System.Convert.ToDateTime(
            //LocCache.USUARIO = dgv_selecc.Cells["usuario"].Value.ToString();

            frmCliente f_cliente = (frmCliente)Fun.AbrirFormulario(typeof(frmCliente), false);

           
            //if (frmLocalidad.B_GUARDO)
            //{
            //    dgv_selecc.Cells["área"].Value = LocCache.AREA;
            //    dgv_selecc.Cells["x"].Value = LocCache.X;
            //    dgv_selecc.Cells["y"].Value = LocCache.Y;
            //    dgv_selecc.Cells["escala"].Value = LocCache.ESCALA;
            //    dgv_selecc.Cells["área"].Value = LocCache.AREA;
            //    dgv_selecc.Cells["usuario"].Value = UserCache.Name;
            //    dgv_selecc.Cells["actualización"].Value = LocCache.ACTUALIZACION;
            //}
            f_cliente.FormClosed += Logout;

            dgvDatos.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.DarkSlateGray;

            //this.TopLevel = true;

            if (mnuActZoom.Checked)
                mnuCadMostrar.PerformClick();

            if (mnuActMarcar.Checked)
                mnuCadMarcar.PerformClick();


        }

        private void Logout(object sender, FormClosedEventArgs e)
        {
            this.TopLevel = true;
        }

        private void mnuOrden_Click(object sender, EventArgs e)
        {
            mnuOrden.Checked = !mnuOrden.Checked;
            tsbOrden.Checked = mnuOrden.Checked;
            this.TopMost = tsbOrden.Checked;

            //if (mnuOrden.Checked == true)
            //{
            //    mnuOrden.Checked = false;
            //    this.TopMost = false;
            //}
            //else
            //{
            //    mnuOrden.Checked = true;
            //    this.TopMost = true;
            //}
        }

        private void mnuVerBarFiltro_Click(object sender, EventArgs e)
        {
            mnuVerBarFiltro.Checked = !mnuVerBarFiltro.Checked;
            tspFiltro.Visible = mnuVerBarFiltro.Checked;
        }

        private void mnuVerBarCad_Click(object sender, EventArgs e)
        {
            mnuVerBarCad.Checked = !mnuVerBarCad.Checked;
            tspBindingNavigatorCad.Visible = mnuVerBarCad.Checked;
        }

        private void mnuVer_Click(object sender, EventArgs e)
        {

        }

        private void mnuAjustarV_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal; ;
            this.Location = new Point(0, 0);
            this.Size = new Size(Screen.PrimaryScreen.WorkingArea.Size.Width, Screen.PrimaryScreen.WorkingArea.Size.Height * 2 / 5);
            //this.Size = new Size(Screen.PrimaryScreen.WorkingArea.Size.Width, Screen.PrimaryScreen.WorkingArea.Size.Height / 3);

        }

        private void rbn1x_Click(object sender, EventArgs e)
        {
            tsbCadMostrar.Text = rbn1x.Text;
            cadModel.ZOOM = 1 * 1;
        }
        private void rbn2x_Click(object sender, EventArgs e)
        {
            tsbCadMostrar.Text = rbn2x.Text;
            cadModel.ZOOM = 2 * 1;
        }
        private void rbn3x_Click(object sender, EventArgs e)
        {
            tsbCadMostrar.Text = rbn3x.Text;
            cadModel.ZOOM = 3 * 1;
        }
        private void rbn4x_Click(object sender, EventArgs e)
        {
            tsbCadMostrar.Text = rbn4x.Text;
            cadModel.ZOOM = 4 * 1;
        }

        private void rbn5m_Click(object sender, EventArgs e)
        {
            tsbCadMarcar.Text = rbn5m.Text;
            cadModel.MARCAR = 5 * 1;
        }
        private void rbn10m_Click(object sender, EventArgs e)
        {
            tsbCadMarcar.Text = rbn10m.Text;
            cadModel.MARCAR = 10 * 1;
        }
        private void rbn15m_Click(object sender, EventArgs e)
        {
            tsbCadMarcar.Text = rbn15m.Text;
            cadModel.MARCAR = 15 * 1;
        }
        private void rbn20m_Click(object sender, EventArgs e)
        {
            tsbCadMarcar.Text = rbn20m.Text;
            cadModel.MARCAR = 20 * 1;
        }

        private void mnuCadMostrar_Click(object sender, EventArgs e)
        {
            int fila = int.Parse(bindingNavigatorPositionItem.Text) - 1;
            if (fila.Equals(-1))
            {
                lblObs.Text = ConfCache.MSG_ROW_NULL;
                return;
            }
            if (dgvDatos.Rows[fila].Cells["x"].Value == System.DBNull.Value)
                return;
            double x = Convert.ToDouble(dgvDatos.Rows[fila].Cells["x"].Value);
            double y = Convert.ToDouble(dgvDatos.Rows[fila].Cells["y"].Value);
            if (x.Equals(0))
            {
                lblObs.Text = ConfCache.MSG_XY_NULL;
                return;
            }

            Int16 _marcar = 0;
            if (mnuActMarcar.Checked)
                _marcar = cadModel.MARCAR;
            if (cadModel.CoordZoom(x, y, _marcar))
            {
                if (mnuOrden.Checked == false)
                    this.WindowState = FormWindowState.Minimized;
            }
            else
            {
                lblObs.Text = this.cadModel.MENSAJE;
            }




        }

        private void mnuCadMarcar_Click(object sender, EventArgs e)
        {
            int fila = int.Parse(bindingNavigatorPositionItem.Text) - 1;
            if (fila.Equals(-1))
            {
                lblObs.Text = "No hay elementos en la tabla";
                return;
            }
            if (dgvDatos.Rows[fila].Cells["x"].Value != System.DBNull.Value)
            {
                double x = Convert.ToDouble(dgvDatos.Rows[fila].Cells["x"].Value);
                double y = Convert.ToDouble(dgvDatos.Rows[fila].Cells["y"].Value);
                if (x.Equals(0))
                {
                    lblObs.Text = ConfCache.MSG_XY_NULL;// "el registro seleccionado no tiene coordenas UTM (X,Y)";
                    return;
                }

                if (!cadModel.CoordMarcar(x, y))
                    lblObs.Text = "La Localidad no se marco en plano Cad ";
            }
        }

        private void tsbCadMostrarCheck_Click(object sender, EventArgs e)
        {
            mnuActZoom.PerformClick();
        }

        private void mnuActZoom_Click(object sender, EventArgs e)
        {
            mnuActZoom.Checked = !mnuActZoom.Checked;

            if (this.tsbCadMostrarCheck.Tag == null)
            {
                this.tsbCadMostrarCheck.Tag = 0;
                this.tsbCadMostrarCheck.Image = global::Presentation.Properties.Resources.Checked;
            }
            else
            {
                //msgb
                this.tsbCadMostrarCheck.Tag = null;
                this.tsbCadMostrarCheck.Image = global::Presentation.Properties.Resources.Unchecked;
            }
        }

        private void tsbCadMostrar_ButtonClick(object sender, EventArgs e)
        {
            mnuCadMostrar.PerformClick();
        }

        private void tsbCadMarcar_ButtonClick(object sender, EventArgs e)
        {
            mnuCadMarcar.PerformClick();
        }

        private void tsbCadMarcarCheck_Click(object sender, EventArgs e)
        {
            mnuActMarcar.PerformClick();
        }

        private void mnuActMarcar_Click(object sender, EventArgs e)
        {
            mnuActMarcar.Checked = !mnuActMarcar.Checked;
            if (this.tsbCadMarcarCheck.Tag == null)
            {
                this.tsbCadMarcarCheck.Tag = 0;
                this.tsbCadMarcarCheck.Image = global::Presentation.Properties.Resources.Checked;
            }
            else
            {
                //msgb
                this.tsbCadMarcarCheck.Tag = null;
                this.tsbCadMarcarCheck.Image = global::Presentation.Properties.Resources.Unchecked;
            }
        }

        private void mnuActBlipmode_Click(object sender, EventArgs e)
        {
            if (mnuActBlipmode.Checked == true)
            {
                mnuActBlipmode.Checked = false;
                if (!cadModel.Activar_BLIPMODE("OFF"))
                {
                    lblObs.Text = "No se logró desactivar el comando BLIPMODE";
                }
            }
            else
            {
                mnuActBlipmode.Checked = true;
                if (!cadModel.Activar_BLIPMODE("ON"))
                {
                    lblObs.Text = "No se logró activar el comando BLIPMODE";
                }
            }
        }

        private void mnuCadLayerErase_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            if (!cadModel.LayerBorrar())
                lblObs.Text = cadModel.MENSAJE;

            this.Cursor = Cursors.Default;
        }

        private void tsbCadLayerBorrar_Click(object sender, EventArgs e)
        {
            mnuCadLayerErase.PerformClick();
        }

        private void tsbCadMarcarAll_Click(object sender, EventArgs e)
        {
            mnuCadMarcarAll.PerformClick();
        }

        private void mnuCadMarcarAll_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            int n_filas = dgvDatos.Rows.Count;
            if (n_filas.Equals(0))
            {
                lblObs.Text = "No hay registros en la tabla";
                this.Cursor = Cursors.Default;
                return;
            }

            Int16 fsc = 0;
            double x, y;
            for (int i = 0; i < n_filas; i++)
            {
                if (dgvDatos.Rows[i].Cells["x"].Value != System.DBNull.Value)
                {

                    x = Convert.ToDouble(dgvDatos.Rows[i].Cells["x"].Value);
                    y = Convert.ToDouble(dgvDatos.Rows[i].Cells["y"].Value);
                    if (!x.Equals(0))
                        cadModel.CoordMarcar(x, y);
                    else
                        fsc += 1;
                }else
                    fsc += 1;
            }

            lblObs.Text = String.Format("Existen {0} registros sin coordenadas X e Y.", fsc);
            this.Cursor = Cursors.Default;
        }

        private void mnuCadDibBloq_Click(object sender, EventArgs e)
        {
            int fila = int.Parse(bindingNavigatorPositionItem.Text) - 1;
            if (fila.Equals(-1))
            {
                lblObs.Text = ConfCache.MSG_ROW_NULL;
                return;
            }
            if (dgvDatos.Rows[fila].Cells["x"].Value != System.DBNull.Value)
            {
                double x = Convert.ToDouble(dgvDatos.Rows[fila].Cells["x"].Value);
                double y = Convert.ToDouble(dgvDatos.Rows[fila].Cells["y"].Value);
                if (x.Equals(0))
                {
                    lblObs.Text = ConfCache.MSG_XY_NULL;
                    return;
                }

                string S_bloque = "vsg_cliente";
                cadModel.BLOQUE = S_bloque;

                cadModel.GRADOS = 0;
                cadModel.X = x;
                cadModel.Y = y;
                //NumberFormatInfo provider = new NumberFormatInfo();
                //provider.NumberDecimalSeparator = ".";

                cadModel.ESC = Convert.ToDouble(dgvDatos.Rows[fila].Cells["escala"].Value);//, provider);

                cadModel.CODIGO = Convert.ToString(dgvDatos.Rows[fila].Cells["id"].Value);


                string nif = Convert.ToString(dgvDatos.Rows[fila].Cells["nif"].Value);
                string inm1 = null, inm2 = null, puerta = null;
                if (mnuVerInmuebles.Checked)
                {
                    inm1 = Convert.ToString(dgvDatos.Rows[fila].Cells["inm1"].Value);
                    inm2 = Convert.ToString(dgvDatos.Rows[fila].Cells["inm2"].Value);
                    puerta = Convert.ToString(dgvDatos.Rows[fila].Cells["puerta"].Value);

                }

                //Convert.ToString(dgvDatos.Rows[fila].Cells["tipo"].Value);

                if (!cadModel.BloqueEscribirCliente(nif, inm1, inm2, puerta))
                    lblObs.Text = ConfCache.MSG_CAD_ER_DIBUJAR;
            }
        }

        private void mnuCadDibBloques_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            int n_filas = dgvDatos.Rows.Count;
            if (n_filas.Equals(0))
            {
                this.Cursor = Cursors.Default;
                lblObs.Text = ConfCache.MSG_ROW_NULL;
                return;
            }

            string S_bloque = "vsg_cliente";
            cadModel.BLOQUE = S_bloque;
            cadModel.GRADOS = 0;
            Int16 fsc = 0;
            double x, y;

            string nif;
            string inm1 = null, inm2 = null, puerta = null;

            for (int i = 0; i < n_filas; i++)
            {
                if (dgvDatos.Rows[i].Cells["x"].Value != System.DBNull.Value)
                {
                    x = Convert.ToDouble(dgvDatos.Rows[i].Cells["x"].Value);
                    y = Convert.ToDouble(dgvDatos.Rows[i].Cells["y"].Value);

                    if (!x.Equals(0))
                    {
                        cadModel.X = x;
                        cadModel.Y = y;
                        cadModel.ESC = Convert.ToDouble(dgvDatos.Rows[i].Cells["escala"].Value);
                        cadModel.CODIGO = Convert.ToString(dgvDatos.Rows[i].Cells["id"].Value);
                        nif = Convert.ToString(dgvDatos.Rows[i].Cells["nif"].Value);
                        inm1 = Convert.ToString(dgvDatos.Rows[i].Cells["inm1"].Value);
                        inm2 = Convert.ToString(dgvDatos.Rows[i].Cells["inm2"].Value);
                        puerta = Convert.ToString(dgvDatos.Rows[i].Cells["puerta"].Value);
                        if (!cadModel.BloqueEscribirCliente(nif, inm1, inm2, puerta))
                            lblObs.Text = ConfCache.MSG_CAD_ER_DIBUJAR;
                    }
                    else
                        fsc += 1;
                }else
                    fsc += 1;

            }

            lblObs.Text = String.Format(ConfCache.MSG_CAD_ER_DIBUJAR_CANT, fsc);
            this.Cursor = Cursors.Default;


        }

        private void btnZoom_Click(object sender, EventArgs e)
        {
            Int16 _marcar = 0;
            if (mnuActMarcar.Checked)
                _marcar = cadModel.MARCAR;

            if (cadModel.ZoomGeo(txtCodigo.Text, _marcar))
            {
                if (mnuOrden.Checked == false)
                    this.WindowState = FormWindowState.Minimized;
            }
            lblObs.Text = cadModel.MENSAJE;
        }

        private void mnuVerInmuebles_Click(object sender, EventArgs e)
        {
            mnuVerInmuebles.Checked = !mnuVerInmuebles.Checked;
            mnuCadVecinos.Enabled = mnuVerInmuebles.Checked;
            tsbCadVecinos.Enabled = mnuVerInmuebles.Checked;
            mnuCadDibBloq.Enabled = mnuVerInmuebles.Checked;
            mnuCadDibBloques.Enabled = mnuVerInmuebles.Checked;
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            mnuEditarId.PerformClick();           

        }

        //private void Logout(object sender, FormClosedEventArgs e)
        //{
        //    //txtpass.Text = "CONTRASEÑA";
        //    //txtpass.UseSystemPasswordChar = false;
        //    //txtuser.Text = "USUARIO";
        //    //lblErrorMessage.Visible = false;
        //    this.Show();
        //    this.BringToFront();
        //    //this.Close();
        //}

        private void tsbOrden_Click(object sender, EventArgs e)
        {
            mnuOrden.PerformClick();
            //tsbOrden.Checked = !tsbOrden.Checked;
            //this.TopMost = tsbOrden.Checked;
        }

        private void mnuEsc1_Click(object sender, EventArgs e)
        {
            tsbCadEsc.Text = mnuEsc1.Text;
        }

        private void mnuEsc1_2_Click(object sender, EventArgs e)
        {
            tsbCadEsc.Text = mnuEsc1_2.Text;
        }

        private void mnuEsc0_8_Click(object sender, EventArgs e)
        {
            tsbCadEsc.Text = mnuEsc0_8.Text;
        }

        private void mnuEsc0_6_Click(object sender, EventArgs e)
        {
            tsbCadEsc.Text = mnuEsc0_6.Text;
        }

        private void mnuActContinuo_Click(object sender, EventArgs e)
        {
            mnuActContinuo.Checked = !mnuActContinuo.Checked;

            if (mnuActContinuo.Checked)
                this.lblObs.Text = "Utilizar ESC para cancelar en AutoCad";

        }

        private void tsbCadEsc_ButtonClick(object sender, EventArgs e)
        {
            NumberFormatInfo provider = new NumberFormatInfo();
            provider.NumberDecimalSeparator = ".";

            //if (!cadModel.bloq_esc(double.Parse(tsbCadEsc.Text)))
            if (!cadModel.bloq_esc(Convert.ToDouble(tsbCadEsc.Text.Replace(",", "."), provider)))
            
                lblObs.Text = "Función cancelada";
            else
            {
                if (mnuActContinuo.Checked)
                    tsbCadEsc.PerformButtonClick();
            }
        }



        private void tsbCadEsc0_6_Click(object sender, EventArgs e)
        {
            tsbCadEsc.Text = mnuEsc0_6.Text;
        }

        private void tsbCadEsc0_8_Click(object sender, EventArgs e)
        {
            tsbCadEsc.Text = mnuEsc0_8.Text;
        }

        private void tsbCadEsc1_0_Click(object sender, EventArgs e)
        {
            tsbCadEsc.Text = mnuEsc1.Text;
        }

        private void tsbCadEsc1_2_Click(object sender, EventArgs e)
        {
            tsbCadEsc.Text = mnuEsc1_2.Text;
        }

        private void btnBuscarCad_Click(object sender, EventArgs e)
        {
            mnuEditarCad.PerformClick();           


        }

        private void mnuBuscar_Click(object sender, EventArgs e)
        {
            if (!Fun.isInt32(txtId.Text))
            {
                lblObs.Text = "Favor incorporar un valor ID";
                return;
            }
            frmCliente.ZOOM = cadModel.ZOOM;
            frmCliente.ID = Int32.Parse(txtId.Text);
            this.TopLevel = false;

            frmCliente f_cliente = (frmCliente)Fun.AbrirFormulario(typeof(frmCliente), true);
   
            this.TopLevel = true;
        }

        private void mnuBuscarCad_Click(object sender, EventArgs e)
        {
            if (tsbOrden.Checked)
                this.TopMost = false;
            this.TopLevel = false;

            if (cadModel.BloqueLeerCliente())
            {
                string S_bloque = "vsg_cliente";
                if (cadModel.BLOQUE.Equals(S_bloque))
                {
                    if (Fun.isInt32(cadModel.ID))
                    {
                        txtId.Text = cadModel.ID;
                        mnuEditarId.PerformClick();

                    }
                    else
                    {
                        System.Media.SystemSounds.Beep.Play();
                        lblObs.Text = "Falta el ID Localidad del Bloque";
                    }
                }
                else
                {
                    System.Media.SystemSounds.Beep.Play();
                    lblObs.Text = "Favor seleccionar Bloque tipo: "+S_bloque;
                }
            }
            else
                lblObs.Text = cadModel.MENSAJE;

            if (tsbOrden.Checked)
                this.TopMost = true;
            this.TopLevel = true;
        }

        private void mnuCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void mnuSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void mnuCadFiltrar_Click(object sender, EventArgs e)
        {
            if (tsbOrden.Checked)
                this.TopMost = false;
            this.TopLevel = false;

            if (cadModel.BloqueLeer())
            {
                string bloque = cadModel.BLOQUE;

                //abre solo bloques de transformador
                if (bloque.Equals("ECADTR01") || bloque.Equals("ECADCT01") || bloque.Equals("ECADST01") || bloque.Equals("ECADTR03") || bloque.Equals("ECADTR04") || bloque.Equals("ECADTR05"))
                {
                    cbxFiltro.Text = "Poste";
                    cbx_ltaVariable.Text = cadModel.CODIGO;
                    //btnFiltrar.PerformClick();
                }
                else if (bloque.Equals("vsg_zona") )
                {
                    cbxFiltro.Text = "id_localidad";
                    cbx_ltaVariable.Text = cadModel.CODIGO;
                }
                else if (bloque.Equals("vsg_cliente") || bloque.Equals("1") || bloque.Equals("3") || bloque.Equals("SUSCRIP2"))
                {
                    cbxFiltro.Text = "Nif";
                    cbx_ltaVariable.Text = cadModel.SIVPLUS;
                }
                else// (bloque.Equals("vsg_zona"))
                {
                    System.Media.SystemSounds.Beep.Play();
                    lblObs.Text = "Favor seleccionar Bloques del tipo:\nTransformador,Localidad,Suscriptor...";
                }
            }
            else
                lblObs.Text = cadModel.MENSAJE;

            if (tsbOrden.Checked)
                this.TopMost = true;
            this.TopLevel = true;
        }

        private void tsbCadFiltrar_Click(object sender, EventArgs e)
        {           
            mnuCadFiltrar.PerformClick();
        }
        private bool VerificarRegistros()
        {
            if (dgvDatos.RowCount.Equals(0))
            {
                System.Media.SystemSounds.Beep.Play();
                lblObs.Text = "No existen registros en la tabla";
                return false;
            }
            return true;
        }
        private void mnuCtaLocalidad_Click(object sender, EventArgs e)
        {
            if (!VerificarRegistros())
                return;
            //cbx_ltaVariable.ComboBox.DataSource = null;
            cbxFiltro.Text = "id_localidad";
            cbx_ltaVariable.Text = Convert.ToString(dgvDatos.Rows[dgvDatos.SelectedCells[0].RowIndex].Cells[cbxFiltro.Text].Value);
            btnFiltrar.PerformClick();
        }

        private void mnuCtaCodExt_Click(object sender, EventArgs e)
        {
            if (!VerificarRegistros())
                return;
            //cbx_ltaVariable.ComboBox.DataSource = null;
            cbxFiltro.Text = "cod_ext";
            cbx_ltaVariable.Text = Convert.ToString(dgvDatos.Rows[dgvDatos.SelectedCells[0].RowIndex].Cells[cbxFiltro.Text].Value);
            btnFiltrar.PerformClick();
        }

        private void mnuCtaNif_Click(object sender, EventArgs e)
        {
            if (!VerificarRegistros())
                return;
            //cbx_ltaVariable.ComboBox.DataSource = null;
            cbxFiltro.Text = "Nif";
            cbx_ltaVariable.Text = Convert.ToString(dgvDatos.Rows[dgvDatos.SelectedCells[0].RowIndex].Cells[cbxFiltro.Text].Value);
            btnFiltrar.PerformClick();
        }

        private void mnuCtaObjCon_Click(object sender, EventArgs e)
        {
            if (!VerificarRegistros())
                return;
            cbxFiltro.Text = "obj_conexion";
            cbx_ltaVariable.Text = Convert.ToString(dgvDatos.Rows[dgvDatos.SelectedCells[0].RowIndex].Cells[cbxFiltro.Text].Value).TrimEnd();
            if (cbx_ltaVariable.Text.Length > 0)
            {
                btnFiltrar.PerformClick();
            }
            else
            {
                MessageBox.Show("No existe Objeto de Conexión", "Notificación", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }               

        private void mnuLtaItinerario_Click(object sender, EventArgs e)
        {
            if (!VerificarRegistros())
                return;

            this.Cursor = Cursors.WaitCursor;

            

            string cod = Convert.ToString(dgvDatos.Rows[dgvDatos.SelectedCells[0].RowIndex].Cells["cod_ext"].Value);
            string itin = Convert.ToString(dgvDatos.Rows[dgvDatos.SelectedCells[0].RowIndex].Cells["itin"].Value);
            string ruta = Convert.ToString(dgvDatos.Rows[dgvDatos.SelectedCells[0].RowIndex].Cells["ruta"].Value);
            string sql2 = " cod_ext='" + cod + "' AND itin=" + itin + " AND ruta=" + ruta;
            string sql = "SELECT nif FROM esq_open.tbl_clientes WHERE " + sql2 + " GROUP BY nif ORDER BY nif ASC;";

            cbxFiltro.Text = "Itinerario";
            cbx_ltaVariable.Text = sql2;
            btnFiltrar.PerformClick();

            cbxFiltro.Text = "Nif";
            System.Data.DataTable dt = new System.Data.DataTable();
            dt = clientesModel.dt_lista(sql);
            int totalRows = dt.Rows.Count;
            cbx_ltaVariable.Items.Clear();

            for (int i = 0; i < totalRows; i++)
            {
                cbx_ltaVariable.Items.Add(dt.Rows[i][0].ToString());
            }
            cbx_ltaVariable.ComboBox.SelectedIndex = 0;
            //cbx_ltaVariable.ComboBox.DataSource = clientesModel.dt_lista(sql);
            //cbx_ltaVariable.ComboBox.DisplayMember = "nombre";
            //cbx_ltaVariable.ComboBox.ValueMember = "id";
           

            
            this.Cursor = Cursors.Default;
        }

        private void mnuActBancoClientes_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            lblObs.Text = clientesModel.update_tbl_cargas();
            this.Cursor = Cursors.Default;
        }

        private void tsbCADVecinos_Click(object sender, EventArgs e)
        {
            mnuCadVecinos.PerformClick();
        }

        private void mnuCadVecinos_Click(object sender, EventArgs e)
        {
            if (tsbOrden.Checked)
                this.TopMost = false;
            this.TopLevel = false;

            if (cadModel.CoordObtener())
            {
                if (tsbOrden.Checked == true)
                    this.TopMost = true;
                this.TopLevel = true;

                string distancia = Microsoft.VisualBasic.Interaction.InputBox("Favor Ingrese la distancia en metros\nseparados de las coordenadas (" + Convert.ToInt32(cadModel.X) + " , " + Convert.ToInt32(cadModel.Y) + ")", "Ubicar Proyectos Vecinos", this.MTS_VEC.ToString(), this.Location.X + this.Width / 2, this.Location.Y + this.Height / 2);
                if (Fun.isShort(distancia))
                {
                    this.MTS_VEC = distancia;
                   
                    this.Cursor = Cursors.WaitCursor;

                    this.customersBindingSource.DataSource = clientesModel.dt_clientes_vec(cadModel.X, cadModel.Y, distancia, mnuVerInmuebles.Checked);
                    this.tspBindingNavigatorCad.BindingSource = this.customersBindingSource;
                    this.dgvDatos.DataSource = this.customersBindingSource;
                    this.Cursor = Cursors.Default;
                }

            }
            else
            {
                lblObs.Text = cadModel.MENSAJE;

                if (tsbOrden.Checked)
                    this.TopMost = true;
                this.TopLevel = true;
            }

        }

        private void mnuXyMap_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;

            if (cadModel.CoordObtener())           
                cadModel.CoordMap(); 
            else            
                lblObs.Text = cadModel.MENSAJE;              
            
        }

        private void mnuAsociarTx_Click(object sender, EventArgs e)
        {
            if (tsbOrden.Checked)
                this.TopMost = false;
            this.TopLevel = false;

            if (cadModel.BloqueLeer())
            {
                string bloque = cadModel.BLOQUE;

                //abre solo bloques de transformador
                if (bloque.Equals("ECADTR01") || bloque.Equals("ECADCT01") || bloque.Equals("ECADST01") || bloque.Equals("ECADTR03") || bloque.Equals("ECADTR04") || bloque.Equals("ECADTR05"))
                {
                    if (cadModel.CODIGO.Length < 8 || cadModel.CODIGO.Length > 10)
                    {
                        System.Media.SystemSounds.Beep.Play();
                        lblObs.Text = "Verificar Código Geográfico";
                    }
                    else
                    {
                        string serie;                          
                            Int32 nis, nif;
                            int idPoste = 0;

                            for (int i = 0; i < dgvDatos.Rows.Count; i++)
                            {
                                if (Convert.IsDBNull(dgvDatos.Rows[i].Cells["id_poste"].Value))
                                {//registro nuevo de asociación cliente transformador
                                    serie = Convert.ToString(dgvDatos.Rows[i].Cells["serie"].Value);
                                    nis = Convert.ToInt32(dgvDatos.Rows[i].Cells["nis"].Value);
                                    nif = Convert.ToInt32(dgvDatos.Rows[i].Cells["nif"].Value);

                                    idPoste = clientesModel.insert_tbl_postes(serie, nis, nif, cadModel.CODIGO, UserCache.LoginUser);
                                   
                                }
                                else
                                {//Actualización de un registro
                                    idPoste = Convert.ToInt32(dgvDatos.Rows[i].Cells["id_poste"].Value);                                  
                                    clientesModel.update_tbl_postes(idPoste, cadModel.CODIGO, UserCache.LoginUser);
                                   
                                }
                                dgvDatos.Rows[i].Cells["id_poste"].Value = idPoste;
                                dgvDatos.Rows[i].Cells["poste"].Value = cadModel.CODIGO;
                            }
                    }
                }               
                else
                {
                    System.Media.SystemSounds.Beep.Play();
                    lblObs.Text = "Favor seleccionar Bloques del tipo 'Transformador'";
                }
            }
            else
                lblObs.Text = cadModel.MENSAJE;

            if (tsbOrden.Checked)
                this.TopMost = true;
            this.TopLevel = true;
        }

        private void bindingNavigatorPositionItem_TextChanged(object sender, EventArgs e)
        {
            if (!mnuActZoom.Checked)
                return;

            int fila = int.Parse(bindingNavigatorPositionItem.Text) - 1;
            if (fila < 0)
                return;
            if (dgvDatos.Rows[fila].Cells["x"].Value == System.DBNull.Value)
                return;
            double x = Convert.ToDouble(dgvDatos.Rows[fila].Cells["x"].Value);
            double y = Convert.ToDouble(dgvDatos.Rows[fila].Cells["y"].Value);
            if (x.Equals(0))
            {
                lblObs.Text = ConfCache.MSG_XY_NULL;
                return;
            }
            

            Int16 _marcar = 0;
            if (mnuActMarcar.Checked)
                _marcar = cadModel.MARCAR;
            if (!cadModel.CoordZoom(x, y, _marcar))
                lblObs.Text = this.cadModel.MENSAJE;
        }

        private void mnuCtaPoste_Click(object sender, EventArgs e)
        {
            if (!VerificarRegistros())
                return;
            
            cbxFiltro.Text = "poste";
            if (dgvDatos.Rows[dgvDatos.SelectedCells[0].RowIndex].Cells[cbxFiltro.Text].Value == System.DBNull.Value)
            {
                lblObs.Text = "Registro sin Código de POSTE";
                return;
            }
            cbx_ltaVariable.Text = Convert.ToString(dgvDatos.Rows[dgvDatos.SelectedCells[0].RowIndex].Cells[cbxFiltro.Text].Value);
            btnFiltrar.PerformClick();
        }

        

        

        

    }
}
