using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Domain;
using Presentation.Util;
using System.Globalization;
using System.IO;
using Common.Cache;
using System.Xml;

namespace Presentation.InfGeográfica
{
#pragma warning disable CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
    public partial class frmListas : Form
#pragma warning restore CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
    {
        private string MTS_VEC = "100";
        CtoModel ctoModel = new CtoModel();
        CadModel cadModel = new CadModel();

        BindingSource customersBindingSource = new BindingSource();

#pragma warning disable CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
        public frmListas()
#pragma warning restore CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
        {
            InitializeComponent();
            ListaCtos();

        }

        private void frmListas_Load(object sender, EventArgs e)
        {
            ConfiguracionFormLectura();
        }

        private void frmListas_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Fun.FormularioCerrar(typeof(frmSub));
            //Fun.FormularioCerrar(typeof(frmTransf));
            //Fun.FormularioCerrar(typeof(frmCto));
            ConfiguracionFormGuarda();
        }

        private void mnuCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void mnuSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
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
                

                if (!Convert.ToBoolean(Fun.Instance.GetValor(nodoActual, "mnuVerBarFiltro").ToString()))
                    mnuVerBarFiltro.PerformClick();
                if (!Convert.ToBoolean(Fun.Instance.GetValor(nodoActual, "mnuVerBarCad").ToString()))
                    mnuVerBarCad.PerformClick();

                if (!Convert.ToBoolean(Fun.Instance.GetValor(nodoActual, "mnuInterOperativos").ToString()))
                    mnuInterOperativos.PerformClick();
                
                if (!Convert.ToBoolean(Fun.Instance.GetValor(nodoActual, "mnuSalaOper").ToString()))
                    mnuSalaOper.PerformClick();
                if (!Convert.ToBoolean(Fun.Instance.GetValor(nodoActual, "mnuFecInter").ToString()))
                    mnuFecIncidencia.PerformClick();


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
                
                Fun.Instance.NodoValor(doc, nodoFormActual, "mnuVerBarFiltro", this.mnuVerBarFiltro.Checked);
                Fun.Instance.NodoValor(doc, nodoFormActual, "mnuVerBarCad", this.mnuVerBarCad.Checked);
                Fun.Instance.NodoValor(doc, nodoFormActual, "mnuInterOperativos", this.mnuInterOperativos.Checked);
                Fun.Instance.NodoValor(doc, nodoFormActual, "mnuSalaOper", this.mnuSalaOper.Checked);
                Fun.Instance.NodoValor(doc, nodoFormActual, "mnuFecInter", this.mnuFecIncidencia.Checked);
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


        private void ListaCtos()
        {
            cbxFiltro.Items.Clear();

            cbxFiltro.Items.Add("Centro de Servicio");
            cbxFiltro.Items.Add("Sala");
            cbxFiltro.Items.Add("Subestación");
            cbxFiltro.Items.Add("Tensión (kV)");
            cbxFiltro.Items.Add("Grupo");
            cbxFiltro.Items.Add("Grupos");
            cbxFiltro.Items.Add("Condición Interruptor");
            cbxFiltro.Items.Add("Horario");
            //cbxFiltro.Items.Add("kW Tot");
            //cbxFiltro.SelectedIndex = 3;
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
                case "Sala":
                    ListaSala();                   
                    break;
                case "Subestación":
                    ListaSE();
                    break;
                case "Tensión (kV)":
                    ListakV();
                    break;
                case "Grupo":
                    ListaGrupo();                  
                    break;
                case "Grupos":
                    ListaGrupos();
                    break;
                case "Condición Interruptor":
                    ListaCondInter();
                    break;
                case "Horario":
                    ListaHorario();
                    break;
                //case "Eje":
                //    ListaParroquia();                  
                //    break;
                default:
                    //this.cbxFiltro.SelectedIndexChanged -= cbxFiltro_SelectedIndexChanged;
                    break;

            }
            cbx_ltaVariable.SelectedIndex = -1;

        }

        private void ListaCS()
        {            
            cbx_ltaVariable.ComboBox.DataSource = ctoModel.dt_combobox("esq_proy.lta_cs", 8);
            cbx_ltaVariable.ComboBox.DisplayMember = "nombre";
            cbx_ltaVariable.ComboBox.ValueMember = "id";
        }

        private void ListaSala()
        {
            cbx_ltaVariable.ComboBox.DataSource = ctoModel.dt_combobox("esq_red.lta_sala", 8);
            cbx_ltaVariable.ComboBox.DisplayMember = "nombre";
            cbx_ltaVariable.ComboBox.ValueMember = "id";
        }

        private void ListaSE()
        {
            cbx_ltaVariable.ComboBox.DataSource = ctoModel.dt_combobox("esq_red.tbl_sub", 0);
            cbx_ltaVariable.ComboBox.DisplayMember = "nombre";
            cbx_ltaVariable.ComboBox.ValueMember = "id";
        }

        private void ListakV()
        {
            cbx_ltaVariable.ComboBox.DataSource = ctoModel.dt_combobox("esq_red.lta_kv", 0);
            cbx_ltaVariable.ComboBox.DisplayMember = "nombre";
            cbx_ltaVariable.ComboBox.ValueMember = "id";
        }

        private void ListaCondInter()
        {
            cbx_ltaVariable.ComboBox.DataSource = ctoModel.dt_combobox("esq_red.lta_cond_inter", 0);
            cbx_ltaVariable.ComboBox.DisplayMember = "nombre";
            cbx_ltaVariable.ComboBox.ValueMember = "id";
        }
        private void ListaGrupo()
        {
            cbx_ltaVariable.ComboBox.Items.Clear();
            cbx_ltaVariable.ComboBox.Text = "";
            //cbx_ltaVariable.ComboBox.DataSource = ctoModel.dt_combobox("esq_red.lta_kv", 0);
            //cbx_ltaVariable.ComboBox.DisplayMember = "nombre";
            //cbx_ltaVariable.ComboBox.ValueMember = "id";
        }
        private void ListaGrupos()
        {
            cbx_ltaVariable.ComboBox.Items.Clear();

            cbx_ltaVariable.ComboBox.Items.Add(">0");
            cbx_ltaVariable.ComboBox.Items.Add("=0");
            cbx_ltaVariable.ComboBox.Items.Add("<18");

            cbx_ltaVariable.ComboBox.SelectedIndex = 0;
            //cbx_ltaVariable.ComboBox.Items.Add(">0 AND a.id_grupo<5");
            //cbx_ltaVariable.ComboBox.DataSource = ctoModel.dt_combobox("esq_red.lta_kv", 0);
            //cbx_ltaVariable.ComboBox.DisplayMember = "nombre";
            //cbx_ltaVariable.ComboBox.ValueMember = "id";
        }
        private void ListaHorario()
        {
            cbx_ltaVariable.ComboBox.Items.Clear();

            cbx_ltaVariable.ComboBox.Items.Add("D");
            cbx_ltaVariable.ComboBox.Items.Add("N");
            cbx_ltaVariable.ComboBox.Items.Add("O");
            cbx_ltaVariable.ComboBox.Items.Add("X");
            cbx_ltaVariable.ComboBox.Items.Add("F");
            cbx_ltaVariable.ComboBox.Items.Add("I");

        }
        //private void ListakW()
        //{
        //    cbx_ltaVariable.ComboBox.Items.Clear();

        //    cbx_ltaVariable.ComboBox.Items.Add("<36");
        //    cbx_ltaVariable.ComboBox.Items.Add("<72");
        //    cbx_ltaVariable.ComboBox.Items.Add("<108");
        //    cbx_ltaVariable.ComboBox.Items.Add("<144");
        //    //cbx_ltaVariable.ComboBox.DataSource = ctoModel.dt_combobox("esq_red.lta_kv", 0);
        //    //cbx_ltaVariable.ComboBox.DisplayMember = "nombre";
        //    //cbx_ltaVariable.ComboBox.ValueMember = "id";
        //}
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

            
            string _col = null, _var = null;          
          
            switch (cbxFiltro.Text)
            {
                case "Centro de Servicio":
                    _col = "a.id_cs";
                    _var = cbx_ltaVariable.ComboBox.SelectedValue.ToString();
                    break;
                case "Sala":
                    _col = "a.id_sala";
                    _var = cbx_ltaVariable.ComboBox.SelectedValue.ToString();
                    break;
                case "Subestación":
                    _col = "c.id";
                    _var = cbx_ltaVariable.ComboBox.SelectedValue.ToString();
                    break;
                case "Tensión (kV)":
                    _col = "b.kv_baja";
                    _var = cbx_ltaVariable.Text;
                    break;
                case "Condición Interruptor":
                    _col = "a.id_cond_inter";
                    _var = cbx_ltaVariable.ComboBox.SelectedValue.ToString();
                    break;
                case "Grupo":
                    _col = "a.id_grupo";
                    _var = cbx_ltaVariable.Text;
                    break;               
                case "Grupos":
                    _col = "Grupos";
                    _var = cbx_ltaVariable.Text;
                    if (!verif_grupos(_var))
                        return;
                    break;
                case "Horario":
                    _col = "a.horario";
                    _var = cbx_ltaVariable.Text;
                    break;
                default:
                    this.Cursor = Cursors.Default;
                    lblObs.Text = "Error aplicando el filtro";
                    return;
            }

            this.Cursor = Cursors.WaitCursor;
            this.customersBindingSource.DataSource = ctoModel.dt_ctos(_col, _var, mnuInterOperativos.Checked, mnuSalaOper.Checked, mnuFecIncidencia.Checked);
            this.tspBindingNavigatorCad.BindingSource = this.customersBindingSource;
            this.dgvDatos.DataSource = this.customersBindingSource;

            SumaKW();

            dgvDatos.Columns["x"].Visible = false;
            dgvDatos.Columns["y"].Visible = false;
            dgvDatos.Columns["pvar"].Visible = false;

            this.Cursor = Cursors.Default;

        }
        private bool verif_grupos(string variable)
        {

            if (variable.Substring(0, 1) == "=" && variable.Length > 1 && Fun.isShort(variable.Substring(1, variable.Length - 1)))
                return true;
            if (variable.Substring(0, 1) == ">" && variable.Length > 1 && Fun.isShort(variable.Substring(1, variable.Length - 1)))
                return true;
            if (variable.Substring(0, 1) == "<" && variable.Length > 1 && Fun.isShort(variable.Substring(1, variable.Length - 1)))
                return true;
             
                 //if (Fun.isShort(variable.Substring(1, variable.Length - 1)))
                 //    return true;
             //MessageBox.Show("Formato permitidos:\n =0\n>2000\n<100", "Error en variable", MessageBoxButtons.OK, MessageBoxIcon.Information);
                 System.Media.SystemSounds.Beep.Play();
             lblObs.Text = "Error de selección: falta incluir el símbolo aritmético: =,>,<";
             return false;

        }

        private void SumaKW()
        {
            NumberFormatInfo provider = new NumberFormatInfo();
            provider.NumberDecimalSeparator = ".";

            int n_filas = dgvDatos.Rows.Count;
            double sum1 = 0, sum2 = 0, frh = double.Parse(txtFrh.Text.Replace(",", "."), provider);
            if (!Fun.IsNumeric(frh.ToString()))
            {
                lblObs.Text = "Favor de Factor R.H. incorrecto";
                System.Media.SystemSounds.Beep.Play();
                return;
            }



            for (int i = 0; i < n_filas; i++)
            {
                dgvDatos.Rows[i].Cells["MWmax"].Value = frh * double.Parse(dgvDatos.Rows[i].Cells["MWmax"].Value.ToString());
                dgvDatos.Rows[i].Cells["MWinc"].Value = frh *double.Parse(dgvDatos.Rows[i].Cells["MWinc"].Value.ToString());
                sum1 += double.Parse(Convert.ToString(dgvDatos.Rows[i].Cells["MWmax"].Value).Replace(",", "."), provider);
                sum2 += double.Parse(Convert.ToString(dgvDatos.Rows[i].Cells["MWinc"].Value).Replace(",", "."), provider);   
            }

            lblObs.Text = String.Format("kW Máx = {0} / kW Inc = {1}", sum1, sum2);

        }
        private void dgvDatos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // abrir en panel si es posible

            DataGridViewRow dgv_selecc = dgvDatos.Rows[e.RowIndex];           

            frmCto.ZOOM = 5;
            CtoModel.ID = Int16.Parse(dgv_selecc.Cells["id"].Value.ToString());
            
            // apertura manejada por UIHelpers; el cierre se gestiona si procede
            dgvDatos.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.DarkSlateGray;

        }

        private void Logout(object sender, FormClosedEventArgs e)
        {
            this.TopLevel = true;
        }

        private void mnuInterOperativos_Click(object sender, EventArgs e)
        {
            mnuInterOperativos.Checked = !mnuInterOperativos.Checked;
        }

        private void mnuExcluirTerceros_Click(object sender, EventArgs e)
        {
            mnuSalaOper.Checked = !mnuSalaOper.Checked;
        }

        private void mnuFecInc_Click(object sender, EventArgs e)
        {
            mnuFecIncidencia.Checked = !mnuFecIncidencia.Checked;
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            mnuIdEditar.PerformClick();
        }

        private void mnuIdEditar_Click(object sender, EventArgs e)
        {
            if (!Fun.isShort(txtId.Text))
            {
                lblObs.Text = "Favor incorporar un valor ID";
                return;
            }

            DataTable dt_fila = ctoModel.dt_cto(txtId.Text);
            if (dt_fila.Rows.Count.Equals(0))
            {
                lblObs.Text = "Id fuera de rango";
                return;
            }

            frmCto.ZOOM = 5;
            CtoModel.ID = Int16.Parse(txtId.Text);

            frmCto f_cto = (frmCto)Fun.AbrirFormulario(typeof(frmCto), true);
                        
            this.TopLevel = true;

        }

        private void mnuIdEditarCad_Click(object sender, EventArgs e)
        {

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

        private void mnuOrden_Click(object sender, EventArgs e)
        {
            mnuOrden.Checked = !mnuOrden.Checked;
            tsbOrden.Checked = mnuOrden.Checked;
            this.TopMost = tsbOrden.Checked;
        }

        private void mnuAjustarV_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal; ;
            this.Location = new Point(0, 0);
            this.Size = new Size(Screen.PrimaryScreen.WorkingArea.Size.Width, Screen.PrimaryScreen.WorkingArea.Size.Height * 2 / 5);
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

        private void mnuActContinuo_Click(object sender, EventArgs e)
        {
            mnuActContinuo.Checked = !mnuActContinuo.Checked;

            if (mnuActContinuo.Checked)
                this.lblObs.Text = "Utilizar ESC para cancelar en AutoCad";
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

        private void mnuCadLayerErase_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            if (!cadModel.LayerBorrar())
                lblObs.Text = cadModel.MENSAJE;

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

        private void mnuEsc0_6_Click(object sender, EventArgs e)
        {
            tsbCadEsc.Text = mnuEsc0_6.Text;
        }

        private void mnuEsc0_8_Click(object sender, EventArgs e)
        {
            tsbCadEsc.Text = mnuEsc0_8.Text;
        }

        private void mnuEsc1_Click(object sender, EventArgs e)
        {
            tsbCadEsc.Text = mnuEsc1.Text;
        }

        private void mnuEsc1_2_Click(object sender, EventArgs e)
        {
            tsbCadEsc.Text = mnuEsc1_2.Text;
        }

        private void mnuCadVecinos_Click(object sender, EventArgs e)
        {
            //if (tsbOrden.Checked)
            //    this.TopMost = false;
            //this.TopLevel = false;

            //if (cadModel.CoordObtener())
            //{
            //    if (tsbOrden.Checked == true)
            //        this.TopMost = true;
            //    this.TopLevel = true;
            //    //this.BringToFront();

            //    string distancia = Microsoft.VisualBasic.Interaction.InputBox("Favor Ingrese la distancia en metros\nseparados de las coordenadas (" + Convert.ToInt32(cadModel.X) + " , " + Convert.ToInt32(cadModel.Y) + ")", "Ubicar Proyectos Vecinos", this.MTS_VEC.ToString(), this.Location.X + this.Width / 2, this.Location.Y + this.Height / 2);
            //    if (Fun.isShort(distancia))
            //    {
            //        this.MTS_VEC = distancia;
            //        string _tipo = "nombre";
            //        if (LocModel.ABREV)
            //            _tipo = "abrev";
            //        this.Cursor = Cursors.WaitCursor;

            //        this.customersBindingSource.DataSource = ctoModel.dt_localidad_vec(cadModel.X, cadModel.Y, distancia, _tipo);
            //        this.tspBindingNavigatorCad.BindingSource = this.customersBindingSource;
            //        this.dgvDatos.DataSource = this.customersBindingSource;
            //        this.Cursor = Cursors.Default;
            //    }

            //}
            //else
            //{
            //    lblObs.Text = cadModel.MENSAJE;

            //    if (tsbOrden.Checked)
            //        this.TopMost = true;
            //    this.TopLevel = true;
            //}
        }

        private void mnuActCtoColor_Click(object sender, EventArgs e)
        {
            // Create a dialog to ask the user for the full path of a file.
            OpenFileDialog dlg = new OpenFileDialog();

            //configuracion  de algunos parametros del openFileDialog
            // directorio inicial donde se abrira
            //dlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            dlg.InitialDirectory = @"C:\Sid\Información Geográfica\DATOS\";
            // filtro de archivos.
            dlg.Filter = "Archivos de texto (*.CSV)|*.CSV";

            // codigo para abrir el cuadro de dialogo
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    this.Cursor = Cursors.WaitCursor;
                    string str_RutaArchivo = dlg.FileName;
                    FileStream fs = new FileStream(str_RutaArchivo, FileMode.Open, FileAccess.Read, FileShare.Read);
                    StreamReader sr = new StreamReader(fs);

                    string linea = sr.ReadLine();
                    linea = sr.ReadLine();
                    char[] delimiterChars = { ',' };
                    string[] elementos;
                    short n_padee,id_color;
                    while (linea != null)
                    {
                        elementos = linea.Split(delimiterChars);

                        n_padee =short.Parse(elementos[0]);
                        id_color = short.Parse(elementos[9]);

                        ctoModel.update_tbl_cto_color(n_padee, id_color);
                        linea = sr.ReadLine();
                    }

                    fs.Close();
                    sr.Close();
                    this.Cursor = Cursors.Default;
                    MessageBox.Show("Datos Guardatos con Éxitos", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        private void btnFrh_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            SumaKW();
            this.Cursor = Cursors.Default;
        }

       

        
    }
}
