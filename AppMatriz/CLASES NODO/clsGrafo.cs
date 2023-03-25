using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMatriz.CLASES_NODO
{
    class clsGrafo
    {
        List<clsNodo> nodos = new List<clsNodo>();
        public void CrearNodos(List<string> datos)
        {
            foreach (string dato in datos)
            {
                clsNodo Nuevo = new clsNodo();
                Nuevo.Dato = dato;
                Nuevo.Arriba = null;
                Nuevo.Derecha = null;
                Nuevo.Abajo = null;
                Nuevo.Izquierda = null;
                nodos.Add(Nuevo);
            }
        }


        public void EnlazarNodos(List<string> infoList)
        {
            foreach (string info in infoList)
            {
                int cont = 0;
                clsNodo aux = new clsNodo();
                clsNodo aux2 = new clsNodo();

           
                foreach (string nodo in info.Split(','))
                {
                    cont++;
                    if (cont == 1)
                    {
                        aux = DarNodo(nodo);
                    }
                    else if (cont == 2)
                    {
                        if (nodo.Equals("-"))
                        {
                            aux.Arriba = null;
                        }
                        else
                        {
                            aux.Arriba = DarNodo(nodo);
                        }
                    }
                    else if (cont == 3)
                    {
                        if (nodo.Equals("-"))
                        {
                            aux.Derecha = null;
                        }
                        else
                        {
                            aux.Derecha = DarNodo(nodo);
                        }
                    }
                    else if (cont == 4)
                    {
                        if (nodo.Equals("-"))
                        {
                            aux.Abajo = null;
                        }
                        else
                        {
                            aux.Abajo = DarNodo(nodo);
                        }
                    }
                    else if (cont == 5)
                    {
                        if (nodo.Equals("-"))
                        {
                            aux.Izquierda = null;
                        }
                        else
                        {
                            aux.Izquierda = DarNodo(nodo);
                        }
                    }
                }
            }
        }

        public clsNodo DarNodo(string dato)
        {
            clsNodo aux = new clsNodo();

            foreach (clsNodo nodo in nodos)
            {
                if (nodo.Dato == dato)
                {
                    aux = nodo;
                    break;
                }

            }

            return aux;
        }

        public string darStrRecorrido(clsNodo inicio, clsNodo fin)
        {
            string recorrido = fin.Dato;
            clsNodo aux = new clsNodo();
            do
            {

                recorrido = recorrido + "-" + fin.Antecesor;
                fin = DarNodo(fin.Antecesor);

            } while (fin.Dato.Equals(inicio.Dato) == false);

            return recorrido;
        }

        public string darDirecciones(clsNodo inicio, clsNodo fin, string recorrido)
        {
            List<string> nodos = new List<string>();
            foreach (string nodo in recorrido.Split('-'))
            {
                //string dato = "A" + nodo;
                nodos.Add(nodo);

            }


            nodos.Reverse();

            clsNodo aux = new clsNodo();
            clsNodo auxAnterior = new clsNodo();
            string direcciones = "";

            aux = inicio;
            foreach (string dato in nodos)
            {
                auxAnterior = aux;
                aux = DarNodo(dato);

                if (aux.Dato.Equals(inicio.Dato))
                {
                    direcciones = "inicio";
                }
                if (auxAnterior.Arriba != null)
                {
                    if (auxAnterior.Arriba.Dato.Equals(aux.Dato))
                    {
                        direcciones = direcciones + "-arriba";
                    }
                }
                if (auxAnterior.Derecha != null)
                {
                    if (auxAnterior.Derecha.Dato.Equals(aux.Dato))
                    {
                        direcciones = direcciones + "-derecha";
                    }
                }
                if (auxAnterior.Abajo != null)
                {
                    if (auxAnterior.Abajo.Dato.Equals(aux.Dato))
                    {
                        direcciones = direcciones + "-abajo";
                    }
                }
                if (auxAnterior.Izquierda != null)
                {
                    if (auxAnterior.Izquierda.Dato.Equals(aux.Dato))
                    {
                        direcciones = direcciones + "-izquierda";
                    }
                }

            }

            return direcciones;
        }

        public void BuscarRecorrido(clsNodo nodo, string strFin)
        {
            if (nodo.Dato.Equals(strFin) == false)
            {
                clsNodo aux = new clsNodo();

                if (nodo.Arriba != null)//<-----------------No es nulo
                {
                    aux = nodo.Arriba;

                    if (string.IsNullOrEmpty(nodo.Antecesor) || nodo.Antecesor.Equals(aux.Dato) == false)//<---no sea el nodo de donde viene
                    {
                        if (aux.Acumulado < 0)
                        {
                            aux.Acumulado = nodo.Acumulado + 1;
                            aux.Antecesor = nodo.Dato;
                            BuscarRecorrido(aux, strFin);
                        }
                        else
                        {
                            int acumulado = nodo.Acumulado + 1;
                            if (acumulado <= aux.Acumulado)
                            {
                                aux.Acumulado = acumulado;
                                aux.Antecesor = nodo.Dato;
                                BuscarRecorrido(aux, strFin);
                            }
                        }
                    }

                }


                if (nodo.Derecha != null)
                {
                    aux = nodo.Derecha;

                    if (string.IsNullOrEmpty(nodo.Antecesor) || nodo.Antecesor.Equals(aux.Dato) == false)
                    {
                        if (aux.Acumulado < 0)
                        {
                            aux.Acumulado = nodo.Acumulado + 1;
                            aux.Antecesor = nodo.Dato;
                            BuscarRecorrido(aux, strFin);
                        }
                        else
                        {
                            int acumulado = nodo.Acumulado + 1;
                            if (acumulado <= aux.Acumulado)
                            {
                                aux.Acumulado = acumulado;
                                aux.Antecesor = nodo.Dato;
                                BuscarRecorrido(aux, strFin);
                            }

                        }
                    }

                }


                if (nodo.Abajo != null)
                {
                    aux = nodo.Abajo;

                    if (string.IsNullOrEmpty(nodo.Antecesor) || nodo.Antecesor.Equals(aux.Dato) == false)
                    {
                        if (aux.Acumulado < 0)
                        {
                            aux.Acumulado = nodo.Acumulado + 1;
                            aux.Antecesor = nodo.Dato;
                            BuscarRecorrido(aux, strFin);
                        }
                        else
                        {
                            int acumulado = nodo.Acumulado + 1;
                            if (acumulado <= aux.Acumulado)
                            {
                                aux.Acumulado = acumulado;
                                aux.Antecesor = nodo.Dato;
                                BuscarRecorrido(aux, strFin);
                            }
                        }
                    }

                }


                if (nodo.Izquierda != null)
                {
                    aux = nodo.Izquierda;

                    if (string.IsNullOrEmpty(nodo.Antecesor) || nodo.Antecesor.Equals(aux.Dato) == false)
                    {
                        if (aux.Acumulado < 0)
                        {
                            aux.Acumulado = nodo.Acumulado + 1;
                            aux.Antecesor = nodo.Dato;
                            BuscarRecorrido(aux, strFin);
                        }
                        else
                        {
                            int acumulado = nodo.Acumulado + 1;
                            if (acumulado <= aux.Acumulado)
                            {
                                aux.Acumulado = acumulado;
                                aux.Antecesor = nodo.Dato;
                                BuscarRecorrido(aux, strFin);
                            }
                        }
                    }

                }


            }

        }

        public void limpiarGrafos()
        {
            nodos.Clear();
        }
    }
}
