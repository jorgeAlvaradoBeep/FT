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
    /// Lógica de interacción para FraccionArancelariaV.xaml
    /// </summary>
    public partial class FraccionArancelariaV : UserControl
    {
        public FraccionesArancelariasVM VM { get; set; }
        public FraccionArancelariaV()
        {
            InitializeComponent();
            VM = new FraccionesArancelariasVM();
            DataContext = VM;
        }

        private async void OrdenesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(VM.SelectedItem != null)
            {
                await VM.GetOrderData();
            }
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            await VM.GetOrders();
        }

        private void ProductosGrid_RowEditEnded(object sender, Telerik.Windows.Controls.GridViewRowEditEndedEventArgs e)
        {
            if (VM.SelectedFraccion != null)
            {
                if (!VM.SelectedFraccion.Nuevo)
                    VM.SelectedFraccion.Modificado = true;
            }
        }
    }
}
