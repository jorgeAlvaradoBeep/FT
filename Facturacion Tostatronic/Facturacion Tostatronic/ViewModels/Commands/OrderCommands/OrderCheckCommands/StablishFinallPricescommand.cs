using Facturacion_Tostatronic.Models.Products;
using Facturacion_Tostatronic.Models;
using Facturacion_Tostatronic.Services;
using Facturacion_Tostatronic.ViewModels.Orders;
using Facturacion_Tostatronic.Views.OrdersV;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace Facturacion_Tostatronic.ViewModels.Commands.OrderCommands.OrderCheckCommands
{
    public class StablishFinallPricescommand : ICommand
    {
        public OrderCheckVM VM { get; set; }
        public event EventHandler CanExecuteChanged;

        public StablishFinallPricescommand(OrderCheckVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            //Extraemos la informacion de precios seleccionados.

            EstablecerPreciosV eP = new EstablecerPreciosV(VM.ComlpleteOrder);
            eP.ShowDialog();

        }
    }
}
