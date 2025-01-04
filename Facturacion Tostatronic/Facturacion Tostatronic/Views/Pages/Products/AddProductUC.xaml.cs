using Facturacion_Tostatronic.ViewModels.Products;
using System;
using System.Collections.Generic;
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

namespace Facturacion_Tostatronic.Views.Pages.Products
{
    /// <summary>
    /// Lógica de interacción para AddProductUC.xaml
    /// </summary>
    public partial class AddProductUC : UserControl
    {
        public AddProductVM VM { get; set; }
        public AddProductUC()
        {
            InitializeComponent();
            //VM = new AddProductVM();
            //DataContext = VM;
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //await VM.GetCodes();
        }
    }
}
