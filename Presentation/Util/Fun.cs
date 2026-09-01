using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using System.Collections;
//using System.Windows.Forms;
//using System.Collections;

namespace Presentation.Util
{
    class Fun : FunBase
    {
        private static Fun _instance = null;
        private static SortedList formInstances = new SortedList(); // Para guardar las referencias de las instancias de los formularios


        public static Fun Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Fun();
                return Fun._instance;
            }
            //set { Fun._instance = value; }
        }

        private Fun() { }

        public static void FormularioCerrar(Type type)
        {
            Form formulario;
            if ((formulario = (Form)formInstances[type.ToString()]) == null)
            {
            //    return false;
            }
            else
            {
                formulario.Dispose();
                //return true;
            }
           
        }

        public static Form AbrirFormulario(Type type, bool dialog)
        {
            Form formulario;
            if ((formulario = (Form)formInstances[type.ToString()]) == null || formulario.IsDisposed)
            {
                formulario = (Form)Activator.CreateInstance(type);
                formInstances[type.ToString()] = formulario; 
            }  
                formulario.Activate();
                if (dialog)
                    formulario.ShowDialog();
                else
                    formulario.Show();            
            return formulario;
        }

        public static Form AbrirFormularioInPanel(Type type, Panel panelChildForm)
        {
            Form formulario;
            if ((formulario = (Form)formInstances[type.ToString()]) == null || formulario.IsDisposed)
            {
                formulario = (Form)Activator.CreateInstance(type);
                formInstances[type.ToString()] = formulario;

                //if (panelChildForm.Controls.Count > 0)
                //    panelChildForm.Controls.RemoveAt(0);
                formulario.TopLevel = false;
                formulario.FormBorderStyle = FormBorderStyle.None;
                formulario.Dock = DockStyle.Fill;
                panelChildForm.Controls.Add(formulario);
                panelChildForm.Tag = formulario;
                formulario.Show();
            }            
               
                formulario.BringToFront();
           
            return formulario;
        }

        /// <summary>
        /// Obtiene el valor de la clave
        /// </summary>
        public object GetValor(XmlNode nodoPadre, string clave)
       {
           try
           {
               XmlNode valor = nodoPadre.SelectSingleNode(clave);
               return valor.LastChild.Value;
           }
           catch
           {
               return "";
           }
       }

       internal static bool IsNumeric(string str)//this
       {
           double _val;
           bool valor = double.TryParse(str, out _val);
           return valor;
       }
       internal static Boolean isSingle(String num)
       {
           try
           {
               Single.Parse(num);
               return true;
           }
           catch
           {
               return false;
           }
       }
       internal static Boolean isShort(String num)
       {
           try
           {
               short.Parse(num);
               return true;
           }
           catch
           {
               return false;
           }
       }
       internal static Boolean isInt(String num)
       {
           try
           {
               int.Parse(num);
               return true;
           }
           catch
           {
               return false;
           }
       }
       internal static Boolean isInt32(String num)
       {
           try
           {
               Int32.Parse(num);
               return true;
           }
           catch
           {
               return false;
           }
       }
       internal static bool IsLetter(KeyPressEventArgs e)
       {
           return (e.KeyChar >= 65 && e.KeyChar <= 90) ||
                   (e.KeyChar >= 97 && e.KeyChar <= 122) ||
                   e.KeyChar == 8 || e.KeyChar == 'Ñ'
                   || e.KeyChar == 'ñ' || e.KeyChar == 32;
       }
       internal static bool IsDate(string str)//this
       {
           DateTime _val;
           bool valor = DateTime.TryParse(str, out _val);
           return valor;
       }

       internal static double funMVA(String tAmp, String Volt)
       {
           return Math.Round(Convert.ToDouble(tAmp) * Convert.ToDouble(Volt) * Math.Sqrt(3) / 1000, 2);

       }
       internal static double funFU(String Demanda, String Capacidad)
       {
           if (Capacidad.Equals(""))
           {
               return 0.0;
           }
           return Math.Round(Convert.ToDouble(Demanda) * 100 / Convert.ToDouble(Capacidad), 2);

       }
      

    }

    //public class ComboBoxItem
    //{
    //    public string id;
    //    public string nombre;

    //    public ComboBoxItem(string _id, string _nombre)
    //    {
    //        this.id = _id.TrimEnd();
    //        this.nombre = _nombre.TrimEnd();
    //    }

    //    public override string ToString()
    //    {
    //        return id;
    //    }
    //}   


}
