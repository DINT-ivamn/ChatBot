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
        private ObservableCollection<MyColor> Colores { get; set; }

        public ConfigurationDialog()
        {
            InitializeComponent();
            LoadColors();
        }

        private void AceptarButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void LoadColors()
        {
            Colores = new ObservableCollection<MyColor>();
            PropertyInfo[] properties = typeof(Colors).GetProperties();
            foreach (PropertyInfo item in properties)
            {
                Colores.Add(new MyColor(item.Name));
            }
            FondoComboBox.DataContext = Colores;
        }
    }
}
