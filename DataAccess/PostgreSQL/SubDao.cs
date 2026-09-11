using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Npgsql;
using NpgsqlTypes;

namespace Visoproy.DataAccess.PostgreSQL
{
    public class SubDao : ConnectionToPool
    {
        //METODOS/FUNCIONES

        public DataTable dt_lista(string sql)
        {
            return data_tabla(sql);
        }

        public bool update_tbl_sub(Int16 id, string gere, string kv, bool b_oper, bool b_pers, bool b_tronc, bool b_resg, bool b_obso
          , bool b_firme, bool b_kv,bool b_scada,string obs,Int16 year,string capa, string usuario)
        {      

            string sql = "UPDATE esq_red.tbl_sub set " +
               "f_act=now(),year_construccion=@year_construccion," +
               "cantidad_mva=@cantidad_mva," +
               "tensiones_kv=@tensiones_kv,gerencia=@gerencia,b_operadores=@b_operadores,b_personal=@b_personal," +
               "b_p_resguardo=@b_p_resguardo,b_p_obsolecencia=@b_p_obsolecencia,b_cap_firme=@b_cap_firme,b_p_tension=@b_p_tension," +
               "b_troncal=@b_troncal,b_scada=@b_scada,observacion=@observacion,usuario=@usuario " +
               "WHERE id=@id";

            using (var cmd = new NpgsqlCommand(sql, traerConexion()))
            {
                cmd.Parameters.Add("@gerencia", NpgsqlDbType.Varchar, 50).Value = gere;
                cmd.Parameters.Add("@tensiones_kv", NpgsqlDbType.Varchar, 50).Value = kv;
                cmd.Parameters.Add("@b_operadores", NpgsqlDbType.Boolean).Value = b_oper;
                cmd.Parameters.Add("@b_personal", NpgsqlDbType.Boolean).Value = b_pers;
                cmd.Parameters.Add("@b_troncal", NpgsqlDbType.Boolean).Value = b_tronc;

                cmd.Parameters.Add("@b_p_resguardo", NpgsqlDbType.Boolean).Value = b_resg;
                cmd.Parameters.Add("@b_p_obsolecencia", NpgsqlDbType.Boolean).Value = b_obso;
                cmd.Parameters.Add("@b_cap_firme", NpgsqlDbType.Boolean).Value = b_firme;
                cmd.Parameters.Add("@b_p_tension", NpgsqlDbType.Boolean).Value = b_kv;
                cmd.Parameters.Add("@b_scada", NpgsqlDbType.Boolean).Value = b_scada;

                cmd.Parameters.Add("@observacion", NpgsqlDbType.Text).Value = obs;
                cmd.Parameters.Add("@usuario", NpgsqlDbType.Varchar, 15).Value =usuario;

                cmd.Parameters.Add("@year_construccion", NpgsqlDbType.Smallint).Value = year;
                cmd.Parameters.Add("@cantidad_mva", NpgsqlDbType.Varchar, 20).Value = capa;
                cmd.Parameters.Add("@id", NpgsqlDbType.Integer).Value = id;

                openConnection();

                int res = cmd.ExecuteNonQuery();
                terminaPool();

                if (res == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public bool update_tbl_sub_xy(int id, double x, double y, double escala, short grados, string usuario)
        {

            string sql = "UPDATE esq_red.tbl_sub set " +
               "f_act=now(),x=@x,y=@y,escala=@escala,grados=@grados,usuario=@usuario " +
               "WHERE id=@id";

            using (var cmd = new NpgsqlCommand(sql, traerConexion()))
            {
                cmd.Parameters.Add("@id", NpgsqlDbType.Integer).Value = id;
                cmd.Parameters.Add("@grados", NpgsqlDbType.Smallint).Value = grados;
                cmd.Parameters.Add("@escala", NpgsqlDbType.Numeric).Value = escala;
                cmd.Parameters.Add("@x", NpgsqlDbType.Integer).Value = x;
                cmd.Parameters.Add("@y", NpgsqlDbType.Integer).Value = y;
                cmd.Parameters.Add("@usuario", NpgsqlDbType.Varchar, 15).Value = usuario;

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
