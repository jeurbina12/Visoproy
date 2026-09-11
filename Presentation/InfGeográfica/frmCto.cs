using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Presentation.Util;
using Domain;
using System.Globalization;
using Common.Cache;
using Microsoft.VisualBasic;
using System.IO;

namespace Presentation.InfGeográfica
{
    public partial class frmCto : Form
    {
        private DateTime originalValue;
        private DateTime originalValue2;
        internal static short ZOOM;
        internal static bool ONPANEL;

        CtoModel ctoModel = new CtoModel();
        CadModel cadModel = new CadModel();

#pragma warning disable CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
        public frmCto()
#pragma warning restore CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
        {
            InitializeComponent();
            listas_combobox();
            cadModel.ZOOM = ZOOM;

        }

        private void frmCto_Load(object sender, EventArgs e)
        {
            Cargar(CtoModel.ID.ToString());
        }

#pragma warning disable CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
        public void Cargar(string id_cto)
#pragma warning restore CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
        {
            System.Data.DataTable dt_fila = ctoModel.dt_cto(id_cto);
            if (dt_fila.Rows.Count.Equals(0))
            {
                MessageBox.Show("Id fuera de rango", "Notificación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            txtId.Text = id_cto;
            txtNombre.Text = dt_fila.Rows[0]["cto"].ToString() + " - " + dt_fila.Rows[0]["transformador"].ToString().Substring(3) + " - S/E " + dt_fila.Rows[0]["subestacion"].ToString();

            txtSalida.Text = dt_fila.Rows[0]["s_interruptor"].ToString();
            txtPadee.Text = dt_fila.Rows[0]["n_padee"].ToString();
            txtColor.Text = dt_fila.Rows[0]["id_color"].ToString();
            txtX.Text = Convert.ToString(dt_fila.Rows[0]["x"]);
            txtY.Text = Convert.ToString(dt_fila.Rows[0]["y"]);
            txtFecAct.Text = Convert.ToDateTime(dt_fila.Rows[0]["f_act"]).ToString("dd-MM-yy HH:mm");
            txtUsuario.Text = dt_fila.Rows[0]["usuario"].ToString();

            cbxCs.SelectedValue = dt_fila.Rows[0]["id_cs"];
            cbxSala.SelectedValue = dt_fila.Rows[0]["id_sala"];
            cbxCondición.SelectedValue = dt_fila.Rows[0]["id_cond_inter"];

            cbxRed.Text = dt_fila.Rows[0]["t_red"].ToString();
            cbxInterruptor.Text = dt_fila.Rows[0]["t_interruptor"].ToString();
            txtCalibre.Text = dt_fila.Rows[0]["calibre_troncal"].ToString();
            txtTroncal.Text = dt_fila.Rows[0]["mts_troncal"].ToString();
            txtMts.Text = dt_fila.Rows[0]["mts_total"].ToString();
            txtkV.Text = Math.Round(Convert.ToDouble(dt_fila.Rows[0]["kv"]), 2).ToString();
            txtSuscriptores.Text = dt_fila.Rows[0]["suscriptores"].ToString();

            txtCarga.Text = dt_fila.Rows[0]["t_carga"].ToString();
            txtkVAInst.Text = dt_fila.Rows[0]["kva_inst"].ToString();
            txtPtos.Text = dt_fila.Rows[0]["n_ptos"].ToString();
            txtNTransf.Text = dt_fila.Rows[0]["n_transf"].ToString();

            txtAmp.Text = dt_fila.Rows[0]["amp_max"].ToString();
            txtFp.Text = dt_fila.Rows[0]["fp"].ToString();
            txtCt.Text = dt_fila.Rows[0]["cv"].ToString();
            txtCc.Text = dt_fila.Rows[0]["cc"].ToString();
            //txtMVACto.Text = dt_fila.Rows[0]["mva_max"].ToString().TrimStart();
            //txtFd.Text = dt_fila.Rows[0][""].ToString();
            originalValue = Convert.ToDateTime(dt_fila.Rows[0]["f_max"]);
            dtpFechaMax.Value = originalValue;

            cbxCondScada.SelectedValue = dt_fila.Rows[0]["id_cond_scada"];

            chkScada.Checked = (Boolean)dt_fila.Rows[0]["b_p_scada"];
            chkMedicion.Checked = (Boolean)dt_fila.Rows[0]["b_p_medicion"];

            numGrupo.Value = (short)dt_fila.Rows[0]["id_grupo"];
            txtHorario.Text = dt_fila.Rows[0]["horario"].ToString();
            txtAmpAdm.Text = dt_fila.Rows[0]["amp_adm"].ToString();
            //txtMwP.Text = dt_fila.Rows[0]["mw_adm"].ToString();
            originalValue2 = Convert.ToDateTime(dt_fila.Rows[0]["f_adm"]);
            dtpFechaAdm.Value = originalValue2;
            chkNoAdm.Checked = (Boolean)dt_fila.Rows[0]["b_no_adm"];

            txtObsCto.Text = dt_fila.Rows[0]["obs_cto"].ToString();
            txtSectores.Text = dt_fila.Rows[0]["sectores"].ToString();
            txtInstituciones.Text = dt_fila.Rows[0]["obs_esp"].ToString();


            if (txtX.Text == "0")
                tsbCadMostrar.Enabled = false;
            else
                tsbCadMostrar.Enabled = true;

            //btnCalcular.PerformClick();

            calcular_indices();
        }

        private void listas_combobox()
        {



            //cbxColor.Items.Add(Color.Black);
            //cbxColor.Items.Add(Color.Blue);
            //cbxColor.Items.Add(Color.Red);
            //cbxColor.Items.Add(Color.White);
            //cbxColor.Items.Add(Color.Pink);
            //cbxColor.Items.Add(Color.Green);
            //cbxColor.Items.Add(Color.Khaki);

            cbxCondición.DataSource = ctoModel.dt_lista("esq_red.lta_cond_inter");
            cbxCondición.DisplayMember = "nombre";
            cbxCondición.ValueMember = "id";

            //IList<CbxLista> ltaSala = new List<CbxLista>();

            //ltaSala.Add(new CbxLista("1", "7kd"));
            //ltaSala.Add(new CbxLista("2", "Cod"));
            //ltaSala.Add(new CbxLista("3", "Code"));
            //ltaSala.Add(new CbxLista("4", "Sen"));
            //ltaSala.Add(new CbxLista("5", "Terc"));
            //cbxSala.DataSource = ltaSala;
            cbxSala.DataSource = ctoModel.dt_lista("esq_red.lta_sala");            
            cbxSala.DisplayMember = "nombre";
            cbxSala.ValueMember = "id";
            
            cbxCs.DataSource = ctoModel.dt_lista("esq_proy.lta_cs");
            cbxCs.DisplayMember = "nombre";
            cbxCs.ValueMember = "id";

            cbxRed.Items.Add("Aérea");
            cbxRed.Items.Add("Subterranea");

            IList<CbxLista> ltaRed = new List<CbxLista>();

            ltaRed.Add(new CbxLista("1", "Aérea"));
            ltaRed.Add(new CbxLista("2", "Subterranea"));

            cbxRed.DataSource = ltaRed;
            cbxRed.DisplayMember = "nombre";
            cbxRed.ValueMember = "id";            

            IList<CbxLista> ltaInterr = new List<CbxLista>();

            ltaInterr.Add(new CbxLista("1", "Intemperie"));
            ltaInterr.Add(new CbxLista("2", "Reconectador"));
            ltaInterr.Add(new CbxLista("3", "Celdas"));

            cbxInterruptor.DataSource = ltaInterr;
            cbxInterruptor.DisplayMember = "nombre";
            cbxInterruptor.ValueMember = "id";            

            cbxCondScada.DataSource = ctoModel.dt_lista("esq_red.lta_cond_scada");
            cbxCondScada.DisplayMember = "nombre";
            cbxCondScada.ValueMember = "id";
        }


        private void cmbColor_DrawItem(object sender,
    DrawItemEventArgs e)
        {
            ComboBox cmb = sender as ComboBox;
            if (cmb == null) return;
            if (e.Index < 0) return;
            if (!(cmb.Items[e.Index] is Color)) return;
            Color color = (Color)cmb.Items[e.Index];
            // Dibujamos el fondo
            e.DrawBackground();
            // Creamos los objetos GDI+
            Brush brush = new SolidBrush(color);
            Pen forePen = new Pen(e.ForeColor);
            Brush foreBrush = new SolidBrush(e.ForeColor);
            // Dibujamos el borde del rectángulo
            e.Graphics.DrawRectangle(
                forePen,
                new Rectangle(e.Bounds.Left + 2, e.Bounds.Top + 2, 19,
                    e.Bounds.Size.Height - 4));
            // Rellenamos el rectángulo con el Color seleccionado
            // en la combo
            e.Graphics.FillRectangle(brush,
                new Rectangle(e.Bounds.Left + 3, e.Bounds.Top + 3, 18,
                    e.Bounds.Size.Height - 5));
            // Dibujamos el nombre del color
            e.Graphics.DrawString(color.Name, cmb.Font,
                foreBrush, e.Bounds.Left + 25, e.Bounds.Top + 2);
            // Eliminamos objetos GDI+
            brush.Dispose();
            forePen.Dispose();
            foreBrush.Dispose();
        }

        private void txtColor_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            // Muestra el diálogo de color.
            ColorDialog dlg = new ColorDialog();
            dlg.ShowDialog();
            // Ver si el usuario presionó ok.
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string str = null;
                str = dlg.Color.Name;
                MessageBox.Show(str);
            }
            //if (result == DialogResult.OK) { 
            //    // Establece el fondo del formulario en el color seleccionado.
            //    this.BackColor = colorDialog1.Color; }}}
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (this.btnGuardar.Tag == null)
            {
                this.btnGuardar.Tag = 0;
                this.btnGuardar.Image = global::Presentation.Properties.Resources.page_save;
                this.btnGuardar.Text = "Guardar";

                btnPerfil.Enabled = true;

                cbxCondScada.DropDownStyle = ComboBoxStyle.DropDown;
                cbxCondición.DropDownStyle = ComboBoxStyle.DropDown;
                cbxInterruptor.DropDownStyle = ComboBoxStyle.DropDown;
                cbxSala.DropDownStyle = ComboBoxStyle.DropDown;
                cbxCs.DropDownStyle = ComboBoxStyle.DropDown;
                cbxRed.DropDownStyle = ComboBoxStyle.DropDown;
                

                dtpFechaMax.ValueChanged -= dtpFechaMax_ValueChanged;
                dtpFechaAdm.ValueChanged -= dtpFechaAdm_ValueChanged;
                

                txtSalida.ReadOnly = false;
                txtPadee.ReadOnly = false;
                txtColor.ReadOnly = false;
                txtCalibre.ReadOnly = false;
                txtTroncal.ReadOnly = false;
                txtAmp.ReadOnly = false;
                txtFp.ReadOnly = false;
                txtCt.ReadOnly = false;
                txtCc.ReadOnly = false;
                txtkVAInst.ReadOnly = false;
                txtMts.ReadOnly = false;
                txtObsCto.ReadOnly = false;
                txtSectores.ReadOnly = false;
                txtInstituciones.ReadOnly = false;

                txtCarga.ReadOnly = false;
                numGrupo.ReadOnly = false;
                numGrupo.KeyPress -= numGrupo_KeyPress;
                //numGrupo.InterceptArrowKeys = true;
                numGrupo.Increment = 1;
                txtHorario.ReadOnly = false;
                txtAmpAdm.ReadOnly = false;
                txtSuscriptores.ReadOnly = false;
                txtPtos.ReadOnly = false;
                txtNTransf.ReadOnly = false;

                chkNoAdm.AutoCheck = true;
                chkMedicion.AutoCheck = true;
                chkScada.AutoCheck = true;
            }
            else
            {
                if (!Verificar())
                    return;

                this.btnGuardar.Tag = null;
                this.btnGuardar.Image = global::Presentation.Properties.Resources.page_edit;
                this.btnGuardar.Text = "Editar";

                btnPerfil.Enabled = false;

                cbxCondScada.DropDownStyle = ComboBoxStyle.Simple;
                cbxCondición.DropDownStyle = ComboBoxStyle.Simple;
                cbxInterruptor.DropDownStyle = ComboBoxStyle.Simple;
                cbxSala.DropDownStyle = ComboBoxStyle.Simple;
                cbxCs.DropDownStyle = ComboBoxStyle.Simple;
                cbxRed.DropDownStyle = ComboBoxStyle.Simple;
                

                originalValue = dtpFechaMax.Value;
                dtpFechaMax.ValueChanged += dtpFechaMax_ValueChanged;

                originalValue2 = dtpFechaAdm.Value;
                dtpFechaAdm.ValueChanged += dtpFechaAdm_ValueChanged;

                txtSalida.ReadOnly = true;
                txtPadee.ReadOnly = true;
                txtColor.ReadOnly = true;
                txtCalibre.ReadOnly = true;
                txtTroncal.ReadOnly = true;
                txtAmp.ReadOnly = true;
                txtFp.ReadOnly = true;
                txtCt.ReadOnly = true;
                txtCc.ReadOnly = true;
                txtkVAInst.ReadOnly = true;
                txtMts.ReadOnly = true;
                txtObsCto.ReadOnly = true;
                txtSectores.ReadOnly = true;
                txtInstituciones.ReadOnly = true;
                txtCarga.ReadOnly = true;
                numGrupo.ReadOnly = false;
                numGrupo.KeyPress += numGrupo_KeyPress;
                //numGrupo.InterceptArrowKeys = true;
                numGrupo.Increment = 0;
                txtHorario.ReadOnly = true;
                txtAmpAdm.ReadOnly = true;
                txtSuscriptores.ReadOnly = true;
                txtPtos.ReadOnly = true;
                txtNTransf.ReadOnly = true;

                chkNoAdm.AutoCheck = false;
                chkMedicion.AutoCheck = false;
                chkScada.AutoCheck = false;
                Guardar();
            }
        }

        private bool Verificar()
        {
            errorProvider1.Clear();

            if (cbxCs.SelectedIndex.Equals(-1))
            {
                errorProvider1.SetError(cbxCs, "Favor Asignar valor de la lista");
                return false;
            }
            if (cbxCondición.SelectedIndex.Equals(-1))
            {
                errorProvider1.SetError(cbxCondición, "Favor Asignar valor de la lista");
                return false;
            }
            if (cbxInterruptor.SelectedIndex.Equals(-1))
            {
                errorProvider1.SetError(cbxInterruptor, "Favor Asignar valor de la lista");
                return false;
            }
            if (cbxRed.SelectedIndex.Equals(-1))
            {
                errorProvider1.SetError(cbxRed, "Favor Asignar valor de la lista");
                return false;
            }
            if (cbxCondScada.SelectedIndex.Equals(-1))
            {
                errorProvider1.SetError(cbxCondScada, "Favor Asignar valor de la lista");
                return false;
            }
            if (cbxSala.SelectedIndex.Equals(-1))
            {
                errorProvider1.SetError(cbxSala, "Favor Asignar valor de la lista");
                return false;
            }

            if (!Fun.isShort(txtPadee.Text))
            {
                errorProvider1.SetError(txtPadee, "Favor corregir valor asignado");
                return false;
            }
            if (!Fun.isShort(txtColor.Text))
            {
                errorProvider1.SetError(txtColor, "Favor corregir valor asignado");
                return false;
            }
            if (!Fun.isShort(txtSuscriptores.Text))
            {
                errorProvider1.SetError(txtSuscriptores, "Favor corregir valor asignado");
                return false;
            }
            if (!Fun.isShort(txtPtos.Text))
            {
                errorProvider1.SetError(txtPtos, "Favor corregir valor asignado");
                return false;
            }
            if (!Fun.isShort(txtNTransf.Text))
            {
                errorProvider1.SetError(txtNTransf, "Favor corregir valor asignado");
                return false;
            }
            if (!Fun.isShort(txtAmp.Text))
            {
                errorProvider1.SetError(txtAmp, "Favor corregir valor asignado");
                return false;
            }
            if (!Fun.isShort(txtAmpAdm.Text))
            {
                errorProvider1.SetError(txtAmpAdm, "Favor corregir valor asignado");
                return false;
            }

            if (!Fun.isInt(txtTroncal.Text))
            {
                errorProvider1.SetError(txtTroncal, "Favor corregir valor asignado");
                return false;
            }
            if (!Fun.isInt(txtMts.Text))
            {
                errorProvider1.SetError(txtMts, "Favor corregir valor asignado");
                return false;
            }

            if (!Fun.isSingle(txtkVAInst.Text))
            {
                errorProvider1.SetError(txtkVAInst, "Favor corregir valor asignado");
                return false;
            }
            if (!Fun.isSingle(txtFp.Text))
            {
                errorProvider1.SetError(txtFp, "Favor corregir valor asignado");
                return false;
            }
            if (!Fun.isSingle(txtCt.Text))
            {
                errorProvider1.SetError(txtCt, "Favor corregir valor asignado");
                return false;
            }
            if (!Fun.isSingle(txtCc.Text))
            {
                errorProvider1.SetError(txtCc, "Favor corregir valor asignado");
                return false;
            }

            return true;
        }

        private void Guardar()
        {
            NumberFormatInfo provider = new NumberFormatInfo();
            provider.NumberDecimalSeparator = ".";

            short id = Convert.ToInt16(txtId.Text);
            string s_interruptor = txtSalida.Text;
            short n_padee = Convert.ToInt16(txtPadee.Text);
            short id_color = Convert.ToInt16(txtColor.Text);
            short id_cs = short.Parse(cbxCs.SelectedValue.ToString());
            short id_cond_inter = short.Parse(cbxCondición.SelectedValue.ToString());
            string t_interruptor = cbxInterruptor.Text;
            string t_red = cbxRed.Text;
            string calibre_troncal = txtCalibre.Text;
            int mts_troncal = Convert.ToInt32(txtTroncal.Text);
            int mts_total = Convert.ToInt32(txtMts.Text);
            string t_carga = txtCarga.Text;
            double kva_inst = Convert.ToSingle(txtkVAInst.Text);
            short suscriptores = Convert.ToInt16(txtSuscriptores.Text);
            short n_ptos = Convert.ToInt16(txtPtos.Text);
            short n_transf = Convert.ToInt16(txtNTransf.Text);
            short id_cond_scada = short.Parse(cbxCondScada.SelectedValue.ToString());
            bool b_p_medicion = chkMedicion.Checked;
            bool b_p_scada = chkScada.Checked;
            short amp_max = Convert.ToInt16(txtAmp.Text);
            float fp = Convert.ToSingle(txtFp.Text);
            float cv = Convert.ToSingle(txtCt.Text);
            float cc = Convert.ToSingle(txtCc.Text);
            DateTime f_max = Convert.ToDateTime(dtpFechaMax.Value);
            short id_grupo =(short) numGrupo.Value;
            char horario = Convert.ToChar(txtHorario.Text);
            short amp_adm = Convert.ToInt16(txtAmpAdm.Text);
            bool b_no_adm = chkNoAdm.Checked;
            DateTime f_adm = Convert.ToDateTime(dtpFechaAdm.Value);
            short id_sala = short.Parse(cbxSala.SelectedValue.ToString()); ;
            string obs_cto = txtObsCto.Text;
            string sectores = txtSectores.Text;
            string obs_esp = txtInstituciones.Text;
            string usuario = UserCache.LoginUser;

            //CtoModel.F_MAX = Convert.ToDateTime(dtpFechaDemanda.Value);
            //CtoModel.MVA = Convert.ToDecimal(txtMvaInst.Text.Replace(",", "."), provider);
            //CtoModel.AMP = Convert.ToInt16(txtAmpMax.Text);
            //CtoModel.PVAR = Convert.ToInt16(txtPvar.Text);
            //CtoModel.KV1 = Convert.ToDecimal(cbxkV1.Text, provider);
            //CtoModel.KV2 = Convert.ToDecimal(cbxkV2.Text, provider);
            //CtoModel.Z = Convert.ToDecimal(txtZ.Text.Replace(",", "."), provider);
            //CtoModel.MARCA = cbxMarca.Text;
            //CtoModel.B_MOVIL = chkMovil.Checked;
            //CtoModel.B_VOLTAJE = chkTension.Checked;
            //CtoModel.B_PROCT = chkProteccion.Checked;
            //CtoModel.B_SCADA = chkScada.Checked;
            //CtoModel.B_VENT = chkForzada.Checked;
            //CtoModel.B_OPER = chkNoOperativo.Checked;
            //CtoModel.ID_COND = short.Parse(cbxCondición.SelectedValue.ToString());
            //CtoModel.OBS = txtObs.Text;
            //CtoModel.USER = UserCache.LoginUser;

            if (ctoModel.update_tbl_cto(id, s_interruptor, n_padee, id_color, id_cs, id_cond_inter,
            t_interruptor, t_red, calibre_troncal, mts_troncal, mts_total, t_carga,
            kva_inst, suscriptores, n_ptos, n_transf,
            id_cond_scada, b_p_medicion, b_p_scada,
            amp_max, fp, cv, cc, f_max,
            id_grupo, horario, amp_adm, b_no_adm, f_adm, id_sala,
           obs_cto, sectores, obs_esp, usuario))
            {
                if (!ONPANEL)
                    this.Close(); //txtFecAct.Text = Convert.ToDateTime(DateTime.Now).ToString("ddd dd-MM-yy HH:mm");         
            }
            else
                MessageBox.Show("Datos No guardados", "Notificación", MessageBoxButtons.OK, MessageBoxIcon.Information);


        }

        private void calcular_indices()
        {
            double tem_double;

            // $Fd MVA / MVA inst
            tem_double = Math.Sqrt(3) * Convert.ToDouble(txtkV.Text) * Convert.ToDouble(txtAmp.Text);
            txtMWmax.Text = Math.Round(tem_double / 1000, 2).ToString();
            if (Fun.isSingle(txtkVAInst.Text) && !txtkVAInst.Text.Equals("0"))
            {
                tem_double = Math.Round(tem_double / Convert.ToDouble(txtkVAInst.Text) * 100, 0);
                txtFd.Text = tem_double.ToString();
            }
            else
                txtFd.Text = "0";

            //MW Adm
            if (Fun.isShort(txtAmpAdm.Text))//   && !txtkVAInst.Text.Equals("0")
            {
                tem_double = Math.Sqrt(3) * Convert.ToDouble(txtkV.Text) * Convert.ToDouble(txtAmpAdm.Text) * Convert.ToDouble(txtFp.Text);
                txtMwAdm.Text = Math.Round(tem_double, 2).ToString();
            }

            // kVA/Clientes
            if (Fun.isShort(txtSuscriptores.Text) && !txtSuscriptores.Text.Equals("0"))//   kVA/Clientes
            {
                tem_double = Math.Round(Convert.ToDouble(txtkVAInst.Text) / Convert.ToDouble(txtSuscriptores.Text), 2);
                txtkVA_Cliente.Text = tem_double.ToString();
            }

            //   kVA/kmts
            if (Fun.isShort(txtMts.Text) && !txtMts.Text.Equals("0"))
            {
                tem_double = Math.Round(Convert.ToDouble(txtkVAInst.Text) / Convert.ToDouble(txtMts.Text) * 1000, 2);
                txtkVA_kmts.Text = tem_double.ToString();
            }

            //if (txtkVAInst.Text.TrimEnd().Equals("0"))
            //{                
            //    txtkVAInst.ForeColor = Color.Red;
            //    txtFu.ForeColor = Color.Red;
            //}
            //if (txtkVAInst.Text.TrimEnd().Equals(""))
            //{                
            //    txtkVAInst.Text = "0";
            //    txtkVAInst.BackColor = Color.DarkRed;
            //    txtFu.ForeColor = Color.Red;
            //}
            //if (!nulo)
            //{
            //    tem_double = tem_double / Convert.ToDouble(txtkVAInst.Text) * 100 * 1000;
            //}
            //else
            //{
            //    tem_double = 0;
            //}
            //txtFu.Text = Math.Truncate(tem_double).ToString();

            ////DATOS DE CLIENTES
            //nulo = false;
            //tem_double = Convert.ToDouble(txtkVAInst.Text);
            //if (txtkVA_Cliente.Text.TrimEnd().Equals("0"))
            //{
            //    nulo = true;
            //    txtClientes.ForeColor = Color.Red;
            //    txtkVA_Cliente.ForeColor = Color.Red;
            //}
            //if (!nulo)
            //{
            //    tem_double = tem_double / Convert.ToDouble(txtClientes.Text);
            //}
            //else
            //{
            //    tem_double = 0;
            //}
            //txtkVA_Cliente.Text = Math.Round(tem_double, 2).ToString();

            //nulo = false;
            //if (txtMts.Text.TrimEnd().Equals("0"))
            //{
            //    nulo = true;
            //    txtMts.ForeColor = Color.Red;
            //    txtkVA_kmts.ForeColor = Color.Red;
            //}
            //if (txtMts.Text.TrimEnd().Equals(""))
            //{
            //    nulo = true;
            //    txtMts.Text = "0";
            //    txtMts.BackColor = Color.DarkRed;
            //    txtkVA_kmts.ForeColor = Color.Red;
            //}
            //if (!nulo)
            //{
            //    tem_double = Convert.ToDouble(txtkVAInst.Text) / Convert.ToDouble(txtMts.Text) * 1000;
            //}
            //else
            //{
            //    tem_double = 0;
            //}
            //txtkVA_kmts.Text = Math.Truncate(tem_double).ToString();

            //nulo = false;
            //tem_double = mva_max2;
            //if (txtkVAInst2.Text.TrimEnd().Equals("0"))
            //{
            //    nulo = true;
            //    txtkVAInst2.ForeColor = Color.Red;
            //}
            //if (txtkVAInst2.Text.TrimEnd().Equals(""))
            //{
            //    nulo = true;
            //    txtkVAInst2.BackColor = Color.DarkRed;
            //}


            //if (!nulo)
            //{
            //    tem_double = tem_double / Convert.ToDouble(txtkVAInst2.Text) * 100 * 1000;
            //}
            //else
            //{
            //    tem_double = 0;
            //}

            //txtFu2.Text = Math.Truncate(tem_double).ToString();

            //nulo = false;
            //if (txtMts2.Text.TrimEnd().Equals("0"))
            //{
            //    nulo = true;
            //    txtMts2.ForeColor = Color.Red;
            //}
            //if (txtMts2.Text.TrimEnd().Equals(""))
            //{
            //    nulo = true;
            //    txtMts2.BackColor = Color.DarkRed;
            //}
            //if (!nulo)
            //{
            //    tem_double = Convert.ToDouble(txtkVAInst2.Text) / Convert.ToDouble(txtMts2.Text) * 1000;
            //}
            //else
            //{
            //    tem_double = 0;
            //}
            //txtDensidad2.Text = Math.Truncate(tem_double).ToString();

        }

        private void dtpFechaMax_ValueChanged(object sender, EventArgs e)
        {
            dtpFechaMax.Value = originalValue;
        }

        private void dtpFechaAdm_ValueChanged(object sender, EventArgs e)
        {
            dtpFechaAdm.Value = originalValue2;
        }

        private void cbxCondScada_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void cbxCondición_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void cbxSala_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void cbxCs_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void cbxRed_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void cbxInterruptor_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void numGrupo_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void chkOrden_Click(object sender, EventArgs e)
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

        private void btnTexMin_Click(object sender, EventArgs e)
        {
            txtInstituciones.Text = txtInstituciones.Text.ToLower();
        }

        private void txtSectoresMin_Click(object sender, EventArgs e)
        {
            txtSectores.Text = txtSectores.Text.ToLower();
        }

        private void btnPerfil_Click(object sender, EventArgs e)
        {
            //cargar archivo PERFIL.CSV
            //string sPath = "C:\\PADEE\\TEMP\\";
            string sPath = @"E:/Sid/Información Geográfica/TEMP/";

            System.Globalization.NumberFormatInfo formato = new System.Globalization.NumberFormatInfo();
            formato.NumberDecimalSeparator = ".";

            string sFileName = sPath + "PERFIL.CSV";
            string linea;
            Single troncal = 0, ccond = 0, ct = 0;
            Int16 amp = 0;
            //char Tipo;
            Boolean cambio = false;

            MsgBoxStyle style = MsgBoxStyle.YesNo;
            string title = "Cambiar valor";
            string msg = "";

            char[] delimiterChars = { ',' };//{ ' ', ',', '.', ':', '\t' };
            string[] elementos;

            //verifico que exista el archivo
            if (File.Exists(sFileName))
            {
                FileStream fs = new FileStream(sFileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                StreamReader sr = new StreamReader(fs);

               

                linea = sr.ReadLine();
                linea = sr.ReadLine();

                elementos = linea.Split(delimiterChars);

                //amp = Convert.ToInt16(Single.Parse(elementos[5]));
                amp = Convert.ToInt16(Single.Parse(elementos[5], formato));
                //troncal += Single.Parse(elementos[1]);
                //ccond = Single.Parse(elementos[1]);
                //ct = Single.Parse(elementos[6]);

                while (linea != null)
                {
                    elementos = linea.Split(delimiterChars);

                    troncal += Single.Parse(elementos[1], formato);

                    if (Single.Parse(elementos[3], formato) > ccond)
                    {
                        ccond = Single.Parse(elementos[3], formato);
                    }

                    //if (Single.Parse(elementos[6], formato) > ct)
                    //{
                    //    ct = Single.Parse(elementos[6]);
                    //}
                    float valor = Single.Parse(elementos[6]) / 100;
                    if (valor > ct)
                    {
                        ct = valor;
                    }

                    linea = sr.ReadLine();

                }
                //cierro los objetos
                fs.Close();
                sr.Close();

                if (!txtAmp.Text.Equals(amp.ToString()))
                {
                    msg = "Reemplazar valor de Amperios?\n";
                    msg += string.Format("Viejo: {0} A\n", txtAmp.Text);
                    msg += string.Format("Nuevo: {0} A", amp);
                    MsgBoxResult result = Interaction.MsgBox(msg, style, title);
                    if (result == MsgBoxResult.Yes)
                    {
                        txtAmp.Text = amp.ToString();
                        cambio = true;
                    }
                }


                Int16 int_ccond = Convert.ToInt16(Math.Truncate(ccond));
                if (!txtCc.Text.Equals(int_ccond.ToString()))
                {
                    msg = "Reemplazar valor de %C.C.?\n";
                    msg += "Viejo: " + txtCc.Text + " %CC\n";
                    msg += "Nuevo: " + int_ccond.ToString() + " %CC";
                    MsgBoxResult result = Interaction.MsgBox(msg, style, title);
                    if (result == MsgBoxResult.Yes)
                    {
                        txtCc.Text = int_ccond.ToString();
                        cambio = true;
                    }
                }

                if (!txtCt.Text.Equals(ct.ToString()))
                {
                    msg = "Reemplazar valor de %C.V.?\n";
                    msg += "Viejo: " + txtCt.Text + " %CV\n";
                    msg += "Nuevo: " + ct.ToString() + " %CV\n";
                    MsgBoxResult result = Interaction.MsgBox(msg, style, title);
                    if (result == MsgBoxResult.Yes)
                    {
                        txtCt.Text = ct.ToString();
                        cambio = true;
                    }
                }

                troncal = troncal * 1000;
                int m_troncal = Convert.ToInt32(Math.Truncate(troncal));

                if (!txtTroncal.Text.Equals(m_troncal.ToString()))
                {
                    msg = "Reemplazar valor de Longitud Troncal (mts)?\n";
                    msg += "Viejo: " + txtTroncal.Text + " mts\n";
                    msg += "Nuevo: " + m_troncal.ToString() + " mts";
                    MsgBoxResult result = Interaction.MsgBox(msg, style, title);
                    if (result == MsgBoxResult.Yes)
                    {
                        txtTroncal.Text = m_troncal.ToString();
                        cambio = true;
                    }
                }
            }
            else
            {
                MessageBox.Show("No existe el archivo: " + sFileName);
                //Console.WriteLine("No existe el archivo: " + sFileName);
                //Console.ReadKey(true);
            }

            //Cargar longitud total
            //sFileName = sPath + "BUSORD.ALM";
            sFileName = sPath + "ANALIS.CSV";
            //verifico que exista el archivo
            if (File.Exists(sFileName))
            {
                int longitud = 0, count = 0;
                Single kva = 0;
                FileStream fs = new FileStream(sFileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                StreamReader sr = new StreamReader(fs);
                linea = sr.ReadLine();

                while (linea != null)
                {
                    elementos = linea.Split(delimiterChars);
                    if (elementos[0].Equals("TOTAL"))
                    {
                        longitud = Convert.ToInt32(Single.Parse(elementos[1], formato) * 1000);

                    }
                    else
                    {
                        float valor = Single.Parse(elementos[8], formato);
                        if (valor > 1)
                        {
                            kva += valor;
                            count += 1;
                        }
                    }

                    linea = sr.ReadLine();
                }
                //cierro los objetos
                fs.Close();
                sr.Close();

                if (!txtMts.Text.Equals(longitud.ToString()))
                {
                    msg = "Reemplazar valor de Longitud Total (mts)?\n";
                    msg += "Viejo: " + txtMts.Text + " mts\n";
                    msg += "Nuevo: " + longitud.ToString() + " mts\n";
                    MsgBoxResult result = Interaction.MsgBox(msg, style, title);
                    if (result == MsgBoxResult.Yes)
                    {
                        txtMts.Text = longitud.ToString();
                        //txtAereo.Text = longitud.ToString();
                        cambio = true;
                    }
                }

                //if (!txtAereo.Text.Equals(longitud.ToString()))
                //{
                //    msg = "Reemplazar valor de conductor aéreo (mts)?\n";
                //    msg += "Viejo: " + txtAereo.Text + " mts\n";
                //    msg += "Nuevo: " + longitud.ToString() + " mts\n";

                //    MsgBoxResult result = Interaction.MsgBox(msg, style, title);
                //    //result = Interaction.MsgBox(msg, style, title);
                //    if (result == MsgBoxResult.Yes)
                //    {
                //        txtAereo.Text = longitud.ToString();
                //        txtSubterr.Text = "0";
                //        cambio = true;
                //    }
                //}
                if (!txtkVAInst.Text.Equals(kva.ToString()))
                {
                    msg = "Reemplazar valor de kVA Instalado?\n";
                    msg += "Viejo: " + txtkVAInst.Text + " kVA\n";
                    msg += "Nuevo: " + kva.ToString() + " kVA\n";

                    MsgBoxResult result = Interaction.MsgBox(msg, style, title);
                    //result = Interaction.MsgBox(msg, style, title);
                    if (result == MsgBoxResult.Yes)
                    {
                        txtkVAInst.Text = kva.ToString();
                        cambio = true;
                    }
                }

                if (!txtPtos.Text.Equals(count.ToString()))
                {
                    msg = "Reemplazar valor de Puntos de Transformación?\n";
                    msg += "Viejo: " + txtPtos.Text + " Unidades\n";
                    msg += "Nuevo: " + count.ToString() + " Unidades\n";

                    MsgBoxResult result = Interaction.MsgBox(msg, style, title);
                    //result = Interaction.MsgBox(msg, style, title);
                    if (result == MsgBoxResult.Yes)
                    {
                        txtPtos.Text = count.ToString();
                        cambio = true;
                    }
                }
            }
            else
            {
                MessageBox.Show("No existe el archivo: " + sFileName);
                //Console.WriteLine("No existe el archivo: " + sFileName);
                //Console.ReadKey(true);
            }

            if (!cambio)
            {
                MessageBox.Show("No existen cambios en los datos");
            }
        }

        private void txtAmpAdm_KeyPress(object sender, KeyPressEventArgs e)
        {
          if (e.KeyChar == 13)
              if (Fun.isShort(txtAmpAdm.Text))//   && !txtkVAInst.Text.Equals("0")
              {
                  double tem_double;
                  tem_double = Math.Sqrt(3) * Convert.ToDouble(txtkV.Text) * Convert.ToDouble(txtAmpAdm.Text) * Convert.ToDouble(txtFp.Text);
                  txtMwAdm.Text = Math.Round(tem_double, 2).ToString();
              }
        }

        private void txtAmp_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                if (Fun.isShort(txtAmp.Text))//   && !txtkVAInst.Text.Equals("0")
                {
                    double tem_double;
                    tem_double = Math.Sqrt(3) * Convert.ToDouble(txtkV.Text) * Convert.ToDouble(txtAmp.Text) * Convert.ToDouble(txtFp.Text);
                    txtMWmax.Text = Math.Round(tem_double, 2).ToString();
                }
        }

        private void tsbCadMostrar_Click(object sender, EventArgs e)
        {
            double[] SelXY = new double[2];
            SelXY = new double[] { double.Parse(txtX.Text), double.Parse(txtY.Text) };

            if (SelXY[0].Equals(0) || SelXY[1].Equals(0))
            {
                System.Media.SystemSounds.Beep.Play();                
                return;
            }

            Int16 _marcar = 0;

            cadModel.CoordZoom(SelXY[0], SelXY[1], _marcar);


            //if (cadModel.CoordZoom(SelXY[0], SelXY[1], _marcar))
            //{
            //    //node.BackColor = Color.Yellow;
            //    if (mnuOrden.Checked == false)
            //        this.WindowState = FormWindowState.Minimized;
            //}
            //else
            //{
            //    lblObs.Text = this.cadModel.MENSAJE;
            //}
        }
    }
}
