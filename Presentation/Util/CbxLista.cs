using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Presentation.Util
{

#pragma warning disable CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
    public class CbxLista
#pragma warning restore CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
    {
#pragma warning disable CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
        public string id { get; set; }
#pragma warning restore CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
#pragma warning disable CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
        public string nombre { get; set; }
#pragma warning restore CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
        

        //private static CbxLista _instance = null;
        //private static SortedList formInstances = new SortedList(); // Para guardar las referencias de las instancias de los formularios


        //public static CbxLista Instance
        //{
        //    get
        //    {
        //        if (_instance == null)
        //            _instance = new CbxLista();
        //        return CbxLista._instance;
        //    }
        //    //set { Fun._instance = value; }
        //}
#pragma warning disable CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
        public CbxLista(string _id, string _nombre)
#pragma warning restore CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
        {
            this.id = _id.TrimEnd();
            this.nombre = _nombre.TrimEnd();
        }

#pragma warning disable CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
        public override string ToString() { return this.nombre; }
#pragma warning restore CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
    }
}
