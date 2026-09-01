using System;
using System.Collections.Generic;
using System.Text;
using DataAccess.PostgreSQL;
using DataAccess.AutoCad;
using System.Data;
//using Common;
using Common.Cache;
using System.Data.SqlClient;

namespace Domain
{
    public class TreeviewModel
    {
        TreeviewDao treeviewDao = new TreeviewDao();
        //CadModel cadModel = new CadModel();
        //BeanCargaDao bnCarga = new BeanCargaDao();

        //Atributes
        public int ID { get; set; }
        public int ROOT { get; set; }
        public Int16 ID_BLOQUE { get; set; }
        public Int16 ID_ICONO { get; set; }
        public string S_NOMBRE { get; set; }
        public string S_TABLA { get; set; }
        public string MENSAJE { get; set; }
        public bool B_NUEVO { get; set; }
        
        public TreeviewModel()
        {
        }

        //public TreeviewModel(int idTree, Int16 id_bloq_Tree)
        //{
        //    this.ID = idTree;
        //    this.ID_BLOQUE = id_bloq_Tree;
        //}
        public TreeviewModel(int idTree, int rootTree, Int16 id_bloq_Tree)
        {
            this.ID = idTree;
            this.ROOT = rootTree;
            this.ID_BLOQUE = id_bloq_Tree;
        }

        //Methods
        public double[] SelectXY(int idTree, Int16 id_icono)
        //public string SelectXY(int idTree, Int16 id_icono)
        {
            //double[] SelXY = new double[2];
            //SelXY = treeviewDao.SelectXY(idTree, id_icono);
          return  treeviewDao.select_XY(idTree, id_icono);
            //CadDao MCad = new CadDao();

            //if (MCad.ZoomCoord(SelXY[0], SelXY[1], 1, 0))
            //{
            //    return string.Format("Nodo:{0}, Coordenadas ubicadas en plano: {0},{1},{2}",idTree, SelXY[0], SelXY[1]);
            //}
            //else
            //{
            //    return "Coordenadas no ubicadas en plano";
            //}
        }
        public string SelectNombre(int idTree, Int16 id_icono)
        {           
            return treeviewDao.select_nombre(idTree, id_icono);
        }

        public bool UpdateRoot(int _id, int _root, Int16 _id_icono)
        {
            try
            {
              //if({

              //  this.MENSAJE = "Los datos fueron guardados satisfactoriamente";
                return treeviewDao.update_root(_id, _root, _id_icono);
            }
            catch (Exception ex)
            {
                this.MENSAJE = "Datos no guardados, pruebe con otro: " + ex;
                return false;
            }
        }
        public string Update_tbl_tramos_clientes()
        {
            try
            {
                treeviewDao.update_tbl_tramos_clientes();

                return "Los datos fueron cargados satisfactoriamente";
            }
            catch (Exception ex)
            {
                return "Datos no cargados, pruebe con otro: " + ex;
            }
        }

        public bool AgregarInstalación(int root, string nombre, double esc, double x, double y, Int16 grados, string capacidad, string sivplus, string bloque, string capa)
        {
            string usuario = UserCache.LoginUser;

                this.ROOT = root;
                this.ID_BLOQUE = Obt_Id_Bloque(bloque);
                this.ID_ICONO = Obt_Id_Icono(bloque);
                this.S_TABLA = treeviewDao.NombreTabla(ID_ICONO);
                if ((capa.Equals("TRANS") && S_TABLA.Equals("esq_red.tbl_tramos")) || (capa.Equals("RED") && S_TABLA.Equals("esq_red.tbl_cargas")))
                {
                    this.MENSAJE = string.Format("El Bloque ({0}) esta en CAPA ({1}) mal Asociada", bloque,capa );
                    return false;
                }
                string sql = "SELECT id FROM " + this.S_TABLA + " WHERE RTRIM(nombre)=RTRIM('" + nombre + "')";
                this.ID = treeviewDao.ValidarName(sql);//valida si existe la instalación    
           
                esc = Math.Round(esc, 1);
                //x = Math.Round(x, 4);
                //y = Math.Round(y, 4);

                if (this.ID.Equals(0))
                {                   
                    this.B_NUEVO = true;
                    //para tbl_cargas 
                    if (this.S_TABLA.Equals("esq_red.tbl_cargas"))
                    { 
                        double kva_inst = kVAinst(capacidad);                       
                        Int16 tip_conex = nTransf(capacidad);
                        if (tip_conex.Equals(1) && kva_inst > 100)
                        {
                            tip_conex = 3;
                        }
                        this.ID = treeviewDao.insert_tbl_carga(nombre, sivplus, capacidad, kva_inst, tip_conex, x, y, grados, esc, ID_BLOQUE, root, usuario);
                        this.S_NOMBRE = nombre + " " + capacidad + " (0)";

                    }
                    else if (this.S_TABLA.Equals("esq_red.tbl_tramos"))
                    {
                        this.ID = treeviewDao.insert_tbl_tramo(nombre, x, y, grados, esc, ID_BLOQUE, root, usuario);
                        this.S_NOMBRE = nombre;
                    }
                    this.MENSAJE = "Se agrego la instalación: " + nombre + " # id_tabla: " + ID;
                    return true;
                }
                else
                {
                    if (root.Equals(this.ID))
                    {
                        this.MENSAJE = "Asignación no permitida id=root="+root;
                        return false;
                    }

                    this.B_NUEVO = false;
                    if (this.S_TABLA.Equals("esq_red.tbl_cargas"))
                    {
                        this.ID = treeviewDao.update_tbl_cargas(ID, root, usuario);
                        this.S_NOMBRE = nombre + " " + capacidad + " (0)";
                    }
                    else if (this.S_TABLA.Equals("esq_red.tbl_tramos"))
                    {
                        this.ID = treeviewDao.update_tbl_tramo(ID, root, usuario);
                        this.S_NOMBRE = nombre;
                    }
                     this.MENSAJE =string.Format("Se Asocio la Instalación Existente:{0} al root:{1}", S_NOMBRE, root);
                     return true;
                }  
        }
                
        public bool UbicarInstalación(string codigo, string bloque)
        {
            this.ID_ICONO = Obt_Id_Icono(bloque);
            this.S_NOMBRE = codigo;
            this.S_TABLA=NombreTabla(this.ID_ICONO);
            string sql = "SELECT id FROM " + this.S_TABLA + " WHERE RTRIM(nombre)=RTRIM('" + codigo + "')";
            this.ID = treeviewDao.ValidarName(sql);//valida si existe la instalación  

                if (this.ID.Equals(0))
                {
                    this.MENSAJE = string.Format("No Existe la instalación en la Base de Datos Código:{0}, Bloque:{1}", S_NOMBRE, bloque);
                    return false;                   
                }
                else
                {
                    this.MENSAJE = string.Format("Existe la instalación en la Base de Datos Código:{0}, Bloque:{1}, id:{2}, tabla:{3}", S_NOMBRE, bloque, ID, this.S_TABLA);                    
                    return true;
                }
        }

        public bool ActualizarInst(string codigo, double esc, double x, double y, Int16 grados, string capacidad, string sivplus, string bloque)
        {
            //CadDao MCadDao = new CadDao();

            //double x = 0, y = 0, esc = 1.0;
            //Int16 grado = 0;
            //string text1 = null, text2 = null, capacidad = null, sivplus = null, bloque = null, capa = null;

            //if (MCadDao.SeleccionarBloque(out x, out y, out  text1, out  text2, out  capacidad, out  sivplus, out  bloque, out  capa, out  esc, out  grado))
            //{
                //this.ID_BLOQUE = Obt_Id_Bloque(bloque);
                //this.ID_ICONO = Obt_Id_Icono(bloque);
            Int16 id_icono_cad = Obt_Id_Icono(bloque);
            Int16 id_bloque_cad = Obt_Id_Bloque(bloque);
            string s_tabla_cad = NombreTabla(id_icono_cad);
                this.S_TABLA = NombreTabla(ID_ICONO);
                //string s_tabla_root = NombreTabla(id_icono);
                //this.S_NOMBRE = text1 + text2;

                if (S_TABLA.Equals(s_tabla_cad))
                {
                    string sql = "SELECT id FROM " + this.S_TABLA + " WHERE RTRIM(nombre)=RTRIM('" + codigo + "')";
                    int id_cad = treeviewDao.ValidarName(sql);//valida si existe la instalación                    
                    string usuario = UserCache.LoginUser;

                    if (id_cad.Equals(0) || this.ID.Equals(id_cad))
                    {
                        if (S_TABLA.Equals("esq_red.tbl_cargas"))
                        {
                            //string descr = sivplus;
                            CadModel cadModel = new CadModel();
                            double kva_inst = kVAinst(capacidad);
                            Int16 tip_conex = nTransf(capacidad);
                            if (tip_conex.Equals(1) && kva_inst > 100)
                            {
                                tip_conex = 3;
                            }
                            //if (sivplus.Length > 30)
                            //{
                            //    sivplus = sivplus.Substring(0, 30);
                            //}
                            this.ID = treeviewDao.update_tbl_cargas(ID, codigo, sivplus, capacidad, kva_inst, tip_conex, x, y, grados, esc, id_bloque_cad, usuario);
                            this.ID_ICONO = id_icono_cad;
                            this.S_NOMBRE = codigo + " " + capacidad + " (0)";
                            this.MENSAJE = string.Format("Instalación Actualizada: {0}, Nodo: {1}", S_NOMBRE, ID);
                            return true;
                        }
                        else if (S_TABLA.Equals("esq_red.tbl_tramos"))
                        {
                            this.S_NOMBRE = codigo;
                            this.ID = treeviewDao.update_tbl_tramo(ID, codigo, x, y, grados, esc, id_bloque_cad, usuario);
                            this.ID_ICONO = id_icono_cad;
                            this.MENSAJE = string.Format("Instalación Actualizada: {0}, Nodo: {1}", S_NOMBRE, ID);
                             return true;
                        }
                        else
                        {                           
                            this.MENSAJE = string.Format("Aplicación en Desarrollo, Nombre de Tabla : {0}, Nodo: {0}", S_TABLA);
                            return false;
                        }
                    }
                    else
                    {
                        this.MENSAJE = string.Format("Instalación Duplicada: {0}, Nodo: {0}", S_NOMBRE, ID);
                        return false;
                    }
                }
                else
                {
                    this.MENSAJE = "No corresponde el tipo de equipo (tbl_tramos / tbl_cargas)";
                    //return string.Format("No Corresponde el tipo de Bloque, con la base de datos: {0} ◄ - ► {1}", s_tabla_root, S_TABLA);
                    return false;
                }
            //}
            //else
            //{
            //    this.S_NOMBRE = "-1";
            //    return "Error en la selección del Bloque en AutoCAD";

            //}

        }

        public double kVAinst(string texto)
        {
            //S_CAPACIDAD = texto;
            return treeviewDao.kVAinst(texto);
        }
        public Int16 nTransf(string texto)
        {
            return treeviewDao.nTransf(texto);
        }
        public  Int16 Obt_Id_Icono(String bloque)
        {
            return treeviewDao.Obt_Id_Icono(bloque);
        }

        public Int16 Obt_Id_Bloque(String bloque)
        {
            return treeviewDao.Obt_Id_Bloque(bloque);
        }

        public string NombreTabla(Int16 id_icono)
        {
            return treeviewDao.NombreTabla(id_icono);
        }

        //public int ValidarBnCarga(string Name, string ntabla)
        //{
        //    return treeviewDao.ValidarName(Name, ntabla);
        //}

        public DataTable dt_lista(string vista, int root, bool ver)
        {
            //LocDao clsLista = new LocDao();
            //return clsLista.dt_lista(vista, root, ver);
            return treeviewDao.dt_lista(vista, root, ver);
        }

        public DataSet ds_lista(string vista)
        {           
            return treeviewDao.ds_lista(vista);         
        }

        public DataSet ds_recursive_cto(int id_cto)
        {
            return treeviewDao.ds_recursive_cto(id_cto);         
        }

        public DataSet ds_recursive_id_roots(int id)
        {
            return treeviewDao.ds_recursive_id_roots(id);
        }
        public DataSet ds_tbl_cargas(int id)
        {
            return treeviewDao.ds_tbl_cargas(id);
        }
        public DataSet ds_tbl_tramos(int id)
        {
            return treeviewDao.ds_tbl_tramos(id);
        }
    }
}
