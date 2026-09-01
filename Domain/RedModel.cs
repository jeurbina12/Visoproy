using System;
using System.Collections.Generic;
using System.Text;
using DataAccess.PostgreSQL;
using System.Data;

namespace Domain
{
    public class RedModel
    {
        RedDao redDao = new RedDao();

         
        public RedModel()
        {
        }

        public DataTable dt_lista(string sql)
        {
            return redDao.dt_lista(sql);
        }
        public bool update_tbl(string s_tabla, int id, int root, string nombre, string usuario)
        {
            return redDao.update_tbl(s_tabla,id, root, nombre, usuario);
        }
    }
}
