using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Npgsql;
using NpgsqlTypes;


namespace Visoproy.DataAccess.PostgreSQL
{
    public class LocDao : ConnectionToPool
    {       
        //METODOS/FUNCIONES
        //public DataTable data_tabla(String sql)
        //{
        //    return data_tabla(sql);
        //}

        //public DataTable dt_lista(string s_tabla,int root,bool ver)
        //{ 
        //    string sql = "SELECT * FROM " + s_tabla;           
        //        if (root > 0)
        //        {
        //            sql += " WHERE root=" + root;
        //            if (ver) sql += ", ver=true";
        //        }
        //        else
        //        {
        //            if (ver) sql += " WHERE ver=true";

        //        }    
        //        return data_tabla(sql);               
        //}

        public DataTable dt_combobox(string s_tabla, int root)
        {
            string sql = "SELECT id,rtrim(nombre) AS nombre FROM " + s_tabla;            
                sql += " WHERE root=" + root + " ORDER BY nombre ASC;";    
            return data_tabla(sql);
        }

        public DataTable dt_combobox_parroquia(int root_estado)
        {            
            String ntablas, fields, nwhere, sql;          
            
                ntablas = "esq_open.lta_municipios a,esq_open.lta_parroquias b";
                fields = "b.id AS id,rtrim(b.nombre) AS nombre";
                nwhere = "a.id=b.root AND a.root ='" + root_estado + "' ";

                sql = "SELECT " + fields + " FROM " + ntablas + " WHERE " + nwhere + " ORDER BY b.nombre ASC;";                        

            return data_tabla(sql); 
        }

        public bool update_tbl_zona(int id, Single escala,int x,int y,int area, string usuario)
        {
            string sql = "UPDATE esq_open.tbl_zona set ";
            sql += "escala=@escala,x=@x,area=@area,y=@y,usuario=@usuario,f_act=now() ";
            sql += "WHERE id=@id";

            using (var cmd = new NpgsqlCommand(sql, traerConexion()))
            {
                cmd.Parameters.Add("@id", NpgsqlDbType.Integer).Value = id;
                cmd.Parameters.Add("@escala", NpgsqlDbType.Numeric).Value = escala;
                cmd.Parameters.Add("@x", NpgsqlDbType.Integer).Value = x;
                cmd.Parameters.Add("@y", NpgsqlDbType.Integer).Value = y;
                cmd.Parameters.Add("@area", NpgsqlDbType.Integer).Value = area;

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
