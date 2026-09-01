using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NpgsqlTypes;
using Npgsql;
using System.Data;

namespace DataAccess.PostgreSQL
{
    public class TransfDao : ConnectionToPool
    {
        //METODOS/FUNCIONES

        public DataTable dt_lista(string sql)
        {
            return data_tabla(sql);
        }

        public bool update_tbl_transf(short id, DateTime f_max, decimal mva, decimal z, string marca,
            short amp, decimal kv1, decimal kv2, string obs, bool b_movil, bool b_p_voltaje,
            bool b_p_proteccion, bool b_p_scada, bool b_p_ventilacion
          , bool b_p_operativo, short id_cond,short pvar, string usuario)
        {

            string sql = "UPDATE esq_red.tbl_transf set " +
               "f_act=now(),f_max=@f_max,mva_inst=@mva_inst," +
               "amp_max=@amp_max,kv_alta=@kv_alta,kv_baja=@kv_baja,z=@z,marca=@marca," +
               "b_movil=@b_movil,b_p_voltaje=@b_p_voltaje,b_p_proteccion=@b_p_proteccion," +
               "b_p_scada=@b_p_scada,b_p_ventilacion=@b_p_ventilacion,b_p_operativo=@b_p_operativo," +
               "id_cond_inter=@id_cond_inter,observacion=@observacion,pvar=@pvar,usuario=@usuario " +
               "WHERE id=@id";

            using (var cmd = new NpgsqlCommand(sql, traerConexion()))
            {
                cmd.Parameters.Add("@f_max", NpgsqlDbType.Timestamp).Value = f_max;// Convert.ToDateTime(f_max);
                cmd.Parameters.Add("@mva_inst", NpgsqlDbType.Real).Value = mva;// System.Convert.ToDecimal(txtMvaInst.Text.Replace(",", "."), provider);
                cmd.Parameters.Add("@z", NpgsqlDbType.Real).Value = z;
                cmd.Parameters.Add("@marca", NpgsqlDbType.Varchar, 20).Value = marca;

                cmd.Parameters.Add("@amp_max", NpgsqlDbType.Smallint).Value = amp;
                cmd.Parameters.Add("@pvar", NpgsqlDbType.Smallint).Value = pvar;
                cmd.Parameters.Add("@kv_alta", NpgsqlDbType.Real).Value = kv1;
                cmd.Parameters.Add("@kv_baja", NpgsqlDbType.Real).Value = kv2;
                cmd.Parameters.Add("@observacion", NpgsqlDbType.Text).Value = obs;

                cmd.Parameters.Add("@b_movil", NpgsqlDbType.Boolean).Value = b_movil;
                cmd.Parameters.Add("@b_p_voltaje", NpgsqlDbType.Boolean).Value = b_p_voltaje;
                cmd.Parameters.Add("@b_p_proteccion", NpgsqlDbType.Boolean).Value = b_p_proteccion;
                cmd.Parameters.Add("@b_p_scada", NpgsqlDbType.Boolean).Value = b_p_scada;
                cmd.Parameters.Add("@b_p_ventilacion", NpgsqlDbType.Boolean).Value = b_p_ventilacion;
                cmd.Parameters.Add("@b_p_operativo", NpgsqlDbType.Boolean).Value = b_p_operativo;

                cmd.Parameters.Add("@id_cond_inter", NpgsqlDbType.Smallint).Value = id_cond;

                cmd.Parameters.Add("@usuario", NpgsqlDbType.Varchar, 15).Value = usuario;

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

        public bool update_tbl_transf_xy(int id, double x, double y, double escala, short grados, string cod, string usuario)
        {

            string sql = "UPDATE esq_red.tbl_transf set " +
               "f_act=now(),x=@x,y=@y,escala=@escala,grados=@grados,codigo=@codigo,usuario=@usuario " +
               "WHERE id=@id";

            using (var cmd = new NpgsqlCommand(sql, traerConexion()))
            {
                cmd.Parameters.Add("@id", NpgsqlDbType.Integer).Value = id;
                cmd.Parameters.Add("@grados", NpgsqlDbType.Smallint).Value = grados;
                cmd.Parameters.Add("@escala", NpgsqlDbType.Numeric).Value = escala;
                cmd.Parameters.Add("@x", NpgsqlDbType.Integer).Value = x;
                cmd.Parameters.Add("@y", NpgsqlDbType.Integer).Value = y;
                cmd.Parameters.Add("@codigo", NpgsqlDbType.Varchar, 10).Value = cod;
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
