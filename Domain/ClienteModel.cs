using System;
using System.Collections.Generic;
using System.Text;
using Visoproy.DataAccess.PostgreSQL;
using System.Data;

namespace Domain
{

    public class ClienteModel
    {
        ClienteDao clienteDao = new ClienteDao();
        ////Atributes
        //public Int16 ID { get; set; }      
        //public string NOMBRE { get; set; }

        public ClienteModel()
        {

        }

        public DataTable dt_lista(string sql)
        {
            return clienteDao.dt_lista(sql);
        }
      
        public DataTable dt_clientes_vec(double _x, double _y, string _mts, bool b_inmuebles)
        {
            int x1 = Convert.ToInt32(_x) + System.Convert.ToInt16(_mts);
            int x2 = Convert.ToInt32(_x) - System.Convert.ToInt16(_mts);
            int y1 = Convert.ToInt32(_y) + System.Convert.ToInt16(_mts);
            int y2 = Convert.ToInt32(_y) - System.Convert.ToInt16(_mts);

            string sql = "k.x <'" + x1 + "' AND k.x >'" + x2 + "' ";
            sql += "AND k.y <'" + y1 + "' AND k.y >'" + y2 + "' ";

            return dt_clientes("MTS_VEC", sql, b_inmuebles);
        }

        public DataTable dt_clientes(string n_col, string variable, bool b_inmuebles)
        {
            switch (n_col)
            {
                case "contrato sap":
                    n_col = "cuenta";
                    break;
                case "instalación sap":
                    n_col = "instalacion";
                    break;
                case "documento":
                    n_col = "doc_id";
                    break;
                case "nombre":
                    n_col = "nom_cli";
                    break;
                default:
                    break;
            }
            string sql = "SELECT ";
            sql += "a.id,a.nif,a.nis,rtrim(a.nom_cli) AS nombre,actividad,a.csmo_fact AS kwh,rtrim(a.obj_conexion) AS obj_conexion,";
            sql += "e.abrev || ' ' || f.localidad AS localidad,";
            sql += "rtrim(g.nombre) AS tip_vía, rtrim(a.nom_via) AS nom_vía ,a.cod_ext,i.t_finca,";
            sql += "to_number(to_char(k.x,'999999'),'999999') AS x,to_number(to_char(k.y,'9999999'),'9999999') AS y,poste,";
            sql += "a.id_localidad AS id_localidad,a.ruta,a.itin,a.serie AS serie,id_poste ";
           
                //sql += ",puerta,inm1,inm2 ";

            if (n_col.Equals("a.id"))
            {
                sql += ",a.nic,a.cuenta,a.instalacion,";
                sql += "rtrim(a.doc_id) AS doc_id,rtrim(a.ref_dir) AS ref_dir,";
                sql += "municipio,parroquia,id_postal,rtrim(a.tfno_cli) AS tfno_cli,";
                sql += "ciau,a.unicom_lect,a.aol_finc,a.tramo,rtrim(a.duplicador) AS duplicador,";
                sql += "a.f_alta_cont,a.f_inst,a.f_p_calibracion,";
                sql += "puerta,inm1,inm2,grados,escala ";
            }else
                if (b_inmuebles)
                    sql += ",puerta,inm1,inm2,grados,escala ";
            
                
            sql += "FROM esq_open.tbl_clientes a ";

            sql += "INNER JOIN (";//combinar cada fila de una tabla con cada fila de la otra tabla
            sql += "SELECT id AS id_tip_cli2, RTRIM(descripcion) AS actividad FROM esq_open.lta_t_clientes) b ";
            sql += "ON b.id_tip_cli2=a.id_tip_cli ";

            sql += "INNER JOIN (SELECT id AS id_municipio2,RTRIM(nombre) AS municipio FROM esq_open.lta_municipios) c ";
            sql += "ON c.id_municipio2=a.id_municipio ";

            sql += "INNER JOIN (SELECT id AS id_parroquia2, RTRIM(nombre) AS parroquia,id_postal FROM esq_open.lta_parroquias) d ";
            sql += "ON d.id_parroquia2=a.id_parroquia ";

            sql += "INNER JOIN (SELECT id AS id_localidad2,id_t_loc, rtrim(nombre) AS localidad FROM esq_open.tbl_zona) f ";
            sql += "ON f.id_localidad2=a.id_localidad ";

            sql += "INNER JOIN (SELECT id AS id_t_loc2, rtrim(abrev) AS abrev FROM esq_open.lta_t_zona) e ";
            sql += "ON e.id_t_loc2=f.id_t_loc ";

            sql += "INNER JOIN (SELECT id AS id_tip_via2, nombre FROM esq_open.lta_vias) g ";
            sql += "ON g.id_tip_via2=a.id_tip_via ";

            sql += "INNER JOIN (SELECT id,rtrim(nombre) AS ciau FROM esq_open.lta_ciau) h ";
            sql += "ON a.id_ciau=h.id ";

            sql += "INNER JOIN (SELECT id AS id_t_finca2, rtrim(nombre) AS t_finca FROM esq_open.lta_t_fincas) i ";
            sql += "ON i.id_t_finca2=a.id_t_finca ";

            if (n_col == "poste")
            {
                sql += "INNER JOIN (";//combinar cada fila de una tabla con cada fila de la otra tabla
            }
            else
            {
                sql += "LEFT OUTER JOIN (";//Si no existe ninguna coincidencia, el lado derecho contendrá null (o vacío).
            }
            sql += "SELECT nis AS nis2,nif AS nif2, poste,id AS id_poste FROM esq_red.tbl_postes) j ";
            sql += "ON j.nis2=a.nis AND j.nif2=a.nif ";

            sql += "LEFT OUTER JOIN (";//Si no existe ninguna coincidencia, el lado derecho contendrá null (o vacío).
            sql += "SELECT nif AS nif3,rtrim(puerta) AS puerta,rtrim(inm1) AS inm1,rtrim(inm2) AS inm2,x,y,grados,escala FROM esq_open.tbl_fincas) k ";
            sql += "ON k.nif3=a.nif ";

            sql += "WHERE ";

            variable = variable.ToUpper();

            switch (n_col)
            {
                case "doc_id":
                    variable = variable.Replace(" ", "").Replace("-", "");//.Replace("V0", "V");
                    sql += "REPLACE(REPLACE(" + n_col + ", ' ', ''), '-', '') ='" + variable + "' ";
                    break;
                case "nom_cli":                    
                    sql += "TRANSLATE(upper(" + n_col + "),'áéíóúÁÉÍÓÚçÇ','aeiouAEIOUcC') like TRANSLATE('%" + variable + "%','áéíóúÁÉÍÓÚçÇ','aeiouAEIOUcC') ";
                    break;
                case "obj_conexion":                    
                    sql += "TRANSLATE(upper(" + n_col + "),'áéíóúÁÉÍÓÚçÇ','aeiouAEIOUcC') like TRANSLATE('%" + variable + "%','áéíóúÁÉÍÓÚçÇ','aeiouAEIOUcC') ";
                    break;
                case "itinerario":                   
                    sql += variable;
                    break;
                case "MTS_VEC":
                    sql +=  variable;  
                    break;
                default:
                    sql += n_col + "='" + variable + "' ";
                    break;
            }

            return clienteDao.data_tabla(sql);
        }

        public bool update_tbl_postes(int id, string codigo, string usuario)
        {
            return clienteDao.update_tbl_postes(id, codigo, usuario);

        }

        public string update_tbl_cargas()
        {
            if (clienteDao.update_tbl_cargas())                
            return "Actualizada con Éxito Cantidad de Clientes por Transformador";
            return "No se actualizarón los Datos";
        }
        public int insert_tbl_postes(string serie, Int32 nis, Int32 nif, string codigo, string usuario)
        {
            return clienteDao.insert_tbl_postes(serie, nis,nif,codigo, usuario);

        }
    }
}
