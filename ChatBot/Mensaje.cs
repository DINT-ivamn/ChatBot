using System.ComponentModel;

namespace ChatBot
{
    public class Mensaje :INotifyPropertyChanged
    {
        private string emisor;
        public string Emisor
        {
            get
            {
                return emisor;
            }
            set
            {
                emisor = value;
                NotifyPropertyChanged("Emisor");
            }
        }
        private string texto;
        public string Texto
        {
            get
            {
                return texto;
            }
            set
            {
                texto = value;
                NotifyPropertyChanged("Texto");
            }
        }

        public Mensaje(string emisor, string texto)
        {
            Emisor = emisor;
            Texto = texto;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}