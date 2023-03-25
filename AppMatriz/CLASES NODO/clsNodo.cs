using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMatriz.CLASES_NODO
{
    class clsNodo
    {
        private string dato;
        private clsNodo arriba;
        private clsNodo derecha;
        private clsNodo abajo;
        private clsNodo izquierda;
        private int acumulado = -1;
        private string antecesor;


        public string Dato
        {
            get { return dato; }
            set { dato = value; }

        }


        public int Acumulado
        {
            get { return acumulado; }
            set { acumulado = value; }

        }

        public string Antecesor
        {
            get { return antecesor; }
            set { antecesor = value; }
        }

        public clsNodo Arriba
        {
            get { return arriba; }
            set { arriba = value; }
        }

        public clsNodo Abajo
        {
            get { return abajo; }
            set { abajo = value; }
        }

        public clsNodo Izquierda
        {
            get { return izquierda; }
            set { izquierda = value; }
        }

        public clsNodo Derecha
        {
            get { return derecha; }
            set { derecha = value; }
        }
    }
}
