using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Net.NetworkInformation;
//using System.Windows.Forms;
using Npgsql;
using System.Configuration;
using System.Data;
//using System.Net;
//using System.Net.Sockets;

namespace DataAccess.PostgreSQL
{
    public abstract class ConnectionToPool
    //public class ConnectionToPool
    {
        //private readonly string connectionString;

        internal static string VERSION = "v21.07.21";
        private static string SERVERHOSTNAME = "127.0.0.1";
        //private static string SERVERHOSTNAME = "10.20.0.96";
        private static Int16 PUERTO = 5432;
        private static string LOGIN = "postgres";
        private static string PASSWORD = "Psql2012";
        private static string BD = "bd_visoproy";
        private NpgsqlConnection con = null;
        //private static int PROTOCOLO = 3;
        //private static bool POOLING = false;
        //private static int MINPOOLSIZE = 5;
        //private static int MAXPOOLSIZE = 200;
        private static int CONNECTIONLIFETIME = 90; //30,300,3 * 5000;

        //private string conn_string = String.Format("Server={0};Port={1};User Id={2};Password={3};Database={4};Protocol={5};Pooling={6};MinPoolSize={7};MaxPoolSize={8};ConnectionLifeTime={9};",
        //    SERVERHOSTNAME, PUERTO, LOGIN, PASSWORD, BD, PROTOCOLO, POOLING, MINPOOLSIZE, MAXPOOLSIZE, CONNECTIONLIFETIME);

        private string conn_string = String.Format("Server={0};Port={1};User Id={2};Password={3};Database={4};commandtimeout={5};",
            SERVERHOSTNAME, PUERTO, LOGIN, PASSWORD, BD, CONNECTIONLIFETIME);

        private void iniciaPool()
        {
            con = new NpgsqlConnection(@conn_string);
            //MessageBox.Show("Inicio Pool de Conexiones: ", "Estatus de Conexión", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //Console.WriteLine("Inicio Pool de Conexiones");
        }

        public NpgsqlConnection traerConexion()
        {
            try
            { //Confirmacion
                if (con == null)
                {
                    iniciaPool();
                }
                //else
                //{
                //    con.Open();
                //}
            }
            catch (Exception)
            { //Error 
                throw;
            }
            return con;
        }

        protected void openConnection()
        {
            try
            { //Confirmacion
                if (con != null)
                {
                    con.Open();
                    //Console.WriteLine("Abre conexión");
                }
            }
            catch (Exception)
            { //Error 
                throw;
            }
        }

        protected void terminaPool()
        {
            try
            { //Estado
                //if (con != null)
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                    //MessageBox.Show("Cierre Pool de Conexiones", "Estatus de Conexión", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    //Console.WriteLine("Cierre Pool de Conexiones");
                }
            }
            catch (Exception)
            { //Error 
                throw;
            }
        }

        //public bool comprobarIP(string ip, int timeoutSegs)
        public static bool comprobarIP()
        {
            string ip = SERVERHOSTNAME;
            int timeoutSegs = 10; // CONNECTIONLIFETIME;

            // Creamos el objeto base y lo configuramos
            Ping ping = new Ping();

            PingOptions opciones = new PingOptions();
            opciones.DontFragment = true;

            // Añadimos 32 bytes de datos
            string datos = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            // Añadimos 64 bytes de datos
            //string datos = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            byte[] buffer = Encoding.ASCII.GetBytes(datos);

            try
            {
                // Lanzamos el ping y esperamos la respuesta
                PingReply respuesta = ping.Send(ip, timeoutSegs * 1000, buffer, opciones);

                if (respuesta.Status == IPStatus.Success) return true;
                else return false;
            }
            catch (Exception)
            {
                // Algo ha pasado...
                //MessageBox.Show("Error: " + ex.Message);
                return false;
            }
        }

        //MOSTRAMOS EL ERROR Y ATRAPAMOS LA CONSULTA
        private static void atraparError(Exception e, String cSql)
        {
            Console.WriteLine("Error: " + cSql);
            //System.Windows.Forms.Clipboard.SetText(cSql);
        }

        //EJECUTAR LA CONSULTA
        public Boolean run_script(String sql)
        {
            Boolean bestado = true;
            try
            {  //Consulta
                NpgsqlCommand coman = new NpgsqlCommand(sql, con);
                coman.ExecuteScalar();
            }
            catch (Exception e)
            {  //Error
                atraparError(e, sql);
                throw;
            }
            finally
            {
                terminaPool();
                //desconeccion
                //Desconectar();
            }
            return bestado;
        }

        public DataTable data_tabla(String sql)
        {
            DataTable dt = new DataTable();
            try
            {  //Consulta
                //openConnection();
                using (var cmd = new NpgsqlCommand(sql, traerConexion()))
                {
                    //NpgsqlCommand cmd = new NpgsqlCommand(sql, traerConexion());
                    //coman.CommandType = CommandType.StoredProcedure;
                    //cmd.CommandType = CommandType.Text;
                    NpgsqlDataAdapter resp = new NpgsqlDataAdapter(cmd);
                    resp.Fill(dt);
                }
            }
            catch (Exception e)
            {
                atraparError(e, sql);
                throw;
            }
            finally
            {
                terminaPool();
                //desconeccion
                //Desconectar(
            }

            return dt;
        }

        public DataSet data_set(String sql)
        {
            DataSet ds = new DataSet();
            try
            {
                using (var cmd = new NpgsqlCommand(sql, traerConexion()))
                {
                    NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
                    da.Fill(ds);
                }
            }
            catch (Exception e)
            {
                atraparError(e, sql);
                throw;
            }
            finally
            {
                terminaPool();
            }
            return ds;
        }

        //METODO REALIZARA LA CONSULTA TRALLENDO DATOS
        public DataSet data_set2(String sql)
        {
            //alias
            return data_set(sql);
        }

        // Valida si existe un registro y devuelve el primer campo (id) o 0
        public int ValidarName(string sql)
        {
            try
            {
                using (var cmd = new NpgsqlCommand(sql, traerConexion()))
                {
                    openConnection();
                    using (var dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            dr.Read();
                            terminaPool();
                            return Convert.ToInt32(dr[0]);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                atraparError(e, sql);
                throw;
            }
            finally
            {
                try { terminaPool(); } catch { }
            }
            return 0;
        }

        //METODOS AUXILIARES
        public string NombreTabla(Int16 id_icono)
        {
            if (id_icono > -1 && id_icono < 6)
            { // Actualizar tabla tbl_sub
                return "esq_red.tbl_sub";
            }
            else if (id_icono > 5 && id_icono < 10)
            { // Actualizar tabla tbl_transf
                return "esq_red.tbl_transf";
            }
            else if (id_icono > 9 && id_icono < 14)
            { // Actualizar tabla tbl_ctos
                return "esq_red.tbl_ctos";
            }
            else if (id_icono > 13 && id_icono < 22)
            { // Actualizar tabla tbl_tramos
                return "esq_red.tbl_tramos";
            }
            else if (id_icono > 21 && id_icono < 28)
            { // Actualizar tabla tbl_cargas
                return "esq_red.tbl_cargas";

            }
            else
            {
                return "-1";
            }

        }

    }
}
