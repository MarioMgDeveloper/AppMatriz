using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AppMatriz.CLASES_NODO;
namespace AppMatriz
{
    public partial class Form1 : Form
    {
        clsGrafo grafo = new clsGrafo();
        bool inicio = true, MarcaInicio;
        Button[,] matriz = new Button[8, 8];
        List<string> listaNodos = new List<string>();
        List<string> listadoEnlaces = new List<string>();
        string nodoInicio = "", nodoFin = "";

        SpeechSynthesizer synthesizer = new SpeechSynthesizer();
        List<VoiceInfo> voiceInfos = new List<VoiceInfo>();
        public Form1()
        {
            InitializeComponent();
           

            foreach (InstalledVoice voice in synthesizer.GetInstalledVoices())
            {
                voiceInfos.Add(voice.VoiceInfo);
                cbxVoces.Items.Add(voice.VoiceInfo.Name);
            }


            cbxVoces.SelectedIndex = 0;
        }

      

        #region GENERAR/CARGAR MATRIZ
        public void iniciarMatriz()
        {
            int cont = 0;
            for (int x = 0; x < 8; x++)
            {

                for (int y = 0; y < 8; y++)
                {
                    cont++;
                    Button boton = new Button();
                    boton.Name = "btn" + cont.ToString();
                    boton.Text = cont.ToString();
                    boton.Width = 40;
                    boton.Height = 40;
                    boton.Left = (boton.Width * y) + 1;
                    boton.Top = (boton.Height * x) + 1;

                    boton.BackColor = Color.White;
                    boton.FlatAppearance.BorderColor = Color.Black;
                    flowLayoutPanel1.Controls.Add(boton);
                    boton.Click += Boton_Click;

                    matriz[x, y] = boton;
                }
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            iniciarMatriz();
        }
        private void Boton_Click(object sender, EventArgs e)
        {
            Button boton = (Button)sender;

            if (MarcaInicio == true)
            {
                if (boton.Text == "I" || boton.Text == "F")
                {
                    boton.Text = boton.Name.Substring(3);
                    boton.BackColor = Color.White;
                }
                else
                {
                    if (inicio == true)
                    {
                        boton.Text = "I";
                        boton.BackColor = Color.SkyBlue;
                        inicio = false;
                        nodoInicio = boton.Name.Substring(3);
                    }
                    else
                    {
                        boton.Text = "F";
                        boton.BackColor = Color.SkyBlue;
                        inicio = true;
                        nodoFin = boton.Name.Substring(3);
                    }
                }

            }
            else
            {
                boton.BackColor = Color.Red;

            }
        }
        private void btnMarcaInicio_Click(object sender, EventArgs e)
        {
            if (MarcaInicio == false)
            {
                MarcaInicio = true;
                btnMarcaInicio.BackColor = Color.Green;
            }
            else
            {
                MarcaInicio = false;
                btnMarcaInicio.BackColor = Color.White;
            }
        }
        private void marcarEnMatriz(string dato)
        {
            dato = dato.Replace("\r", "");
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    Button boton = new Button();
                    boton = matriz[x, y];

                    if (boton.Text.Equals(dato))
                    {
                        if (string.Equals(boton.BackColor.Name, Color.Red.Name) == false)
                        {
                            boton.BackColor = Color.Red;
                            y = 10;
                            x = 10;
                        }
                    }
                }
            }
        }
        private void btnResolver_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(nodoInicio))
            {
                MessageBox.Show("INICIO/FIN NO ENCONTRADO ");
            }
            else
            {
              
                    fillListaNodos();
                    fillListaEnlaces();

                    grafo.CrearNodos(listaNodos);
                    grafo.EnlazarNodos(listadoEnlaces);


                    clsNodo NodoInicio = new clsNodo();
                    clsNodo NodoFin = new clsNodo();

                    NodoInicio = grafo.DarNodo(nodoInicio);
                    NodoInicio.Acumulado = 0;
                    grafo.BuscarRecorrido(NodoInicio, nodoFin);

                    NodoFin = grafo.DarNodo(nodoFin);


                    string recorrido = grafo.darStrRecorrido(NodoInicio, NodoFin);
                    string direcciones = grafo.darDirecciones(NodoInicio, NodoFin, recorrido);
                    //marcarRecorrido(recorrido);
                   marcarRecorridoLeds(recorrido,direcciones);
               
            }
        }
        #endregion

                #region RESOLVER
        public void fillListaNodos()
        {
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    Button boton = new Button();
                    boton = matriz[x, y];

                    if (string.Equals(boton.BackColor.Name, Color.Red.Name) == false)
                    {
                        string nodo = boton.Name.Substring(3);
                        listaNodos.Add(nodo);
                    }
                }
            }
        }
        public void fillListaEnlaces()
        {
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    Button boton = new Button();
                    boton = matriz[x, y];

                    if (string.Equals(boton.BackColor.Name, Color.Red.Name) == false)
                    {
                        string nodo = boton.Name.Substring(3);
                        nodo = nodo + "," + darAdyacentes(x, y);
                        listadoEnlaces.Add(nodo);
                    }
                }
            }
        }
        private void btnReiniciar_Click(object sender, EventArgs e)
        {
            List<Control> listControls = new List<Control>();

            foreach (Control control in flowLayoutPanel1.Controls)
            {
                listControls.Add(control);
            }

            foreach (Control control in listControls)
            {
                flowLayoutPanel1.Controls.Remove(control);
                control.Dispose();
            }
            iniciarMatriz();
            grafo.limpiarGrafos();
           
        }
        public string darAdyacentes(int x, int y)
        {
            string nodosAdyacentes = "";

            int xArriba = x - 1, xAbajo = x + 1;
            int yDerecha = y + 1, yIzquierda = y - 1;

            if (xArriba >= 0)
            {
                Button boton = new Button();
                boton = matriz[xArriba, y];
                if (string.Equals(boton.BackColor.Name, Color.Red.Name) == false)
                {
                    nodosAdyacentes = boton.Name.Substring(3);
                }
                else
                {
                    nodosAdyacentes = "-";
                }
            }
            else
            {
                nodosAdyacentes = "-";
            }

            if (yDerecha <= 7)
            {
                Button boton = new Button();
                boton = matriz[x, yDerecha];
                if (string.Equals(boton.BackColor.Name, Color.Red.Name) == false)
                {
                    nodosAdyacentes = nodosAdyacentes + "," + boton.Name.Substring(3);
                }
                else
                {
                    nodosAdyacentes = nodosAdyacentes + "," + "-";
                }
            }
            else
            {
                nodosAdyacentes = nodosAdyacentes + "," + "-";
            }

            if (xAbajo <= 7)
            {
                Button boton = new Button();
                boton = matriz[xAbajo, y];
                if (string.Equals(boton.BackColor.Name, Color.Red.Name) == false)
                {
                    nodosAdyacentes = nodosAdyacentes + "," + boton.Name.Substring(3);
                }
                else
                {
                    nodosAdyacentes = nodosAdyacentes + "," + "-";
                }
            }
            else
            {
                nodosAdyacentes = nodosAdyacentes + "," + "-";
            }

            if (yIzquierda >= 0)
            {
                Button boton = new Button();
                boton = matriz[x, yIzquierda];
                if (string.Equals(boton.BackColor.Name, Color.Red.Name) == false)
                {
                    nodosAdyacentes = nodosAdyacentes + "," + boton.Name.Substring(3);
                }
                else
                {
                    nodosAdyacentes = nodosAdyacentes + "," + "-";
                }
            }
            else
            {
                nodosAdyacentes = nodosAdyacentes + "," + "-";
            }

            return nodosAdyacentes;
        }
        public void marcarRecorrido(string recorrido)
        {
            foreach (string nodo in recorrido.Split('-'))
            {
                if (nodo.Equals("I") == false && nodo.Equals("F") == false)
                {
                    for (int x = 0; x < 8; x++)
                    {
                        for (int y = 0; y < 8; y++)
                        {
                            Button boton = new Button();
                            boton = matriz[x, y];
                            if (boton.Text.Equals(nodo))
                            {
                                boton.BackColor = Color.GreenYellow;
                                y = 10;
                                x = 10;
                            }
                        }
                    }
                }
            }
        }
        public void marcarRecorridoLeds(string recorrido, string Direcciones)
        {
            List<string> nodos = new List<string>();
            List<string> direcciones = new List<string>();
            foreach (string nodo in recorrido.Split('-'))
            {
                string dato =  nodo;
                nodos.Add(dato);
            }

            foreach (string nodo in Direcciones.Split('-'))
            {

                direcciones.Add(nodo);
            }



            nodos.Reverse();

            for (int x = 0; x < nodos.Count; x++)
            {
                Reproducir(direcciones[x].ToString());
                marcarBoton(nodos[x].ToString());
                this.Refresh();
                Thread.Sleep(400);
            }

            Reproducir("Has llegado al final del recorrido");

        }


        public void marcarBoton(string dato)
        {
           
                for (int x = 0; x < 8; x++)
                {
                    for (int y = 0; y < 8; y++)
                    {
                        Button boton = new Button();
                        boton = matriz[x, y];
                        if (boton.Name.Substring(3).Equals(dato))
                        {
                            boton.BackColor = Color.GreenYellow;
                            y = 10;
                            x = 10;
                           
                        }
                    }
                }
            
        }
        #endregion

        #region VOZ
        public void Reproducir(string texto)
        {
            int indice;
            indice = cbxVoces.SelectedIndex;
            String nombre = voiceInfos.ElementAt(indice).Name;
            synthesizer.SelectVoice(nombre);

            //synthesizer.Volume = (int)(volumen);
            synthesizer.Rate = 0;
            synthesizer.Speak(texto );
        }
        #endregion


    }
}
