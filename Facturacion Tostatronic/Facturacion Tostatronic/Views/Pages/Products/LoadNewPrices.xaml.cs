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
    /// Lógica de interacción para LoadNewPrices.xaml
    /// </summary>
    public partial class LoadNewPrices : UserControl
    {
        public LoadNewPricesVM VM { get; set; }
        public LoadNewPrices()
        {
            InitializeComponent();
            VM = new LoadNewPricesVM();
            this.DataContext = VM;
        }

        private async void cbOrdenes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            await VM.GetOrderComplete();
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            await VM.GetOrders();
        }
    }
}
