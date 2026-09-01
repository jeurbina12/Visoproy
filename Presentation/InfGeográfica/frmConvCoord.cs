using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Domain;
using System.Globalization;

namespace Presentation.InfGeográfica
{
    public partial class frmConvCoord : Form
    {
        CadModel cadModel = new CadModel();

#pragma warning disable CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
        public frmConvCoord()
#pragma warning restore CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
        {
            InitializeComponent();
        }

        private void btnAplicar_Click(object sender, EventArgs e)
        {
            NumberFormatInfo provider = new NumberFormatInfo();
            provider.NumberDecimalSeparator = ".";

            //string coor_dig = Microsoft.VisualBasic.Interaction.InputBox("Favor Ingrese Coordenadas Digital = ", "Conversión de Coordenadas", "", this.Location.X + this.Width / 2, this.Location.Y + this.Height / 2);
            string[] long_lati = txtDatos.Text.Split(',');
            //double.Parse(Banco.Substring(2, Banco.Length - 2), provider) 
            //double longi = Convert.ToDouble(long_lati[0]), lati = Convert.ToDouble(long_lati[1]);
            double longi = double.Parse(long_lati[0], provider), lati = double.Parse(long_lati[1], provider);

            double[] SelXY = new double[2];
            cadModel.CambioDG_UTM(ref   longi,ref lati, out SelXY[0], out SelXY[1]);
            txtResultados.Text = Math.Round(SelXY[0], 4).ToString().Replace(",", ".") + "," + Math.Round(SelXY[1], 4).ToString().Replace(",", ".");
        }

        private void btnCopiar_Click(object sender, EventArgs e)
        {
            try
            {
                Clipboard.SetDataObject(txtResultados.Text, true);
                MessageBox.Show("Texto copiado al portapapeles de Windows.",
            "Copiado", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception err)
            {
                MessageBox.Show("Error al copiar texto al portapapeles: " +
                    Environment.NewLine + err.Message, "Error al copiar",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
