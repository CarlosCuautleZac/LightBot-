using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightBot.Models
{
    internal class Juego
    {
        public int Vidas { get; set; } = 0;

        public char[] Posicion { get; set; }

        public int Movimientos { get; set; }

        public double Puntos { get; set; } = 0;
    }
}
