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
//using Microsoft.VisualBasic;


namespace Presentation.InfGeográfica
{
    public partial class frmLocalidades : Form
    {
        private string MTS_VEC = "1000";

        LocModel locModel = new LocModel();
        CadModel cadModel = new CadModel();

        

        BindingSource customersBindingSource = new BindingSource();

        //private static SortedList formInstances = new SortedList(); // Para guardar las referencias de las instancias de los formularios

#pragma warning disable CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
        public frmLocalidades()
#pragma warning restore CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
        {
            InitializeComponent();
            ListaZona();
            cadModel.CAPA_TEMP = ConfCache.CAD_LAYER_TEM_LOCALIDAD;
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
            //string ficheroXML = Path.Combine(ConfCache.RUTA_XML, nombreForm + ".xml");
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
                if (Convert.ToBoolean(Fun.Instance.GetValor(nodoActual, "VerArbol").ToString()))
                    mnuVerAbrev.PerformClick();

                if (!Convert.ToBoolean(Fun.Instance.GetValor(nodoActual, "mnuVerBarFiltro").ToString()))
                    mnuVerBarFiltro.PerformClick();
                if (!Convert.ToBoolean(Fun.Instance.GetValor(nodoActual, "mnuVerBarCad").ToString()))
                    mnuVerBarCad.PerformClick();
                if (!Convert.ToBoolean(Fun.Instance.GetValor(nodoActual, "mnuVerAbrev").ToString()))
                    mnuVerAbrev.PerformClick();


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

                cadModel.ZOOM = 5;
                cadModel.MARCAR = 25;
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
                Fun.Instance.NodoValor(doc, nodoFormActual, "VerArbol", this.mnuVerAbrev.Checked);
                Fun.Instance.NodoValor(doc, nodoFormActual, "mnuVerBarFiltro", this.mnuVerBarFiltro.Checked);
                Fun.Instance.NodoValor(doc, nodoFormActual, "mnuVerBarCad", this.mnuVerBarCad.Checked);
                Fun.Instance.NodoValor(doc, nodoFormActual, "mnuVerAbrev", this.mnuVerAbrev.Checked);

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

        private void ListaZona()
        {
            cbxFiltro.Items.Clear();
            cbxFiltro.Items.Add("Centro de Servicio");
            cbxFiltro.Items.Add("Municipio");
            cbxFiltro.Items.Add("Parroquia");
            cbxFiltro.Items.Add("Localidad");
            //cbxFiltro.SelectedIndex = 1;
        }

        private void ListaCS()
        {
            cbx_ltaVariable.ComboBox.DataSource = locModel.dt_combobox("esq_proy.lta_cs", 8);
            cbx_ltaVariable.ComboBox.DisplayMember = "nombre";
            cbx_ltaVariable.ComboBox.ValueMember = "id";
        }

        private void ListaMunicipio()
        {
            cbx_ltaVariable.ComboBox.DataSource = locModel.dt_combobox("esq_open.lta_municipios", 8);
            cbx_ltaVariable.ComboBox.DisplayMember = "nombre";
            cbx_ltaVariable.ComboBox.ValueMember = "id";
        }

        private void ListaParroquia()
        {
            cbx_ltaVariable.ComboBox.DataSource = locModel.dt_lista_parroquia(8);
            cbx_ltaVariable.ComboBox.DisplayMember = "nombre";
            cbx_ltaVariable.ComboBox.ValueMember = "id";
        }

        private void ListaFiltro()
        {
            cbx_ltaVariable.ComboBox.DataSource = null;
            cbx_ltaVariable.ComboBox.Items.Clear();

            switch (cbxFiltro.Text)
            {
                case "Centro de Servicio":
                    ListaCS();
                    break;
                case "Municipio":
                    ListaMunicipio();
                    break;
                case "Parroquia":
                    ListaParroquia();
                    break;
                default:
                    break;

            }
            cbx_ltaVariable.SelectedIndex = -1;

        }

        private void cbx_ltaVariable_SelectedIndexChanged(object sender, EventArgs e)
        {
            //MessageBox.Show(cbx_ltaVariable.SelectedIndex.ToString());
            //MessageBox.Show(cbx_ltaVariable.ComboBox.SelectedValue.ToString());
            //MessageBox.Show(cbx_ltaVariable.SelectedItem);
        }

        private void cbxFiltro_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListaFiltro();
        }

        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            if (cbx_ltaVariable.Text == "")
            {
                MessageBox.Show("Favor seleccionar un elemento de la lista", "Notificación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            this.Cursor = Cursors.WaitCursor;
            string _col = null, _var = null;
            string _tipo = "nombre";
            if (LocModel.ABREV)
                _tipo = "abrev";

            switch (cbxFiltro.Text)
            {
                case "Centro de Servicio":
                    _col = "b.id_cs";
                    _var = cbx_ltaVariable.ComboBox.SelectedValue.ToString();
                    break;
                case "Municipio":
                    _col = "c.id";
                    _var = cbx_ltaVariable.ComboBox.SelectedValue.ToString();
                    break;
                case "Parroquia":
                    _col = "b.id";
                    _var = cbx_ltaVariable.ComboBox.SelectedValue.ToString();
                    break;
                case "Localidad":
                    _col = "a.nombre";
                    _var = cbx_ltaVariable.Text;
                    break;
                default:
                    this.Cursor = Cursors.Default;
                    lblObs.Text = "Error aplicando el filtro";
                    return;
            }

            this.customersBindingSource.DataSource = locModel.dt_localidad(_col, _var, _tipo);
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

        //    if (dialog)
        //        formulario.ShowDialog();
        //    else
        //        formulario.Show();

        //    return formulario;
        //}

        private void dgvDatos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            frmLocalidad.ZOOM = cadModel.ZOOM;

            this.TopLevel = false;

            DataGridViewRow dgv_selecc = dgvDatos.Rows[e.RowIndex];

            LocModel.ID = Int16.Parse(dgv_selecc.Cells["id"].Value.ToString());
            LocModel.LOCALIDAD = dgv_selecc.Cells["localidad"].Value.ToString();
            LocModel.TIPO = dgv_selecc.Cells["tipo"].Value.ToString();
            LocModel.MUNICIPIO = dgv_selecc.Cells["municipio"].Value.ToString();
            LocModel.PARROQUIA = dgv_selecc.Cells["parroquia"].Value.ToString();
            LocModel.X = Int32.Parse(dgv_selecc.Cells["x"].Value.ToString());
            LocModel.Y = Int32.Parse(dgv_selecc.Cells["y"].Value.ToString());
            LocModel.ESCALA = Single.Parse(dgv_selecc.Cells["escala"].Value.ToString());
            LocModel.AREA = Int32.Parse(dgv_selecc.Cells["área"].Value.ToString());
            LocModel.CLIENTES = Int16.Parse(dgv_selecc.Cells["clientes"].Value.ToString());
            LocModel.ACTUALIZACION = System.Convert.ToDateTime(dgv_selecc.Cells["actualización"].Value);//System.Convert.ToDateTime(
            LocModel.USUARIO = dgv_selecc.Cells["usuario"].Value.ToString();

            frmLocalidad f_localida = (frmLocalidad)Fun.AbrirFormulario(typeof(frmLocalidad), false);

            //this.TopLevel = true;
            if (frmLocalidad.B_GUARDO)
            {
                dgv_selecc.Cells["área"].Value = LocModel.AREA;
                dgv_selecc.Cells["x"].Value = LocModel.X;
                dgv_selecc.Cells["y"].Value = LocModel.Y;
                dgv_selecc.Cells["escala"].Value = LocModel.ESCALA;
                dgv_selecc.Cells["área"].Value = LocModel.AREA;
                dgv_selecc.Cells["usuario"].Value = LocModel.USUARIO;
                dgv_selecc.Cells["actualización"].Value = LocModel.ACTUALIZACION;
            }
            //this.Owner.TopLevel = true;

            dgvDatos.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.DarkSlateGray;

            f_localida.FormClosed += Logout;

            if (mnuActZoom.Checked)
                mnuCadMostrar.PerformClick();

            if (mnuActMarcar.Checked)
                mnuCadMarcar.PerformClick();

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
            tspBusqueda.Visible = mnuVerBarFiltro.Checked;
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
            this.Size = new Size(1000, Screen.PrimaryScreen.WorkingArea.Size.Height * 2 / 5);
            //this.Size = new Size(Screen.PrimaryScreen.WorkingArea.Size.Width, Screen.PrimaryScreen.WorkingArea.Size.Height / 3);

        }

        private void rbn1x_Click(object sender, EventArgs e)
        {
            tsbCadMostrar.Text = rbn1x.Text;
            cadModel.ZOOM = 1 * 5;
        }
        private void rbn2x_Click(object sender, EventArgs e)
        {
            tsbCadMostrar.Text = rbn2x.Text;
            cadModel.ZOOM = 2 * 5;
        }
        private void rbn3x_Click(object sender, EventArgs e)
        {
            tsbCadMostrar.Text = rbn3x.Text;
            cadModel.ZOOM = 3 * 5;
        }
        private void rbn4x_Click(object sender, EventArgs e)
        {
            tsbCadMostrar.Text = rbn4x.Text;
            cadModel.ZOOM = 4 * 5;
        }

        private void rbn5m_Click(object sender, EventArgs e)
        {
            tsbCadMarcar.Text = rbn5m.Text;
            cadModel.MARCAR = 5 * 5;
        }
        private void rbn10m_Click(object sender, EventArgs e)
        {
            tsbCadMarcar.Text = rbn10m.Text;
            cadModel.MARCAR = 10 * 5;
        }
        private void rbn15m_Click(object sender, EventArgs e)
        {
            tsbCadMarcar.Text = rbn15m.Text;
            cadModel.MARCAR = 15 * 5;
        }
        private void rbn20m_Click(object sender, EventArgs e)
        {
            tsbCadMarcar.Text = rbn0m.Text;
            cadModel.MARCAR = 20 * 5;
        }

        private void mnuCadMostrar_Click(object sender, EventArgs e)
        {

            int fila = int.Parse(bindingNavigatorPositionItem.Text) - 1;
            if (fila.Equals(-1))
            {
                lblObs.Text = ConfCache.MSG_ROW_NULL;
                return;
            }
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

            double x = Convert.ToDouble(dgvDatos.Rows[fila].Cells["x"].Value);
            double y = Convert.ToDouble(dgvDatos.Rows[fila].Cells["y"].Value);
            if (x.Equals(0))
            {
                lblObs.Text = "el registro seleccionado no tiene coordenas UTM (X,Y)";
                return;
            }

            if (!cadModel.CoordMarcar(x, y))
                lblObs.Text = "La Localidad no se marco en plano Cad ";
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
                x = Convert.ToDouble(dgvDatos.Rows[i].Cells["x"].Value);
                y = Convert.ToDouble(dgvDatos.Rows[i].Cells["y"].Value);
                if (!x.Equals(0))
                    cadModel.CoordMarcar(x, y);
                else
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

            double x = Convert.ToDouble(dgvDatos.Rows[fila].Cells["x"].Value);
            double y = Convert.ToDouble(dgvDatos.Rows[fila].Cells["y"].Value);
            if (x.Equals(0))
            {
                lblObs.Text = ConfCache.MSG_XY_NULL;
                return;
            }

            string S_bloque = "vsg_zona";

            cadModel.GRADOS = 0;
            cadModel.X = x;
            cadModel.Y = y;

            cadModel.ESC = System.Convert.ToDouble(dgvDatos.Rows[fila].Cells["escala"].Value);
            cadModel.BLOQUE = S_bloque;
            cadModel.CODIGO = System.Convert.ToString(dgvDatos.Rows[fila].Cells["id"].Value);
            cadModel.CAPACIDAD = System.Convert.ToString(dgvDatos.Rows[fila].Cells["tipo"].Value);
            cadModel.SIVPLUS = System.Convert.ToString(dgvDatos.Rows[fila].Cells["localidad"].Value);
            if (!cadModel.BloqueEscribir())
                lblObs.Text = ConfCache.MSG_CAD_ER_DIBUJAR;

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

            string S_bloque = "vsg_zona";
            cadModel.BLOQUE = S_bloque;
            cadModel.GRADOS = 0;
            Int16 fsc = 0;
            double x, y;
            for (int i = 0; i < n_filas; i++)
            {
                x = Convert.ToDouble(dgvDatos.Rows[i].Cells["x"].Value);
                y = Convert.ToDouble(dgvDatos.Rows[i].Cells["y"].Value);

                if (!x.Equals(0))
                {
                    cadModel.X = x;
                    cadModel.Y = y;
                    cadModel.ESC = System.Convert.ToDouble(dgvDatos.Rows[i].Cells["escala"].Value);
                    cadModel.CODIGO = System.Convert.ToString(dgvDatos.Rows[i].Cells["id"].Value);
                    cadModel.CAPACIDAD = System.Convert.ToString(dgvDatos.Rows[i].Cells["tipo"].Value);
                    cadModel.SIVPLUS = System.Convert.ToString(dgvDatos.Rows[i].Cells["localidad"].Value);
                    if (!cadModel.BloqueEscribir())
                        lblObs.Text = ConfCache.MSG_CAD_ER_DIBUJAR;
                }
                else
                    fsc += 1;
            }

            lblObs.Text = String.Format(ConfCache.MSG_CAD_ER_DIBUJAR_CANT, fsc);
            this.Cursor = Cursors.Default;


        }

        private void btnZoom_Click(object sender, EventArgs e)
        {
            Int16 _marcar = 0;
            if (mnuCadMarcar.Checked)
                _marcar = cadModel.MARCAR;

            if (cadModel.ZoomGeo(txtCodigo.Text, _marcar))
            {
                if (mnuOrden.Checked == false)
                    this.WindowState = FormWindowState.Minimized;
            }
            lblObs.Text = cadModel.MENSAJE;
        }

        private void mnuVerAbrev_Click(object sender, EventArgs e)
        {
            mnuVerAbrev.Checked = !mnuVerAbrev.Checked;
            LocModel.ABREV = mnuVerAbrev.Checked;
            //if (mnuVerAbrev.Checked == true)
            //{
            //    LocModel.ABREV = true;
            //    //mnuVerAbrev.Checked = false;                
            //}
            //else
            //{
            //    LocModel.ABREV = false;
            //    //mnuVerAbrev.Checked = true;              
            //}
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            mnuIdEditar.PerformClick();

        }

        private void Logout(object sender, FormClosedEventArgs e)
        {
            //txtpass.Text = "CONTRASEÑA";
            //txtpass.UseSystemPasswordChar = false;
            //txtuser.Text = "USUARIO";
            //lblErrorMessage.Visible = false;

            this.TopLevel = true;
            //this.Show();
            //this.BringToFront();

            //this.Close();
        }

        private void tsbOrden_Click(object sender, EventArgs e)
        {
            mnuOrden.PerformClick();          
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
            mnuIdEditarCad.PerformClick();


        }

        private void mnuBuscar_Click(object sender, EventArgs e)
        {
            if (!Fun.isShort(txtId.Text))
            {
                lblObs.Text = "Favor incorporar un valor ID";
                return;
            }

            string _tipo = "nombre";
            if (LocModel.ABREV)
                _tipo = "abrev";

            DataTable dt_fila = locModel.dt_localidad("a.id", txtId.Text, _tipo);
            if (dt_fila.Rows.Count.Equals(0))
            {
                lblObs.Text = "Id fuera de rango";
                return;
            }
            //this.Hide();

            frmLocalidad.ZOOM = cadModel.ZOOM;

            this.TopLevel = false;

            LocModel.ID = Int16.Parse(txtId.Text);
            LocModel.LOCALIDAD = dt_fila.Rows[0]["localidad"].ToString();
            LocModel.TIPO = dt_fila.Rows[0]["tipo"].ToString();
            LocModel.MUNICIPIO = dt_fila.Rows[0]["municipio"].ToString();
            LocModel.PARROQUIA = dt_fila.Rows[0]["parroquia"].ToString();
            LocModel.X = Int32.Parse(dt_fila.Rows[0]["x"].ToString());
            LocModel.Y = Int32.Parse(dt_fila.Rows[0]["y"].ToString());
            LocModel.ESCALA = Single.Parse(dt_fila.Rows[0]["escala"].ToString());
            LocModel.AREA = Int32.Parse(dt_fila.Rows[0]["área"].ToString());
            LocModel.CLIENTES = Int16.Parse(dt_fila.Rows[0]["clientes"].ToString());
            LocModel.ACTUALIZACION = System.Convert.ToDateTime(dt_fila.Rows[0]["actualización"]);//System.Convert.ToDateTime(
            LocModel.USUARIO = dt_fila.Rows[0]["usuario"].ToString();

            frmLocalidad f_localidad = (frmLocalidad)Fun.AbrirFormulario(typeof(frmLocalidad), true);

            //f_localidad.FormClosed += Logout;
            this.TopLevel = true;
        }

        private void mnuBuscarCad_Click(object sender, EventArgs e)
        {
            if (tsbOrden.Checked)
                this.TopMost = false;
            this.TopLevel = false;

            if (cadModel.BloqueLeer())
            {
                if (cadModel.BLOQUE.Equals("vsg_zona"))
                {
                    if (Fun.isShort(cadModel.CODIGO))
                    {
                        txtId.Text = cadModel.CODIGO;
                        mnuIdEditar.PerformClick();

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
                    lblObs.Text = "Favor seleccionar Bloque tipo: 'vsg_zona.dwg'";
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
                //this.BringToFront();

                string distancia = Microsoft.VisualBasic.Interaction.InputBox("Favor Ingrese la distancia en metros\nseparados de las coordenadas (" + Convert.ToInt32(cadModel.X) + " , " + Convert.ToInt32(cadModel.Y) + ")", "Ubicar Proyectos Vecinos", this.MTS_VEC.ToString(), this.Location.X + this.Width / 2, this.Location.Y + this.Height / 2);
                if (Fun.isShort(distancia))
                {
                    this.MTS_VEC = distancia;
                    string _tipo = "nombre";
                    if (LocModel.ABREV)
                        _tipo = "abrev";
                    this.Cursor = Cursors.WaitCursor;

                    this.customersBindingSource.DataSource = locModel.dt_localidad_vec(cadModel.X, cadModel.Y, distancia, _tipo);
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

        private void bindingNavigatorPositionItem_TextChanged(object sender, EventArgs e)
        {
            if (!mnuActZoom.Checked)
                return; 
           
                int fila = int.Parse(bindingNavigatorPositionItem.Text) - 1;
                if (fila<0)                
                    return;                

                double x = Convert.ToDouble(dgvDatos.Rows[fila].Cells["x"].Value);
                double y = Convert.ToDouble(dgvDatos.Rows[fila].Cells["y"].Value);
                if (x.Equals(0))
                {
                    lblObs.Text = ConfCache.MSG_XY_NULL;
                    return;
                }                            
                    //if (dgvDatos.Rows[fila].Cells["x"].Value != System.DBNull.Value)
                    
                    Int16 _marcar = 0;
                    if (mnuActMarcar.Checked)
                        _marcar = cadModel.MARCAR;
                    if (!cadModel.CoordZoom(x, y, _marcar))                                            
                        lblObs.Text = this.cadModel.MENSAJE;
           
        }

        private void mnuXyMap_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;

            if (cadModel.CoordObtener())
                cadModel.CoordMap();
            else
                lblObs.Text = cadModel.MENSAJE;    
        }

        private void mnuCoorDgsUtm_Click(object sender, EventArgs e)
        {
            try
            {
                string txt_dgs = Microsoft.VisualBasic.Interaction.InputBox("Favor Ingrese las coordenadas en formato Digital", "Conversor de unidades DGS a UTM", this.MTS_VEC.ToString(), this.Location.X + this.Width / 2, this.Location.Y + this.Height / 2);
                char[] delimiterChars = { ',' };
                string[] elementos;

                elementos = txt_dgs.Split(delimiterChars);

                double lati = Convert.ToDouble(elementos[0]);
                double longi = Convert.ToDouble(elementos[1]);
                double mapX; double mapY;

                cadModel.CambioDG_UTM(ref lati, ref longi, out mapX, out mapY);

                string txt_utm = Microsoft.VisualBasic.Interaction.InputBox("Coordenadas en formato UTM", "Conversor de unidades DGS a UTM", mapX + "," + mapY, this.Location.X + this.Width / 2, this.Location.Y + this.Height / 2);
            }
            catch (Exception f)
            {
                MessageBox.Show("Error: " + f.Message);
            }
        }

       





    }
}
