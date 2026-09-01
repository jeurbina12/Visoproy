using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using Presentation.Util;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
namespace Presentation
{
#pragma warning disable CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
    public partial class FrmAcerca : Form
#pragma warning restore CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
    {
        private string TopCaption = "Acerca de " + Application.ProductName;

#pragma warning disable CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
        public FrmAcerca()
#pragma warning restore CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
        {
            InitializeComponent();
            this.lblProductoNombre.Text = Application.ProductName.Length <= 0 ? "{Product Name}" : Application.ProductName;
            this.lblCompany.Text = Application.CompanyName;
            this.lblProductoVersión.Text ="Versión: "+ Application.ProductVersion;
            this.lblCopyright.Text = "Elaborado por MSc. Jesús Urbina, 2012";
            //this.lblIp.Text = "Ip: " + localIPAddress();
            //this.lblIp.Text = "Ip: " + GetLocalIPAddress();
            this.lblIp.Text = "Ip: " + GetLocalIPv4(NetworkInterfaceType.Ethernet);
            //this.TopCaption = TopCaption;
            //this.lblLink.Text = Link;
        }

        //public frmAcerca(string TopCaption, string Link)
        //{
        //    InitializeComponent();
        //    this.lblProductoNombre.Text = Application.ProductName.Length <= 0 ? "{Product Name}" : Application.ProductName;
        //    this.lblCompany.Text = Application.CompanyName;
        //    this.lblProductoVersión.Text = Application.ProductVersion;
        //    this.TopCaption = TopCaption;
        //    this.lblLink.Text = Link;
        //}

        private void topPanel_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawIcon(Icon.ExtractAssociatedIcon(Application.ExecutablePath), 20, 8);
            e.Graphics.DrawString(TopCaption, new Font("Segoe UI", 14f), Brushes.Azure, new PointF(70, 10));
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void linkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(this.lblLink.Text);
            }
            catch { }
        }

#pragma warning disable CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
        public static string GetLocalIPAddress()
#pragma warning restore CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

#pragma warning disable CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
        public static string localIPAddress()
#pragma warning restore CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
        {
            IPHostEntry host;
            string localIP = "";
            host = Dns.GetHostEntry(Dns.GetHostName());

            foreach (IPAddress ip in host.AddressList)
            {
                localIP = ip.ToString();

                string[] temp = localIP.Split('.');

                if (ip.AddressFamily == AddressFamily.InterNetwork && temp[0] == "192")
                {
                    break;
                }
                else
                {
                    localIP = null;
                }
            }

            return localIP;
        }

#pragma warning disable CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
        public string GetLocalIPv4(NetworkInterfaceType _type)
#pragma warning restore CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
        {
            string output = "";
            foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (item.NetworkInterfaceType == _type && item.OperationalStatus == OperationalStatus.Up)
                {
                    foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            output = ip.Address.ToString();
                        }
                    }
                }
            }
            return output;
        }
    }
}
