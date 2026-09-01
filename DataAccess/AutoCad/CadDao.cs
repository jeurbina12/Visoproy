using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Common.Cache;
using AutoCAD;
using Microsoft.VisualBasic;
using Npgsql;
using NpgsqlTypes;
using System.Globalization;
using System.Threading;

//using System;
//using System.Collections.Generic;
//using System.Text;


namespace DataAccess.AutoCad
{
    public class CadDao //: ConnectionToPool
    {
        /* 
       *  AutoCAD.Application.16   = AutoCAD 2004 
       *  AutoCAD.Application.16.1 = AutoCAD 2005 
       *  AutoCAD.Application.16.2 = AutoCAD 2006 
       *  AutoCAD.Application.17   = AutoCAD 2007 
       *  AutoCAD.Application.17.1 = AutoCAD 2008          
       *  AutoCAD.Application.20   = AutoCAD 2015 
       */
        //const string ID_version = "AutoCAD.Application.17.1"; 
        //const string ID_VERSION = "AutoCAD.Application.17";
        //const string ID_version = "AutoCAD.Application.20";
        const string ID_VERSION = "AutoCAD.Application.24";
        //[STAThread]
        private AcadApplication OBJACAD = null;
        private AcadDocument DOC = null;
        // Set current thread culture to en-US. es-VE
        //IFormatProvider provider = CultureInfo.CreateSpecificCulture("en-US");
        //IFormatProvider provider = System.Globalization.CultureInfo.InvariantCulture;
        //        private string S_DEC_SEP = Convert.ToString(System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator);
        //        NumberFormatInfo provider = new NumberFormatInfo();
        //private provider.NumberDecimalSeparator = ".";
        //provider.NumberGroupSeparator = ",";

        public bool IniciarAutoCAD()
        {
            try
            {
                OBJACAD = (AcadApplication)Marshal.GetActiveObject(ID_VERSION);
                Type acType = Type.GetTypeFromProgID(ID_VERSION);
                OBJACAD = (AcadApplication)Activator.CreateInstance(acType, true);
                return true;
            }
            catch
            {
                return false;
                //MessageBox.Show("Cannot create object of type \"" + ID_VERSION + "\"");
            }
            //}

            //if (objAcad != null)
            //{
            //    objAcad.Application.Documents.Open(System.Windows.Forms.Application.StartupPath + "\\plano.dwg", true, "");
            //    //objAcad.WindowState = Autodesk.AutoCAD.Interop.Common.AcWindowState.acMax;
            //    objAcad.WindowState = AutoCAD.AcWindowState.acMax;
            //    objAcad.Visible = true;
            //    doc = objAcad.ActiveDocument;
            //}
        }

        public bool coord_zoom(double X, double Y, int Zoom, int Marcar, string Layer)
        {
            double[] insertionPnt;
            insertionPnt = new double[3];
            insertionPnt[0] = X;
            insertionPnt[1] = Y;
            insertionPnt[2] = 0;

            double[] insertionPnt1, insertionPnt2;
            insertionPnt1 = new double[3];
            insertionPnt1[2] = 0;
            insertionPnt2 = new double[3];
            insertionPnt2[2] = 0;

            try
            {
                OBJACAD = (AcadApplication)Marshal.GetActiveObject(ID_VERSION);
                DOC = OBJACAD.ActiveDocument;
                insertionPnt1[0] = X - 125 / 2 * Zoom;
                insertionPnt1[1] = Y - 62.5 / 2 * Zoom;
                insertionPnt2[0] = X + 125 / 2 * Zoom;
                insertionPnt2[1] = Y + 62.5 / 2 * Zoom;

                DOC.Application.ZoomWindow(insertionPnt1, insertionPnt2);
                if (Marcar > 0)
                {
                    LayerNuevo(Layer);
                    DOC.ModelSpace.AddCircle(insertionPnt, Marcar).Update();
                    //doc.SendCommand("zoom" + vbCr + "W" + vbCr + x1 + "," + y1 + vbCr + x2 + "," + y2 + vbCr);
                }

                return true;
            }

            catch (System.Exception)
            {
                // Algo ha pasado...
                //MessageBox.Show("Error: " + ex.Message);
                return false;
            }

        }

        public double[] coord_obtener()
        {
            try
            {
                double[] insertionPnt;
                insertionPnt = new double[3];

                OBJACAD = (AcadApplication)Marshal.GetActiveObject(ID_VERSION);
                DOC = OBJACAD.ActiveDocument;
                insertionPnt = DOC.Utility.GetPoint(Type.Missing, "Hacer Click para obtener coordenadas") as double[];
                return insertionPnt;
                //abierto = true;
            }
            //catch (System.Exception ex)
            catch
            {
                return null;
                //// Algo ha pasado...
                //abierto = false;
                //MessageBox.Show("Error en la aplicación de AutoCAD ");
                //MessageBox.Show("Error: " + ex.Message);

                //return false;
            }



        }

        public bool coord_marcar(double x, double y, int Marcar, string Layer)
        {
            try
            {

                OBJACAD = (AcadApplication)Marshal.GetActiveObject(ID_VERSION);
                DOC = OBJACAD.ActiveDocument;

                double[] insertionPnt;
                insertionPnt = new double[3];
                insertionPnt[0] = x;
                insertionPnt[1] = y;
                insertionPnt[2] = 0;

                LayerNuevo(Layer);
                DOC.ModelSpace.AddCircle(insertionPnt, Marcar).Update();
                return true;
            }
            catch
            {
                return false;
                //MessageBox.Show("Tiene que abrir el archivo CAD ");
            }
        }

        public bool bloq_dibujar(double x, double y, string codigo, string sivplus, string capacidad, string bloque, string Layer, double esc, double grados)
        {
            try
            {
                OBJACAD = (AcadApplication)Marshal.GetActiveObject(ID_VERSION);
                DOC = OBJACAD.ActiveDocument;

                double[] insertionPnt;
                insertionPnt = new double[3];
                insertionPnt[0] = x;
                insertionPnt[1] = y;
                insertionPnt[2] = 0;
                //string cod2=
                LayerNuevo(Layer);

                AcadBlockReference blockRefObj = DOC.ModelSpace.InsertBlock(insertionPnt, string.Format("C:\\VSG\\dwg\\{0}.dwg", bloque), 1, 1, 1, 0, System.Type.Missing);
                System.Object[] attDef = blockRefObj.GetAttributes() as object[];

                for (int i = 0; i < attDef.Length; i++)
                {
                    AcadAttributeReference attRef;
                    attRef = (AcadAttributeReference)attDef[i];
                    if (attRef.TagString == "CAPACIDAD" || attRef.TagString == "TIPO")//|| attRef.TagString == "INMUEBLE1"
                    {
                        attRef.TextString = capacidad;
                    }
                    else if (attRef.TagString == "IDSIV" || attRef.TagString == "NOMBRE" || attRef.TagString == "SIVPLUS")// attRef.TagString == "PUERTA" ||
                    {
                        attRef.TextString = sivplus;
                    }
                    //else if (attRef.TagString == "NIF")
                    //{
                    //    attRef.TextString = capacidad;
                    //}
                    else if (attRef.TagString == "NPLA")
                    {
                        if (codigo.Length == 7)
                        {
                            attRef.TextString = codigo.Substring(0, 3); ;
                        }
                        else
                        {
                            attRef.TextString = codigo.Substring(0, 5); ;
                        }
                    }
                    else if (attRef.TagString == "NPOSTE")
                    {
                        if (codigo.Length == 7)
                        {
                            attRef.TextString = codigo.Substring(3, 4); ;
                        }
                        else
                        {
                            attRef.TextString = codigo.Substring(5, 3); ;
                        }
                    }
                    //else if (attRef.TagString == "NIF")
                    //{
                    //    attRef.TextString = codigo;
                    //}
                    else if (attRef.TagString == "ID")
                    {
                        attRef.TextString = codigo.ToString();
                    }
                }

                blockRefObj.XScaleFactor = esc;
                blockRefObj.YScaleFactor = esc;
                blockRefObj.ZScaleFactor = esc;

                blockRefObj.Rotation = grados / 180 * Math.PI;

                blockRefObj.Update();

                return true;
                //doc.SendCommand("listo");
                //doc.SendCommand("-insert " + "ECADTR05" + vbCr);

            }
            catch
            {
                return false;
            }
        }

        public bool bloq_dibujar_cliente(double x, double y, string id, string nif, string inm1, string inm2, string puerta, string bloque, string Layer, double esc, double grados)
        {
            try
            {
                OBJACAD = (AcadApplication)Marshal.GetActiveObject(ID_VERSION);
                DOC = OBJACAD.ActiveDocument;

                double[] insertionPnt;
                insertionPnt = new double[3];
                insertionPnt[0] = x;
                insertionPnt[1] = y;
                insertionPnt[2] = 0;
                //string cod2=
                LayerNuevo(Layer);

                AcadBlockReference blockRefObj = DOC.ModelSpace.InsertBlock(insertionPnt, string.Format("C:\\VSG\\dwg\\{0}.dwg", bloque), 1, 1, 1, 0, System.Type.Missing);
                System.Object[] attDef = blockRefObj.GetAttributes() as System.Object[];

                for (int i = 0; i < attDef.Length; i++)
                {
                    AcadAttributeReference attRef;
                    attRef = (AcadAttributeReference)attDef[i];
                    if (attRef.TagString == "INMUEBLE1")
                    {
                        attRef.TextString = inm1;
                    }
                    else if (attRef.TagString == "INMUEBLE2")
                    {
                        attRef.TextString = inm2;
                    }
                    else if (attRef.TagString == "NIF")
                    {
                        attRef.TextString = nif;
                    }
                    else if (attRef.TagString == "PUERTA")
                    {
                        attRef.TextString = puerta;
                    }

                    else if (attRef.TagString == "ID")
                    {
                        attRef.TextString = id.ToString();
                    }
                }

                blockRefObj.XScaleFactor = esc;
                blockRefObj.YScaleFactor = esc;
                blockRefObj.ZScaleFactor = esc;

                blockRefObj.Rotation = grados / 180 * Math.PI;

                blockRefObj.Update();

                return true;
                //doc.SendCommand("listo");
                //doc.SendCommand("-insert " + "ECADTR05" + vbCr);

            }
            catch
            {
                return false;
            }
        }

        public bool bloq_leer(out double x, out double y, out string text1, out string text2, out string capacidad, out string sivplus, out string bloque, out string capa, out double esc, out Int16 grado)
        {

            try
            {
                OBJACAD = (AcadApplication)Marshal.GetActiveObject(ID_VERSION);
                DOC = OBJACAD.ActiveDocument;

                Object out1 = new object();
                object point = new object();

                DOC.Utility.GetEntity(out out1, out point, "\nSeleccione Bloque:");

                AcadBlockReference blockRefObj = out1 as AcadBlockReference;
                //System.Double[] blockPoint = (System.Double[])point;
                System.Double[] blockPoint = blockRefObj.InsertionPoint as double[];

                if (blockRefObj == null)
                {
                    //picked object is not AcadLWPolyline, prompt user, or stop the process
                    //MessageBox.Show("Error, no selecciono un bloque ");
                    grado = 0;
                    esc = 0;
                    capa = null;
                    bloque = null;
                    sivplus = null;
                    capacidad = null;
                    //id = null;
                    text1 = null;
                    text2 = null;
                    x = 0;
                    y = 0;
                    return false;
                }
                else
                {
                    //do something with the polyline "pl"
                    //MessageBox.Show("selecciono un bloque " + blockRefObj.Name);

                    //insertionPnt = blockRefObj.InsertionPoint;
                    x = blockPoint[0]; y = blockPoint[1];
                    esc = (double)blockRefObj.XScaleFactor;
                    double rota = blockRefObj.Rotation / Math.PI * 180;
                    grado = (Int16)Math.Round(rota);
                    bloque = blockRefObj.Name;
                    capa = blockRefObj.Layer;
                    sivplus = "";
                    capacidad = "";
                    //id = "";
                    text1 = "";
                    text2 = "";
                    //if (blockRefObj.Name.Equals("SUSCRIP2"))
                    //{
                    //    text2 = "08";
                    //}
                    //else { text2 = "06"; }                  


                    //AcadBlockReference blockRefObj = doc.ModelSpace.InsertBlock(insertionPnt, "C:\\VSG\\dwg\\ECADTR05.dwg", 1, 1, 1, 0, System.Type.Missing);
                    System.Object[] attDef = blockRefObj.GetAttributes() as System.Object[];


                    for (int i = 0; i < attDef.Length; i++)
                    {
                        AcadAttributeReference attRef;
                        attRef = (AcadAttributeReference)attDef[i];
                        if (attRef.TagString == "ID")
                        {
                            text1 = attRef.TextString;
                        }
                        else if (attRef.TagString == "NPLA")
                        {
                            text1 = attRef.TextString;
                        }
                        else if (attRef.TagString == "NPOSTE")
                        {
                            text2 = attRef.TextString;
                        }
                        else if (attRef.TagString == "CAPACIDAD")
                        {
                            capacidad = attRef.TextString;
                        }
                        else if (attRef.TagString == "SIVPLUS" || attRef.TagString == "NIF" || attRef.TagString == "IDENTIFICADOS" || attRef.TagString == "IDENTIFICADOS_Y_NO_IDENTIFICADOS")
                        {
                            sivplus = attRef.TextString;
                        }


                    }
                    return true;
                }

                //blockRefObj.Update();



                //doc.Application.Update();
                //doc.SendCommand("listo");
                //doc.SendCommand("-insert " + "ECADTR05" + vbCr);
            }
            catch (System.Exception)
            {
                // Algo ha pasado...
                grado = 0;
                esc = 0;
                capa = null;
                bloque = null;
                sivplus = null;
                capacidad = null;
                //id = null;
                text1 = null;
                text2 = null;
                x = 0;
                y = 0;
                return false;
                //MessageBox.Show("Error: " + ex.Message);
                //return false;
            }
        }

        public bool bloq_leer_cliente(out double x, out double y, out string id, out string nif, out string inm1, out string inm2, out string puerta, out string bloque, out string capa, out double esc, out Int16 grado)
        {

            try
            {
                OBJACAD = (AcadApplication)Marshal.GetActiveObject(ID_VERSION);
                DOC = OBJACAD.ActiveDocument;

                Object out1 = new object();
                object point = new object();

                DOC.Utility.GetEntity(out out1, out point, "\nSeleccione Bloque:");

                AcadBlockReference blockRefObj = out1 as AcadBlockReference;
                //System.Double[] blockPoint = (System.Double[])point;
                System.Double[] blockPoint = blockRefObj.InsertionPoint as System.Double[];

                if (blockRefObj == null)
                {
                    //picked object is not AcadLWPolyline, prompt user, or stop the process
                    //MessageBox.Show("Error, no selecciono un bloque ");
                    grado = 0;
                    esc = 0;
                    capa = null;
                    bloque = null;

                    inm1 = null;
                    inm2 = null;
                    puerta = null;
                    id = null;
                    nif = null;
                    x = 0;
                    y = 0;
                    return false;
                }
                else
                {
                    //do something with the polyline "pl"
                    //MessageBox.Show("selecciono un bloque " + blockRefObj.Name);

                    //insertionPnt = blockRefObj.InsertionPoint;
                    x = blockPoint[0]; y = blockPoint[1];
                    esc = (double)blockRefObj.XScaleFactor;
                    double rota = blockRefObj.Rotation / Math.PI * 180;
                    grado = (Int16)Math.Round(rota);
                    bloque = blockRefObj.Name;
                    capa = blockRefObj.Layer;
                    inm2 = "";
                    inm1 = "";
                    //id = "";
                    id = "";
                    nif = "";
                    puerta = "";
                    //if (blockRefObj.Name.Equals("SUSCRIP2"))
                    //{
                    //    text2 = "08";
                    //}
                    //else { text2 = "06"; }                  


                    //AcadBlockReference blockRefObj = doc.ModelSpace.InsertBlock(insertionPnt, "C:\\VSG\\dwg\\ECADTR05.dwg", 1, 1, 1, 0, System.Type.Missing);
                    System.Object[] attDef = blockRefObj.GetAttributes() as System.Object[];


                    for (int i = 0; i < attDef.Length; i++)
                    {
                        AcadAttributeReference attRef;
                        attRef = (AcadAttributeReference)attDef[i];
                        if (attRef.TagString == "ID")
                        {
                            id = attRef.TextString;
                        }
                        else if (attRef.TagString == "NIF")
                        {
                            //attRef.TextString = capacidad;
                            nif = attRef.TextString;
                        }

                        else if (attRef.TagString == "IDENTIFICADOS" || attRef.TagString == "IDENTIFICADOS_Y_NO_IDENTIFICADOS" || attRef.TagString == "ANTENAS")
                        {
                            nif = attRef.TextString;
                        }
                        else if (attRef.TagString == "INMUEBLE1")
                        {
                            inm1 = attRef.TextString;
                        }
                        else if (attRef.TagString == "INMUEBLE2")
                        {
                            inm2 = attRef.TextString;
                        }
                        else if (attRef.TagString == "PUERTA")
                        {
                            puerta = attRef.TextString;
                        }


                    }
                    return true;
                }

                //blockRefObj.Update();



                //doc.Application.Update();
                //doc.SendCommand("listo");
                //doc.SendCommand("-insert " + "ECADTR05" + vbCr);
            }
            catch (System.Exception)
            {
                // Algo ha pasado...
                grado = 0;
                esc = 0;
                capa = null;
                bloque = null;
                inm2 = null;
                inm1 = null;
                puerta = null;
                id = null;
                nif = null;
                x = 0;
                y = 0;
                return false;
                //MessageBox.Show("Error: " + ex.Message);
                //return false;
            }
        }

        public string bloq_codificar()
        {
            try
            {
                OBJACAD = (AcadApplication)Marshal.GetActiveObject(ID_VERSION);
                DOC = OBJACAD.ActiveDocument;

                Object out1 = new object();
                object point = new object();

                DOC.Utility.GetEntity(out out1, out point, "\nSeleccione Bloque para crear CÓDIGO GEOGRÁFICO:");

                AcadBlockReference blockRefObj = out1 as AcadBlockReference;

                System.Double[] blockPoint = blockRefObj.InsertionPoint as System.Double[];
                double x = blockPoint[0]; double y = blockPoint[1];


                if (blockRefObj == null)
                    return ConfCache.MSG_CAD_ER_SELEC;

                System.Object[] attDef = blockRefObj.GetAttributes() as System.Object[];

                string cod = codigo_obtener(x, y);

                if (cod.Length < 5)
                    return cod;
                //MessageBox.Show("código asignado: " + CódigoGeo);
                for (int i = 0; i < attDef.Length; i++)
                {
                    AcadAttributeReference attRef;
                    attRef = (AcadAttributeReference)attDef[i];
                    if (attRef.TagString == "ID")
                    {
                        attRef.TextString = cod;
                    }
                    else if (attRef.TagString == "NPLA")
                    {
                        if (cod.Length == 7)
                        {
                            attRef.TextString = cod.Substring(0, 3);
                        }
                        else
                        {
                            attRef.TextString = cod.Substring(0, 5);
                        }
                    }
                    else if (attRef.TagString == "NPOSTE")
                    {
                        if (cod.Length == 7)
                        {
                            attRef.TextString = cod.Substring(3, 4);
                        }
                        else
                        {
                            attRef.TextString = cod.Substring(5, 3);
                        }
                    }
                }

                blockRefObj.Update();

                return cod;
            }
            catch
            {
                return "-1";
                // Algo ha pasado...
                //abierto = false;
                //MessageBox.Show("Error: " + ex.Message);
                //return false;
            }

        }

        public bool bloq_esc(double esc)
        {
            try
            {
                OBJACAD = (AcadApplication)Marshal.GetActiveObject(ID_VERSION);
                DOC = OBJACAD.ActiveDocument;

                Object out1 = new object();
                object point = new object();
                //string cap, bloque;

                DOC.Utility.GetEntity(out out1, out point, "\nSeleccione Bloque para ajustar escala:");

                AcadBlockReference blockRefObj = out1 as AcadBlockReference;
                //System.Double[] blockPoint = (System.Double[])point;
                //System.Double[] blockPoint = (System.Double[])blockRefObj.InsertionPoint;

                if (blockRefObj == null)
                {
                    //picked object is not AcadLWPolyline, prompt user, or stop the process
                    //MessageBox.Show("Error, no selecciono un bloque ");               

                    return false;
                }
                else
                {
                    //bloque = blockRefObj.Name;

                    blockRefObj.XScaleFactor = esc;
                    blockRefObj.YScaleFactor = esc;
                    blockRefObj.ZScaleFactor = esc;
                    blockRefObj.Update();

                    return true;
                }

            }
            catch (System.Exception)
            {
                // Algo ha pasado...
                return false;
                //MessageBox.Show("Error: " + ex.Message);
                //return false;
            }
        }

        public bool Activar_BLIPMODE(string opt)
        {
            try
            {
                OBJACAD = (AcadApplication)Marshal.GetActiveObject(ID_VERSION);
                DOC = OBJACAD.ActiveDocument;
                //DOC.SendCommand("BLIPMODE");
                string vbCr = " ";
                //doc.SendStringToExecute("-layer T,Capa,0 4 ", true, false, true);
                DOC.SendCommand("BLIPMODE" + vbCr + opt + vbCr);
                if (opt.Equals("OFF"))
                    DOC.SendCommand("REDRAW" + vbCr);
                return true;
            }
            catch
            {
                return false;
                //MessageBox.Show("Cannot create object of type \"" + ID_VERSION + "\"");
            }
        }

        public bool LayerNuevo(string Capa)
        {
            try
            {
                OBJACAD = (AcadApplication)Marshal.GetActiveObject(ID_VERSION);
                DOC = OBJACAD.ActiveDocument;
                DOC.ActiveLayer = DOC.Layers.Add(Capa);
                return true;
            }
            catch
            {
                return LayerNuevo2(Capa);
                //MessageBox.Show("Tiene que abrir el archivo CAD er1");
            }
        }

        public bool LayerNuevo2(string Capa)
        {
            try
            {
                OBJACAD = (AcadApplication)Marshal.GetActiveObject(ID_VERSION);
                DOC = OBJACAD.ActiveDocument;
                //doc.ActiveLayer = doc.Layers.Item();
                //doc.ActiveLayer = doc.Layers.Add(Capa);
                string vbCr = " ";
                //doc.SendStringToExecute("-layer T,Capa,0 4 ", true, false, true);
                DOC.SendCommand("-layer" + vbCr + "T" + vbCr + Capa + "\n" + "M" + vbCr + Capa + "\n" + vbCr);
                //doc.SendCommand("-layer" + vbCr + "T" + vbCr + Capa + vbCr + "T" + vbCr + Capa + vbCr);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool LayerBorrar(string Capa)
        {
            try
            {


                OBJACAD = (AcadApplication)Marshal.GetActiveObject(ID_VERSION);
                DOC = OBJACAD.ActiveDocument;
                //doc.ActiveLayer = doc.ActiveLayer.re;

                string vbCr = " ";
                DOC.SendCommand("(LOAD " + @"""C:/VSG/LISP/DELLAYER""" + ")" + vbCr + "DELLAYER" + vbCr + Capa + vbCr + "N" + vbCr);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public Int32 PolylineObtArea()
        {
            try
            {
                OBJACAD = (AcadApplication)Marshal.GetActiveObject(ID_VERSION);
                DOC = OBJACAD.ActiveDocument;

                Object obj1 = new object();
                object obj2 = new object();
                //string cap, bloque;

                DOC.Utility.GetEntity(out obj1, out obj2, "\nSeleccione la Polyline para obtener el Área:");

                AcadObject tmpObj = obj1 as AcadObject;
                AcadLWPolyline tmpObj2 = tmpObj as AcadLWPolyline;
                string strObjName = tmpObj.ObjectName;

                //AcadBlockReference blockRefObj = out1 as AcadBlockReference;
                //System.Double[] blockPoint = (System.Double[])point;
                //System.Double[] blockPoint = (System.Double[])blockRefObj.InsertionPoint;

                if (strObjName.Equals("AcDbPolyline"))
                {
                    Int32 _area = Convert.ToInt32(tmpObj2.Area);
                    //picked object is not AcadLWPolyline, prompt user, or stop the process
                    //MessageBox.Show("Error, no selecciono un bloque ");               

                    return _area;
                }
                else
                {
                    //bloque = blockRefObj.Name;

                    //blockRefObj.XScaleFactor = esc;
                    //blockRefObj.YScaleFactor = esc;
                    //blockRefObj.ZScaleFactor = esc;
                    //blockRefObj.Update();

                    return 0;
                }

            }
            catch (System.Exception)
            {
                // Algo ha pasado...
                return 0;
                //MessageBox.Show("Error: " + ex.Message);
                //return false;
            }

        }

        public bool ColorObj(string Color)
        {
            string vbCr = " ";
            try
            {
                OBJACAD = (AcadApplication)Marshal.GetActiveObject(ID_VERSION);
                DOC = OBJACAD.ActiveDocument;


                DOC.SendCommand("-Color" + vbCr + Color + vbCr);
                return true;
            }
            catch
            {

                return false;
            }
        }

        public string codigo_obtener(double x, double y)
        {
            int pasos_x, pasos_y;
            string codigo = "";

            double Xi = x - 491789.4362, Yi = y - 1037634.3819;//coodificación de Eleval

            //Coodificación ELEVAL
            if (x >= 487370.5808 & y >= 1039623.1692 & x < 731789.4362 & y < 1145634.3819)
            {//determina la 1ra letra del codigo.
                pasos_x = Convert.ToInt16(Convert.ToInt32(Xi) / 48000);
                pasos_y = Convert.ToInt16(Convert.ToInt32(Yi) / 36000);

                string cuadrante = Convert.ToString(pasos_x) + Convert.ToString(pasos_y);

                switch (cuadrante)
                {
                    case "00":
                        codigo = "U";
                        break;
                    case "10":
                        codigo = "V";
                        break;
                    case "20":
                        codigo = "X";
                        break;
                    case "30":
                        codigo = "Y";
                        break;
                    case "40":
                        codigo = "Z";
                        break;

                    case "01":
                        codigo = "P";
                        break;
                    case "11":
                        codigo = "Q";
                        break;
                    case "21":
                        codigo = "R";
                        break;
                    case "31":
                        codigo = "S";
                        break;
                    case "41":
                        codigo = "T";
                        break;

                    case "02":
                        codigo = "K";
                        break;
                    case "12":
                        codigo = "L";
                        break;
                    case "22":
                        codigo = "E";
                        break;
                    case "32":
                        codigo = "N";
                        break;
                    case "42":
                        codigo = "O";
                        break;

                    default:
                        return "err1";
                }

                //Console.WriteLine(CódigoGeo);
                Xi -= 48000 * (pasos_x); Yi -= 36000 * (pasos_y);

                //determina la 2da letra del codigo.
                pasos_x = Convert.ToInt16(Convert.ToInt32(Xi) / 24000);
                pasos_y = Convert.ToInt16(Convert.ToInt32(Yi) / 18000);
                cuadrante = Convert.ToString(pasos_x) + Convert.ToString(pasos_y);
                switch (cuadrante)
                {
                    case "00":
                        codigo += "3";
                        break;
                    case "10":
                        codigo += "4";
                        break;
                    case "01":
                        codigo += "1";
                        break;
                    case "11":
                        codigo += "2";
                        break;
                    default:
                        return "err2";
                }
                Xi -= 24000 * (pasos_x); Yi -= 18000 * (pasos_y);

                //determina la 3ra letra del codigo.
                pasos_x = Convert.ToInt16(Convert.ToInt32(Xi) / 8000);
                pasos_y = Convert.ToInt16(Convert.ToInt32(Yi) / 6000);
                cuadrante = Convert.ToString(pasos_x) + Convert.ToString(pasos_y);
                switch (cuadrante)
                {
                    case "00":
                        codigo += "G";
                        break;
                    case "10":
                        codigo += "H";
                        break;
                    case "20":
                        codigo += "J";
                        break;
                    case "01":
                        codigo += "D";
                        break;
                    case "11":
                        codigo += "E";
                        break;
                    case "21":
                        codigo += "F";
                        break;
                    case "02":
                        codigo += "A";
                        break;
                    case "12":
                        codigo += "B";
                        break;
                    case "22":
                        codigo += "C";
                        break;

                    default:
                        return "err3";
                }
                Xi -= 8000 * (pasos_x); Yi -= 6000 * (pasos_y);

                //determina la 4ta letra del codigo.
                pasos_x = Convert.ToInt16(Convert.ToInt16(Xi) / 4000);
                pasos_y = Convert.ToInt16(Convert.ToInt16(Yi) / 3000);
                cuadrante = Convert.ToString(pasos_x) + Convert.ToString(pasos_y);
                switch (cuadrante)
                {
                    case "00":
                        codigo += "C";
                        break;
                    case "10":
                        codigo += "D";
                        break;
                    case "01":
                        codigo += "A";
                        break;
                    case "11":
                        codigo += "B";
                        break;

                    default:
                        return "err4";
                }
                Xi -= 4000 * (pasos_x); Yi -= 3000 * (pasos_y);

                //determina la 5ta letra del codigo.
                pasos_x = Convert.ToInt16(Convert.ToInt16(Xi) / 1000);
                pasos_y = Convert.ToInt16(Convert.ToInt16(Yi) / 500);
                cuadrante = Convert.ToString(pasos_x) + Convert.ToString(pasos_y);
                switch (cuadrante)
                {
                    case "00":
                        codigo += "W";
                        break;
                    case "10":
                        codigo += "X";
                        break;
                    case "20":
                        codigo += "Y";
                        break;
                    case "30":
                        codigo += "Z";
                        break;

                    case "01":
                        codigo += "S";
                        break;
                    case "11":
                        codigo += "T";
                        break;
                    case "21":
                        codigo += "U";
                        break;
                    case "31":
                        codigo += "V";
                        break;

                    case "02":
                        codigo += "N";
                        break;
                    case "12":
                        codigo += "P";
                        break;
                    case "22":
                        codigo += "Q";
                        break;
                    case "32":
                        codigo += "R";
                        break;

                    case "03":
                        codigo += "J";
                        break;
                    case "13":
                        codigo += "K";
                        break;
                    case "23":
                        codigo += "L";
                        break;
                    case "33":
                        codigo += "M";
                        break;

                    case "04":
                        codigo += "E";
                        break;
                    case "14":
                        codigo += "F";
                        break;
                    case "24":
                        codigo += "G";
                        break;
                    case "34":
                        codigo += "H";
                        break;

                    case "05":
                        codigo += "A";
                        break;
                    case "15":
                        codigo += "B";
                        break;
                    case "25":
                        codigo += "C";
                        break;
                    case "35":
                        codigo += "D";
                        break;

                    default:
                        return "err5";
                }
                Xi -= 1000 * (pasos_x); Yi -= 500 * (pasos_y);

                //determina la 6ta letra del codigo.
                pasos_x = Convert.ToInt16(Convert.ToInt16(Xi) / 250);
                pasos_y = Convert.ToInt16(Convert.ToInt16(Yi) / 125);
                cuadrante = Convert.ToString(pasos_x) + Convert.ToString(pasos_y);
                switch (cuadrante)
                {
                    case "00":
                        codigo += "N";
                        break;
                    case "10":
                        codigo += "P";
                        break;
                    case "20":
                        codigo += "Q";
                        break;
                    case "30":
                        codigo += "R";
                        break;

                    case "01":
                        codigo += "J";
                        break;
                    case "11":
                        codigo += "K";
                        break;
                    case "21":
                        codigo += "L";
                        break;
                    case "31":
                        codigo += "M";
                        break;

                    case "02":
                        codigo += "E";
                        break;
                    case "12":
                        codigo += "F";
                        break;
                    case "22":
                        codigo += "G";
                        break;
                    case "32":
                        codigo += "H";
                        break;

                    case "03":
                        codigo += "A";
                        break;
                    case "13":
                        codigo += "B";
                        break;
                    case "23":
                        codigo += "C";
                        break;
                    case "33":
                        codigo += "D";
                        break;


                    default:
                        return "err6";
                }
                Xi -= 250 * (pasos_x); Yi -= 125 * (pasos_y);


                //determina los 2 digitos numérico del codigo.
                pasos_x = Convert.ToInt16(Math.Truncate(Xi / 25));
                pasos_y = Convert.ToInt16(Math.Truncate(Yi / 12.5));
                codigo += Convert.ToString(pasos_x) + Convert.ToString(pasos_y);

            }
            //Codificación Calife
            else if (x >= 562370.5808 & y >= 1145634.3819 & x < 624870.5808 & y < 1177123.1692)
            {
                Xi = x - 562370.5808; Yi = y - 1139623.1692;//coodificación Calife                
                Xi += 8.8102; Yi -= 113.4456;

                pasos_x = Convert.ToInt16(Convert.ToInt32(Xi) / 2500);

                switch (pasos_x)//1er dígito
                {
                    case 0:
                        codigo = "A";
                        break;
                    case 1:
                        codigo = "B";
                        break;
                    case 2:
                        codigo = "C";
                        break;
                    case 3:
                        codigo = "D";
                        break;
                    case 4:
                        codigo = "E";
                        break;
                    case 5:
                        codigo = "F";
                        break;
                    case 6:
                        codigo = "G";
                        break;
                    case 7:
                        codigo = "H";
                        break;
                    case 8:
                        codigo = "I";
                        break;
                    case 9:
                        codigo = "J";
                        break;
                    case 10:
                        codigo = "K";
                        break;
                    case 11:
                        codigo = "L";
                        break;
                    case 12:
                        codigo = "M";
                        break;
                    case 13:
                        codigo = "N";
                        break;
                    case 14:
                        codigo = "O";
                        break;
                    case 15:
                        codigo = "P";
                        break;
                    case 16:
                        codigo = "Q";
                        break;
                    case 17:
                        codigo = "R";
                        break;
                    case 18:
                        codigo = "S";
                        break;
                    case 19:
                        codigo = "T";
                        break;
                    case 20:
                        codigo = "U";
                        break;
                    case 21:
                        codigo = "V";
                        break;
                    case 22:
                        codigo = "X";
                        break;
                    case 23:
                        codigo = "Y";
                        break;
                    case 24:
                        codigo = "Z";
                        break;
                    default:
                        return "err1";
                }

                pasos_y = Convert.ToInt16(Convert.ToInt32(Yi) / 2500);
                switch (pasos_y)//2do dígito
                {
                    case 0:
                        codigo += "A";
                        break;
                    case 1:
                        codigo += "B";
                        break;
                    case 2:
                        codigo += "C";
                        break;
                    case 3:
                        codigo += "D";
                        break;
                    case 4:
                        codigo += "E";
                        break;
                    case 5:
                        codigo += "F";
                        break;
                    case 6:
                        codigo += "G";
                        break;
                    case 7:
                        codigo += "H";
                        break;
                    case 8:
                        codigo += "I";
                        break;
                    case 9:
                        codigo += "J";
                        break;
                    case 10:
                        codigo += "K";
                        break;
                    case 11:
                        codigo += "L";
                        break;
                    case 12:
                        codigo += "M";
                        break;
                    case 13:
                        codigo += "N";
                        break;
                    case 14:
                        codigo += "O";
                        break;

                    default:
                        return "err2";
                }

                //3er dígito

                Xi -= 2500 * (pasos_x); Yi -= 2500 * (pasos_y);
                pasos_x = Convert.ToInt16(Convert.ToInt16(Xi) / 500);
                pasos_y = Convert.ToInt16(Convert.ToInt16(Yi) / 500);
                string cuadrante = Convert.ToString(pasos_x) + Convert.ToString(pasos_y);

                switch (cuadrante)//3Er dígito
                {
                    case "00":
                        codigo += "U";
                        break;
                    case "10":
                        codigo += "V";
                        break;
                    case "20":
                        codigo += "X";
                        break;
                    case "30":
                        codigo += "Y";
                        break;
                    case "40":
                        codigo += "Z";
                        break;

                    case "01":
                        codigo += "P";
                        break;
                    case "11":
                        codigo += "Q";
                        break;
                    case "21":
                        codigo += "R";
                        break;
                    case "31":
                        codigo += "S";
                        break;
                    case "41":
                        codigo += "T";
                        break;

                    case "02":
                        codigo += "K";
                        break;
                    case "12":
                        codigo += "L";
                        break;
                    case "22":
                        codigo += "M";
                        break;
                    case "32":
                        codigo += "N";
                        break;
                    case "42":
                        codigo += "O";
                        break;

                    case "03":
                        codigo += "F";
                        break;
                    case "13":
                        codigo += "G";
                        break;
                    case "23":
                        codigo += "H";
                        break;
                    case "33":
                        codigo += "I";
                        break;
                    case "43":
                        codigo += "J";
                        break;

                    case "04":
                        codigo += "A";
                        break;
                    case "14":
                        codigo += "B";
                        break;
                    case "24":
                        codigo += "C";
                        break;
                    case "34":
                        codigo += "D";
                        break;
                    case "44":
                        codigo += "E";
                        break;

                    default:
                        return "err3";
                }
                Xi -= 500 * (pasos_x); Yi -= 500 * (pasos_y);
                pasos_x = Convert.ToInt16(Convert.ToInt16(Xi) / 50);
                pasos_y = Convert.ToInt16(Convert.ToInt16(Yi) / 50);
                codigo += Convert.ToString(pasos_x) + Convert.ToString(pasos_y);
                Xi -= 50 * (pasos_x); Yi -= 50 * (pasos_y);

                pasos_x = Convert.ToInt16(Convert.ToInt16(Xi) / 5);
                pasos_y = Convert.ToInt16(Convert.ToInt16(Yi) / 5);
                Xi -= 5 * (pasos_x); Yi -= 5 * (pasos_y);
                codigo += Convert.ToString(pasos_x) + Convert.ToString(pasos_y);
            }
            else
            {
                return "err0";
            }

            return codigo;
        }






    }
}
