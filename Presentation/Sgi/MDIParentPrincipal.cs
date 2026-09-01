using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using Presentation.Util;
using Presentation.InfGeográfica;

namespace Presentation.Sgi
{
#pragma warning disable CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
    public partial class MDIParentPrincipal : Form
#pragma warning restore CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
    {
        //private int childFormNumber = 0;
        private static SortedList formInstances = new SortedList(); // Para guardar las referencias de las instancias de los formularios

#pragma warning disable CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
        public MDIParentPrincipal()
#pragma warning restore CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
        {
            InitializeComponent();
        }

        private void ShowNewForm(object sender, EventArgs e)
        {
            //Form childForm = new Form();            
            frmInstalaciones childForm = (frmInstalaciones)AbrirVentana(typeof(frmInstalaciones),false);
            //frmInstalaciones childForm = new frmInstalaciones();
            //childForm.MdiParent = this;
            //childForm.Text =childForm.Name+ "Ventana " + childFormNumber++;
            //childForm.WindowState = FormWindowState.Maximized;
            //childForm.Show();
            
        }

        private void OpenFile(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            openFileDialog.Filter = "Archivos de texto (*.txt)|*.txt|Todos los archivos (*.*)|*.*";
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = openFileDialog.FileName;
            }
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            saveFileDialog.Filter = "Archivos de texto (*.txt)|*.txt|Todos los archivos (*.*)|*.*";
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = saveFileDialog.FileName;
            }
        }

        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CutToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void ToolBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStrip.Visible = toolBarToolStripMenuItem.Checked;
        }

        private void StatusBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            statusStrip.Visible = statusBarToolStripMenuItem.Checked;
        }

        private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void TileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void ArrangeIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

#pragma warning disable CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
        public static Form AbrirVentana(Type type)//static
#pragma warning restore CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
        {
            return AbrirVentana(type, false);
        }

#pragma warning disable CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
        public static Form AbrirVentana(Type type, bool dialog)//static
#pragma warning restore CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
        {
            Form formulario;
            if ((formulario = (Form)formInstances[type.ToString()]) == null || formulario.IsDisposed)
            {
                formulario = (Form)Activator.CreateInstance(type);
                formInstances[type.ToString()] = formulario;
            }

            formulario.Activate();
            //formulario.WindowState = FormWindowState.Normal;
            //formulario.MdiParent = this;
            //formulario.Text = formulario.Name + "Ventana " + childFormNumber++;
            //formulario.Text = formulario.Text;
            if (dialog)
                formulario.ShowDialog();
            else
                formulario.Show();

            return formulario;
        }
    }
}
