using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

namespace Common.Cache
{
    public static class UserCache
    {
        public static int IdUser { get; set; }
        public static string LoginUser { get; set; }
        //public static string Password { get; set; }
        public static string Name { get; set; }
        public static int ci { get; set; }
        public static string Telef { get; set; }
        public static string Mail { get; set; }
        public static string Cargo { get; set; }
        public static string Obs { get; set; }
        public static short id_estado { get; set; }
        public static short id_grupo { get; set; }
        public static short id_perfil { get; set; }
        public static short id_dpto { get; set; }

        public static bool bloq_pc { get; set; }
        public static bool bloq_perfil { get; set; }

        public static bool b1 { get; set; }//Cambia clave/perfil
        public static bool b2 { get; set; }//Esquemas de Red
        public static bool b3 { get; set; }//Programador
        public static bool b4 { get; set; }// Nuevo Proyecto
        public static bool b5 { get; set; }//Editar Proyecto
        public static bool b6 { get; set; }//Editar Contactos
        public static bool b7 { get; set; }//Hoja de Evaluación
        public static bool b8 { get; set; }//Incorpora Estatus
        public static bool b9 { get; set; }//Modificar Estatus
        public static bool b10 { get; set; }//Nuevo Plan Maestro
        public static bool b11 { get; set; }//Edita Solicitud
        public static bool b12 { get; set; }//Asignar Presupuestos
        public static bool b13 { get; set; }//Solicitud de Servicios
        public static bool b14 { get; set; }//Obras de Distribución
        public static bool b15 { get; set; }//Gestión de Medición
        public static bool b16 { get; set; }//Gestión de Contratos
        public static bool b17 { get; set; }//Aprueba Cortes
        public static bool b18 { get; set; }//Modifica Coordenadas
        //public static bool b { get; set; }
        //public static bool b { get; set; }
        //public static bool b { get; set; }
        //public static bool b { get; set; }
        //public static bool b { get; set; }
        //public static bool b { get; set; }
        //public static bool b { get; set; }
    }
}
