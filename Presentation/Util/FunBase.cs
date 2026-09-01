using System;
using System.Xml;

namespace Presentation.Util
{
    internal class FunBase
    {
        /// <summary>
        /// Crea un nuevo par clave-valor en el nodo actual en caso de que no exista
        /// <returns></returns>
        public XmlNode NodoValor(XmlDocument doc, XmlNode nodoPadre, string Clave, Object Valor)
        {
            XmlNode nodoActual = nodoPadre.SelectSingleNode(Clave);
            if (nodoActual == null)
                nodoActual = nodoPadre.AppendChild(doc.CreateNode(XmlNodeType.Element, Clave, null));
            nodoActual.InnerText = Valor.ToString();
            return nodoActual;
        }
    }
}