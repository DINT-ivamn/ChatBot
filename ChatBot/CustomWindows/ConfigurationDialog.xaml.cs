using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ChatBot.CustomWindows
{
    /// <summary>
    /// Lógica de interacción para ConfigurationDialog.xaml
    /// </summary>
    public partial class ConfigurationDialog : Window
    {
        public ObservableCollection<string> Colores { get; set; }
        public string ColorUsuario { get; set; }
        public string ColorFondo { get; set; }
        public string ColorRobot { get; set; }

        public ConfigurationDialog()
        {
            LoadColors();
            ColorUsuario = FindColor(Properties.Settings.Default.ColorUsuario);
            ColorFondo = FindColor(Properties.Settings.Default.ColorFondo);
            ColorRobot = FindColor(Properties.Settings.Default.ColorBot);
            InitializeComponent();

        }

        private void AceptarButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void LoadColors()
        {
            Colores = new ObservableCollection<string>(typeof(Colors).GetProperties().Select(s => s.Name));
            
        }

        private string FindColor(string nombre)
        {
            return Colores.ToList().Find(c => c == nombre);
        }
    }
}
