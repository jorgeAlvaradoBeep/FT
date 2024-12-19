using Facturacion_Tostatronic.Models;
using Facturacion_Tostatronic.Models.EF_Models.EF_Orders;
using Facturacion_Tostatronic.Services;
using Facturacion_Tostatronic.ViewModels.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Facturacion_Tostatronic.ViewModels.Commands.OrderCommands.CheckPricesCommands
{
    public class SaverTargetPricesOrderCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public CalculatePricesVM VM { get; set; }
        public SaverTargetPricesOrderCommand(CalculatePricesVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            VM.GettingData = true;
            //Convertimos la Orden para que cuadre con el objeto de TargetPrice y no enviar datos innecesarios
            List<EFOrderProductsTargetPrice> targetPrices = new List<EFOrderProductsTargetPrice>();
            await Task.Run(() =>
            {
                foreach (var product in VM.OrdenComplete.ProductosDeOrdenesNavigation)
                {
                    targetPrices.Add(new EFOrderProductsTargetPrice(VM.OrdenComplete.OrdenID, product.CodigoProducto, product.TargetPrice));
                }
            });
            
            //Guardamos los precios en la base de datos
            Response res = await WebService.ModifyData(targetPrices,URLData.OrderProductsTargetPriceEditList);
            if(!res.succes)
            {
                //Si no se guardaron los precios mostramos un mensaje de error
                MessageBox.Show("No se pudieron guardar los precios, intente de nuevo", "Ok", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                MessageBox.Show("Precios guardados correctamente", "Ok", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            VM.GettingData= false;
        }
    }
}
