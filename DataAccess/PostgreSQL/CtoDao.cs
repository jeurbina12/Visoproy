using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using NpgsqlTypes;
using Npgsql;

namespace Visoproy.DataAccess.PostgreSQL
{
    public class CtoDao : ConnectionToPool
    {
        //METODOS/FUNCIONES

        public DataTable dt_lista(string sql)
        {
            return data_tabla(sql);
        }

        public DataTable dt_combobox(string s_tabla, int root)
        {
            string sql = "SELECT id,rtrim(nombre) AS nombre FROM " + s_tabla;
            if (!root.Equals(0))
                sql += " WHERE root=" + root;
            sql += " ORDER BY nombre ASC;";
            return data_tabla(sql);
        }        

        public bool update_tbl_cto(int id, string s_interruptor, short n_padee, short id_color, short id_cs, short id_cond_inter,
            string t_interruptor, string t_red, string calibre_troncal, int mts_troncal, int mts_total,string t_carga,
            double kva_inst, short suscriptores, short n_ptos, short n_transf,
            short id_cond_scada, bool b_p_medicion, bool b_p_scada,
            short amp_max,float fp, float cv, float cc, DateTime f_max,
            short id_grupo, char horario, short amp_adm, bool b_no_adm, DateTime f_adm, short id_sala,
           string obs_cto, string sectores, string obs_esp, string usuario)
        {

            string sql = "UPDATE esq_red.tbl_ctos set " +
               "f_act=now(),s_interruptor=@s_interruptor," +
               "n_padee=@n_padee,id_color=@id_color,id_cs=@id_cs,id_cond_inter=@id_cond_inter,t_interruptor=@t_interruptor," +
               "t_red=@t_red,calibre_troncal=@calibre_troncal,mts_troncal=@mts_troncal,mts_total=@mts_total,t_carga=@t_carga," +
               "kva_inst=@kva_inst,suscriptores=@suscriptores,n_ptos=@n_ptos,n_transf=@n_transf," +
               "id_cond_scada=@id_cond_scada,b_p_medicion=@b_p_medicion,b_p_scada=@b_p_scada," +
               "amp_max=@amp_max,fp=@fp,cv=@cv,cc=@cc,f_max=@f_max," +
               "id_grupo=@id_grupo,horario=@horario,amp_adm=@amp_adm,b_no_adm=@b_no_adm,f_adm=@f_adm,id_sala=@id_sala," +
               "obs_cto=@obs_cto,sectores=@sectores,obs_esp=@obs_esp,usuario=@usuario " +
               "WHERE id=@id";

            using (var cmd = new NpgsqlCommand(sql, traerConexion()))
            {
                cmd.Parameters.Add("@s_interruptor", NpgsqlDbType.Varchar, 5).Value = s_interruptor;
                cmd.Parameters.Add("@n_padee", NpgsqlDbType.Smallint).Value = n_padee;
                cmd.Parameters.Add("@id_color", NpgsqlDbType.Smallint).Value = id_color;
                cmd.Parameters.Add("@id_cs", NpgsqlDbType.Smallint).Value = id_cs;
                cmd.Parameters.Add("@id_cond_inter", NpgsqlDbType.Smallint).Value = id_cond_inter;
                cmd.Parameters.Add("@t_interruptor", NpgsqlDbType.Varchar, 20).Value = t_interruptor;
                cmd.Parameters.Add("@t_red", NpgsqlDbType.Varchar, 11).Value = t_red;
                cmd.Parameters.Add("@calibre_troncal", NpgsqlDbType.Varchar, 10).Value = calibre_troncal;
                
                cmd.Parameters.Add("@mts_troncal", NpgsqlDbType.Integer).Value = mts_troncal;
                cmd.Parameters.Add("@mts_total", NpgsqlDbType.Integer).Value = mts_total;
                cmd.Parameters.Add("@t_carga", NpgsqlDbType.Varchar, 10).Value = t_carga;

                cmd.Parameters.Add("@kva_inst", NpgsqlDbType.Real).Value = kva_inst;
                cmd.Parameters.Add("@suscriptores", NpgsqlDbType.Smallint).Value = suscriptores;
                cmd.Parameters.Add("@n_ptos", NpgsqlDbType.Smallint).Value = n_ptos;
                cmd.Parameters.Add("@n_transf", NpgsqlDbType.Smallint).Value = n_transf;
                

                cmd.Parameters.Add("@id_cond_scada", NpgsqlDbType.Smallint).Value = id_cond_scada;
                cmd.Parameters.Add("@b_p_medicion", NpgsqlDbType.Boolean).Value = b_p_medicion;
                cmd.Parameters.Add("@b_p_scada", NpgsqlDbType.Boolean).Value = b_p_scada;

                cmd.Parameters.Add("@amp_max", NpgsqlDbType.Smallint).Value = amp_max;
                cmd.Parameters.Add("@fp", NpgsqlDbType.Real).Value = fp;
                cmd.Parameters.Add("@cv", NpgsqlDbType.Real).Value = cv;
                cmd.Parameters.Add("@cc", NpgsqlDbType.Real).Value = cc;
                cmd.Parameters.Add("@f_max", NpgsqlDbType.Timestamp).Value = f_max;

                cmd.Parameters.Add("@id_grupo", NpgsqlDbType.Smallint).Value = id_grupo;
                cmd.Parameters.Add("@horario", NpgsqlDbType.Char).Value = horario;
                cmd.Parameters.Add("@amp_adm", NpgsqlDbType.Smallint).Value = amp_adm;
                cmd.Parameters.Add("@b_no_adm", NpgsqlDbType.Boolean).Value = b_no_adm;
                cmd.Parameters.Add("@f_adm", NpgsqlDbType.Timestamp).Value = f_adm;
                cmd.Parameters.Add("@id_sala", NpgsqlDbType.Smallint).Value = id_sala;

                cmd.Parameters.Add("@obs_cto", NpgsqlDbType.Text).Value = obs_cto;
                cmd.Parameters.Add("@sectores", NpgsqlDbType.Text).Value = sectores;
                cmd.Parameters.Add("@obs_esp", NpgsqlDbType.Text).Value = obs_esp;
                cmd.Parameters.Add("@usuario", NpgsqlDbType.Varchar, 15).Value = usuario;
                
               
                cmd.Parameters.Add("@id", NpgsqlDbType.Integer).Value = id;

                openConnection();

                int res = cmd.ExecuteNonQuery();
                terminaPool();

                if (res == 0)                
                    return false;               
                else                
                    return true;
                
            }
        }

        public bool update_tbl_cto_color(short n_padee, short id_color)
        {

            string sql = "UPDATE esq_red.tbl_ctos set " +               
               "id_color=@id_color " +
               "WHERE n_padee=@n_padee";

            using (var cmd = new NpgsqlCommand(sql, traerConexion()))
            {               
                cmd.Parameters.Add("@id_color", NpgsqlDbType.Smallint).Value = id_color;
                cmd.Parameters.Add("@n_padee", NpgsqlDbType.Smallint).Value = n_padee;

                openConnection();

                int res = cmd.ExecuteNonQuery();
                terminaPool();

                if (res == 0)
                    return false;
                else
                    return true;

            }
        }
    }
}
