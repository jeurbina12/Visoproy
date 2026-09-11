using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Visoproy.DataAccess.PostgreSQL;

namespace Domain
{
    public class CtoModel
    {
        CtoDao ctoDao = new CtoDao();

        public static short ID { get; set; }
        //public static short ID_PADEE { get; set; }
        //public static short AMP { get; set; }
        //public static short AMP_P { get; set; }
        //public static short ID_COND { get; set; }
        //public static short ID_COND_SCADA { get; set; }
        //public static short ID_COLOR { get; set; }
        //public static short ID_CS { get; set; }
        //public static short SUSCRIP { get; set; }
        //public static short MTS { get; set; }

        public static int X { get; set; }
        public static int Y { get; set; }

        //public static float FP { get; set; }
        //public static float CV { get; set; }
        //public static float CC { get; set; }

        //public static string OBS { get; set; }
        //public static string OBS_ESP { get; set; }
        //public static string USER { get; set; }
        //public static string T_INTERR { get; set; }
        //public static string SALA { get; set; }

        //public static char HORARIO { get; set; }
        //public static char BLOQUE { get; set; }

        //public static DateTime F_MAX { get; set; }
        //public static DateTime F_ADM { get; set; }


        public DataTable dt_lista(string s_tabla)
        {
            string sql = "SELECT id, rtrim(nombre) as nombre FROM " + s_tabla;
            return ctoDao.dt_lista(sql);
        }

        public DataTable dt_combobox(string s_tabla, int root)
        {
            return ctoDao.dt_combobox(s_tabla, root);
        }              

        public DataTable dt_cto(string id_cto)
        {
            string sql = "SELECT ";
            sql += "rtrim(b.nombre) AS cto,rtrim(a.nombre) AS transformador, rtrim(c.nombre) AS subestacion,";
            sql += "rtrim(b.s_interruptor) AS s_interruptor,n_padee,id_color,b.x AS x,b.y AS y,b.f_act,b.usuario,";
            sql += "b.id_cs,b.id_cond_inter,rtrim(t_interruptor) as t_interruptor,";
            sql += "rtrim(t_red) as t_red,rtrim(calibre_troncal) as calibre_troncal,mts_troncal,mts_total,rtrim(t_carga) as t_carga,";
            sql += "b.kva_inst,suscriptores,n_ptos,n_transf,";
            sql += "id_cond_scada,b.b_p_medicion,b.b_p_scada,";
            sql += "b.amp_max,b.fp,b.cv,b.cc,a.kv_baja*a.pvar/100 AS kv,b.f_max,";//to_char(a.kv_baja*a.pvar/100::real,'999D99') AS kv

            sql += "b.id_grupo,b.horario,b.amp_adm,b_no_adm,b.f_adm,id_sala,";//,to_char(b.amp_adm*a.kv_baja*sqrt(3)*b.fp/1000::real,'999D99') AS mw_adm

            sql += "rtrim(b.obs_cto) as obs_cto,rtrim(b.sectores) as sectores,rtrim(obs_esp) as obs_esp ";                      

            sql += "FROM esq_red.tbl_transf a,esq_red.tbl_ctos b,esq_red.tbl_sub c ";
           
            sql += "WHERE a.root= c.id AND b.root= a.id AND b.id='" + id_cto + "'; ";

            return ctoDao.dt_lista(sql);
        }

        public DataTable dt_ctos(string n_col, string variable, bool b_oper, bool b_salas,bool b_f_inc)
        {
            string sql = "SELECT a.id AS id,";

            //sql += "row_number() OVER (ORDER BY ";
            //if (b_f_inc)
            //        sql += "a.f_adm ";
            //    else
            //        sql += "a.f_max ";             
            //sql += "ASC) AS orden,";

            sql += "rtrim(a.s_interruptor) AS \"Código\",";
            sql += "RTRIM(a.nombre) AS \"Alimentador\",";
            sql += "substring(b.nombre,4,2) AS \"Tx\",";
            sql += "RTRIM(c.nombre) AS \"Subestación\",";
            sql += "a.amp_max AS \"Imax\",a.amp_adm AS \"Iprom\",a.fp,b.kv_baja AS \"kV\",b.pvar,";
            sql += "round(a.amp_max*b.kv_baja*sqrt(3)*a.fp*b.pvar/100000,2) AS \"MWmax\",";
            sql += "round(a.amp_adm*b.kv_baja*sqrt(3)*a.fp*b.pvar/100000,2) AS \"MWinc\",";
            sql += "sum(round(a.amp_adm*b.kv_baja*sqrt(3)*a.fp*b.pvar/100000,2)) over (order by ";
            if (b_f_inc)
                sql += "a.f_adm ";
            else
                sql += "a.f_max ";              
            sql += "ASC rows unbounded preceding) as kWtot,";

            if (b_f_inc)           
                sql += "a.f_adm as \"Fecha Incidencia\",";  
            else
                sql += "a.f_max as \"Fecha Máxima\",";

            sql += "a.horario,a.id_grupo AS \"Grupo\",d.nombre AS \"Sala\",";
            sql += "a.kva_inst,a.suscriptores AS clientes,";

            sql += "a.x AS x,a.y AS y,RTRIM(a.usuario) AS usuario ";//,RTRIM(e.nombre) AS usuario

            sql += "FROM esq_red.tbl_ctos a, esq_red.tbl_transf b,esq_red.tbl_sub c,esq_red.lta_sala d ";

            sql += "WHERE a.root=b.id AND b.root=c.id AND a.id_sala=d.id ";
            //sql += " AND a.usuario=e.usuario";

            if (n_col != "")
            {
                if (n_col.Equals("Grupos"))
                {
                    sql += " AND a.id_grupo" + variable;
                    //if (variable.Substring(0, 1) == "=")
                    //{
                    //    //variable = variable.Replace("=", "");
                    //    sql += " AND a.id_grupo" + variable;

                    //}
                    //else if (variable.Substring(0, 1) == ">")
                    //{
                    //    variable = variable.Replace(">", "");
                    //    sql += " AND a.id_grupo=>" + variable;
                    //}
                    //else
                    if (variable.Substring(0, 1) == "<")
                    {
                        sql += " AND a.id_grupo>0";
                    }
                    //else
                    //{
                    //MessageBox.Show("Formato permitidos:\n =0\n>2000\n<100", "Error en variable", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //cbx_ltaVariable.Text = "=0";
                    //variable = "0";
                    //sql += "='" + variable + "' ";
                    //System.Environment.Exit(0); 
                    //}
                    
                }
                //else if (n_col.Equals("kWinc"))
                //{
                //    sql += " AND a.id_grupo>0 AND kWtot" + variable;
                //}
                else
                {
                    sql += " AND " + n_col + "='" + variable + "' ";
                }

                if (b_oper)
                 if (n_col != "a.id_cond_inter")           
                     sql += " AND a.id_cond_inter=1 ";

                if (b_salas)
                    if (n_col != "a.id_sala")
                        sql += " AND (a.id_sala=2 OR a.id_sala=3 OR a.id_sala=4) ";

                if (b_f_inc)
                    sql += " ORDER BY a.f_adm ASC";
                else
                    sql += " ORDER BY a.f_max ASC";

                //switch (n_col)
                //{
                //    case "":
                        
                //        break;

                //}
                
                
                    
                //if (n_col.Equals("MTS_VEC"))
                //    sql += " AND " + variable;
                //else if (!n_col.Equals("a.nombre"))
                //    sql += " AND " + n_col + "='" + variable + "' ";
                //else
                //{
                //    variable = variable.ToUpper();
                //    sql += " AND TRANSLATE(upper(" + n_col + "),'áéíóúÁÉÍÓÚçÇ','aeiouAEIOUcC') like TRANSLATE('%" + variable + "%','áéíóúÁÉÍÓÚçÇ','aeiouAEIOUcC') ";
                //}

            }


            return ctoDao.data_tabla(sql);
        }

        public bool update_tbl_cto(int id, string s_interruptor, short n_padee, short id_color, short id_cs, short id_cond_inter,
            string t_interruptor, string t_red, string calibre_troncal, int mts_troncal, int mts_total, string t_carga,
            double kva_inst, short suscriptores, short n_ptos, short n_transf,
            short id_cond_scada, bool b_p_medicion, bool b_p_scada,
            short amp_max, float fp, float cv, float cc, DateTime f_max,
            short id_grupo, char horario, short amp_adm, bool b_no_adm, DateTime f_adm, short id_sala,
           string obs_cto, string sectores, string obs_esp, string usuario)
        {
            return ctoDao.update_tbl_cto(id, s_interruptor, n_padee, id_color, id_cs,  id_cond_inter,
            t_interruptor, t_red,  calibre_troncal,  mts_troncal,  mts_total,  t_carga,
            kva_inst,  suscriptores, n_ptos,  n_transf,
            id_cond_scada, b_p_medicion, b_p_scada,
            amp_max, fp, cv, cc,  f_max,
            id_grupo, horario, amp_adm, b_no_adm,  f_adm, id_sala,
           obs_cto, sectores, obs_esp, usuario);
        }

        public bool update_tbl_cto_color(short n_padee, short id_color)
        {
            return ctoDao.update_tbl_cto_color(n_padee, id_color);
        }
    }
}
