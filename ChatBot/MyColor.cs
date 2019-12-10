using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ChatBot
{
    class MyColor
    {
        public Color Color { get; set; }
        public string Nombre { get; set; }

        public MyColor(Color color, string nombre)
        {
            Color = color;
            Nombre = nombre;
        }
    }
}
