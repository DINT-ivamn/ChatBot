using ChatBot.CustomWindows;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChatBot
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<Mensaje> Mensajes { get; set; }

        public MainWindow()
        {
            Mensajes = new ObservableCollection<Mensaje>();
            InitializeComponent();
        }

        private void NewCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Mensajes.Clear();
        }

        private void NewCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = Mensajes.Count > 0;
        }

        private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SaveFileDialog dialogo = new SaveFileDialog();
            dialogo.Filter = "Archivos de texto|.txt";
            if (dialogo.ShowDialog() == true)
            {
                string textoMensajes = ObtenerConversacion();
                File.WriteAllText(dialogo.FileName, textoMensajes);
            }
        }

        private void SaveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = Mensajes.Count > 0;
        }

        private void ExitCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }

        private void Configure_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ConfigurationDialog dialog = new ConfigurationDialog();
            if (dialog.ShowDialog() == true)
            {
                // Cambiar los colores por los colores de la configuración
            }
        }

        private void CheckConnection_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            
        }

        private string ObtenerConversacion()
        {
            StringBuilder builder = new StringBuilder();
            foreach (Mensaje mensaje in Mensajes)
            {
                builder.AppendLine(mensaje.Emisor + ":" + mensaje.Texto);
            }
            return builder.ToString();
        }
    }
}
