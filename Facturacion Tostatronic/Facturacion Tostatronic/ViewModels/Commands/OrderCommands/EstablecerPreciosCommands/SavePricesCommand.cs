using Facturacion_Tostatronic.Models;
using Facturacion_Tostatronic.Models.EF_Models.EF_Orders;
using Facturacion_Tostatronic.Services;
using Facturacion_Tostatronic.ViewModels.Orders;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

namespace Facturacion_Tostatronic.ViewModels.Commands.OrderCommands.EstablecerPreciosCommands
{
    public class SavePricesCommand : ICommand
    {
        public EstablecerPreciosVM VM { get; set; }
        public event EventHandler CanExecuteChanged;

        public SavePricesCommand(EstablecerPreciosVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            var productos = VM.DetallesOrden.Cast<DetalleOrdenEF>().ToList();
            if(productos== null || productos.Count == 0)
            {
                MessageBox.Show("No hay productos para guardar", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Response response = await WebService.ModifyData(productos, URLData.UpdateProductsEstablecerOrden);
            if (response != null)
            {
                if(!response.succes)
                {
                    MessageBox.Show($"Error al guardar los precios: {response.message}","Error",MessageBoxButton.OK,MessageBoxImage.Error); ;
                    return;
                }
                MessageBox.Show("Precios guardados correctamente", "Guardado", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
