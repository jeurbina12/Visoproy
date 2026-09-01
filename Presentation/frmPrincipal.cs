using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Domain;
using Common.Cache;
using Presentation.Util;
using System.Collections;
using Presentation.InfGeográfica;
using Presentation.Sgi;

namespace Presentation
{
#pragma warning disable CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
    public partial class frmPrincipal : Form
#pragma warning restore CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
    {
        //private static SortedList formInstances = new SortedList(); // Para guardar las referencias de las instancias de los formularios

#pragma warning disable CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
        public frmPrincipal()
#pragma warning restore CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
        {
            InitializeComponent();
            hideSubMenu();
            //Estas lineas eliminan los parpadeos del formulario o controles en la interfaz grafica (Pero no en un 100%)
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.DoubleBuffered = true;

        }
        //RESIZE METODO PARA REDIMENCIONAR/CAMBIAR TAMAÑO A FORMULARIO EN TIEMPO DE EJECUCION
        private int tolerance = 12;
        private const int WM_NCHITTEST = 132;
        private const int HTBOTTOMRIGHT = 17;
        private Rectangle sizeGripRectangle;

#pragma warning disable CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
        protected override void WndProc(ref Message m)
#pragma warning restore CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
        {
            switch (m.Msg)
            {
                case WM_NCHITTEST:
                    base.WndProc(ref m);
                    var hitPoint = this.PointToClient(new Point(m.LParam.ToInt32() & 0xffff, m.LParam.ToInt32() >> 16));
                    if (sizeGripRectangle.Contains(hitPoint))
                        m.Result = new IntPtr(HTBOTTOMRIGHT);
                    break;
                default:
                    base.WndProc(ref m);
                    break;
            }
        }
        //----------------DIBUJAR RECTANGULO / EXCLUIR ESQUINA PANEL 
#pragma warning disable CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
        protected override void OnSizeChanged(EventArgs e)
#pragma warning restore CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
        {
            base.OnSizeChanged(e);
            var region = new Region(new Rectangle(0, 0, this.ClientRectangle.Width, this.ClientRectangle.Height));
            sizeGripRectangle = new Rectangle(this.ClientRectangle.Width - tolerance, this.ClientRectangle.Height - tolerance, tolerance, tolerance);
            region.Exclude(sizeGripRectangle);
            this.PanelContenedor.Region = region;
            this.Invalidate();
        }
        //----------------COLOR Y GRIP DE RECTANGULO INFERIOR
#pragma warning disable CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
        protected override void OnPaint(PaintEventArgs e)
#pragma warning restore CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
        {
            SolidBrush blueBrush = new SolidBrush(Color.FromArgb(244, 244, 244));
            e.Graphics.FillRectangle(blueBrush, sizeGripRectangle);
            base.OnPaint(e);
            ControlPaint.DrawSizeGrip(e.Graphics, Color.Transparent, sizeGripRectangle);
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            //if (MessageBox.Show("¿Está seguro de cerrar?", "Alerta¡", MessageBoxButtons.YesNo) == DialogResult.Yes)
            //{
            this.Close();
            //Application.Exit();
            //}
        }

        //Capturar posicion y tamaño antes de maximizar para restaurar
        int lx, ly;
        int sw, sh;

        private void btnMaximizar_Click(object sender, EventArgs e)
        {
            lx = this.Location.X;
            ly = this.Location.Y;
            sw = this.Size.Width;
            sh = this.Size.Height;
            btnMaximizar.Visible = false;
            btnRestaurar.Visible = true;
            this.Size = Screen.PrimaryScreen.WorkingArea.Size;
            this.Location = Screen.PrimaryScreen.WorkingArea.Location;

        }

        private void btnRestaurar_Click(object sender, EventArgs e)
        {
            btnMaximizar.Visible = true;
            btnRestaurar.Visible = false;
            this.Size = new Size(sw, sh);
            this.Location = new Point(lx, ly);
        }

        private void btnMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        private void panelBarraTitulo_MouseMove(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        //private void AbrirFormularioInPanel<MiForm>() where MiForm : Form, new()
        //{
            //Form formulario;
            //formulario = panelChildForm.Controls.OfType<MiForm>().FirstOrDefault();//Busca en la colecion el formulario
            ////si el formulario/instancia no existe
            //if (formulario == null)
            //{
            //    formulario = new MiForm();
            //    formulario.TopLevel = false;
            //    formulario.FormBorderStyle = FormBorderStyle.None;
            //    formulario.Dock = DockStyle.Fill;
            //    panelChildForm.Controls.Add(formulario);
            //    panelChildForm.Tag = formulario;
            //    formulario.Show();
            //    formulario.BringToFront();
            //}
            ////si el formulario/instancia existe
            //else
            //{
            //    formulario.BringToFront();
            //}           
        //}

        //private void AddFormInPanel(object formHijo)
        //{
        //    if (this.panelChildForm.Controls.Count > 0)
        //        this.panelChildForm.Controls.RemoveAt(0);
        //    Form fh = formHijo as Form;
        //    fh.TopLevel = false;
        //    fh.FormBorderStyle = FormBorderStyle.None;
        //    fh.Dock = DockStyle.Fill;
        //    this.panelChildForm.Controls.Add(fh);
        //    this.panelChildForm.Tag = fh;
        //    fh.Show();
        //} 

        //private void openChildFormInPanel(Form childForm)
        //{
        //    if (activeForm != null)
        //        activeForm.Close();

        //    activeForm = childForm;
        //    childForm.TopLevel = false;
        //    childForm.FormBorderStyle = FormBorderStyle.None;
        //    childForm.Dock = DockStyle.Fill;
        //    panelChildForm.Controls.Add(childForm);
        //    panelChildForm.Tag = childForm;
        //    childForm.BringToFront();
        //    childForm.Show();
        //}


        //public static Form AbrirFormulario(Type type)//static
        //{
        //    return AbrirFormulario(type, false);
        //}

        //public static Form AbrirFormulario(Type type, bool dialog)//static
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

        //public Form AbrirFormularioInPanel(Type type)//static
        //{
        //    Form formulario;
        //    if ((formulario = (Form)formInstances[type.ToString()]) == null || formulario.IsDisposed)
        //    {
        //        formulario = (Form)Activator.CreateInstance(type);
        //        formInstances[type.ToString()] = formulario;

        //        //formulario = new MiForm();
        //        formulario.TopLevel = false;
        //        formulario.FormBorderStyle = FormBorderStyle.None;
        //        formulario.Dock = DockStyle.Fill;
        //        panelChildForm.Controls.Add(formulario);
        //        panelChildForm.Tag = formulario;
        //        formulario.Show();
                
        //    }

        //    formulario.Activate();
        //    formulario.BringToFront();
        //    //formulario.WindowState = FormWindowState.Normal;
        //    //formulario.MdiParent = this;
        //    //formulario.Text = formulario.Name + "Ventana " + childFormNumber++;
        //    //formulario.Text = formulario.Text;          

        //    return formulario;
        //}

        private void Logout(object sender, FormClosedEventArgs e)
        {
            //txtpass.Text = "CONTRASEÑA";
            //txtpass.UseSystemPasswordChar = false;
            //txtuser.Text = "USUARIO";
            //lblErrorMessage.Visible = false;
            //this.Show();
            //this.Close();
            //this.BringToFront();
            //this.TopLevel = true;
            //this.TopMost = true;
            this.WindowState = FormWindowState.Normal;
        }
        
        private void btnMenu_Click(object sender, EventArgs e)
        {
            //-------CON EFECTO SLIDING
            if (panelSideMenu.Width == 230)
            {
                this.tmContraerMenu.Start();
            }
            else if (panelSideMenu.Width == 55)
            {
                this.tmExpandirMenu.Start();
            }
        }

        private void tmExpandirMenu_Tick(object sender, EventArgs e)
        {
            if (panelSideMenu.Width >= 230)
                this.tmExpandirMenu.Stop();
            else
                panelSideMenu.Width = panelSideMenu.Width + 5;


        }

        private void tmContraerMenu_Tick(object sender, EventArgs e)
        {
            if (panelSideMenu.Width <= 55)
                this.tmContraerMenu.Stop();
            else
                panelSideMenu.Width = panelSideMenu.Width - 5;
        }

        private void tmFechaHora_Tick(object sender, EventArgs e)
        {
            lblFecha.Text = DateTime.Now.ToLongDateString();
            lblHora.Text = DateTime.Now.ToString("HH:mm:ssss");
        }

        private void hideSubMenu()
        {
            panelSubMenu1.Visible = false;
            panelSubMenu2.Visible = false;
            panelSubMenu3.Visible = false;

        }
        private void showSubMenu(Panel subMenu)
        {
            if (subMenu.Visible == false)
            {
                hideSubMenu();
                subMenu.Visible = true;
            }
            else
                subMenu.Visible = false;
        }

        private void btnMenu1_Click(object sender, EventArgs e)
        {
            showSubMenu(panelSubMenu1);
        }

        private void btnMenu2_Click(object sender, EventArgs e)
        {
            showSubMenu(panelSubMenu2);
        }

        private void btnMenu3_Click(object sender, EventArgs e)
        {
            showSubMenu(panelSubMenu3);
        }
        //private Form activeForm = null;

      
      
       

        private void btnSubMenu1_1_Click(object sender, EventArgs e)
        {
            //AbrirFormularioInPanel<frmLoginEditar>();
           

            //openChildFormInPanel(new frmLoginEditar());

            //frmLoginEditar childForm = (frmLoginEditar)AbrirFormularioInPanel(typeof(frmLoginEditar));
            frmLoginEditar childForm = (frmLoginEditar)Fun.AbrirFormularioInPanel(typeof(frmLoginEditar), this.panelChildForm);
        }

        private void FormMenuPrincipal_Load(object sender, EventArgs e)
        {
            ManagePermissions();
            security();
        }

        private void security()
        {
            var userModel = new Domain.UserModel();
            if (userModel.securityLogin() == false)
            {
                MessageBox.Show("Error Fatal, se detectó que está intentando acceder al sistema sin credenciales, por favor inicie sesión e indentifiquese", "Notificación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }

        }

        private void ManagePermissions()
        {
            //if (UserCache.Position == Positions.Accounting)
            //{
            //    btnPacient.Enabled = false;
            //    btnClinicalHistory.Enabled = false;
            //}
            //if (UserCache.Position == Positions.Receptionist)
            //{
            //    Button2.Enabled = false;
            //}
            //if (UserCache.Position == Positions.Administrator)
            //{
            //    //Codes
            //}
        }

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

        private void btnSubMenu2_2_Click(object sender, EventArgs e)
        {
            if (!UserCache.b18)
            {
                MessageBox.Show(ConfCache.MSG_NO_AUTORIZADO, "Información", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            frmListas childForm = (frmListas)Fun.AbrirFormulario(typeof(frmListas), false);
            this.WindowState = FormWindowState.Minimized;

            childForm.FormClosed += Logout;
        }

        

        private void btnSubMenu2_1_Click(object sender, EventArgs e)
        {
            if (!UserCache.b2)
            {
                 MessageBox.Show(ConfCache.MSG_NO_AUTORIZADO,"Información", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                 return;                
            }

            //frmInstalaciones childForm = (frmInstalaciones)Fun.AbrirFormularioInPanel(typeof(frmInstalaciones), this.panelChildForm);

            //openChildFormInPanel(new FormUserProfile());
            frmInstalaciones childForm = (frmInstalaciones)Fun.AbrirFormulario(typeof(frmInstalaciones), false);            
            //this.ShowInTaskbar
            this.WindowState = FormWindowState.Minimized;
           
            childForm.FormClosed += Logout;
            //..
            //your codes
            //..
            //hideSubMenu();
        }

        private void btnSubMenu1_2_Click(object sender, EventArgs e)
        {
            //openChildFormInPanel(new frmAcerca("Visoproy", "jeurbina1210@gmail.com"));
            //frmAcerca childForm = (frmAcerca)AbrirFormularioInPanel(typeof(frmAcerca));
            FrmAcerca childForm = (FrmAcerca)Fun.AbrirFormularioInPanel(typeof(FrmAcerca), this.panelChildForm);
            //AbrirFormularioInPanel<frmAcerca>();
            //openChildFormInPanel(new frmAcerca());
                  //childForm = (frmInstalaciones)AbrirVentana(typeof(frmInstalaciones), false);
            //..
            //your codes
            //..
            //hideSubMenu();
        }

        private void btnSubMenu2_3_Click(object sender, EventArgs e)
        {
            if (!UserCache.b18)
            {
                MessageBox.Show(ConfCache.MSG_NO_AUTORIZADO, "Información", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            frmLocalidades childForm = (frmLocalidades)Fun.AbrirFormulario(typeof(frmLocalidades), false);
            this.WindowState = FormWindowState.Minimized;

            childForm.FormClosed += Logout;
            //this.TopLevel = false;


            ////..
            ////your codes
            ////..
            //hideSubMenu();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnSubMenu2_4_Click(object sender, EventArgs e)
        {
            if (!UserCache.b18)
            {
                MessageBox.Show(ConfCache.MSG_NO_AUTORIZADO, "Información", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            frmClientes childForm = (frmClientes)Fun.AbrirFormulario(typeof(frmClientes), false);
            this.WindowState = FormWindowState.Minimized;

            childForm.FormClosed += Logout;
            //this.TopLevel = false;


            ////..
            ////your codes
            ////..
            //hideSubMenu();
        }

        private void btnSubMenu3_1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Módulo en desarrollo", "Información", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        private void btnSubMenu3_2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Módulo en desarrollo", "Información", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;

#pragma warning disable CS0162 // Se detectó código inaccesible
            if (!UserCache.b18)
#pragma warning restore CS0162 // Se detectó código inaccesible
            {
                MessageBox.Show(ConfCache.MSG_NO_AUTORIZADO, "Información", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            frmIncidencias childForm = (frmIncidencias)Fun.AbrirFormulario(typeof(frmIncidencias), false);
            this.WindowState = FormWindowState.Minimized;

            childForm.FormClosed += Logout;
        }

       

     

      

    }
}
