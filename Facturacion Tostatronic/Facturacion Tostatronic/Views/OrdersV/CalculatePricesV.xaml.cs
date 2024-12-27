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
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await VM.SetOrder();
        }

        private void radGv_CellEditEnded(object sender, Telerik.Windows.Controls.GridViewCellEditEndedEventArgs e)
        {
            if(string.IsNullOrEmpty(e.NewData.ToString()))
            {
                return;
            }
            if (e.Cell.Column.UniqueName == "TargetPrice")
            {
                VM.UpdateTPCommand.Execute(VM.SelectedItem);
            }
            else if (e.Cell.Column.UniqueName == "Porcentaje")
            {
                decimal.TryParse(e.NewData.ToString(), out decimal porcentaje);
                decimal.TryParse(e.OldData.ToString(), out decimal oldProcentaje);
                decimal dif = oldProcentaje - porcentaje;
                if(dif==0)
                    return;
                decimal newCostoEnvio = VM.OrdenComplete.GastosEA * porcentaje;
                VM.SelectedItem.CostoEnvio = newCostoEnvio;
                newCostoEnvio = newCostoEnvio/VM.SelectedItem.Cantidad;
                VM.SelectedItem.CostoEnvioPP = newCostoEnvio;
                VM.SelectedItem.CostoAI = VM.SelectedItem.PrecioMXN + VM.SelectedItem.CostoEnvioPP;
                VM.SelectedItem.Costo=VM.SelectedItem.CostoAI * 1.16m;
                decimal porWininw = (decimal)VM.OrdenComplete.PorcentajeGanancia / 100m;
                VM.SelectedItem.MinimoRecomendado = (VM.SelectedItem.Costo / (1 - porWininw));
                VM.TotalPorcentaje = VM.TotalPorcentaje - dif;
            }
        }
    }
}
