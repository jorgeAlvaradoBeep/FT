using Facturacion_Tostatronic.Services;
using Facturacion_Tostatronic.Views.WareHouseViews;
using System;
using System.Collections.Generic;
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
using Telerik.Windows.Controls;

namespace Facturacion_Tostatronic
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            string prePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string path = System.IO.Path.Combine(prePath, "Tostatronic");
            StyleManager.ApplicationTheme = new Windows11Theme();
            InitializeComponent();
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }
    }
}
