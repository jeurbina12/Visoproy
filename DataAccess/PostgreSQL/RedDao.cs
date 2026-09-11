using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Npgsql;
using NpgsqlTypes;

namespace Visoproy.DataAccess.PostgreSQL
{
    public class RedDao : ConnectionToPool
    {
        //METODOS/FUNCIONES
        public DataTable dt_lista(String sql)
        {
            return data_tabla(sql);
        }

        public bool update_tbl(string s_tabla, int id,int root, string nombre, string usuario)
        {
            string sql = "UPDATE esq_red."+s_tabla+ " set ";
            sql += "root=@root,nombre=@nombre,usuario=@usuario,f_act=now() ";
            sql += "WHERE id=@id";

            using (var cmd = new NpgsqlCommand(sql, traerConexion()))
            {
                cmd.Parameters.Add("@root", NpgsqlDbType.Integer).Value = root;
                cmd.Parameters.Add("@nombre", NpgsqlDbType.Varchar, 25).Value = nombre;
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

    }
}
