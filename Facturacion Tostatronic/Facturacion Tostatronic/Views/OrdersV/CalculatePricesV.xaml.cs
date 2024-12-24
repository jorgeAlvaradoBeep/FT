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
    /// Lógica de interacción para CalculatePricesV.xaml
    /// </summary>
    public partial class CalculatePricesV : Window
    {
        public CalculatePricesVM VM { get; set; }
        public CalculatePricesV()
        {
            InitializeComponent();
            VM = new CalculatePricesVM();
            this.DataContext = VM;
        }

        private void radGv_CellEditEnded(object sender, Telerik.Windows.Controls.GridViewCellEditEndedEventArgs e)
        {
            if(e.Cell.Column.UniqueName == "TargetPrice")
            {
                VM.UpdateTPCommand.Execute(VM.SelectedItem);
            }
        }
    }
}
