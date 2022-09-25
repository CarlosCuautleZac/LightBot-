using LightBot.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightBot.ViewModels
{
    internal class LightBotViewModel:INotifyPropertyChanged
    {

        Juego juego;

        

        //
        public string Resultado { get; set; } = "";

        public LightBotViewModel()
        {
            juego = new();
            NuevoJuego(1);
            Mover("ARRIBA,ABAJO,IZQUIERDA,DERECHA,ABAJO");
        }

        //Metodo para empezar un nuevo juego
        public void NuevoJuego(int nivel)
        {
            juego = new();
            juego.Vidas = 2;
            juego.Puntos = 0;
            juego.Movimientos = 5;

            if (nivel == 1)
            {
                juego.Posicion = new char[2];
                juego.Posicion[0] = '1';
                juego.Posicion[1] = 'C';
            }
        }



        //Metodo para mover el personaje a la zona de meta o a otra posicion, obteien como parametro 
        //La serie de movimientos
        public async void Mover(string movimientos)
        {
            //Validamos los movimientos
            if (ValidarMovimientos(movimientos))
            {
                //Separamos las instrucciones
                var instrucciones = movimientos.Split(',');


                //Obtenemos la pieza actual
                for (int i = 0; i < instrucciones.Length; i++)
                {
                    //Aqui vemos si es izquierda o derecha y separamos todo
                    if (instrucciones[i] == "IZQUIERDA" && juego.Posicion[1]!='A' || instrucciones[i] == "DERECHA" && juego.Posicion[1] != 'G')
                    {
                        if (instrucciones[i] == "IZQUIERDA")
                        {
                            juego.Posicion[1] = (char)(juego.Posicion[1] + 1);
                            juego.Movimientos -= 1;
                        }
                        else
                        {
                            juego.Posicion[1] = (char)(juego.Posicion[1] - 1);
                            juego.Movimientos -= 1;
                        }
                    }
                    else
                    {
                        //Aqui vemos si es arriba o abajo y separamos todo
                        if (instrucciones[i] == "ARRIBA" && juego.Posicion[0] != '0' || instrucciones[i] == "ABAJO" && juego.Posicion[0] != '7')
                        {
                            if (instrucciones[i] == "ARRIBA")
                            {
                                juego.Posicion[0] = (char)(juego.Posicion[0] + 1);
                                juego.Movimientos -= 1;

                            }
                            else
                            {
                                juego.Posicion[0] = (char)(juego.Posicion[0] - 1);
                                juego.Movimientos -= 1;
                            }
                        }
                            
                    }


                    //Si ya no quedan movimientos game over si no llegaste o si estas en la meta win
                    //VerificarMovimientos(juego.Movimientos);

                    Actualizar();
                    //El programa se detiene 3 segundos para avanzar a la siguiente posicion
                    await Task.Delay(3000);
                    
                }

                
                VerificarIntento();
                Actualizar();
            }
        }

        private void VerificarIntento()
        {
            if (juego.Vidas >0)
            {
                if (juego.Posicion[0] == '2' && juego.Posicion[1] == 'D')
                {
                    Resultado = "¡Felicidades, superaste el primer nivel!";
                }

                else
                {
                    Resultado = "¡sigamos buscando a la vaquita!";
                    juego.Vidas -= 1;
                }

            }
            else
            {
                throw new ArgumentException("Perdiste, Fin del Juego");
            }

        }

        private void VerificarMovimientos(int movimientos)
        {
            if(movimientos == 0)
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
                    if (instrucciones[x] == "IZQUIERDA" || instrucciones[x] == "DERECHA"
                        || instrucciones[x] == "ARRIBA" || instrucciones[x] == "ABAJO")
                        continue;
                    else
                        return false;
                    }

                return true;    
            }
        }

        
        public void Actualizar()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
