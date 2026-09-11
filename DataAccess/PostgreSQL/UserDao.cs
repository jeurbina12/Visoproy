using System;
using System.Collections.Generic;
using System.Text;
using Npgsql;
using Common.Cache;
using NpgsqlTypes;
using System.Data;
using DataAccess.MailServices;


namespace Visoproy.DataAccess.PostgreSQL
{
    public class UserDao : ConnectionToPool
    {        

        public bool LoginSelect(string user, string pass)
        {
            using (var connection = traerConexion())
            {
                String ntablas, fields, nwhere;

                fields = "a.id AS id_usuario,RTRIM(a.usuario),RTRIM(a.nombre) AS nombre,a.ci,a.telefono,";
                fields += "RTRIM(a.mail),RTRIM(a.cargo),RTRIM(a.observacion) AS obs, a.id_estado,a.id_grupo,a.id_perfil,";
                fields += "a.id_dpto,a.bloq_pc,a.bloq_perfil,";

                fields += "b.b1 AS b1,b.b2 AS b2,b.b3 AS b3,b.b4 AS b4,";
                fields += "b.b5 AS b5,b.b6 AS b6,b.b7 AS b7,b.b8 AS b8,b.b9 AS b9,b.b10 AS b10,b.b11 AS b11,b.b12 AS b12,";
                fields += "b.b13 AS b13,b.b14 AS b14,b.b15 AS b15,b.b16 AS b16,b.b17 AS b17,b.b18 AS b18";

                ntablas = "esq_proy.tbl_personal a,esq_proy.tbl_perfiles b";

                nwhere = " a.id_perfil=b.id AND a.usuario='" + user.ToUpper() + "' ";

                string sql = "SELECT " + fields + " FROM " + ntablas + " WHERE " + nwhere;
                                
                using (var command = new NpgsqlCommand(sql, traerConexion()))
                {
                    openConnection();
                    NpgsqlDataReader dr = command.ExecuteReader();

                    if (dr.HasRows)
                    {
                        while (dr.Read())//Obtenemos los datos de la columna y asignamos a los campos de la Cache de Usuario
                        {
                            UserCache.IdUser = (int)dr.GetInt32(0);
                            UserCache.LoginUser = dr.GetString(1);
                            //UserCache.Password = dr.GetString(2);
                            UserCache.Name = dr.GetString(2);
                            UserCache.ci = dr.GetInt32(3);
                            UserCache.Telef = dr.GetString(4);
                            UserCache.Mail = dr.GetString(5);
                            UserCache.Cargo = dr.GetString(6);
                            UserCache.Obs = dr.GetString(7);

                            UserCache.id_estado = dr.GetInt16(8);
                            UserCache.id_grupo = dr.GetInt16(9);
                            UserCache.id_perfil = dr.GetInt16(10);
                            UserCache.id_dpto = dr.GetInt16(11);

                            UserCache.bloq_pc = dr.GetBoolean(12);
                            UserCache.bloq_perfil = dr.GetBoolean(13);

                            UserCache.b1 = dr.GetBoolean(14);
                            UserCache.b2 = dr.GetBoolean(15);//modifica estructura de la red
                            UserCache.b3 = dr.GetBoolean(16);
                            UserCache.b4 = dr.GetBoolean(17);
                            UserCache.b5 = dr.GetBoolean(18);
                            UserCache.b6 = dr.GetBoolean(19);
                            UserCache.b7 = dr.GetBoolean(20);
                            UserCache.b8 = dr.GetBoolean(21);
                            UserCache.b9 = dr.GetBoolean(22);
                            UserCache.b10 = dr.GetBoolean(23);
                            UserCache.b11 = dr.GetBoolean(24);
                            UserCache.b12 = dr.GetBoolean(25);
                            UserCache.b13 = dr.GetBoolean(26);
                            UserCache.b14 = dr.GetBoolean(27);
                            UserCache.b15 = dr.GetBoolean(28);
                            UserCache.b16 = dr.GetBoolean(29);
                            UserCache.b17 = dr.GetBoolean(30);
                            UserCache.b18 = dr.GetBoolean(31);//modifica coordenadas
                            //clsPermisos.b = dr.GetBoolean(1);
                            //clsPermisos.b = dr.GetBoolean(1);


                        }
                        return true;
                    }
                    else
                        return false;
                }
            }
        }

        public bool LoginValidar(string user)
        {
            try
            {
                using (var connection = traerConexion())
                {
                    String nFuncion = string.Format("esq_proy.f_validar_usuario('{0}');", user);
                    using (var cmd = new NpgsqlCommand(nFuncion, traerConexion()))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        //connection.Open();
                        openConnection();
                        int res = Convert.ToInt16(cmd.ExecuteScalar());
                        if (res == 0)                        
                            return false;                        
                        else
                           return true;                        
                    }
                }
            }
            catch (Exception)
            {//Error 
                return false;
            }
        }

        public bool LoginUpdate(int id, string nombre, int ci, string cargo, string telef, string mail,
            Int16 id_dpto, Int16 id_estado, Int16 id_grupo, string obs)
        {
            string sql = "UPDATE esq_proy.tbl_personal set ";
            sql += "nombre=@nombre,ci=@ci,cargo=@cargo,telefono=@telefono,mail=@mail,observacion=@observacion ";
            if (!UserCache.bloq_perfil)
            {
                sql += ",id_dpto=@id_dpto,id_estado=@id_estado,id_grupo=@id_grupo ";
            }
            sql += "WHERE id=@id";

            using (var cmd = new NpgsqlCommand(sql, traerConexion()))
            {
                cmd.Parameters.Add("@id", NpgsqlDbType.Integer).Value = id;
                cmd.Parameters.Add("@nombre", NpgsqlDbType.Varchar, 40).Value = nombre;
                cmd.Parameters.Add("@ci", NpgsqlDbType.Integer).Value = ci;
                cmd.Parameters.Add("@cargo", NpgsqlDbType.Varchar, 50).Value = cargo;
                cmd.Parameters.Add("@telefono", NpgsqlDbType.Varchar, 50).Value = telef;
                cmd.Parameters.Add("@mail", NpgsqlDbType.Varchar, 50).Value = mail;
                if (!UserCache.bloq_perfil)
                {
                    cmd.Parameters.Add("@id_dpto", NpgsqlDbType.Integer).Value = id_dpto;
                    cmd.Parameters.Add("@id_estado", NpgsqlDbType.Integer).Value = id_estado;
                    cmd.Parameters.Add("@id_grupo", NpgsqlDbType.Integer).Value = id_grupo;
                }
                cmd.Parameters.Add("@observacion", NpgsqlDbType.Varchar, 255).Value = obs;

                openConnection();

                int res = cmd.ExecuteNonQuery();
                
                if (res == 0)
                {
                    return false;
                }
                else
                {
                    UserCache.Name = nombre;
                    UserCache.ci = ci;
                    UserCache.Cargo = cargo;
                    UserCache.Telef = telef;
                    UserCache.Mail = mail;
                    if (!UserCache.bloq_perfil)
                    {
                        UserCache.id_dpto = id_dpto;
                        UserCache.id_estado = id_estado;
                        UserCache.id_grupo = id_grupo;
                    }
                    
                    return true;
                }
            }
        }

        public bool LoginInsert(string usuario, string nombre, Int16 ci, string cargo, string telef, string mail, Int16 id_dpto,
            Int16 id_estado, Int16 id_grupo, string obs)
        {
            string sql = "SELECT MAX(id) FROM esq_proy.tbl_personal";
            Int16 count;
            using (var command = new NpgsqlCommand(sql, traerConexion()))
            {
                openConnection();
                NpgsqlDataReader dr = command.ExecuteReader();
                dr.Read();
                count = System.Convert.ToInt16(dr[0]); count += 1;

                sql = "INSERT INTO esq_proy.tbl_personal(";
                sql += "id,usuario,nombre,ci,cargo,telefono,mail,observacion,id_dpto,id_estado,id_grupo";
                sql += ") ";
                sql += "VALUES(@id,@usuario,@nombre,@ci,@cargo,@telefono,@mail,@observacion,@id_dpto,@id_estado,@id_grupo)";
                 
                //terminaPool();
            }
            using (var cmd = new NpgsqlCommand(sql, traerConexion()))
            {
                cmd.Parameters.Add("@id", NpgsqlDbType.Integer).Value = count;
                cmd.Parameters.Add("@usuario", NpgsqlDbType.Varchar, 15).Value = usuario;
                cmd.Parameters.Add("@nombre", NpgsqlDbType.Varchar, 40).Value = nombre;
                cmd.Parameters.Add("@ci", NpgsqlDbType.Integer).Value = ci;
                cmd.Parameters.Add("@cargo", NpgsqlDbType.Varchar, 50).Value = cargo;
                cmd.Parameters.Add("@telefono", NpgsqlDbType.Varchar, 50).Value = telef;
                cmd.Parameters.Add("@mail", NpgsqlDbType.Varchar, 50).Value = mail;
                cmd.Parameters.Add("@id_dpto", NpgsqlDbType.Integer).Value = id_dpto;
                cmd.Parameters.Add("@id_estado", NpgsqlDbType.Integer).Value = id_estado;
                cmd.Parameters.Add("@id_grupo", NpgsqlDbType.Integer).Value = id_grupo;

                cmd.Parameters.Add("@observacion", NpgsqlDbType.Varchar, 255).Value = obs;
               
                int res = cmd.ExecuteNonQuery();
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

        public string recoverPassword(string userRequesting)
        {
            string sql = "SELECT usuario,mail,clave FROM esq_proy.tbl_personal WHERE usuario=@usuario or mail=@mail";

            using (var connection = traerConexion())
            {
                openConnection();
                using (var cmd = new NpgsqlCommand(sql, traerConexion()))
                {
                    cmd.Parameters.AddWithValue("@usuario", userRequesting);
                    cmd.Parameters.AddWithValue("@mail", userRequesting);
                    cmd.CommandType = System.Data.CommandType.Text;

                    NpgsqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read() == true)
                    {
                        string userName = dr.GetString(0);
                        string userMail = dr.GetString(1);
                        string accountPassword = dr.GetString(2);

                        var mailService = new SystemSupportMail();
                        mailService.sendMail(
            subject: "SYSTEM: Password recovery request",
            body: "Hi, " + userName + "\nYou Requested to Recover your password.\n" +
            "your current password is: " + accountPassword +
            "\nHowever, we ask that you change your password inmediately once you enter the system.",
            recipientMail: new List<string> { userMail }
            );
                        return "Hola, " + userName + "\nSolicitaste recuperar tu contraseña.\n" +
                          "Por favor revisa tu correo: " + userMail +
                          "\nSin embargo, le pedimos que cambie su contraseña inmediatamente al recibirla del sistema.";
                    }
                    else
                        return "Lo Sentimos, no tenemos una cuaenta con este nombre se usuario o correo electrónico";
                }

                
            }

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

    }
}
