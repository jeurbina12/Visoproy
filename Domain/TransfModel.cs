using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DataAccess.PostgreSQL;

namespace Domain
{
    public class TransfModel
    {
        TransfDao transfDao = new TransfDao();

        public static short ID { get; set; }
        public static short AMP { get; set; }
        public static short ID_COND { get; set; }
        public static short PVAR { get; set; }

        public static decimal MVA { get; set; }
        public static decimal Z { get; set; }
        public static decimal KV1 { get; set; }
        public static decimal KV2 { get; set; }

        public static string CODIGO { get; set; }
        public static int X { get; set; }
        public static int Y { get; set; }
        public static float ESCALA { get; set; }
        public static short GRADOS { get; set; }

        public static string MARCA { get; set; }
        public static string OBS { get; set; }
        public static string USER { get; set; }

        public static bool B_MOVIL { get; set; }
        public static bool B_VOLTAJE { get; set; }
        public static bool B_PROCT { get; set; }
        public static bool B_SCADA { get; set; }
        public static bool B_VENT { get; set; }
        public static bool B_OPER { get; set; }

        public static DateTime F_MAX { get; set; }

        public DataTable dt_lista(string s_tabla)
        {
            string sql = "SELECT id, rtrim(nombre) as nombre FROM " + s_tabla;
            return transfDao.dt_lista(sql);

        }

        public DataTable dt_transf(string id_transf)
        {
            string sql = "SELECT a.id AS id,rtrim(a.codigo) as codigo,rtrim(a.nombre) AS transformador,rtrim(a.observacion) as observacion,";
            sql += "a.kv_alta,a.kv_baja,a.mva_inst,a.z,rtrim(a.marca) as marca,a.id_cond_inter,rtrim(d.nombre) AS t_condicion, ";
            sql += "a.f_max,a.f_act,a.amp_max,a.pvar,rtrim(a.usuario) as usuario,rtrim(c.nombre) AS subestacion,a.x,a.y,a.grados,a.escala, ";
            sql += "a.b_movil,a.b_p_voltaje,a.b_p_proteccion,a.b_p_scada,b_p_ventilacion,b_p_operativo ";

            sql += "FROM esq_red.tbl_transf a,esq_red.tbl_sub c,esq_red.lta_cond_inter d " +
           "WHERE a.root= c.id  AND a.id_cond_inter= d.id AND a.id='" + id_transf + "'; ";


            return transfDao.dt_lista(sql);
        }

        public bool update_tbl_transf()
        {
            return transfDao.update_tbl_transf(ID, F_MAX, MVA, Z, MARCA, AMP, KV1, KV2, OBS, B_MOVIL, B_VOLTAJE, B_PROCT, B_SCADA, B_VENT, B_OPER, ID_COND,PVAR, USER);
        }

        public bool update_tbl_transf_xy()
        {
            return transfDao.update_tbl_transf_xy(ID, X, Y, ESCALA, GRADOS,CODIGO, USER);
        }

    }
}
