using Facturacion_Tostatronic.Models.EF_Models.EF_Orders;
using Facturacion_Tostatronic.Models;
using Facturacion_Tostatronic.Services;
using Facturacion_Tostatronic.ViewModels.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace Facturacion_Tostatronic.ViewModels.Commands.OrderCommands.CheckPricesCommands
{
    public class StablishOrderCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public CalculatePricesVM VM { get; set; }
        public StablishOrderCommand(CalculatePricesVM vm)
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
            List<DetalleOrdenEF> targetPrices = new List<DetalleOrdenEF>();
            await Task.Run(() =>
            {
                foreach (var product in VM.OrdenComplete.ProductosDeOrdenesNavigation)
                {
                    product.IDOrden = VM.OrdenComplete.OrdenID;
                    targetPrices.Add(DetalleOrdenEF.ToDetalleOrden(product));
                }
            });
            //Eliminamos cualquier producto que no tenga un precio establecido
            Response res = await WebService.GetDataNode(URLData.DeleteEstablecerOrden,VM.OrdenComplete.OrdenID.ToString());
            if (!res.succes)
            {
                if(!res.message.Equals("0 Producos a eliminar"))
                    MessageBox.Show($"Error: {res.message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            //Guardamos los precios en la base de datos
            res = await WebService.InsertData(targetPrices, URLData.EstablecerOrden);
            if (!res.succes)
            {
                //Si no se guardaron los precios mostramos un mensaje de error
                MessageBox.Show($"No se pudieron guardar los precios, intente de nuevo" +
                    $"{Environment.NewLine}Razon: {res.message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                MessageBox.Show("Orden establecida, ya pude ir a asignar los precios", "Ok", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            VM.GettingData = false;
        }
    }
}
