using System;
using System.Collections.Generic;
using System.Text;
using DataAccess.AutoCad;

namespace Domain
{
    public class CadModel
    {
        CadDao cadDao = new CadDao();

        //Atributes
        public string ID { get; set; }
        public Int16 GRADOS { get; set; }
        public Int16 ZOOM { get; set; }
        public Int16 MARCAR { get; set; }
        public string BLOQUE { get; set; }

        public string CODIGO { get; set; }
        public string CAPACIDAD { get; set; }
        public string SIVPLUS { get; set; }

        public string MENSAJE { get; set; }

        public string CAPA { get; set; }
        public string CAPA_TEMP { get; set; }

        public double X { get; set; }
        public double Y { get; set; }
        public double ESC { get; set; }

        public CadModel()
        {

        }

        public bool Activar_BLIPMODE(string opt)
        {
            return cadDao.Activar_BLIPMODE(opt);

        }
        public bool BloqueEscribir()
        {
            return cadDao.bloq_dibujar(X, Y, CODIGO, SIVPLUS, CAPACIDAD, BLOQUE, CAPA_TEMP, ESC, (double)GRADOS);
        }
        public bool BloqueEscribirCliente(string nif,string inm1,string inm2,string puerta)
        {
            return cadDao.bloq_dibujar_cliente(X, Y, CODIGO, nif,inm1,inm2,puerta, BLOQUE, CAPA_TEMP, ESC, (double)GRADOS);
        }
        public string BloqueCodificar()
        {
            return cadDao.bloq_codificar();
        }
        public bool bloq_esc(double esc)
        {
            return cadDao.bloq_esc(esc);
        }
       
        public bool BloqueLeer()
        {
            double x = 0, y = 0, esc = 1.0;
            Int16 grados = 0;
            string text1 = null, text2 = null, capacidad = null, sivplus = null, bloque = null, capa = null;

            if (cadDao.bloq_leer(out x, out y, out  text1, out  text2, out  capacidad, out  sivplus, out  bloque, out  capa, out  esc, out  grados))
            {
                //this.X = Math.Round(x, 4);
                //this.Y = Math.Round(y, 4);
                this.X = x;
                this.Y = y;
                //this.ID = id;
                this.CODIGO = text1 + text2;
                if (CODIGO.Length > 10)
                {
                    this.MENSAJE = "Favor verificar longitud del Código";
                    return false;
                }
                this.CAPACIDAD = capacidad;
                this.SIVPLUS = sivplus;
                if (SIVPLUS.Length > 50)
                {
                    this.MENSAJE = "Favor verificar longitud del SIVPLUS";
                    return false;
                }
                this.BLOQUE = bloque;
                this.CAPA = capa;
                this.ESC = Math.Round(esc, 1);
                this.GRADOS = grados;
                return true;
            }
            else
            {
                this.MENSAJE = "El bloque no fue seleccionado en AutoCAD";
                return false;
            }
        }

        public bool BloqueLeerCliente()
        {
            double x = 0, y = 0, esc = 1.0;
            Int16 grados = 0;
            string id = null, nif = null, inm1 = null, inm2 = null, puerta = null, bloque = null, capa = null;

            if (cadDao.bloq_leer_cliente(out x, out y,out id, out  nif, out  inm1, out  inm2, out  puerta, out  bloque, out  capa, out  esc, out  grados))
            {
                //this.X = Math.Round(x, 4);
                //this.Y = Math.Round(y, 4);
                this.X = x;
                this.Y = y;
                this.ID = id;
                this.CODIGO = nif;
                if (CODIGO.Length > 10)
                {
                    this.MENSAJE = "Favor verificar longitud del Código";
                    return false;
                }
                this.CAPACIDAD = inm1;
                this.SIVPLUS = inm2;
                if (SIVPLUS.Length > 40)
                {
                    this.MENSAJE = "Favor verificar longitud del SIVPLUS";
                    return false;
                }
                this.BLOQUE = bloque;
                this.CAPA = capa;
                this.ESC = Math.Round(esc, 1);
                this.GRADOS = grados;
                return true;
            }
            else
            {
                this.MENSAJE = "El bloque no fue seleccionado en AutoCAD";
                return false;
            }
        }

        public bool CoordMarcar(double X, double Y)
        {
            //this.PASO=cadDao.ZoomCoord(X, Y, Zoom, Marcar);
            if (MARCAR.Equals(0))
            {
                this.MENSAJE = "Falta definir el radio del Círculo";
                return false;
            }
            if (cadDao.coord_marcar(X, Y, MARCAR, CAPA_TEMP))
            {
                //this.MENSAJE = string.Format("Coordenadas ubicadas en plano: {0},{1}", X, Y);
                return true;
            }
            else
            {
                this.MENSAJE = "Coordenadas no ubicadas en plano";
                return false;
            }
        }

        public bool CoordObtener()
        {
            double[] insertionPnt = cadDao.coord_obtener();
            //insertionPnt = new double[3];
            //insertionPnt = cadDao.coord_obtener();
            this.X = insertionPnt[0];
            this.Y = insertionPnt[1];

            if (insertionPnt[0].Equals(0))
            {
                MENSAJE = "No se logró obtener las coordenadas X e Y";
                return false;
            }
            else
            {
                return true;
            }


        }
        public bool CoordZoom(double X, double Y, Int16 marcar)
        {
            //this.PASO=cadDao.ZoomCoord(X, Y, Zoom, Marcar);
            if (cadDao.coord_zoom(X, Y, ZOOM, marcar, CAPA_TEMP))
            {
                //this.MENSAJE = string.Format("Coordenadas ubicadas en plano: {0},{1}", X, Y);
                return true;
            }
            else
            {
                this.MENSAJE = "Coordenadas no ubicadas en plano";
                return false;
            }
        }

        public void CoordMap()//double x, double y
        {
            double longi = 0, lati = 0;
            //this.X = x;
            //this.Y = y;          

            CambioUTM_DG(out lati, out longi);
            string dirección = Convert.ToString(longi).Replace(",", ".") + "," + Convert.ToString(lati).Replace(",", ".");
            System.Diagnostics.Process.Start(string.Format("http://www.google.co.ve/maps?q={0}", dirección));
        }

        //public void CambioDG_UTM(ref double longi, ref double lati, out double x1, out double y1)//double x, double y
        //{
        //    //double longi = 0, lati = 0;
        //    //this.X = x;
        //    //this.Y = y;          

        //    CambioDG_UTM(ref lati,ref longi,out x1, out y1);
           
        //}

        public Int32 PolylineObtArea()
        {
            return cadDao.PolylineObtArea();
        }
        public bool LayerBorrar()
        {
            if (!cadDao.LayerBorrar(CAPA_TEMP))
            {
                this.MENSAJE = "No se logró Borrar la Capa";
                return false;
            }
            return true;

        }
        public bool ZoomGeo(string codigo, Int16 _marcar)
        {
            codigo = codigo_limpiar(codigo);
            if (codigo.Length < 3)//Codigo errado
            {
                this.MENSAJE = string.Format("lóngitud del Código {0}, corto", codigo);
                return false;
            }

            if (Coordenadas(codigo))
            {
                //this.PASO=cadDao.ZoomCoord(X, Y, 1, 0);

                if (cadDao.coord_zoom(X, Y, ZOOM, _marcar, CAPA_TEMP))
                {
                    this.MENSAJE = string.Format("Coordenadas ubicadas en plano: X = {0} , Y = {1}", X, Y);
                    return true;
                }
                else
                {
                    this.MENSAJE = "Coordenadas no ubicadas en plano";
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        internal Boolean isInt16(String num)
        {
            try
            {
                Int16.Parse(num);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public string codigo_limpiar(string codigo)
        {
            codigo = codigo.ToUpper().Replace("\"", "").Replace("-", "").TrimEnd();

            //string cod1, cod2;
            if (isInt16(codigo.Substring(1, 1)))//Codigo Eleval
            {
                //cod1 = codigo.Substring(0, 5);
                //cod2 = codigo.Substring(5, 3);
                return codigo.Substring(0, 8);
            }
            else //Codigo Calife
            {
                //cod1 = codigo.Substring(0, 3);
                //cod2 = codigo.Substring(3, 4);
                return codigo.Substring(0, 7);
            }
            //return cod1 + cod2;

        }
        public bool Coordenadas(string codigo)
        {
            try
            {

                double Xi = 587789.4362, Yi = 1109634.3819;
                //codigo = codigo.ToUpper().Replace("\"", "").Replace("S1", "").Replace("S2", "").TrimEnd();

                //if (codigo.Substring(5, 1) == "-")
                //{
                //    codigo = codigo.Substring(0, 5) + codigo.Substring(6, 3);
                //}

                //MessageBox.Show("codigo Geográfico Eje Costero");
                if (codigo.Length == 7)
                {

                    //malo = true;

                    //Xi = 562365  ; Yi = 1139736 ;
                    Xi = 562370.5808; Yi = 1139623.1692;
                    Xi -= 8.8102; Yi += 113.4456;

                    switch (codigo.Substring(0, 1))//primer dígito
                    {
                        case "A":
                            X = Xi;
                            break;
                        case "B":
                            X = Xi + 2500 * 1;
                            break;
                        case "C":
                            X = Xi + 2500 * 2;
                            break;
                        case "D":
                            X = Xi + 2500 * 3;
                            break;
                        case "E":
                            X = Xi + 2500 * 4;
                            break;
                        case "F":
                            X = Xi + 2500 * 5;
                            break;
                        case "G":
                            X = Xi + 2500 * 6;
                            break;
                        case "H":
                            X = Xi + 2500 * 7;
                            break;
                        case "I":
                            X = Xi + 2500 * 8;
                            break;
                        case "J":
                            X = Xi + 2500 * 9;
                            break;
                        case "K":
                            X = Xi + 2500 * 10;
                            break;
                        case "L":
                            X = Xi + 2500 * 11;
                            break;
                        case "M":
                            X = Xi + 2500 * 12;
                            break;
                        case "N":
                            X = Xi + 2500 * 13;
                            break;
                        case "O":
                            X = Xi + 2500 * 14;
                            break;
                        case "P":
                            X = Xi + 2500 * 15;
                            break;
                        case "Q":
                            X = Xi + 2500 * 16;
                            break;
                        case "R":
                            X = Xi + 2500 * 17;
                            break;
                        case "S":
                            X = Xi + 2500 * 18;
                            break;
                        case "T":
                            X = Xi + 2500 * 19;
                            break;
                        case "U":
                            X = Xi + 2500 * 20;
                            break;
                        case "V":
                            X = Xi + 2500 * 21;
                            break;
                        case "X":
                            X = Xi + 2500 * 22;
                            break;
                        case "Y":
                            X = Xi + 2500 * 23;
                            break;
                        case "Z":
                            X = Xi + 2500 * 24;
                            break;
                        default:
                            this.MENSAJE = "Error no corresponde la codificación del 1er dígito del Eje Costero";
                            return false;
                        //MessageBox.Show("Error no corresponde la codificación del 1er dígito del Eje Costero: " + codigo);
                        //malo = true;
                        //break;
                    }

                    //if (malo != true)
                    //{

                    switch (codigo.Substring(1, 1))//2do dígito
                    {
                        case "A":
                            Y = Yi;
                            break;
                        case "B":
                            Y = Yi + 2500 * 1;
                            break;
                        case "C":
                            Y = Yi + 2500 * 2;
                            break;
                        case "D":
                            Y = Yi + 2500 * 3;
                            break;
                        case "E":
                            Y = Yi + 2500 * 4;
                            break;
                        case "F":
                            Y = Yi + 2500 * 5;
                            break;
                        case "G":
                            Y = Yi + 2500 * 6;
                            break;
                        case "H":
                            Y = Yi + 2500 * 7;
                            break;
                        case "I":
                            Y = Yi + 2500 * 8;
                            break;
                        case "J":
                            Y = Yi + 2500 * 9;
                            break;
                        case "K":
                            Y = Yi + 2500 * 10;
                            break;
                        case "L":
                            Y = Yi + 2500 * 11;
                            break;
                        case "M":
                            Y = Yi + 2500 * 12;
                            break;
                        case "N":
                            Y = Yi + 2500 * 13;
                            break;
                        case "O":
                            Y = Yi + 2500 * 14;
                            break;
                        default:
                            //MessageBox.Show("Error: no corresponde la codificación del 2do dígito del Eje Costero: " + codigo);
                            //malo = true;
                            //break;
                            this.MENSAJE = "Error no corresponde la codificación del 2do dígito del Eje Costero";
                            return false;
                    }
                    //}

                    //if (malo != true)
                    //{

                    switch (codigo.Substring(2, 1))//3er dígito
                    {
                        case "A":
                            X = X + 500 * 0; Y = Y + 500 * 4;
                            break;
                        case "B":
                            X = X + 500 * 1; Y = Y + 500 * 4;
                            break;
                        case "C":
                            X = X + 500 * 2; Y = Y + 500 * 4;
                            break;
                        case "D":
                            X = X + 500 * 3; Y = Y + 500 * 4;
                            break;
                        case "E":
                            X = X + 500 * 4; Y = Y + 500 * 4;
                            break;
                        case "F":
                            X = X + 500 * 0; Y = Y + 500 * 3;
                            break;
                        case "G":
                            X = X + 500 * 1; Y = Y + 500 * 3;
                            break;
                        case "H":
                            X = X + 500 * 2; Y = Y + 500 * 3;
                            break;
                        case "I":
                            X = X + 500 * 3; Y = Y + 500 * 3;
                            break;
                        case "J":
                            X = X + 500 * 4; Y = Y + 500 * 3;
                            break;
                        case "K":
                            X = X + 500 * 0; Y = Y + 500 * 2;
                            break;
                        case "L":
                            X = X + 500 * 1; Y = Y + 500 * 2;
                            break;
                        case "M":
                            X = X + 500 * 2; Y = Y + 500 * 2;
                            break;
                        case "N":
                            X = X + 500 * 3; Y = Y + 500 * 2;
                            break;
                        case "O":
                            X = X + 500 * 4; Y = Y + 500 * 2;
                            break;
                        case "P":
                            X = X + 500 * 0; Y = Y + 500 * 1;
                            break;
                        case "Q":
                            X = X + 500 * 1; Y = Y + 500 * 1;
                            break;
                        case "R":
                            X = X + 500 * 2; Y = Y + 500 * 1;
                            break;
                        case "S":
                            X = X + 500 * 3; Y = Y + 500 * 1;
                            break;
                        case "T":
                            X = X + 500 * 4; Y = Y + 500 * 1;
                            break;
                        case "U":
                            X = X + 500 * 0; Y = Y + 500 * 0;
                            break;
                        case "V":
                            X = X + 500 * 1; Y = Y + 500 * 0;
                            break;
                        case "X":
                            X = X + 500 * 2; Y = Y + 500 * 0;
                            break;
                        case "Y":
                            X = X + 500 * 3; Y = Y + 500 * 0;
                            break;
                        case "Z":
                            X = X + 500 * 4; Y = Y + 500 * 0;
                            break;
                        default:
                            //MessageBox.Show("Error: no corresponde la codificación del 3er dígito del Eje Costero:" + codigo);
                            //malo = true;
                            //break;
                            this.MENSAJE = "Error no corresponde la codificación del 3er dígito del Eje Costero";
                            return false;
                    }

                    //}

                    //if (malo != true)
                    //{
                    X += 50 * (int.Parse(codigo.Substring(3, 1))) + 5 * (int.Parse(codigo.Substring(5, 1)));
                    Y += 50 * (int.Parse(codigo.Substring(4, 1))) + 5 * (int.Parse(codigo.Substring(6, 1)));
                    //}

                }
                //coodificación ELEVAL
                else
                {
                    switch (codigo.Substring(0, 1))
                    {
                        case "E":
                            X = Xi; Y = Yi;
                            break;
                        case "L":
                            X = -18000 - 30000 + Xi; Y = 0 + Yi;
                            break;
                        case "N":
                            X = +18000 + 30000 + Xi; Y = 0 + Yi;
                            break;
                        case "Q":
                            X = -18000 - 30000 + Xi; Y = -30000 - 6000 + Yi;
                            break;
                        case "R":
                            X = Xi; Y = -30000 - 6000 + Yi;
                            break;
                        case "S":
                            X = +18000 + 30000 + Xi; Y = -30000 - 6000 + Yi;
                            break;
                        case "V":
                            X = -18000 - 30000 + Xi; Y = -60000 - 12000 + Yi;
                            break;
                        case "X":
                            X = +Xi; Y = -60000 - 12000 + Yi;
                            break;
                        default:
                            //MessageBox.Show("Error: no corresponde la codificación del 1er dígito de Valencia", "Error de Codificación", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                            //malo = true;
                            //break;
                            this.MENSAJE = "Error: no corresponde la codificación del 1er dígito de Valencia";
                            return false;
                    }

                    //if (malo != true)
                    //{

                    switch (codigo.Substring(1, 1))
                    {
                        case "1":
                            Y = Y + 18000;
                            break;
                        case "2":
                            X = X + 24000; Y = Y + 18000;
                            break;
                        case "3":

                            break;
                        case "4":
                            X = X + 24000;
                            break;
                        default:
                            //MessageBox.Show("Error: no corresponde la codificación del 2do dígito de Valencia: " + codigo);
                            //malo = true;
                            //break;
                            this.MENSAJE = "Error: no corresponde la codificación del 2do dígito de Valencia";
                            return false;
                    }
                    //}
                    //if (malo != true)
                    //{
                    switch (codigo.Substring(2, 1))
                    {
                        case "A":
                            Y = Y + 12000;
                            break;
                        case "B":
                            X = X + 8000; Y = Y + 12000;
                            break;
                        case "C":
                            X = X + 16000; Y = Y + 12000;
                            break;
                        case "D":
                            X = X + 0; Y = Y + 6000;
                            break;
                        case "E":
                            X = X + 8000; Y = Y + 6000;
                            break;
                        case "F":
                            X = X + 16000; Y = Y + 6000;
                            break;
                        case "G":

                            break;
                        case "H":
                            X = X + 8000;

                            break;
                        case "J":
                            X = X + 16000;
                            break;

                        default:
                            this.MENSAJE = "Error: no corresponde la codificación del 3er dígito de Valencia";
                            return false;
                        //MessageBox.Show("Error: no corresponde la codificación del 3er dígito de Valencia: " + codigo);
                        //malo = true;
                        //break;
                    }
                    //}
                    //if (malo != true)
                    //{
                    switch (codigo.Substring(3, 1))
                    {
                        case "A":
                            Y = Y + 3000;
                            break;
                        case "B":
                            X = X + 4000; Y = Y + 3000;
                            break;
                        case "C":

                            break;
                        case "D":
                            X = X + 4000;
                            break;
                        default:
                            //MessageBox.Show("Error: no corresponde la codificación del 4to dígito de Valencia: " + codigo);
                            //malo = true;
                            //break;
                            this.MENSAJE = "Error: no corresponde la codificación del 4to dígito de Valencia";
                            return false;

                    }
                    //}
                    //if (malo != true)
                    //{
                    switch (codigo.Substring(4, 1))
                    {
                        case "A":
                            Y = Y + 2500;
                            break;
                        case "B":
                            X = X + 1000; Y = Y + 2500;
                            break;
                        case "C":
                            X = X + 2000; Y = Y + 2500;
                            break;
                        case "D":
                            X = X + 3000; Y = Y + 2500;
                            break;
                        case "E":
                            Y = Y + 2000;
                            break;
                        case "F":
                            X = X + 1000; Y = Y + 2000;
                            break;
                        case "G":
                            X = X + 2000; Y = Y + 2000;
                            break;
                        case "H":
                            X = X + 3000; Y = Y + 2000;
                            break;
                        case "J":
                            Y = Y + 1500;
                            break;
                        case "K":
                            X = X + 1000; Y = Y + 1500;
                            break;
                        case "L":
                            X = X + 2000; Y = Y + 1500;
                            break;
                        case "M":
                            X = X + 3000; Y = Y + 1500;
                            break;
                        case "N":
                            X = X + 0; Y = Y + 1000;
                            break;
                        case "P":
                            X = X + 1000; Y = Y + 1000;
                            break;
                        case "Q":
                            X = X + 2000; Y = Y + 1000;
                            break;
                        case "R":
                            X = X + 3000; Y = Y + 1000;
                            break;
                        case "S":
                            X = X + 0; Y = Y + 500;
                            break;
                        case "T":
                            X = X + 1000; Y = Y + 500;
                            break;
                        case "U":
                            X = X + 2000; Y = Y + 500;
                            break;
                        case "V":
                            X = X + 3000; Y = Y + 500;
                            break;
                        case "W":
                            break;
                        case "X":
                            X = X + 1000;
                            break;
                        case "Y":
                            X = X + 2000;
                            break;
                        case "Z":
                            X = X + 3000;
                            break;
                        default:
                            //MessageBox.Show("Error: no corresponde la codificación del 5to dígito de Valencia: " + codigo);
                            //malo = true;
                            //break;
                            this.MENSAJE = "Error: no corresponde la codificación del 5to dígito de Valencia";
                            return false;
                    }
                    //}
                    //if (malo != true)
                    //{
                    //MessageBox.Show(codigo.Substring(5, 1)); E1JDWJ54
                    switch (codigo.Substring(5, 1))
                    {
                        case "A":
                            Y = Y + 375;
                            break;
                        case "B":
                            X = X + 250; Y = Y + 375;
                            break;
                        case "C":
                            X = X + 500; Y = Y + 375;
                            break;
                        case "D":
                            X = X + 750; Y = Y + 375;
                            break;
                        case "E":
                            Y = Y + 250;
                            break;
                        case "F":
                            X = X + 250; Y = Y + 250;
                            break;
                        case "G":
                            X = X + 500; Y = Y + 250;
                            break;
                        case "H":
                            X = X + 750; Y = Y + 250;
                            break;
                        case "J":
                            Y = Y + 125;
                            break;
                        case "K":
                            X = X + 250; Y = Y + 125;
                            break;
                        case "L":
                            X = X + 500; Y = Y + 125;
                            break;
                        case "M":
                            X = X + 750; Y = Y + 125;
                            break;
                        case "N":
                            X = X + 0; Y = Y + 0;
                            break;
                        case "P":
                            X = X + 250; Y = Y + 0;
                            break;
                        case "Q":
                            X = X + 500; Y = Y + 0;
                            break;
                        case "R":
                            X = X + 750; Y = Y + 0;
                            break;
                        default:
                            //MessageBox.Show("Error: no corresponde la codificación del 6to dígito de Valencia: " + codigo);
                            //malo = true;
                            //break;
                            this.MENSAJE = "Error: no corresponde la codificación del 6to dígito de Valencia";
                            return false;
                    }
                    //}
                    //if (malo != true)
                    //{
                    X += 25 * int.Parse(codigo.Substring(6, 1));
                    Y += 25 / 2 * int.Parse(codigo.Substring(7, 1));
                    //}
                }
                //else
                //{
                //    this.MENSAJE = "Error: El formato no corresponde a un codigo Geográfico de Carabobo";
                //    return false;
                //    //MessageBox.Show("El formato no corresponde a un codigo Geográfico de Carabobo" + "\n" + codigo, "Error de Codificación", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                //    //malo = true;
                //}
                return true;
            }
            catch (System.Exception ex)
            {
                // Algo ha pasado...
                this.MENSAJE = "Error: " + ex.Message;
                return false;
                //MessageBox.Show("Error: " + ex.Message);
                //malo = true;
            }
        }

        public void CambioDG_UTM(ref double lati, ref double longi, out double mapX, out double mapY)
        {
            /// Sobre la geometría del delipsoide WGS84
            double a = 6378137.0;
            double b = 6356752.314247833;

            //  float e = sqrt((a*a) + (b*b))/a; ///< Excentricidad.
            double e = Math.Sqrt((a * a) - (b * b)) / b; ///< Segunda excentricidad.
            double e2 = e * e; ///< al cuadrado. Usaremos esta directamente.

            double c = a * a / b; ///< Radio Polar de Curvatura.
            ///

            /// Sobre la longitud y latitud. Conversión de grados decimales a radianes.

            /*!
             * Cálculo del signo de la longitud:
             *      - Si la longitud está referida al Oeste del meridiano de Greenwich, 
             *        entonces la longitud es negativa (-).
             *      - Si la longitud está referida al Este del meridiano de Greenwich,
             *        entonces la longitud es positiva 8+).
             */

            double latRad = lati * Math.PI / 180.0; ///< Latitud en Radianes.
            double lonRad = longi * Math.PI / 180.0; ///< Longitud en Radianes.
            ///
            /// Sobre el huso.
            //int h = System.Convert.ToInt32((lati / 6.0) + 31.0);  ///< Nos interesa quedarnos solo con la parte entera.
            //double huso = (longi / 6) + 31;  ///< Nos interesa quedarnos solo con la parte entera.
            //int h = System.Convert.ToInt32(huso);
            int h = 19;
            int landa0 = h * 6 - 183; ///< Cálculo del meridiano central del huso en radianes.
            double Dlanda = lonRad - (landa0 * Math.PI) / 180.0;  ///< Desplazamiento del punto a calcular con respecto al meridiano central del huso.

            /*!
   * Ecuaciones de Coticchia-Surace para el paso de Geográficas a UTM (Problema directo);
   */

            /// Cálculo de Parámetros.
            double coslatRad = Math.Cos(latRad);
            double coslatRad2 = coslatRad * coslatRad;

            double A = coslatRad * Math.Sin(Dlanda);
            double xi = 0.5 * Math.Log((1 + A) / (1 - A));
            double n = Math.Atan(Math.Tan(latRad) / Math.Cos(Dlanda)) - latRad;
            double v = (c / Math.Sqrt(1 + e2 * coslatRad2)) * 0.9996;
            double z = (e2 / 2.0) * xi * xi * coslatRad2;
            double A1 = Math.Sin(2 * latRad);
            double A2 = A1 * coslatRad2;
            double J2 = latRad + (A1 / 2.0);
            double J4 = (3.0 * J2 + A2) / 4.0;
            double J6 = (5.0 * J4 + A2 * coslatRad2) / 3.0;
            double alf = 0.75 * e2;
            double bet = (5.0 / 3.0) * alf * alf;
            double gam = (35.0 / 27.0) * alf * alf * alf;
            double Bfi = 0.9996 * c * (latRad - alf * J2 + bet * J4 - gam * J6);

            /*! 
          * Cálculo final de coordenadas UTM
          */

            mapX = xi * v * (1 + (z / 3.0)) + 500000; /*!< 500.000 es el retranqueo que se realiza en cada huso sobre el origen de
  coordenadas en el eje X con el objeto de que no existan coordenadas negativas. */

            mapY = n * v * (1 + z) + Bfi;  /*!< En el caso de latitudes al sur del ecuador, se sumará al valor de Y 10.000.000
  para evitar coordenadas negativas. */

        }

        public void CambioUTM_DG(out double longi, out double lati)
        {
            /// Sobre la geometría del delipsoide WGS84
            double a = 6378137.0;
            double b = 6356752.314247833;
            double e = Math.Sqrt((a * a) - (b * b)) / b; ///< Segunda excentricidad.
            double e2 = e * e; ///< al cuadrado. Usaremos esta directamente.

            double c = a * a / b; ///< Radio Polar de Curvatura.

            X -= 500000;///<Retranqueo del eje de las x
            //Y -= 10000000;
            //double se = Y;
            int utmZone = 19;
            double mc = (6 * utmZone - 183);///<Meridiano central del uso
            ///
            //Cálculo de parámetros
            double fi = Y / (6366197.724 * 0.9996);
            double v = c * 0.9996 / Math.Sqrt(1 + e2 * Math.Cos(fi) * Math.Cos(fi));
            a = X / v;
            double a1 = Math.Sin(2.0 * fi);
            double a2 = a1 * Math.Pow(Math.Cos(fi), 2);
            double j2 = fi + (a1 / 2);
            double j4 = (3.0 * j2 + a2) / 4.0;
            double j6 = (5.0 * j4 + a2 * Math.Pow(Math.Cos(fi), 2.0)) / 3.0;
            double alfa = (3.0 / 4.0) * e2;
            double beta = (5.0 / 3.0) * Math.Pow(alfa, 2);
            double ganma = (35.0 / 27.0) * Math.Pow(alfa, 3);
            double bfi = 0.9996 * c * (fi - (alfa * j2) + (beta * j4) - (ganma * j6));
            b = (Y - bfi) / v;
            double zeta = e2 * a * a * Math.Cos(fi) / 2;

            double xi = a * (1 - zeta / 3);
            double n = b * (1 - zeta) + fi;
            double sxi = (Math.Exp(xi) - Math.Exp(-xi)) / 2.0;
            double delta = Math.Atan(sxi / Math.Cos(n));
            double tau = Math.Atan(Math.Cos(delta) * Math.Tan(n));

            longi = delta / Math.PI * 180 + mc;

            double radiantes = fi + (1 + 0.006739497 * Math.Pow(Math.Cos(fi), 2) - (3 / 2) * 0.006739497 * Math.Sin(fi) * Math.Cos(fi) * (tau - fi)) * (tau - fi);
            lati = (radiantes / Math.PI) * 180;

        }

    }
}
