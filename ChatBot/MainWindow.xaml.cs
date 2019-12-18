using ChatBot.CustomWindows;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Linq;

namespace ChatBot
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<Mensaje> Mensajes { get; set; }

        // Para comprobar si me ha respondido el bot
        private bool RespuestaRecibida { get; set; }
        private ClienteQnA QnA { get; set; }

        // El mensaje del TextBox...
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
            SaveFileDialog dialogo = new SaveFileDialog
            {
                Filter = "Archivos de texto|.txt"
            };
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
                // Al terminar el diálogo, cambiar la configuración y guardarla
                Properties.Settings.Default.ColorUsuario = dialog.ColorUsuario;
                Properties.Settings.Default.ColorFondo = dialog.ColorFondo;
                Properties.Settings.Default.ColorBot = dialog.ColorRobot;
                Properties.Settings.Default.Save();
            }
        }

        private async void CheckConnection_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                // Preguntar algo, si me responde hay conexión, si no se lanza excepción
                Task<string> t = QnA.PreguntarAsync("Hola");
                await t;
                if (t.IsCompleted)
                {
                    MessageBox.Show("Conexión correcta", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private string ObtenerConversacion()
        {
            // Construir un texto a partir de todos los mensajes
            StringBuilder builder = new StringBuilder();
            foreach (Mensaje mensaje in Mensajes)
            {
                builder.AppendLine(mensaje.Emisor + ": " + mensaje.Texto);
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
            // Si no he recibido respuesta o si el mensaje del TextBox es vacío
            e.CanExecute = RespuestaRecibida && !string.IsNullOrEmpty(Mensaje);
        }

        private async Task ObtenerRespuestaBot()
        {
            string ultimoMensaje = Mensajes.Last().Texto;
            Mensaje mensajeBot = new Mensaje("Robot", "Procesando...");
            // Cada vez que el bot responda algo, hacer scroll hasta el final
            MainScrollViewer.ScrollToEnd();
            Mensajes.Add(mensajeBot);
            try
            {
                mensajeBot.Texto = await QnA.PreguntarAsync(ultimoMensaje);
                RespuestaRecibida = true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}