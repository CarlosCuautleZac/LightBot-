using GalaSoft.MvvmLight.Command;
using LightBot.Models;
using LightBot.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LightBot.ViewModels
{
    internal class LightBotViewModel : INotifyPropertyChanged
    {
        //Instanciacion del juego
        private Juego juego;
        public Juego Juego
        {
            get { return juego; }
            set { juego = value; Actualizar("Juego"); }
        }
        //Comandos Utilizados
        public ICommand NuevoJuegoCommand { get; set; }
        public ICommand MoverCommand { get; set; }
        public ICommand ConcatenarMovimientosCommand { get; set; }
        public ICommand VerNivelesCommand { get; set; }

        //Propiedades Utilizadas
        public string Resultado { get; set; } = "";
        public string TotalMovimientos { get; set; } = "";

        public bool EnMovimiento { get; set; } = true;

        public string Vista { get; set; } = "Inicio";

        //string arriba = "↑";
        //string abajo = "↓";
        //string izquierda = "←";
        //string derecha = "→";.

        //Movimientos Permitidos
        string derecha = "↑";
        string arriba = "→";
        string izquierda = "↓";
        string abajo = "←";

        //JugandoView jugandoView;
        //Constructor//
        public LightBotViewModel()
        {
            //Instanciacion de nuevo Juego
            Juego = new();

            //Comandos
            NuevoJuegoCommand = new RelayCommand<string>(NuevoJuego);
            MoverCommand = new RelayCommand(Mover);
            ConcatenarMovimientosCommand = new RelayCommand<string>(ConcatenarMovimientos);
            VerNivelesCommand = new RelayCommand(VerNiveles);

            Actualizar("");
        }

        private void VerNiveles()
        {
            Vista = "VerNiveles";
            Actualizar();
        }

        private void ConcatenarMovimientos(string movimiento)
        {
            movimiento = movimiento.ToUpper();
            TotalMovimientos += movimiento + ",";
            Actualizar("TotalMovimientos");
        }

        //Metodo para empezar un nuevo juego
        public void NuevoJuego(string nivel)
        {
            EnMovimiento = true;
            //Nivel a Jugar
            int nivelajugar = int.Parse(nivel);
            Juego = new();
            Juego.Vidas = 2;
            Juego.Puntos = 5000;
            Juego.Movimientos = 5;

            //Nivel 1
            if (nivelajugar == 1)
            {
                Juego.Posicion = new char[2];
                Juego.Posicion[0] = 'C';
                Juego.Posicion[1] = '5';
                Vista = "Juego";
                //jugandoView = new() { DataContext = this };
                //jugandoView.ShowDialog();
            }
            Actualizar("");
        }

        //Metodo para mover el personaje a la zona de meta o a otra posicion, obtiene como parametro 
        //La serie de movimientos
        public async void Mover()
        {
            EnMovimiento = false;
            if (TotalMovimientos != "")
            {
                TotalMovimientos = TotalMovimientos.Remove(TotalMovimientos.Length - 1, 1);
                Actualizar("TotalMovimientos");
            }
            //Validamos los movimientos
            if (ValidarMovimientos(TotalMovimientos))
            {
                //Separamos las instrucciones
                var instrucciones = TotalMovimientos.Split(',');
                //Obtenemos la pieza actual
                for (int i = 0; i < instrucciones.Length; i++)
                {
                    //Aqui vemos si es izquierda o derecha y separamos todo, ademas limitamos la movilidad
                    if (instrucciones[i] == izquierda && Juego.Posicion[1] != 'A' || instrucciones[i] == derecha && Juego.Posicion[1] != 'G')
                    {
                        //Izquierda(Abajo)
                        if (instrucciones[i] == izquierda)
                        {
                            if (Juego.Posicion[1]=='5')
                            {
                                break;
                            }
                            else
                            {
                                Juego.Posicion[1] = (char)(Juego.Posicion[1] + 1);
                                Juego.Movimientos -= 1;
                            }
                            
                        }
                        //Derecha(Arriba)
                        if (instrucciones[i]== derecha)
                        {
                            if (Juego.Posicion[1]=='1')
                            {
                                break;
                            }
                            else
                            {
                                Juego.Posicion[1] = (char)(Juego.Posicion[1] - 1);
                                Juego.Movimientos -= 1;
                            }
                        }
                        //Quitamos Puntos
                        juego.Puntos -= 500;
                    }
                    else
                    {
                        //Aqui vemos si es arriba o abajo y separamos todo, ademas limitamos la movilidad
                        if (instrucciones[i] == arriba && Juego.Posicion[0] != '0' || instrucciones[i] == abajo && Juego.Posicion[0] != '7')
                        {
                            //Arriba(Derecha)
                            if (instrucciones[i] == arriba)
                            {
                                if (Juego.Posicion[0]=='E')
                                {
                                    break;
                                }
                                else
                                {
                                    Juego.Posicion[0] = (char)(Juego.Posicion[0] + 1);
                                    Juego.Movimientos -= 1;
                                }
                            }
                            //Abajo (Izquierda)
                            if (instrucciones[i]==abajo)
                            {
                                if (Juego.Posicion[0]=='A')
                                {
                                    break;
                                }
                                else
                                {
                                    Juego.Posicion[0] = (char)(Juego.Posicion[0] - 1);
                                    Juego.Movimientos -= 1;
                                }
                            }
                            //Quitamos Puntos
                            juego.Puntos -= 500;
                        }
                        Actualizar("Juego");
                    }
                    //Si ya no quedan movimientos game over si no llegaste o si estas en la meta win
                    //VerificarMovimientos(juego.TotalMovimientos);

                    Actualizar("");
                    //El programa se detiene 3 segundos para avanzar a la siguiente posicion
                    await Task.Delay(1000);
                }
                //Reiniciamos el contador despues de todos los movimientos
                juego.Movimientos = 5;
                VerificarIntento();
                Actualizar();
            }
            else
            {
                MessageBox.Show("Solo puedes tener 5 movimientos");
                TotalMovimientos = "";
                Actualizar();
            }
            EnMovimiento = true;
            Actualizar();
        }

        private void VerificarIntento()
        {
            if (Juego.Vidas >=1)
            {
                juego.Vidas -= 1;
                if (juego.Posicion[0] == 'C' && juego.Posicion[1] == '2')
                {
                    FinDeJuego(true);
                }
                else if (juego.Vidas == 0)
                    FinDeJuego(false);
                else
                {
                    Resultado = "¡sigamos buscando a la vaquita!";
                    TotalMovimientos = "";
                    Actualizar("");
                }
            }
            else
            {
                FinDeJuego(false);
            }
        }

        private void FinDeJuego(bool ganojuego)
        {
            TotalMovimientos = "";
            if (ganojuego)
            {
                Resultado = "¡Felicidades, superaste el primer nivel!";
                Vista = "Mensaje";
                //if (jugandoView != null)
                //    jugandoView.Close();               
            }
            else
            {
                Resultado = "Perdiste, Fin del Juego";
                Vista = "Mensaje";
                //jugandoView.Close();
            }

            EnMovimiento = true;
            Actualizar("");
        }

        private void VerificarMovimientos(int movimientos)
        {
            if (movimientos == 0)
            {
                throw new ArgumentException("Felicidades, C");
            }
        }

        private bool ValidarMovimientos(string movimientos)
        {
            var instrucciones = movimientos.Split(',');
            if (instrucciones.Length > 5)
                return false;
            else
            {
                for (int x = 0; x < instrucciones.Length; x++)
                {
                    if (instrucciones[x] == arriba || instrucciones[x] == abajo
                        || instrucciones[x] == izquierda || instrucciones[x] == derecha)
                        continue;
                    else
                        return false;
                }
                return true;
            }
        }


        public void Actualizar(string nombre = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nombre));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
