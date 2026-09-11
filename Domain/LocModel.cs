using System;
using System.Collections.Generic;
using System.Text;
using Visoproy.DataAccess.PostgreSQL;
using System.Data;

namespace Domain
{
    
    public class LocModel
    {
        LocDao locDao = new LocDao();
        ////Atributes
        public static short ID { get; set; }
        public static string TIPO { get; set; }
        public static string LOCALIDAD { get; set; }
        public static string MUNICIPIO { get; set; }
        public static string PARROQUIA { get; set; }
        public static int X { get; set; }
        public static int Y { get; set; }
        public static Single ESCALA { get; set; }
        public static int AREA { get; set; }
        public static short CLIENTES { get; set; }
        public static bool ABREV { get; set; }

        public static DateTime ACTUALIZACION { get; set; }
        public static string USUARIO { get; set; }

        public LocModel()
        {

        }

        //public DataTable dt_lista(string s_tabla, int root,bool ver)
        //{
        //    return locDao.dt_lista(s_tabla, root, ver);
        //}

        public DataTable dt_combobox(string s_tabla, int root)
        {
            return locDao.dt_combobox(s_tabla, root);
        }

        public DataTable dt_lista_parroquia(int root)
        {
            return locDao.dt_combobox_parroquia(root);
        }

        public DataTable dt_localidad_vec(double _x,double _y,string _mts,string _tipo)
        {
            int x1 = Convert.ToInt32(_x) + System.Convert.ToInt16(_mts);
            int x2 = Convert.ToInt32(_x) - System.Convert.ToInt16(_mts);
            int y1 = Convert.ToInt32(_y) + System.Convert.ToInt16(_mts);
            int y2 = Convert.ToInt32(_y) - System.Convert.ToInt16(_mts);

            string sql = "a.x <'" + x1 + "' AND a.x >'" + x2 + "' ";
            sql += "AND a.y <'" + y1 + "' AND a.y >'" + y2 + "' ";

            return dt_localidad("MTS_VEC", sql, _tipo);
        }

        public DataTable dt_localidad(string n_col, string variable, string _tipo)
        {           
            string sql = "SELECT a.id AS id,";
            sql += "RTRIM(d."+_tipo+") AS tipo,";
            sql += "RTRIM(a.nombre) AS localidad,";
            sql += "RTRIM(c.nombre) AS municipio,RTRIM(b.nombre) AS parroquia,";
            //sql += "RTRIM(b.nombre) AS parroquia,b.id AS id_parroquia,RTRIM(c.nombre) AS municipio,c.id AS id_municipio,";
            sql += "a.x AS x,a.y AS y,a.escala AS escala,a.area AS área,a.num_clientes AS clientes,";
            sql += "a.f_act as actualización,RTRIM(e.nombre) AS usuario ";//,RTRIM(e.nombre) AS usuario

            sql += "FROM esq_open.tbl_zona a, esq_open.lta_parroquias b,esq_open.lta_municipios c, esq_open.lta_t_zona d ";
            sql += ", esq_proy.tbl_personal e ";

            sql += "WHERE a.root=b.id AND b.root=c.id AND a.id_t_loc=d.id AND c.root=8";
            sql += " AND a.usuario=e.usuario";

            if (n_col != "")
            {
                if (n_col.Equals("MTS_VEC"))                
                    sql += " AND " + variable;                
                else if (!n_col.Equals("a.nombre"))                
                    sql += " AND " + n_col + "='" + variable + "' ";                             
                else
                {
                    variable = variable.ToUpper();
                    sql += " AND TRANSLATE(upper(" + n_col + "),'áéíóúÁÉÍÓÚçÇ','aeiouAEIOUcC') like TRANSLATE('%" + variable + "%','áéíóúÁÉÍÓÚçÇ','aeiouAEIOUcC') ";
                }

            }


            return locDao.data_tabla(sql);
        }

        public bool update_tbl_zona(int id, Single escala, int x, int y, int area, string usuario)
        {
            return locDao.update_tbl_zona(id, escala, x, y, area, usuario);

        }
    }
}
