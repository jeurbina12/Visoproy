using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Npgsql;
using NpgsqlTypes;
using System.Globalization;

namespace DataAccess.PostgreSQL
{
    public class TreeviewDao : ConnectionToPool
    {
        // Wrappers to expose ConnectionToPool helpers in case of resolution issues
        public new int ValidarName(string sql)
        {
            return base.ValidarName(sql);
        }

        public new string NombreTabla(Int16 id_icono)
        {
            return base.NombreTabla(id_icono);
        }

        //METODOS/FUNCIONES
        public DataSet ds_lista(string vista)
        {
            string nVista = "SELECT * FROM " + vista;
            return data_set(nVista);
        }

        public DataTable dt_lista(string s_tabla, int root, bool ver)
        {
            string sql = "SELECT * FROM " + s_tabla;

            if (root > 0)
            {
                sql += " WHERE root=" + root;
                if (ver) sql += ", ver=true";
            }
            else
            {
                if (ver) sql += " WHERE ver=true";

            }


            return data_tabla(sql);

        }

        public DataSet ds_tbl_tramos(int id)
        {
            string fields, ntablas, nwhere;

            fields = "a.nombre,x,y,grados,escala,b.nombre AS bloque";
            ntablas = "esq_red.tbl_tramos a,esq_red.lta_bloques b";
            nwhere = " a.id_bloque=b.id AND a.id=" + id;

            string sql = "SELECT " + fields + " FROM " + ntablas + " WHERE " + nwhere;

            return data_set(sql);
        }

        public DataSet ds_tbl_cargas(int id)
        {        
                string fields, ntablas, nwhere;

                fields = "a.nombre,a.descripcion,kva_text,x,y,grados,escala,b.nombre AS bloque";
                ntablas = "esq_red.tbl_cargas a,esq_red.lta_bloques b";
                nwhere = " a.id_bloque=b.id AND a.id=" + id;

                string sql = "SELECT " + fields + " FROM " + ntablas + " WHERE " + nwhere;

                return data_set(sql);
        }

        public DataSet ds_recursive_cto(int id_cto)
        {
            string sql = "WITH RECURSIVE v_treeview_cto (id, nombre, root,id_icono) AS ";
            sql += "(";
            sql += "SELECT id,nombre,root,id_icono FROM esq_red.v_treeview2 WHERE id =" + id_cto + " ";
            sql += "UNION ALL ";
            sql += "SELECT p.id,p.nombre,p.root,p.id_icono FROM esq_red.v_treeview2 p ";
            sql += "INNER JOIN v_treeview_cto ON p.root = v_treeview_cto.id ";
            sql += ") ";
            sql += "SELECT * FROM v_treeview_cto";

            return data_set(sql);
        }

        public DataSet ds_recursive_id_roots(int id)
        {
            string sql = "WITH RECURSIVE v_treeview_id_roots (id, nombre, root,id_icono) AS ";
            sql += "(";
            sql += "SELECT id,nombre,root,id_icono FROM esq_red.v_treeview2 WHERE id =" + id + " ";
            sql += "UNION ALL ";
            sql += "SELECT p.id,p.nombre,p.root,p.id_icono FROM esq_red.v_treeview2 p ";
            sql += "INNER JOIN v_treeview_id_roots ON p.id = v_treeview_id_roots.root ";
            sql += ") ";
            sql += "SELECT * FROM v_treeview_id_roots";

            return data_set(sql);
        }

        public bool update_root(int id, int root, Int16 id_icono)
        {
            string ntabla = NombreTabla(id_icono);

            if (ntabla.Equals("-1"))
            {
                return false;
            }

            string sql = "UPDATE " + ntabla + " set ";
            sql += "root=@root ";

            sql += "WHERE id=@id";

            using (var cmd = new NpgsqlCommand(sql, traerConexion()))
            {
                cmd.Parameters.Add("@id", NpgsqlDbType.Integer).Value = id;
                cmd.Parameters.Add("@root", NpgsqlDbType.Integer).Value = root;

                openConnection();

                int res = cmd.ExecuteNonQuery();
                terminaPool();

                if (res == 0)                
                    return false;                
                else                
                    return true;                
            }
        }
        public int update_tbl_cargas(int id, int root, string usuario)
        {
            string sql = "UPDATE esq_red.tbl_cargas set ";
            sql += "root=@root,f_act=now() ";
            sql += "WHERE id=@id";

            using (var cmd = new NpgsqlCommand(sql, traerConexion()))
            {
                cmd.Parameters.Add("@id", NpgsqlDbType.Integer).Value = id;
                cmd.Parameters.Add("@root", NpgsqlDbType.Integer).Value = root;               
                cmd.Parameters.Add("@usuario", NpgsqlDbType.Varchar, 15).Value = usuario;

                openConnection();

                int res = cmd.ExecuteNonQuery();
                terminaPool();

                if (res == 0)
                {
                    return 0;
                }
                else
                {
                    return id;
                }
            }
        }
        public int update_tbl_cargas(int id, string name, string descr, string kva_text, double kva_inst, Int16 tip_conex, double x, double y, Int16 grados,double escala, Int16 id_bloque, string usuario)
        {
            string sql = "UPDATE esq_red.tbl_cargas set ";
            //sql += "nombre=@nombre,descripcion=@descripcion,x=@x,y=@y,grados=@grados,escala=@escala,id_bloque=@id_bloque,usuario=@usuario,f_act=now() ";
            sql += "nombre=@nombre,descripcion=@descripcion,kva_text=@kva_text,kva_inst=@kva_inst,tip_conex=@tip_conex,x=@x,y=@y,grados=@grados,escala=@escala,id_bloque=@id_bloque,usuario=@usuario,f_act=now() ";
            sql += "WHERE id=@id";

            using (var cmd = new NpgsqlCommand(sql, traerConexion()))
            {
                if (descr.Length > 40)                
                    descr = descr.Substring(0, 40);                

                cmd.Parameters.Add("@id", NpgsqlDbType.Integer).Value = id;
                cmd.Parameters.Add("@nombre", NpgsqlDbType.Varchar, 10).Value = name;
                cmd.Parameters.Add("@descripcion", NpgsqlDbType.Varchar, 50).Value = descr;
                cmd.Parameters.Add("@kva_text", NpgsqlDbType.Varchar, 20).Value = kva_text;
                cmd.Parameters.Add("@kva_inst", NpgsqlDbType.Double).Value = kva_inst;
                cmd.Parameters.Add("@tip_conex", NpgsqlDbType.Smallint).Value = tip_conex;
                cmd.Parameters.Add("@x", NpgsqlDbType.Double).Value = x;
                cmd.Parameters.Add("@y", NpgsqlDbType.Double).Value = y;
                cmd.Parameters.Add("@grados", NpgsqlDbType.Smallint).Value = grados;
                cmd.Parameters.Add("@escala", NpgsqlDbType.Double).Value = escala;
                cmd.Parameters.Add("@id_bloque", NpgsqlDbType.Smallint).Value = id_bloque;              
                cmd.Parameters.Add("@usuario", NpgsqlDbType.Varchar, 15).Value = usuario;

                openConnection();

                int res = cmd.ExecuteNonQuery();
                terminaPool();

                if (res == 0)                
                    return 0;               
                else                
                    return id;                
            }

        }

        public int update_tbl_tramo(int id, int root, string usuario)
        {
            string sql = "UPDATE esq_red.tbl_tramos set ";
            sql += "root=@root,usuario=@usuario,f_act=now() ";

            sql += "WHERE id=@id";

            using (var cmd = new NpgsqlCommand(sql, traerConexion()))
            {
                cmd.Parameters.Add("@id", NpgsqlDbType.Integer).Value = id;
                cmd.Parameters.Add("@root", NpgsqlDbType.Integer).Value = root;
                cmd.Parameters.Add("@usuario", NpgsqlDbType.Varchar, 15).Value = usuario;

                openConnection();

                int res = cmd.ExecuteNonQuery();
                terminaPool();

                if (res == 0)
                    return 0;                
                else               
                    return id;
                
            }

        }

        public int update_tbl_tramo(int id, string name, double x, double y, Int16 grados, double escala, Int16 id_bloque, string usuario)
        {
            string sql = "UPDATE esq_red.tbl_tramos set ";
            sql += "nombre=@nombre,x=@x,y=@y,grados=@grados,escala=@escala,id_bloque=@id_bloque,usuario=@usuario,f_act=now() ";

            sql += "WHERE id=@id";

            using (var cmd = new NpgsqlCommand(sql, traerConexion()))
            {
                cmd.Parameters.Add("@id", NpgsqlDbType.Integer).Value = id;
                cmd.Parameters.Add("@nombre", NpgsqlDbType.Varchar, 10).Value = name;
                cmd.Parameters.Add("@x", NpgsqlDbType.Double).Value = x;
                cmd.Parameters.Add("@y", NpgsqlDbType.Double).Value = y;
                cmd.Parameters.Add("@grados", NpgsqlDbType.Smallint).Value = grados;
                cmd.Parameters.Add("@escala", NpgsqlDbType.Double).Value = escala;
                cmd.Parameters.Add("@id_bloque", NpgsqlDbType.Smallint).Value = id_bloque;
                cmd.Parameters.Add("@usuario", NpgsqlDbType.Varchar, 15).Value = usuario;
                
                openConnection();

                int res = cmd.ExecuteNonQuery();
                terminaPool();

                if (res == 0)               
                    return 0;               
                else                
                    return id;
                
            }

        }

        public bool update_tbl_tramos_clientes()
        {
            string sql = "with t as (select poste,COUNT(id) AS Total FROM esq_red.tbl_postes v GROUP BY poste) ";
            sql += "UPDATE esq_red.tbl_cargas SET num_clientes=t.Total ";
            sql += "FROM t WHERE t.poste=nombre ";

            using (var cmd = new NpgsqlCommand(sql, traerConexion()))
            {
                openConnection();

                int res = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                terminaPool();

                if (res == 0)                
                    return false;                
                else               
                    return true;                
            }
        }

        public int insert_tbl_carga(string name, string descr, string kva_text, double kva_inst, Int16 tip_conex, double x, double y, Int16 grados,
          double escala, Int16 id_bloque, int root, string usuario)
        {
            try
            {
                //string sql = "SELECT last_value FROM esq_red.tbl_cargas_id_seq;";
                string sql = "SELECT nextval('esq_red.tbl_cargas_id_seq');";
                int count;
                using (var command = new NpgsqlCommand(sql, traerConexion()))
                {
                    openConnection();
                    count = Convert.ToInt32(command.ExecuteScalar());
                }

                string esq = "esq_red";
                string tabla = "tbl_cargas";

                sql = "INSERT INTO " + esq + "." + tabla +
                    "(id,nombre,descripcion,kva_text,kva_inst,tip_conex,x,y,grados,escala,id_bloque,root,usuario) " +
            "VALUES(@id,@nombre,@descripcion,@kva_text,@kva_inst,@tip_conex,@x,@y,@grados,@escala,@id_bloque,@root,@usuario);";


                using (var cmd = new NpgsqlCommand(sql, traerConexion()))
                {
                    //if (descr.Length > 30)
                    //{
                    //    descr = descr.Substring(0, 30);
                    //}

                    //cmd.Parameters.Add("@id", NpgsqlDbType.Integer).Value = count;
                    //cmd.Parameters.AddWithValue("@nombre", name);
                    //cmd.Parameters.AddWithValue("@descripcion", descr);
                    //cmd.Parameters.AddWithValue("@kva_text", kva_text);
                    //cmd.Parameters.AddWithValue("@kva_inst", kva_inst);
                    //cmd.Parameters.AddWithValue("@tip_conex", tip_conex);
                    //cmd.Parameters.AddWithValue("@x", x);
                    //cmd.Parameters.AddWithValue("@y", y);
                    //cmd.Parameters.AddWithValue("@grados", grados);
                    //cmd.Parameters.AddWithValue("@escala", escala);
                    //cmd.Parameters.AddWithValue("@id_bloque", id_bloque);
                    //cmd.Parameters.AddWithValue("@root", root);
                    //cmd.Parameters.AddWithValue("@usuario", usuario);

                    cmd.Parameters.Add("@id", NpgsqlDbType.Integer).Value = count;
                    cmd.Parameters.Add("@nombre", NpgsqlDbType.Varchar, 10).Value = name;
                    cmd.Parameters.Add("@descripcion", NpgsqlDbType.Varchar, 50).Value = descr;
                    cmd.Parameters.Add("@kva_text", NpgsqlDbType.Varchar, 15).Value = kva_text;
                    cmd.Parameters.Add("@kva_inst", NpgsqlDbType.Double).Value = kva_inst;
                    cmd.Parameters.Add("@tip_conex", NpgsqlDbType.Smallint).Value = tip_conex;
                    cmd.Parameters.Add("@x", NpgsqlDbType.Double).Value = x;
                    cmd.Parameters.Add("@y", NpgsqlDbType.Double).Value = y;
                    cmd.Parameters.Add("@grados", NpgsqlDbType.Smallint).Value = grados;
                    cmd.Parameters.Add("@escala", NpgsqlDbType.Double).Value = escala;
                    cmd.Parameters.Add("@id_bloque", NpgsqlDbType.Smallint).Value = id_bloque;
                    cmd.Parameters.Add("@root", NpgsqlDbType.Integer).Value = root;
                    cmd.Parameters.Add("@usuario", NpgsqlDbType.Varchar, 15).Value = usuario;

                    ////cmd.Parameters.Add("@id", NpgsqlDbType.Integer).Value = count;
                    //cmd.Parameters.Add("@nombre", NpgsqlDbType.Varchar, 10).Value = name;
                    //cmd.Parameters.Add("@descripcion", NpgsqlDbType.Varchar, 50).Value = descr;
                    //cmd.Parameters.Add("@kva_text", NpgsqlDbType.Varchar, 15).Value = kva_text;
                    //cmd.Parameters.Add("@kva_inst", NpgsqlDbType.Double).Value = kva_inst;
                    ////cmd.Parameters.Add("@num_clientes", NpgsqlDbType.Smallint).Value = 0;
                    //cmd.Parameters.Add("@tip_conex", NpgsqlDbType.Smallint).Value = tip_conex;
                    //cmd.Parameters.Add("@x", NpgsqlDbType.Double).Value = Math.Round(x, 4);
                    //cmd.Parameters.Add("@y", NpgsqlDbType.Double).Value = Math.Round(y, 4);
                    //cmd.Parameters.Add("@grados", NpgsqlDbType.Smallint).Value = grados;
                    //cmd.Parameters.Add("@escala", NpgsqlDbType.Double).Value = escala;
                    //cmd.Parameters.Add("@id_bloque", NpgsqlDbType.Smallint).Value = id_bloque;
                    //cmd.Parameters.Add("@root", NpgsqlDbType.Integer).Value = root;
                    //cmd.Parameters.Add("@usuario", NpgsqlDbType.Varchar, 15).Value = usuario;

                    //cmd.CommandType = System.Data.CommandType.Text;
                    //openConnection();
                    //openConnection();

                    //int res = Int32.Parse(cmd.ExecuteScalar().ToString());
                    int res = cmd.ExecuteNonQuery();
                    //terminaPool();

                    if (res == 0)                 
                         return 0;
                        
                    else                   
                        return count;
                    
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                terminaPool();
                //desconeccion
                //Desconectar();
            }
            //return 0;
        }

        public int insert_tbl_tramo(string name, double x, double y, Int16 grados, double escala, Int16 id_bloque, int root, string usuario)
        {
            try
            {               
                string sql = "SELECT nextval('esq_red.tbl_tramos_id_seq');";
                int count;
                using (var command = new NpgsqlCommand(sql, traerConexion()))
                {
                    openConnection();
                    count = Convert.ToInt32(command.ExecuteScalar());                    
                }

                string esq = "esq_red";
                string tabla = "tbl_tramos";

                sql = "INSERT INTO " + esq + "." + tabla +
                    "(id,nombre,x,y,grados,escala,id_bloque,root,usuario) " +
                        "VALUES(@id,@nombre,@x,@y,@grados,@escala,@id_bloque,@root,@usuario);";

                using (var cmd = new NpgsqlCommand(sql, traerConexion()))
                {
                    cmd.Parameters.Add("@id", NpgsqlDbType.Integer).Value = count;
                    cmd.Parameters.Add("@nombre", NpgsqlDbType.Varchar, 10).Value = name;
                    cmd.Parameters.Add("@x", NpgsqlDbType.Double).Value = x;
                    cmd.Parameters.Add("@y", NpgsqlDbType.Double).Value = y;
                    cmd.Parameters.Add("@grados", NpgsqlDbType.Smallint).Value = grados;
                    cmd.Parameters.Add("@escala", NpgsqlDbType.Double).Value = escala;
                    cmd.Parameters.Add("@id_bloque", NpgsqlDbType.Smallint).Value = id_bloque;
                    cmd.Parameters.Add("@root", NpgsqlDbType.Smallint).Value = root;
                    cmd.Parameters.Add("@usuario", NpgsqlDbType.Varchar, 15).Value = usuario;

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

        public double[] select_XY(int id, Int16 id_icono)
        {
            try
            {
                double[] SelXY = new double[2];

                string ntabla = NombreTabla(id_icono);

                if (ntabla.Equals("-1"))
                {
                    SelXY[0] = 0; SelXY[1] = 0;

                    return SelXY;
                }

                string sql = "SELECT x,y FROM " + ntabla + " WHERE id=" + id;

                using (var cmd = new NpgsqlCommand(sql, traerConexion()))
                {

                    openConnection();

                    NpgsqlDataReader dr = cmd.ExecuteReader();

                    if (dr.HasRows)
                    {
                        dr.Read();
                        SelXY[0] = (double)dr["x"];
                        SelXY[1] = (double)dr["y"];

                        return SelXY;
                    }
                    else
                    {
                        SelXY[0] = 0; SelXY[1] = 0;

                        return SelXY;
                    }
                }
            }
            catch (Exception)
            {  //Error
                //atraparError(e, cSql);
                throw;
            }
            finally
            {
                terminaPool();
                //desconeccion
                //Desconectar();
            }
        }

        public string select_nombre(int id, Int16 id_icono)
        {
            try
            {
               
                string ntabla = NombreTabla(id_icono);

                if (ntabla.Equals("-1")) 
                    return null;
              

                string sql = "SELECT nombre FROM " + ntabla + " WHERE id=" + id;

                using (var cmd = new NpgsqlCommand(sql, traerConexion()))
                {

                    openConnection();

                    NpgsqlDataReader dr = cmd.ExecuteReader();

                    if (dr.HasRows)
                    {
                        dr.Read();                       

                        return (string)dr["nombre"];
                    }
                    else 
                        return null;                    
                }
            }
            catch (Exception)
            {  //Error
                //atraparError(e, cSql);
                throw;
            }
            finally
            {
                terminaPool();
                //desconeccion
                //Desconectar();
            }
        }

        public double kVAinst(string texto)
        {
            //char s = Convert.ToChar(System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator);
            char[] delimiterChars = { ' ' };//{ ' ', ',', '.', ':', '\t' };
            char[] delimiterChars2 = { '+' };
            string[] elementos;
            string[] elementos2;
            string Banco = "";
            double kva_inst = 0;
            Int16 Transf = 0, nTransf = 0;//Puntos = 0,
            //NumberStyles styles= NumberStyles.Float;
            //creating an object of NumberFormatInfo
            NumberFormatInfo provider = new NumberFormatInfo();
            provider.NumberDecimalSeparator = ".";
            //provider.NumberGroupSeparator = ",";


            //texto = texto.Replace(".", S_DEC_SEP);
            elementos = texto.Split(delimiterChars);
            if (!elementos.Length.Equals(1))
            {
                Banco = elementos[1];
                elementos2 = Banco.Split(delimiterChars2);
            }
            else
            {
                elementos2 = texto.Split(delimiterChars2);
            }

            Banco = elementos2[0];
            Transf = Int16.Parse(Banco.Substring(0, 1));



            if (Banco.Length > 2 && !Banco.Substring(2, 1).Equals("F"))
            {
                kva_inst += double.Parse(Banco.Substring(2, Banco.Length - 2), provider) * Transf;
            }
            nTransf += Transf;

            if (elementos2.Length > 1)
            {
                Banco = elementos2[1];
                Transf = Int16.Parse(Banco.Substring(0, 1));
                if (Banco.Length > 2 && !Banco.Substring(2, 1).Equals("F"))
                {
                    kva_inst += double.Parse(Banco.Substring(2, Banco.Length - 2), provider) * Transf;
                }
                nTransf += Transf;
            }

            if (elementos2.Length > 2)
            {
                Banco = elementos2[2];
                Transf = Int16.Parse(Banco.Substring(0, 1));

                if (Banco.Length > 2 && !Banco.Substring(2, 1).Equals("F"))
                {
                    kva_inst += double.Parse(Banco.Substring(2, Banco.Length - 2), provider) * Transf;
                }

                nTransf += Transf;
            }

            return kva_inst;
        }

        public Int16 nTransf(string texto)
        {
            char[] delimiterChars = { ' ' };//{ ' ', ',', '.', ':', '\t' };
            char[] delimiterChars2 = { '+' };
            string[] elementos;
            string[] elementos2;
            string Banco = "";
            //double kva_inst = 0;
            Int16 Transf = 0, nTransf = 0;//Puntos = 0,

            elementos = texto.Split(delimiterChars);
            if (!elementos.Length.Equals(1))
            {
                Banco = elementos[1];
                elementos2 = Banco.Split(delimiterChars2);
            }
            else
            {
                elementos2 = texto.Split(delimiterChars2);
            }

            Banco = elementos2[0];
            Transf = Int16.Parse(Banco.Substring(0, 1));



            //if (Banco.Length > 2 && !Banco.Substring(2, 1).Equals("F"))
            //{
            //    kva_inst += double.Parse(Banco.Substring(2, Banco.Length - 2)) * Transf;
            //}
            nTransf += Transf;

            if (elementos2.Length > 1)
            {
                Banco = elementos2[1];
                Transf = Int16.Parse(Banco.Substring(0, 1));
                //if (Banco.Length > 2 && !Banco.Substring(2, 1).Equals("F"))
                //{
                //    kva_inst += double.Parse(Banco.Substring(2, Banco.Length - 2)) * Transf;
                //}
                nTransf += Transf;
            }

            if (elementos2.Length > 2)
            {
                Banco = elementos2[2];
                Transf = Int16.Parse(Banco.Substring(0, 1));

                //if (Banco.Length > 2 && !Banco.Substring(2, 1).Equals("F"))
                //{
                //    kva_inst += double.Parse(Banco.Substring(2, Banco.Length - 2)) * Transf;
                //}

                nTransf += Transf;
            }

            return nTransf;
        }

        public  Int16 Obt_Id_Icono(String bloque)
        {
            Int16 id_icono = 0;

            switch (bloque)
            {
                case "ECADSZ01":
                    id_icono = 14;
                    break;
                case "ECADSZ02":
                    id_icono = 18;
                    break;
                case "ECADRR01":
                    id_icono = 15;
                    break;
                case "ECADSE01":
                    id_icono = 16;
                    break;
                case "ECADSE02":
                    id_icono = 17;
                    break;
                case "ECADSE03":
                    id_icono = 16;
                    break;
                case "ECADSE04":
                    id_icono = 17;
                    break;
                case "ECADSE05":
                    id_icono = 16;
                    break;
                case "ECADSE06":
                    id_icono = 17;
                    break;
                case "ECADSE07":
                    id_icono = 16;
                    break;
                case "ECADSE08":
                    id_icono = 17;
                    break;
                case "ECADSE09":
                    id_icono = 20;
                    break;
                case "ECADSE10":
                    id_icono = 21;
                    break;
                case "ECADSE11":
                    id_icono = 20;
                    break;
                case "ECADSE12":
                    id_icono = 21;
                    break;
                case "ECADSE13":
                    id_icono = 20;
                    break;
                case "ECADSE14":
                    id_icono = 21;
                    break;
                case "ECADSE15":
                    id_icono = 20;
                    break;
                case "ECADSE16":
                    id_icono = 21;
                    break;
                case "ECADSE20":
                    id_icono = 0;
                    break;
                case "ECADSE21":
                    id_icono = 0;
                    break;
                case "ECADSE22":
                    id_icono = 0;
                    break;
                case "ECADSE23":
                    id_icono = 0;
                    break;
                case "ECADSE24":
                    id_icono = 0;
                    break;
                case "ECADSE25":
                    id_icono = 0;
                    break;
                case "ECADPOTE":
                    id_icono = 0;
                    break;
                case "ECADESTE":
                    id_icono = 0;
                    break;
                case "ECADTAAE":
                    id_icono = 0;
                    break;
                case "ECADTABE":
                    id_icono = 0;
                    break;
                case "ECADTR01":
                    id_icono = 22;
                    break;
                case "ECADTR02":
                    id_icono = 26;
                    break;
                case "ECADTR03":
                    id_icono = 27;
                    break;
                case "ECADTR04":
                    id_icono = 23;
                    break;
                case "ECADCT01":
                    id_icono = 24;
                    break;
                case "ECADST01":
                    id_icono = 25;
                    break;
                case "ECADRR02":
                    id_icono = 19;
                    break;
                default:
                    id_icono = 0;
                    //MessageBox.Show("El bloque no existe");
                    break;
            }

            return id_icono;
        }

        public Int16 Obt_Id_Bloque(String bloque)
        {
            Int16 id_bloque = 0;
            //string sql = "SELECT id FROM esq_red.lta_bloques WHERE nombre='" + bloque+"'";
            //using (var cmd = new NpgsqlCommand(sql, traerConexion()))
            //{

            //    openConnection();

            //    NpgsqlDataReader dr = cmd.ExecuteReader();

            //    if (dr.HasRows)
            //    {
            //        dr.Read();
            //        id_bloque = Int16.Parse(dr[0].ToString());
            //        terminaPool();
            //        return id_bloque;
            //    }
            //    else
            //    {
            //        terminaPool();
            //        return id_bloque;
            //    }
            //}
            switch (bloque)
            {
                case "ECADSZ01":
                    id_bloque = 1;
                    break;
                case "ECADSZ02":
                    id_bloque = 2;
                    break;
                case "ECADRR01":
                    id_bloque = 3;
                    break;
                case "ECADSE01":
                    id_bloque = 4;
                    break;
                case "ECADSE02":
                    id_bloque = 5;
                    break;
                case "ECADSE03":
                    id_bloque = 6;
                    break;
                case "ECADSE04":
                    id_bloque = 7;
                    break;
                case "ECADSE05":
                    id_bloque = 8;
                    break;
                case "ECADSE06":
                    id_bloque = 9;
                    break;
                case "ECADSE07":
                    id_bloque = 10;
                    break;
                case "ECADSE08":
                    id_bloque = 11;
                    break;
                case "ECADSE09":
                    id_bloque = 12;
                    break;
                case "ECADSE10":
                    id_bloque = 13;
                    break;
                case "ECADSE11":
                    id_bloque = 14;
                    break;
                case "ECADSE12":
                    id_bloque = 15;
                    break;
                case "ECADSE13":
                    id_bloque = 16;
                    break;
                case "ECADSE14":
                    id_bloque = 17;
                    break;
                case "ECADSE15":
                    id_bloque = 18;
                    break;
                case "ECADSE16":
                    id_bloque = 19;
                    break;
                case "ECADSE20":
                    id_bloque = 20;
                    break;
                case "ECADSE21":
                    id_bloque = 21;
                    break;
                case "ECADSE22":
                    id_bloque = 22;
                    break;
                case "ECADSE23":
                    id_bloque = 23;
                    break;
                case "ECADSE24":
                    id_bloque = 24;
                    break;
                case "ECADSE25":
                    id_bloque = 25;
                    break;
                case "ECADPOTE":
                    id_bloque = 26;
                    break;
                case "ECADESTE":
                    id_bloque = 27;
                    break;
                case "ECADTAAE":
                    id_bloque = 28;
                    break;
                case "ECADTABE":
                    id_bloque = 29;
                    break;
                case "ECADTR01":
                    id_bloque = 30;
                    break;
                case "ECADTR02":
                    id_bloque = 31;
                    break;
                case "ECADTR03":
                    id_bloque = 32;
                    break;
                case "ECADTR04":
                    id_bloque = 33;
                    break;
                case "ECADCT01":
                    id_bloque = 34;
                    break;
                case "ECADST01":
                    id_bloque = 35;
                    break;
                case "ECADRR02":
                    id_bloque = 36;
                    break;
                default:
                    id_bloque = 0;
                    //MessageBox.Show("El bloque no existe");
                    break;
            }

            return id_bloque;

        }

    }
}
