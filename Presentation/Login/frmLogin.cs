using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Domain;
using Common.Cache;
using Presentation;

namespace FormLogin
{
#pragma warning disable CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
    public partial class frmLogin : Form
#pragma warning restore CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
    {
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hwnd, int wmsg, int wparam, int lparam);

#pragma warning disable CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
        public frmLogin()
#pragma warning restore CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
        {
            InitializeComponent();
            txtUser.Text = SystemInformation.UserName.ToUpper();
           
             UserModel userModel = new UserModel();
             if (!userModel.comprobarIP())
             {
                 btnLogin.Text = "PROBLEMA DE CONEXIÓN CON LA BASE DE DATOS";
                 btnLogin.ForeColor = Color.Yellow;
                 //btnLogin.Enabled = false;
                 this.btnLogin.Click -= new System.EventHandler(this.btnLogin_Click);
                 lblLinkPass.Visible = false;
                 return;
             }
                    bool validLogin = userModel.LoginUserVal(txtUser.Text);
                    if (validLogin == true)
                    {
                        btnLogin.Text = "ACCEDER";
                        lblLinkPass.Visible = true;
                        txtPass.Text = " ";
                    }
                    else { 
                        btnLogin.Text = "REGISTRAR";
                        lblLinkPass.Visible = false;
                    }
                   
        }

#pragma warning disable CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
        public void load()
#pragma warning restore CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
        {
            Presentation.Properties.Settings.Default.Reload();
            //_rutaXML = Presentation.Properties.Settings.Default.MSG_CONF_FRM;
            ConfCache.RUTA_XML = Presentation.Properties.Settings.Default.RUTA_XML;
            ConfCache.MSG_CONF_FRM = Presentation.Properties.Settings.Default.MSG_CONF_FRM;

            ConfCache.MSG_ROW_NULL = "No existen registros en la tabla";
            ConfCache.MSG_XY_NULL = "el registro seleccionado no tiene coordenas UTM (X,Y)";
            ConfCache.MSG_CAD_ER_DIBUJAR = "No se logró Dibujar el Bloque";
            ConfCache.MSG_CAD_ER_DIBUJAR_CANT = "Existen {0} registros sin coordenadas X e Y.";
            ConfCache.MSG_CAD_ER_SELEC = "No se logró Seleccionar el Bloque";
            ConfCache.MSG_NO_AUTORIZADO = "Usuario no Autorizado";

            ConfCache.CAD_LAYER_TEM_LOCALIDAD = "v_localidades";
            ConfCache.CAD_LAYER_TEM_CLIENTES = "v_clientes";

           
            //_rutaServBD = Presentacion.Properties.Settings.Default.RUTA_SERVBD;
            //_connString = Presentacion.Properties.Settings.Default.ConeccionLocalBD;
        }

        private void txtuser_Enter(object sender, EventArgs e)
        {
            if (txtUser.Text == "USUARIO")
            {
                txtUser.Text = "";
                txtUser.ForeColor = Color.LightGray;
            }
        }

        private void txtuser_Leave(object sender, EventArgs e)
        {
            if (txtUser.Text == "")
            {
                txtUser.Text = "USUARIO";
                txtUser.ForeColor = Color.DimGray;
            }
        }

        private void txtpass_Enter(object sender, EventArgs e)
        {
            if (txtPass.Text == "CONTRASEÑA")
            {
                txtPass.Text = "";
                txtPass.ForeColor = Color.LightGray;
                txtPass.UseSystemPasswordChar = true;
            }
        }

        private void txtpass_Leave(object sender, EventArgs e)
        {
            if (txtPass.Text == "")
            {
                txtPass.Text = "CONTRASEÑA";
                txtPass.ForeColor = Color.DimGray;
                txtPass.UseSystemPasswordChar = false;
            }
        }

        private void btncerrar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnminimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (txtUser.Text != "USUARIO" && txtUser.TextLength > 2)
            {
                if (txtPass.Text != "CONTRASEÑA")
                {
                    UserModel user = new UserModel();
                    var validLogin = user.LoginSelect(txtUser.Text, txtPass.Text);
                    

                    if (validLogin == true)
                    {
                        load();
                        this.Hide();
                        //FormWelcome Welcome = new FormWelcome();
                        //Welcome.ShowDialog();

                        //FormMenuPrincipal mainMenu = new FormMenuPrincipal();
                        //MDIParentPrincipal mainMenu = new MDIParentPrincipal();
                        
                       
                        //frm_instalaciones.Show();
                        //frm_instalaciones.FormClosed += Logout;

                        //MDIParentPrincipal frm_princ = new MDIParentPrincipal();
                        //frmInstalaciones frm_princ = new frmInstalaciones();
                        //frmLocalidades frm_princ = new frmLocalidades();                       
                        frmPrincipal frm_princ = new frmPrincipal();
                        //Presentation.InfGeográfica.frmCto frm_princ = new Presentation.InfGeográfica.frmCto();
                        
                        
                        frm_princ.Show();
                        frm_princ.FormClosed += Logout;
                        //this.Hide();

                        
                    }
                    else
                    {
                        msgError("Usuario o Clave Incorrecta. \n     Favor Corregir.");
                        txtPass.Text = "CONTRASEÑA";
                        txtPass.UseSystemPasswordChar = false;
                        txtUser.Focus();
                    }
                }
                else msgError("Por Favor Ingrese Clave.");
            }
            else msgError("Por Favor Ingrese el Usuario.");

        }

       

        private void msgError(string msg)
        {
            lblErrorMessage.Text = "    " + msg;
            lblErrorMessage.Visible = true;
        }

        private void Logout(object sender, FormClosedEventArgs e)
        {
            //txtpass.Text = "CONTRASEÑA";
            //txtpass.UseSystemPasswordChar = false;
            //txtuser.Text = "USUARIO";
            //lblErrorMessage.Visible = false;
            //this.Show();
            this.Close();
        }

        private void linkpass_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var recoverPassword = new frmLoginRecoverPassword();
            recoverPassword.ShowDialog();

        }

        private void panel2_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

     
       

       
    }
}
