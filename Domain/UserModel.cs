using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
//using DataAccess;
using Common.Cache;
using Visoproy.DataAccess.PostgreSQL;
using System.Data;

namespace Domain
{
    public class UserModel
    {
        UserDao userDao = new UserDao();
        

        //Atributes
        private int ID_LOGIN;
        private string LOGIN;
        //private string Password;
        private string NAME;
        private int CI;
        private string CARGO;
        private string TELEF;
        private string MAIL;
        private short ID_ESTADO;
        private short ID_GRUPO;
        // ID_PERFIL removed (unused) to clean warnings
        private short ID_DPTO;
        private string TXT_OBS;

        // Removed unused flags to clean compilation warnings

        //Constructors
        public UserModel(int idLogin, string login, string nombre,int ci,string cargo,string telef,
            string mail, Int16 id_estado,Int16 id_grupo,Int16 id_dpto,string txt_obs)
        //,bool bloq_pc,bool bloq_perfil,Int16 id_perfil,bool b1,bool b2,bool b3,bool b4,bool b5,bool b6,bool b7,bool b8,bool b9,bool b10,bool b11,
            //bool b12,bool b13,bool b14,bool b15,bool b16,bool b17,bool b18)
        {
            this.ID_LOGIN = idLogin;
            this.LOGIN = login;
            //this.Password = Pass;
            this.NAME = nombre;
            this.CI = ci;
            this.CARGO = cargo;
            this.TELEF = telef;
            this.MAIL = mail;
            this.ID_ESTADO = id_estado;
            this.ID_GRUPO = id_grupo;
            //this.ID_PERFIL = id_perfil;
            this.ID_DPTO = id_dpto;
            this.TXT_OBS = txt_obs;
            //this.BLO_PC = bloq_pc;
            //this.BLO_PERFIL = bloq_perfil;
        }
        public UserModel()
        {
        }
        //public UserModel(string correo)
        //{
        //    this.MAIL = correo;
        //}

        //Methods
        public bool comprobarIP()
        {
            return UserDao.comprobarIP();

        }

        public string loginUpdate()
        {
            try
            {
                userDao.LoginUpdate(ID_LOGIN, NAME, CI, CARGO, TELEF, MAIL,
                    ID_DPTO, ID_ESTADO, ID_GRUPO, TXT_OBS);
                
                //LoginSelect(LOGIN, "Password");
                return "Los datos del perfil de usuario fueron guardados satisfactoriamente";
            }
            catch (Exception ex)
            {
                return "Datos del perfil de usuario No Guardados: " + ex;
            }
        }

        public bool LoginSelect(string user, string pass)
        {
            return userDao.LoginSelect(user, pass);
        }
        public bool LoginUserVal(string user)
        {
            //return false;
          return  userDao.LoginValidar(user);
        }

        public string recoverPassword(string userRequesting)
        {
            return userDao.recoverPassword(userRequesting);
        }

        public bool securityLogin()
        {
            if (UserCache.IdUser >= 1)
            {
                //if (userDao.existsUser(UserCache.IdUser, UserCache.LoginName, UserCache.Password) == true)
                if (userDao.LoginValidar(UserCache.LoginUser) == true)
                  
                return true;
                else
                    return false;
            }
            else
                return false;
        }

        public DataTable dt_lista(string vista, int root, bool ver)
        {
            //LocDao clsLista = new LocDao();
            return userDao.dt_lista(vista, root, ver);
        }
        //public DataSet ds_lista(string vista)
        //{
        //    TreeviewDao cls_Treeview = new TreeviewDao();
        //    return cls_Treeview.ds_lista(vista);
        //    //
        //}



    }
}
