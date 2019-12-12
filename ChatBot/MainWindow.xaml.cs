using ChatBot.CustomWindows;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ChatBot
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<Mensaje> Mensajes { get; set; }
        private bool RespuestaRecibida { get; set; }
        private ClienteQnA QnA { get; set; }
        public string Mensaje { get; set; }

        public MainWindow()
        {
            Mensajes = new ObservableCollection<Mensaje>();
            RespuestaRecibida = true;
            try
            {
                QnA = new ClienteQnA();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
            ConfigurationDialog dialog = new ConfigurationDialog
            {
                Owner = this
            };
            if (dialog.ShowDialog() == true)
            {
                Properties.Settings.Default.ColorUsuario = dialog.ColorUsuario;
                Properties.Settings.Default.ColorFondo = dialog.ColorFondo;
                Properties.Settings.Default.ColorBot = dialog.ColorRobot;
                Properties.Settings.Default.Save();
            }
        }

        private void CheckConnection_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                // Preguntar al bot y comprobar que recibimos un mensaje
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
            Mensajes.Add(new Mensaje("Usuario", MensajeTextBox.Text));
            RespuestaRecibida = false;
            MensajeTextBox.Text = "";
            await ObtenerRespuestaBot();
        }

        private void SendMessage_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = RespuestaRecibida && !string.IsNullOrEmpty(Mensaje);
        }

        private async Task ObtenerRespuestaBot()
        {
            Mensaje mensajeBot = new Mensaje("Robot", "Procesando...");
            Mensajes.Add(mensajeBot);
            try
            {
                Task<string> t = QnA.PreguntarAsync(Mensajes.Last().Texto);
                await t;
                mensajeBot.Texto = t.Result;
                RespuestaRecibida = t.IsCompleted;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}