using System;
using System.Collections.Generic;
using System.Text;
using DataAccess.PostgreSQL;
using System.Data;

namespace Domain
{
   public class SubModel
    {
       SubDao subDao = new SubDao();

       public static short ID { get; set; }
       public static short YEAR { get; set; }
       public static string CAPACIDAD { get; set; }
       public static string KV { get; set; }
       public static string GERENCIA { get; set; }
       public static string OBS { get; set; }
       public static string USER { get; set; }

       public static bool B_OPER { get; set; }
       public static bool B_PERS { get; set; }
       public static bool B_RESG { get; set; }
       public static bool B_OBSO { get; set; }
       public static bool B_FIRM { get; set; }
       public static bool B_KV { get; set; }
       public static bool B_TRONC { get; set; }
       public static bool B_SCADA{ get; set; }

       public static int X { get; set; }
       public static int Y { get; set; }
       public static float ESCALA { get; set; }       
       public static short GRADOS { get; set; }


       public SubModel()
       {
       }
       public DataTable dt_lista(string sql)
       {
           return subDao.dt_lista(sql);
       }

       public DataTable dt_sub(string id_sub)
       {
           string sql = "SELECT c.id AS id,RTRIM(c.nombre) AS subestacion,RTRIM(b.nombre) AS sistema,RTRIM(c.gerencia) AS gerencia,";
           sql += "RTRIM(c.tensiones_kv) AS tensiones_kv,c.year_construccion,RTRIM(c.cantidad_mva) AS cantidad_mva,c.b_scada,";
           sql += "c.b_operadores AS b_operadores,c.b_personal AS b_personal,c.b_troncal AS b_troncal,";
           sql += "c.b_p_resguardo,c.b_p_obsolecencia,c.b_cap_firme,c.b_p_tension,";
           sql += "c.x,c.y,c.grados,c.escala,";
           sql += "RTRIM(c.direccion) AS direccion,c.id_estado,RTRIM(a.nombre) AS estado,c.id_municipio,RTRIM(f.nombre) AS municipio,";

           sql += "RTRIM(g.nombre) AS parroquia,c.id_parroquia,g.id_postal,RTRIM(h.nombre) AS cs,";

           sql += "RTRIM(c.observacion) AS observacion,c.f_act AS f_act,RTRIM(c.usuario) AS usuario ";
         
               sql += "FROM esq_open.lta_estados a,esq_red.lta_sistema b,esq_red.tbl_sub c, ";
               sql += "esq_open.lta_municipios f,esq_open.lta_parroquias g,esq_proy.lta_cs h ";
         
               sql += "WHERE a.id= b.root AND b.id= c.root AND f.id=c.id_municipio AND g.id=c.id_parroquia AND h.id=g.id_cs ";
               sql += "AND c.id='" + id_sub + "' ";

               return subDao.data_tabla(sql);
       }

       public bool update_tbl_Sub(){
           return subDao.update_tbl_sub(ID, GERENCIA, KV, B_OPER, B_PERS, B_TRONC, B_RESG, B_OBSO, B_FIRM, B_KV, B_SCADA, OBS, YEAR, CAPACIDAD,USER);
                  }

       public bool update_tbl_sub_xy()
       {
           return subDao.update_tbl_sub_xy(ID,X, Y, ESCALA, GRADOS, USER);
                
       }
    }
}
