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
        public ObservableCollection<MyColor> Colores { get; set; }
        public MyColor ColorUsuario { get; set; }
        public MyColor ColorFondo { get; set; }
        public MyColor ColorRobot { get; set; }

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
            Colores = new ObservableCollection<MyColor>();
            PropertyInfo[] properties = typeof(Colors).GetProperties();
            foreach (PropertyInfo item in properties)
            {
                Colores.Add(new MyColor(item.Name));
            }
        }

        private MyColor FindColor(string nombre)
        {
            return Colores.ToList().Find(c => c.Nombre == nombre);
        }
    }
}
