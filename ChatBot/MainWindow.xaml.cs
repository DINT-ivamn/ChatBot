using ChatBot.CustomWindows;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
        private bool RespuestaRecibida { get; set; }
        private ClienteQnA QnA { get; }

        public MainWindow()
        {
            Mensajes = new ObservableCollection<Mensaje>();
            RespuestaRecibida = true;
            QnA = new ClienteQnA();
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
                Properties.Settings.Default.ColorUsuario = dialog.ColorUsuario.Nombre;
                Properties.Settings.Default.ColorFondo = dialog.ColorFondo.Nombre;
                Properties.Settings.Default.ColorBot = dialog.ColorRobot.Nombre;
                Properties.Settings.Default.Save();
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

        private async void SendMessage_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Mensajes.Add(new Mensaje("Usuario", MensajeTextBox.Text, false));
            RespuestaRecibida = false;
            MensajeTextBox.Text = "";
            await ObtenerRespuestaBot();
        }

        private void SendMessage_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = RespuestaRecibida;
        }

        private async Task ObtenerRespuestaBot()
        {
            Mensaje mensajeBot = new Mensaje("Robot", "Procesando...", true);
            Mensajes.Add(mensajeBot);
            try
            {
                mensajeBot.Texto = await QnA.PreguntarAsync(Mensajes.Last().Texto);
                // ????
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
