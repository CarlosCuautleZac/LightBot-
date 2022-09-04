using LightBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightBot.ViewModels
{
    internal class LightBotViewModel
    {

        Juego juego;

        public LightBotViewModel()
        {
            juego = new();
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
                    if (instrucciones[i] == "IZQUIERDA" || instrucciones[i] == "DERECHA")
                    {
                        if (instrucciones[i] == "IZQUIERDA")
                            juego.Posicion[1] = (char)(juego.Posicion[1] + 1);
                        else
                            juego.Posicion[1] = (char)(juego.Posicion[1] - 1);
                    }

                    else
                    {
                        if (instrucciones[i] == "ARRIBA")
                            juego.Posicion[1] = (char)(juego.Posicion[0] + 1);
                        else
                            juego.Posicion[1] = (char)(juego.Posicion[0] - 1);
                    }

                    //El programa se detiene 3 segundos para avanzar a la siguiente posicion
                    await Task.Delay(3000);
                }
            }
        }

        private bool ValidarMovimientos(string movimientos)
        {
            return true;
        }
    }
}
