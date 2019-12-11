using System.Collections.ObjectModel;

namespace ChatBot
{
    public class Mensaje
    {

        public string Emisor { get; set; }
        public string Texto { get; set; }
        public bool IsBot { get; set; }

        public Mensaje(string emisor, string texto, bool bot)
        {
            Emisor = emisor;
            Texto = texto;
            IsBot = bot;
        }

        // Delete this
        public static ObservableCollection<Mensaje> GenerarMensajes()
        {
            ObservableCollection<Mensaje> mensajes = new ObservableCollection<Mensaje>();
            mensajes.Add(new Mensaje("Usuario", "Mensaje de ejemplo", false));
            mensajes.Add(new Mensaje("Robot", "Mensaje de ejemplo", true));
            return mensajes;
        }
    }
}