using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Npgsql;
using NpgsqlTypes;


namespace DataAccess.PostgreSQL
{
    public class ClienteDao : ConnectionToPool
    {
       
        //METODOS/FUNCIONES
       
        public DataTable dt_lista(string sql)
        {           
            return data_tabla(sql);
        }          

        public bool update_tbl_postes(int id_poste, string poste, string usuario)
        {
            string sql = "UPDATE esq_red.tbl_postes set ";
            sql += "poste=@poste,usuario=@usuario,f_act=now() ";           
            sql += "WHERE id=@id";

            using (var cmd = new NpgsqlCommand(sql, traerConexion()))
            {
                cmd.Parameters.Add("@id", NpgsqlDbType.Integer).Value = id_poste;
                cmd.Parameters.Add("@poste", NpgsqlDbType.Varchar, 10).Value = poste;
                cmd.Parameters.Add("@usuario", NpgsqlDbType.Varchar, 15).Value = usuario;

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

        public bool update_tbl_cargas()
        {
            string sql = "with t as (select poste,COUNT(id) AS Total FROM esq_red.tbl_postes v GROUP BY poste) ";
            sql += "UPDATE esq_red.tbl_cargas SET num_clientes=t.Total ";
            sql += "FROM t WHERE t.poste=nombre ";

            using (var cmd = new NpgsqlCommand(sql, traerConexion()))
            { 
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

        public int insert_tbl_postes(string serie,Int32 nis,Int32 nif,string poste,string usuario)
        {
            try
            {
                string sql = "SELECT nextval('esq_red.tbl_postes_id_seq');";
                int count;
                using (var command = new NpgsqlCommand(sql, traerConexion()))
                {
                    openConnection();
                    count = Convert.ToInt32(command.ExecuteScalar());
                }

                string esq = "esq_red";
                string tabla = "tbl_postes";

                sql = "INSERT INTO " + esq + "." + tabla +
                    "(id,serie,nis,nif,poste,usuario) " +
                        "VALUES(@id,@serie,@nis,@nif,@poste,@usuario);";                      
                


                using (var cmd = new NpgsqlCommand(sql, traerConexion()))
                {
                    cmd.Parameters.Add("@serie", NpgsqlDbType.Varchar, 2).Value = serie;
                    cmd.Parameters.Add("@nis", NpgsqlDbType.Integer).Value = nis;
                    cmd.Parameters.Add("@nif", NpgsqlDbType.Integer).Value = nif;
                    cmd.Parameters.Add("@poste", NpgsqlDbType.Varchar, 10).Value = poste;
                    cmd.Parameters.Add("@usuario", NpgsqlDbType.Varchar, 15).Value = usuario;
                    cmd.Parameters.Add("@id", NpgsqlDbType.Integer).Value = count;                  

                    int res = cmd.ExecuteNonQuery();

                    if (res == 0)
                    {
                        return 0;
                    }
                    else
                    {
                        return count;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                terminaPool();
            }
        }   



    }
}
