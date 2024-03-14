using Facturacion_Tostatronic.Models.EF_Models.EF_Orders;
using Facturacion_Tostatronic.Models;
using Facturacion_Tostatronic.Services;
using Facturacion_Tostatronic.ViewModels.Orders;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace Facturacion_Tostatronic.ViewModels.Commands.OrderCommands.OrderCheckCommands
{
    public class SaveOrderInfoCommand : ICommand
    {
        public OrderCheckVM VM { get; set; }
        public event EventHandler CanExecuteChanged;

        public SaveOrderInfoCommand(OrderCheckVM vm)
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
            if(VM.ComlpleteOrder.TipoCambio== 0 || VM.ComlpleteOrder.CostoEnvio == 0 || VM.ComlpleteOrder.CostoAA == 0
                || VM.ComlpleteOrder.PorcentajeGanancia == 0)
            {
                MessageBox.Show("Debe de llenar todos los campos de calculo de costos " +
                    "requeridos en los campos de texto.","Error de llenado", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            APIOrder updateOrder = new APIOrder();
            updateOrder.OrdenID = VM.ComlpleteOrder.OrdenID;
            updateOrder.FechaCreacion = VM.ComlpleteOrder.FechaCreacion;
            updateOrder.TipoCambio = VM.ComlpleteOrder.TipoCambio;
            updateOrder.CostoEnvio = VM.ComlpleteOrder.CostoEnvio;
            updateOrder.CostoAA = VM.ComlpleteOrder.CostoAA;
            updateOrder.PorcentajeGanancia = VM.ComlpleteOrder.PorcentajeGanancia;
            updateOrder.SubTotal = VM.ComlpleteOrder.SubTotal;

            Response res = await WebService.InsertData(updateOrder,URLData.Orders);
            if (!res.succes)
            {
                MessageBox.Show(res.message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                VM.GettingData = false;
                return;
            }
            MessageBox.Show($"La Orden {VM.ComlpleteOrder.OrdenID} se actualizo corectamente."
                ,"Exito al actualizar", MessageBoxButton.OK, MessageBoxImage.Information);

            VM.GettingData = false;
        }
    }
}
