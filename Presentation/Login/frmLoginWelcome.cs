using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common.Cache;

namespace Presentation
{
#pragma warning disable CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
    public partial class frmLoginWelcome : Form
#pragma warning restore CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
    {
#pragma warning disable CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
        public frmLoginWelcome()
#pragma warning restore CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
        {
            InitializeComponent();
        }
        //int count = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (this.Opacity < 1) this.Opacity += 0.05;
            progressBar1.Value += 1;
            if (progressBar1.Value == 100)
            {
                timer1.Stop();
            timer2.Start();

            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            this.Opacity -= 0.1;
            if (this.Opacity == 0)
            {
                timer2.Stop();
                this.Close();
            }
        }

        private void FormWelcome_Load(object sender, EventArgs e)
        {
            lblUserName.Text = UserCache.Name;
            this.Opacity = 0.0;
            timer1.Start();
        }
    }
}
