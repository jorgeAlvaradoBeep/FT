using Facturacion_Tostatronic.Models.EF_Models.EF_Orders;
using Facturacion_Tostatronic.Models;
using Facturacion_Tostatronic.Services;
using Facturacion_Tostatronic.ViewModels.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Facturacion_Tostatronic.Views.OrdersV;
using Facturacion_Tostatronic.Models.Products;
using Newtonsoft.Json;

namespace Facturacion_Tostatronic.ViewModels.Commands.OrderCommands.OrderCheckCommands
{
    public class CalculatePirceCommand : ICommand
    {
        public OrderCheckVM VM { get; set; }
        public event EventHandler CanExecuteChanged;

        public CalculatePirceCommand(OrderCheckVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            if(VM.ComlpleteOrder.CostoAA == 0 || VM.ComlpleteOrder.CostoEnvio == 0
                || VM.ComlpleteOrder.PorcentajeGanancia == 0 || VM.ComlpleteOrder.TipoCambio == 0)
            {
                MessageBox.Show("Favor de llenar todos los campos para poder calcular los precios");
                return;
            }
            Application.Current.Properties["CalculatePrices"] = VM.ComlpleteOrder;
            await GtPorductsInfo();
            CalculatePricesV cP = new CalculatePricesV();
            cP.ShowDialog();

        }
        async Task GtPorductsInfo()
        {
            VM.GettingData = true;
            Response rmp = await WebService.GetDataForInvoice(URLData.getProductsNet);
            if (rmp.succes)
            {
                IReadOnlyList< UpdateProductM> _allProducts = JsonConvert.DeserializeObject<IReadOnlyList<UpdateProductM>>(rmp.data.ToString());
                Application.Current.Properties["AllProducts"] = _allProducts;
            }
            else
                MessageBox.Show("Error al traer la información solicitada", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            VM.GettingData = false;
        }
    }
}
