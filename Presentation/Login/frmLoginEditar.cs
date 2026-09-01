using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Common.Cache;
using Domain;

namespace Presentation
{
#pragma warning disable CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
    public partial class frmLoginEditar : Form
#pragma warning restore CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
    {     
        UserModel userModel = new UserModel();

#pragma warning disable CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
        public frmLoginEditar()
#pragma warning restore CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
        {
            InitializeComponent();
        }

        private void FormUserProfile_Load(object sender, EventArgs e)
        {
            
            initializePassEditControls();
            ListarDpto();
            ListarGrupo();
            ListarEstado();
            loadUserData();
        }

        private void loadUserData()
        {
            //View
            txtUser.Text = UserCache.LoginUser;
            txtNombre.Text = UserCache.Name;
            txtCargo.Text = UserCache.Cargo;
            txtCI.Text =Convert.ToString(UserCache.ci);
            txtTelef.Text = UserCache.Telef;
            txtId.Text =Convert.ToString(UserCache.IdUser);
            txtMail.Text = UserCache.Mail;
            txtObs.Text = UserCache.Obs;
            
            cbxDpto.SelectedValue = UserCache.id_dpto;
            cbxGrupo.SelectedValue = UserCache.id_grupo;
            cbxEstado.SelectedValue = UserCache.id_estado;
                        

        }
        private void initializePassEditControls()
        {
            btnGuardar.Text = "Editar";
            //txtPassword.UseSystemPasswordChar = true;
            //txtPassword.Enabled = false;
            //txtConfirmPass.UseSystemPasswordChar = true;
            //txtConfirmPass.Enabled = false;
        }
        private void reset()
        {
            loadUserData();
            initializePassEditControls();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (btnGuardar.Text.Equals("Editar"))
            {
                txtNombre.ReadOnly = false;
                txtCI.ReadOnly = false;
                txtTelef.ReadOnly = false;
                txtMail.ReadOnly = false;
                txtCargo.ReadOnly = false;
                txtObs.ReadOnly = false;
                if (!UserCache.bloq_perfil)
                {
                    cbxDpto.Enabled = true;
                    cbxEstado.Enabled = true;
                    cbxGrupo.Enabled = true;
                }
                btnGuardar.Text = "Guardar";
                return;
            }

            var userModel = new UserModel(
                    idLogin: UserCache.IdUser,
                    login: txtUser.Text,
                    nombre: txtNombre.Text,
                    ci: int.Parse(txtCI.Text),
                    cargo: txtCargo.Text,
                    telef: txtTelef.Text,
                    mail: txtMail.Text,
                    id_estado: Convert.ToInt16(cbxEstado.SelectedValue.ToString()),
                    id_grupo: Convert.ToInt16(cbxGrupo.SelectedValue.ToString()),
                    id_dpto: Convert.ToInt16(cbxDpto.SelectedValue.ToString()),
                    txt_obs: txtObs.Text);
            var result = userModel.loginUpdate();
            MessageBox.Show(result, "Notificación", MessageBoxButtons.OK, MessageBoxIcon.Information);
            
            this.Close();
           
        }
        private void ListarDpto()
        {

            cbxDpto.DataSource = userModel.dt_lista("esq_proy.v_lta_dpto",0,false);
            cbxDpto.DisplayMember = "nombre";
            cbxDpto.ValueMember = "id";
        }
        private void ListarGrupo()
        {
            cbxGrupo.DataSource = userModel.dt_lista("esq_proy.v_lta_grupo", UserCache.id_dpto, false);
            cbxGrupo.DisplayMember = "nombre";
            cbxGrupo.ValueMember = "id";
        }

        private void ListarEstado()
        {
            cbxEstado.DataSource = userModel.dt_lista("esq_open.v_lta_estado", 0, false);
            cbxEstado.DisplayMember = "nombre";
            cbxEstado.ValueMember = "id";
        }

        private void cbxDpto_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Int16 n = Convert.ToInt16(cbxDpto.SelectedValue.ToString());
                UserCache.id_dpto = n;
                ListarGrupo();

            }
            catch{ }
        }
        

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtId_TextChanged(object sender, EventArgs e)
        {

        }

       
    }
}
