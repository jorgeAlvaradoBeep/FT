using Facturacion_Tostatronic.Models.EF_Models.EF_Orders;
using Facturacion_Tostatronic.ViewModels.Orders;
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

namespace Facturacion_Tostatronic.Views.Pages.Orders
{
    /// <summary>
    /// Lógica de interacción para EstablecerPreciosUC.xaml
    /// </summary>
    public partial class EstablecerPreciosUC : UserControl
    {
        public EstablecerPreciosVM VM { get; set; }
        public EstablecerPreciosUC()
        {
            InitializeComponent();
            VM = new EstablecerPreciosVM();
            this.DataContext = VM;
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            await VM.GetOrders();
        }

        private async void cbOrdenes_Selected(object sender, RoutedEventArgs e)
        {

        }
    }
}
