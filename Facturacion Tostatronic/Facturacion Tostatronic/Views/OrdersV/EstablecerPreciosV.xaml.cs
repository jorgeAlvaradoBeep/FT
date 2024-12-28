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
using System.Windows.Shapes;

namespace Facturacion_Tostatronic.Views.OrdersV
{
    /// <summary>
    /// Lógica de interacción para EstablecerPreciosV.xaml
    /// </summary>
    public partial class EstablecerPreciosV : Window
    {
        public EstablecerPreciosVM VM { get; set; }
        public EstablecerPreciosV(OrderComplete orderComplete)
        {
            InitializeComponent();
            VM = new EstablecerPreciosVM(orderComplete);
            this.DataContext = VM;
        }
    }
}
