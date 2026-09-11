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
using System.IO;
using System.Linq;

namespace Presentation
{
#pragma warning disable CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
    public partial class frmPrincipal : Form
#pragma warning restore CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
    {
        private const string MSG_MODULE_DEV = "Módulo en desarrollo";
        private const string MSG_ERROR_OPEN = "Error al abrir módulo: ";
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
            // Tooltips para botones de la barra de título
            try
            {
                var tt = new ToolTip();
                tt.SetToolTip(this.btnCerrar, "Cerrar");
                tt.SetToolTip(this.btnMaximizar, "Maximizar");
                tt.SetToolTip(this.btnMinimizar, "Minimizar");
                tt.SetToolTip(this.btnRestaurar, "Restaurar");
                // Tooltips para botones del menú lateral (si existen)
                tt.SetToolTip(this.btnMenu, "Menú");
                tt.SetToolTip(this.btnMenu1, "Módulos");
                tt.SetToolTip(this.btnMenu2, "Operaciones");
                tt.SetToolTip(this.btnMenu3, "Ayuda");
            }
            catch { }

            // Asegurar panelChildForm dock
            this.panelChildForm.Dock = DockStyle.Fill;

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

        /// <summary>
        /// Helper para abrir módulos (formularios) con comprobación opcional de permisos
        /// </summary>
        /// <param name="formType">Tipo del formulario a abrir (typeof(MiForm))</param>
        /// <param name="permiso">Función que retorna true si el usuario tiene permiso (puede ser null)</param>
        /// <param name="openInPanel">Si true se abre dentro de panelChildForm, si false abre como ventana y minimiza el principal</param>
        private Form OpenModule(Type formType, Func<bool> permiso = null, bool openInPanel = false)
        {
            try
            {
                if (permiso != null && !permiso())
                {
                    // permiso() debe encargarse de mostrar mensaje cuando sea necesario
                    return null;
                }

                Form result = null;
                if (openInPanel)
                {
                    result = (Form)Fun.AbrirFormularioInPanel(formType, this.panelChildForm);
                }
                else
                {
                    Form childForm = (Form)Fun.AbrirFormulario(formType, false);
                    result = childForm;
                    if (childForm != null)
                    {
                        this.WindowState = FormWindowState.Minimized;
                        childForm.FormClosed += Logout;
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                LogError(ex);
                MessageBox.Show(MSG_ERROR_OPEN + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        /// <summary>
        /// Comprueba permiso y muestra mensaje estandarizado si no lo tiene
        /// </summary>
        /// <param name="permisoFlag">flag de permiso (UserCache.bX)</param>
        /// <returns>true si tiene permiso</returns>
        private bool HasPermission(bool permisoFlag)
        {
            if (!permisoFlag)
            {
                MessageBox.Show(ConfCache.MSG_NO_AUTORIZADO, "Información", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Registra excepciones en un fichero de logs (logs/error.log)
        /// </summary>
        /// <param name="ex">Excepción a registrar</param>
        private void LogError(Exception ex)
        {
            try
            {
                string dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
                Directory.CreateDirectory(dir);
                string path = Path.Combine(dir, "error.log");
                File.AppendAllText(path, DateTime.Now.ToString("s") + " - " + ex.ToString() + Environment.NewLine);
            }
            catch
            {
                // Ignorar errores de logging para no romper la aplicación
            }
        }



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
            OpenModule(typeof(frmInstalaciones), () => HasPermission(UserCache.b2), true);
            //..
            //your codes
            //..
            //hideSubMenu();
        }

        private void btnSubMenu1_2_Click(object sender, EventArgs e)
        {
            OpenModule(typeof(FrmAcerca), null, true);
        }

        private void btnSubMenu2_3_Click(object sender, EventArgs e)
        {
            OpenModule(typeof(frmLocalidades), () => HasPermission(UserCache.b18), true);
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
            OpenModule(typeof(frmClientes), () => HasPermission(UserCache.b18), true);
            //this.TopLevel = false;


            ////..
            ////your codes
            ////..
            //hideSubMenu();
        }

        private void btnSubMenu3_1_Click(object sender, EventArgs e)
        {
            MessageBox.Show(MSG_MODULE_DEV, "Información", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        private void btnSubMenu3_2_Click(object sender, EventArgs e)
        {
            MessageBox.Show(MSG_MODULE_DEV, "Información", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;

#pragma warning disable CS0162 // Se detectó código inaccesible
            if (!UserCache.b18)
#pragma warning restore CS0162 // Se detectó código inaccesible
            {
                MessageBox.Show(ConfCache.MSG_NO_AUTORIZADO, "Información", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            OpenModule(typeof(frmIncidencias), () => HasPermission(UserCache.b18), true);
        }

       

     

      

    }
}
