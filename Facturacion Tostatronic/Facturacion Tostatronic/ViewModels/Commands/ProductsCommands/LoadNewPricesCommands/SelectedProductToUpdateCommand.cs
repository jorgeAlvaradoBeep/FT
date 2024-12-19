using Facturacion_Tostatronic.Models;
using Facturacion_Tostatronic.Models.EF_Models.EF_Orders;
using Facturacion_Tostatronic.Models.Products;
using Facturacion_Tostatronic.Services;
using Facturacion_Tostatronic.ViewModels.Products;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Facturacion_Tostatronic.ViewModels.Commands.ProductsCommands.LoadNewPricesCommands
{
    public class SelectedProductToUpdateCommand : ICommand
    {
        public LoadNewPricesVM VM { get; set; }

        public SelectedProductToUpdateCommand(LoadNewPricesVM vm)
        {
            VM = vm;
        }
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            ProductOrderComplete p = (ProductOrderComplete)parameter;
            if (p == null)
                return;
            IReadOnlyList<UpdateProductM> updateProductMs;
            VM.IsBusy = true;
            Response rmp = await WebService.GetDataForInvoice(URLData.getProductsNet);
            if (rmp.succes)
            {
                updateProductMs = await Task.Run(() => JsonConvert.DeserializeObject<IReadOnlyList<UpdateProductM>>(rmp.data.ToString()));
            }
            else
            {
                MessageBox.Show("Error al traer la información solicitada", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                VM.IsBusy = false;
                return;
            }
            ProductComplete p2 =  new ProductComplete();
            p2.Code = p.CodigoProducto;
            p2.Name = p.NombreEs;
            VM.CurrentPage = new UpdateProductNOVM(p);
            VM.IsBusy = false;
        }
    }
}
