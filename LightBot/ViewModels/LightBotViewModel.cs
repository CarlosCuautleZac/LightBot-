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
        public ICommand MoverNivel2Command { get; set; }
        public ICommand MoverNivel3Command { get; set; }
        public ICommand MoverNivel4Command { get; set; }
        public ICommand CambiarVistaAJugarCommand { get; set; }
        public ICommand ConcatenarMovimientosCommand { get; set; }
        public ICommand VerNivelesCommand { get; set; }
        public ICommand MoverDependiendoElNivelCommand { get; set; }

        //Propiedades Utilizadas
        public string Resultado { get; set; } = "";
        public string TotalMovimientos { get; set; } = "";
        public bool EnMovimiento { get; set; } = true;
        public string Vista { get; set; } = "Inicio";
        public int Nivel { get; set; }
        public bool Daño { get; set; } = false;

        public bool HayMensaje { get; set; }
        //Posicion de la Vaca(Vista)
        public int ColAleatoria { get; set; }
        public int RowAleatoria { get; set; }
        //Posicion de la Vaca(Codigo)
        public char ColumnaVaca;
        private char RenglonVaca;
        //Movimientos Permitidos
        string derecha = "→";
        string arriba = "↑";
        string izquierda = "←";
        string abajo = "↓";
        string salto = "↷";

        //variable de soporte para cambiar la vista al final
        int v = 0;
        //Random//
        Random R = new Random();

        //Constructor//
        public LightBotViewModel()
        {
            //Instanciacion de nuevo Juego
            Juego = new();
            
            //Comandos
            NuevoJuegoCommand = new RelayCommand<string>(NuevoJuego);
            MoverCommand = new RelayCommand(Mover);
            MoverNivel2Command = new RelayCommand(MovenEnNivel2);
            MoverNivel3Command = new RelayCommand(MoverEnNivel3);
            MoverNivel4Command = new RelayCommand(MoverEnNivel4);
            ConcatenarMovimientosCommand = new RelayCommand<string>(ConcatenarMovimientos);
            VerNivelesCommand = new RelayCommand(VerNiveles);
            CambiarVistaAJugarCommand = new RelayCommand(CambiarVistaAJugar);
            MoverDependiendoElNivelCommand = new RelayCommand(MoverDependiendoElNivel);
            HayMensaje = false;
            Actualizar("");
        }

        private void MoverDependiendoElNivel()
        {
            switch (Nivel)
            {
                case 1: Mover(); break;
                case 2: MovenEnNivel2(); break;
                case 3: MoverEnNivel3(); break;
                case 4: MoverEnNivel4(); break;
                //Cambiar dependiendo el nivel
            }
        }

        #region Metodos en comun de todos los niveles
        private void CambiarVistaAJugar()
        {
            if (Vista=="Instrucciones" && v==1)
            {
                Vista="Inicio";
            }
            else
            {
                Vista = "Juego";
            }
            Actualizar(nameof(Vista));
            v=0;
        }
        public void NuevoJuego(string nivel)
        {
            Nivel = int.Parse(nivel);
            EnMovimiento = true;
            //Nivel a Jugar
            int nivelajugar = int.Parse(nivel);
            Juego = new();
            Juego.Puntos = 5000;
            Juego.Movimientos = 5;

            //Nivel 1
            if (nivelajugar == 1)
            {
                Juego.Vidas = 2;
                Juego.Posicion = new char[2];
                Juego.Posicion[0] = 'C';
                Juego.Posicion[1] = '5';
                //Vista = "Juego";
                Vista = "Instrucciones";
                //jugandoView = new() { DataContext = this };
                //jugandoView.ShowDialog();
            }
            if (nivelajugar == 2)
            {
                Juego.Vidas = 3;
                Juego.Posicion = new char[2];
                Juego.Posicion[0] = 'C';
                Juego.Posicion[1] = '5';
                Vista = "Instrucciones";
                //Vista = "Juego2";
                //jugandoView = new() { DataContext = this };
                //jugandoView.ShowDialog();
            }
            if (nivelajugar == 3)
            {
                Juego.Vidas = 3;
                Juego.Posicion = new char[2];
                Juego.Posicion[0] = 'C';
                Juego.Posicion[1] = '5';
                Vista = "Instrucciones";
            }
            if (nivelajugar == 4)
            {
                Juego.Vidas = 3;
                Juego.Movimientos=8;
                Juego.Posicion = new char[2];
                //Jugador
                Juego.Posicion[0] = 'E';
                Juego.Posicion[1] = '5';
                GeneracionRowAleatoria();
                Vista = "Instrucciones";
            }
            if(nivelajugar == 5)
            {
                Vista="Instrucciones";
            }
            Actualizar("");
        }
        private void ConcatenarMovimientos(string movimiento)
        {
            movimiento = movimiento.ToUpper();
            TotalMovimientos += movimiento + ",";
            Actualizar("TotalMovimientos");
        }
        private void VerNiveles()
        {
            if (Resultado == "Solo puedes tener hasta 5 movimientos")
            {
                Vista = "Juego";
            }
            else if (Resultado == "¡Felicidades, superaste el primer nivel!")
            {
                NuevoJuego("2");
            }

            else if (Resultado == "¡Felicidades, superaste el segundo nivel!")
            {
                NuevoJuego("3");
                
            }
            else if (Resultado == "¡Felicidades, superaste el tercer nivel!")
            {
                NuevoJuego("4");
            }
            else if (Resultado=="¡Felicidades, superaste el cuarto nivel!")
            {
                //Falso nivel 5
                v=1;
                NuevoJuego("5");
            }
            else
            {
                Vista = "VerNiveles";
            }
            HayMensaje = false;
            Actualizar(); 
            Resultado="Instrucciones";
        }
        private void FinDeJuego(bool ganojuego)
        {
            TotalMovimientos = "";

            if (ganojuego && Nivel == 1)
            {
                Resultado = "¡Felicidades, superaste el primer nivel!";
                
                HayMensaje = true;
                //if (jugandoView != null)
                //    jugandoView.Close();
                Actualizar();
            }
            else if (ganojuego && Nivel == 2)
            {
                Resultado = "¡Felicidades, superaste el segundo nivel!";
                HayMensaje = true;

                //if (jugandoView != null)
                //    jugandoView.Close();
                Actualizar();
            }
            else if (ganojuego && Nivel == 3)
            {
                Resultado = "¡Felicidades, superaste el tercer nivel!";
                HayMensaje = true;
                Actualizar();
            }
            else if (ganojuego && Nivel == 4)
            {
                Resultado = "¡Felicidades, superaste el cuarto nivel!";
                Vista = "Mensaje";
                Actualizar();
            }
            else
            {
                Resultado = "Perdiste, Fin del Juego";
                HayMensaje = true;
                //jugandoView.Close();
            }
            EnMovimiento = true;
            Actualizar("");
        }
        private bool ValidarMovimientos(string movimientos)
        {
            var instrucciones = movimientos.Split(',');
            if ((Nivel>0 && Nivel<4) && instrucciones.Length > 5)
            {
                return false;
            }
            else if (Nivel==4 && instrucciones.Length>8)
            {
                return false;
            }
            else
            {
                for (int x = 0; x < instrucciones.Length; x++)
                {
                    if (instrucciones[x] == arriba || instrucciones[x] == abajo ||
                    instrucciones[x] == izquierda || instrucciones[x] == derecha ||
                    instrucciones[x] == salto)
                        continue;
                    else
                        return false;
                }
                return true;
            }
        }
        #endregion
        #region Nivel 1
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
                    if (instrucciones[i] == izquierda && Juego.Posicion[0] == 'A' || instrucciones[i] == derecha && Juego.Posicion[0] == 'E'
                      || instrucciones[i] == arriba && Juego.Posicion[1] == '1' || instrucciones[i] == abajo && Juego.Posicion[1] == '5')
                    {
                        break;
                    }
                    //Izquierda
                    if (instrucciones[i] == izquierda && Juego.Posicion[1] != 'A')
                    {
                        Juego.Posicion[0] = (char)(Juego.Posicion[0] - 1);
                    }
                    //Derecha
                    if (instrucciones[i] == derecha && Juego.Posicion[1] != 'E')
                    {
                        Juego.Posicion[0] = (char)(Juego.Posicion[0] + 1);
                    }
                    //Arriba
                    if (instrucciones[i] == arriba && Juego.Posicion[0] != '0')
                    {
                        Juego.Posicion[1] = (char)(Juego.Posicion[1] - 1);
                    }
                    //Abajo
                    if (instrucciones[i] == abajo && Juego.Posicion[0] != '5')
                    {
                        Juego.Posicion[1] = (char)(Juego.Posicion[1] + 1);
                    }
                    //Quitamos un movimiento
                    Juego.Movimientos -= 1;
                    //Quitamos Puntos
                    juego.Puntos -= 500;
                    Actualizar("Juego");
                    //Si ya no quedan movimientos game over si no llegaste o si estas en la meta win
                    //VerificarMovimientos(juego.TotalMovimientos);
                    Actualizar("");
                    //El programa se detiene 3 segundos para avanzar a la siguiente posicion
                    await Task.Delay(1000);
                }
                //Reiniciamos el contador despues de todos los movimientos
                juego.Movimientos = 5;
                VerificarIntento();
            }
            else
            {
                MessageBox.Show("Solo puedes tener 5 movimientos");
                TotalMovimientos = "";
            }
            EnMovimiento = true;
            Actualizar();
        }
        private void VerificarIntento()
        {
            if (Juego.Vidas >= 1)
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

        #endregion
        #region Nivel 2
        private async void MovenEnNivel2()
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
                    Daño = false;
                    Actualizar(nameof(Daño));

                    if (instrucciones[i] == izquierda && Juego.Posicion[0] == 'A' || instrucciones[i] == derecha && Juego.Posicion[0] == 'E'
                      || instrucciones[i] == arriba && Juego.Posicion[1] == '1' || instrucciones[i] == abajo && Juego.Posicion[1] == '5')
                    {
                        break;
                    }
                    //Izquierda
                    if (instrucciones[i] == izquierda && Juego.Posicion[1] != 'A')
                    {
                        Juego.Posicion[0] = (char)(Juego.Posicion[0] - 1);
                    }
                    //Derecha
                    if (instrucciones[i] == derecha && Juego.Posicion[1] != 'E')
                    {
                        Juego.Posicion[0] = (char)(Juego.Posicion[0] + 1);
                    }
                    //Arriba
                    if (instrucciones[i] == arriba && Juego.Posicion[0] != '0')
                    {
                        Juego.Posicion[1] = (char)(Juego.Posicion[1] - 1);
                    }
                    //Abajo
                    if (instrucciones[i] == abajo && Juego.Posicion[0] != '5')
                    {
                        Juego.Posicion[1] = (char)(Juego.Posicion[1] + 1);
                    }
                    //Quitamos un movimiento
                    Juego.Movimientos -= 1;
                    //Ver si la monita se movio a un cactus para que pierda una vida
                    if (Juego.Posicion[1] == '4' && Juego.Posicion[0] == 'A' || Juego.Posicion[1] == '2' && Juego.Posicion[0] == 'B' ||
                        Juego.Posicion[1] == '4' && Juego.Posicion[0] == 'C' || Juego.Posicion[1] == '4' && Juego.Posicion[0] == 'D' ||
                        Juego.Posicion[1] == '5' && Juego.Posicion[0] == 'D')
                    {
                        Juego.Vidas -= 1;
                        juego.Puntos -= 1000;
                        Daño = true;
                        Actualizar("");

                    }
                    if (juego.Vidas == 0)
                    {
                        await Task.Delay(1000);
                        FinDeJuego(false);
                        break;
                    }
                    //Quitamos Puntos
                    juego.Puntos -= 500;
                    Actualizar("Juego");
                    //Si ya no quedan movimientos game over si no llegaste o si estas en la meta win
                    //VerificarMovimientos(juego.TotalMovimientos);
                    Actualizar("");
                    //El programa se detiene 3 segundos para avanzar a la siguiente posicion
                    await Task.Delay(1000);
                }
                //Reiniciamos el contador despues de todos los movimientos
                Daño = false;
                Actualizar(nameof(Daño));
                juego.Movimientos = 5;
                VerificarIntentoNivel2();
            }
            else
            {
                MessageBox.Show("Solo puedes tener 5 movimientos");
                TotalMovimientos = "";
            }
            EnMovimiento = true;
            Actualizar();
        }
        private void VerificarIntentoNivel2()
        {
            if (Juego.Vidas >= 1)
            {
                if (Juego.Posicion[1] == '4' && Juego.Posicion[0] == 'A' || Juego.Posicion[1] == '2' && Juego.Posicion[0] == 'B' ||
                    Juego.Posicion[1] == '4' && Juego.Posicion[0] == 'C' || Juego.Posicion[1] == '4' && Juego.Posicion[0] == 'D' ||
                    Juego.Posicion[1] == '5' && Juego.Posicion[0] == 'D')
                {
                    Juego.Posicion[0] = 'C';
                    Juego.Posicion[1] = '5';
                }
                Actualizar("");
                juego.Vidas -= 1;
                if (juego.Posicion[0] == 'D' && juego.Posicion[1] == '3')
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

        #endregion
        #region Nivel 3
        private async void MoverEnNivel3()
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
                    Daño = false;
                    Actualizar(nameof(Daño));
                    if (instrucciones[i] == izquierda && Juego.Posicion[0] == 'A' || instrucciones[i] == derecha && Juego.Posicion[0] == 'E'
                      || instrucciones[i] == arriba && Juego.Posicion[1] == '1' || instrucciones[i] == abajo && Juego.Posicion[1] == '5')
                    {
                        break;
                    }
                    //Izquierda
                    if (instrucciones[i] == izquierda && Juego.Posicion[1] != 'A')
                    {
                        Juego.Posicion[0] = (char)(Juego.Posicion[0] - 1);
                    }
                    //Derecha
                    if (instrucciones[i] == derecha && Juego.Posicion[1] != 'E')
                    {
                        Juego.Posicion[0] = (char)(Juego.Posicion[0] + 1);
                    }
                    //Arriba
                    if (instrucciones[i] == arriba && Juego.Posicion[0] != '0')
                    {
                        //parar en la cuarta fila
                        if (juego.Posicion[1] != '4')
                        {
                            Juego.Posicion[1] = (char)(Juego.Posicion[1] - 1);
                        }
                        else
                        {
                            continue;
                        }
                    }
                    //Abajo
                    if (instrucciones[i] == abajo && Juego.Posicion[0] != '5')
                    {
                        //parar en la segunda fila
                        if (juego.Posicion[1] != '2')
                        {
                            Juego.Posicion[1] = (char)(Juego.Posicion[1] + 1);
                        }
                        else
                        {
                            continue;
                        }
                    }
                    //Salto hacia abajo
                    if (instrucciones[i] == salto && juego.Posicion[1] == '2' && instrucciones[i-1] == abajo)
                    {
                        juego.Posicion[1] = (char)(Juego.Posicion[1] + 2);
                    }
                    if (instrucciones[i] == salto && juego.Posicion[1] == '4' && instrucciones[i-1] == arriba)
                    {
                        if (juego.Posicion[0]=='E')
                        {
                            Juego.Vidas -= 1;
                            juego.Puntos -= 1000;
                            Daño = true;
                            Actualizar("");
                        }
                        juego.Posicion[1] = (char)(Juego.Posicion[1] - 2);
                    }
                    //Quitamos un movimiento
                    Juego.Movimientos -= 1;
                    //Ver si la monita se movio a un cactus para que pierda una vida
                    if (Juego.Posicion[1] == '4' && Juego.Posicion[0] == 'A' || Juego.Posicion[1] == '2' && Juego.Posicion[0] == 'B')
                    {
                        Juego.Vidas -= 1;
                        juego.Puntos -= 1000;
                        Daño = true;
                        Actualizar("");
                    }
                    //Si no tiene vidas Pierde el juego
                    if (juego.Vidas == 0)
                    {
                        await Task.Delay(1000);
                        FinDeJuego(false);
                        break;
                    }
                    //Quitamos Puntos
                    juego.Puntos -= 500;
                    Actualizar("Juego");
                    //Si ya no quedan movimientos game over si no llegaste o si estas en la meta win
                    //VerificarMovimientos(juego.TotalMovimientos);
                    Actualizar("");
                    //El programa se detiene 3 segundos para avanzar a la siguiente posicion
                    await Task.Delay(1000);
                }
                //Reiniciamos el contador despues de todos los movimientos
                Daño = false;
                Actualizar(nameof(Daño));
                juego.Movimientos = 5;
                VerificarIntentoNivel3();
            }
            else
            {
                MessageBox.Show("Solo puedes tener 5 movimientos");
                TotalMovimientos = "";
            }
            EnMovimiento = true;
            Actualizar();
        }

        private void VerificarIntentoNivel3()
        {
            if (Juego.Vidas >= 1)
            {
                //Posicion de los cactus
                if (Juego.Posicion[1] == '4' && Juego.Posicion[0] == 'A' || Juego.Posicion[1] == '2' && Juego.Posicion[0] == 'B')
                {
                    Juego.Posicion[0] = 'C';
                    Juego.Posicion[1] = '5';
                }
                Actualizar("");

                juego.Vidas -= 1;
                if (juego.Posicion[0] == 'C' && juego.Posicion[1] == '2') { FinDeJuego(true); }
                else if (juego.Vidas == 0) { FinDeJuego(false); }
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
        #endregion
        #region Nivel 4
        private void GeneracionRowAleatoria()
        {
            //Vaca
            RowAleatoria = R.Next(0, 5);
            //La generacion tiene que ser entera para dibujar la vaca en el juego
            if (RowAleatoria == 1 || RowAleatoria == 3)
            {
                GeneracionRowAleatoria();

            }
            else
            {
                //Asignar Posicion al Renglon
                if (RowAleatoria==0)
                {
                    RenglonVaca='1';
                }
                if (RowAleatoria==1)
                {
                    RenglonVaca='2';
                }
                if (RowAleatoria==2)
                {
                    RenglonVaca='3';
                }
                if (RowAleatoria==3)
                {
                    RenglonVaca='4';
                }
                if (RowAleatoria==4)
                {
                    RenglonVaca='5';
                }
                GeneracionColAleatoria();
            }
        }
        private void GeneracionColAleatoria()
        {
            ColAleatoria = R.Next(0, 4);
            //La generacion tiene que ser entera para dibujar la vaca en el juego
            if (((RowAleatoria==0||RowAleatoria==2) && (ColAleatoria==1 || ColAleatoria== 4)) || (RowAleatoria==4 && (ColAleatoria==0 || ColAleatoria==2)) || RowAleatoria==5)
            {
                GeneracionColAleatoria();
            }
            //Asignar Posicion a la Columna
            if (ColAleatoria==0)
            {
                ColumnaVaca = 'A';
            }
            if (ColAleatoria==1)
            {
                ColumnaVaca='B';
            }
            if (ColAleatoria==2)
            {
                ColumnaVaca='C';
            }
            if (ColAleatoria==3)
            {
                ColumnaVaca='D';
            }
        }

        private async void MoverEnNivel4()
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
                    Daño = false;
                    Actualizar(nameof(Daño));

                    if (instrucciones[i] == izquierda && Juego.Posicion[0] == 'A' || instrucciones[i] == derecha && Juego.Posicion[0] == 'E'
                      || instrucciones[i] == arriba && Juego.Posicion[1] == '1' || instrucciones[i] == abajo && Juego.Posicion[1] == '5')
                    {
                        break;
                    }
                    //Izquierda
                    if (instrucciones[i] == izquierda && Juego.Posicion[1] != 'A')
                    {
                        Juego.Posicion[0] = (char)(Juego.Posicion[0] - 1);
                    }
                    //Derecha
                    if (instrucciones[i] == derecha && Juego.Posicion[1] != 'E')
                    {
                        Juego.Posicion[0] = (char)(Juego.Posicion[0] + 1);
                    }
                    //Arriba
                    if (instrucciones[i] == arriba && Juego.Posicion[0] != '0')
                    {
                        //parar en la quinta y tercera fila
                        if (juego.Posicion[1] == '5' || juego.Posicion[1] =='3')
                        {
                            continue;
                        }
                        else
                        {
                            Juego.Posicion[1] = (char)(Juego.Posicion[1] - 1);
                        }
                    }
                    //Abajo
                     if (instrucciones[i] == abajo && Juego.Posicion[0] != '5')
                    {
                        //parar en la primera y tercera fila
                        if (juego.Posicion[1] == '1' || juego.Posicion[1] == '3')
                        {
                            continue;
                        }
                        else
                        {
                            Juego.Posicion[1] = (char)(Juego.Posicion[1] + 1);
                        }
                    }
                   
                    if (instrucciones[i] == salto && i>=1)
                    {
                        //Salto hacia abajo
                        if ((juego.Posicion[1] == '1' || juego.Posicion[1] == '3') && instrucciones[i - 1] == abajo)
                        {
                            juego.Posicion[1] = (char)(Juego.Posicion[1] + 2);
                        }
                        else if (i>=2 && (juego.Posicion[1]=='1'||juego.Posicion[1]=='3') && (instrucciones[i-1]==salto && instrucciones[i-2]==abajo))
                        {
                            juego.Posicion[1] = (char)(Juego.Posicion[1] + 2);
                        } 
                        //Salto hacia arriba
                        if ((juego.Posicion[1] == '5'||juego.Posicion[1] == '3') && instrucciones[i - 1] == arriba)
                        {
                            juego.Posicion[1] = (char)(Juego.Posicion[1] - 2);
                        }
                        else if (i>=2 && (juego.Posicion[1]=='1'||juego.Posicion[1]=='3') && (instrucciones[i-1]==salto && instrucciones[i-2]==arriba))
                        {
                            juego.Posicion[1] = (char)(Juego.Posicion[1] - 2);
                        }
                    }

                    //Quitamos un movimiento
                    Juego.Movimientos -= 1;
                    //Ver si la monita se movio a un cactus para que pierda una vida
                    if (Juego.Posicion[1] == '5' && Juego.Posicion[0] == 'A' || Juego.Posicion[1] == '3' && Juego.Posicion[0] == 'B' ||
                        Juego.Posicion[1] == '1' && Juego.Posicion[0] == 'B' || Juego.Posicion[1] == '5' && Juego.Posicion[0] == 'C' ||
                        Juego.Posicion[1] == '3' && Juego.Posicion[0] == 'B' || Juego.Posicion[1] == '1' && Juego.Posicion[0] == 'E' ||
                        Juego.Posicion[1] == '3' && Juego.Posicion[0] == 'E')
                    {
                        Juego.Vidas -= 1;
                        juego.Puntos -= 1000;
                        Daño = true;
                        Actualizar("");
                    }
                    //Si no tiene vidas Pierde el juego
                    if (juego.Vidas == 0)
                    {
                        await Task.Delay(1000);
                        FinDeJuego(false);
                        break;
                    }
                    //Quitamos Puntos
                    juego.Puntos -= 500;
                    Actualizar("Juego");
                    //Si ya no quedan movimientos game over si no llegaste o si estas en la meta win
                    //VerificarMovimientos(juego.TotalMovimientos);
                    Actualizar("");
                    //El programa se detiene 3 segundos para avanzar a la siguiente posicion
                    await Task.Delay(1000);
                }
                //Reiniciamos el contador despues de todos los movimientos
                Daño = false;
                Actualizar(nameof(Daño));
                juego.Movimientos = 5;
                VerificarIntentoNivel4();
            }
            else
            {
                MessageBox.Show("Solo puedes tener 8 movimientos");
                TotalMovimientos = "";
            }
            EnMovimiento = true;
            Actualizar();
        }

        private void VerificarIntentoNivel4()
        {
            if (Juego.Vidas >= 1)
            {
                //Posicion de los cactus
                if (Juego.Posicion[1] == '5' && Juego.Posicion[0] == 'A' || Juego.Posicion[1] == '3' && Juego.Posicion[0] == 'B' ||
                    Juego.Posicion[1] == '1' && Juego.Posicion[0] == 'B' || Juego.Posicion[1] == '5' && Juego.Posicion[0] == 'C' ||
                    Juego.Posicion[1] == '3' && Juego.Posicion[0] == 'B' || Juego.Posicion[1] == '1' && Juego.Posicion[0] == 'E' ||
                    Juego.Posicion[1] == '3' && Juego.Posicion[0] == 'E')
                {
                    Juego.Posicion[0] = 'E';
                    Juego.Posicion[1] = '5';
                }
                Actualizar("");

                juego.Vidas -= 1;
                //Checar si la posicion actual es la misma posicion que la vaca
                if (juego.Posicion[0] == ColumnaVaca && juego.Posicion[1] == RenglonVaca) { FinDeJuego(true); }
                else if (juego.Vidas == 0) { FinDeJuego(false); }
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
        #endregion

        public void Actualizar(string nombre = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nombre));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
