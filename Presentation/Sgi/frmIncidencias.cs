using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Presentation.Sgi
{
#pragma warning disable CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
    public partial class frmIncidencias : Form
#pragma warning restore CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
    {
#pragma warning disable CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
        public frmIncidencias()
#pragma warning restore CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
        {
            InitializeComponent();
            Listas();
        }

        private void mnuCerrar_Click(object sender, EventArgs e)
        {

        }

        private void frmIncidencias_Load(object sender, EventArgs e)
        {

        }

        private void frmIncidencias_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void Listas()
        {
            cbxFiltro.Items.Clear();

            cbxFiltro.Items.Add("Sala");
            cbxFiltro.Items.Add("Centro de Servicio");            
            cbxFiltro.Items.Add("Subestación");
            cbxFiltro.Items.Add("Tensión (kV)");
            cbxFiltro.Items.Add("Grupo");
            cbxFiltro.Items.Add("Tipo de Incidencia");
            cbxFiltro.Items.Add("Estado en Operaciones");
            cbxFiltro.Items.Add("Alcance");
            cbxFiltro.Items.Add("Duración Incidencia");
            cbxFiltro.Items.Add("Tipo de Incidencia"); 
            
            //cbxFiltro.Items.Add("kW Tot");
            //cbxFiltro.SelectedIndex = 3;
        }

       
    }
}
