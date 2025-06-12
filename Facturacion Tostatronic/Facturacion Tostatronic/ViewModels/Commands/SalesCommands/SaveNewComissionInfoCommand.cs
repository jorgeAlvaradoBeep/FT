using Facturacion_Tostatronic.Models;
using Facturacion_Tostatronic.Models.EF_Models.EFSale;
using Facturacion_Tostatronic.Services;
using Facturacion_Tostatronic.ViewModels.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Telerik.Windows.Controls.GridView;

namespace Facturacion_Tostatronic.ViewModels.Commands.SalesCommands
{
    public class SaveNewComissionInfoCommand : ICommand
    {
        public EarningsVM VM { get; set; }
        public event EventHandler CanExecuteChanged;
        public SaveNewComissionInfoCommand(EarningsVM vm)
        {
            VM = vm;
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            if(parameter == null)
                return;
            var row = parameter as GridViewRow;
            EarningSale sale = row.DataContext as EarningSale;
            if(sale == null)
                return; 
            VM.GettinData = true;
            Response res = await WebService.InsertData(sale.Comisiones, URLData.Comisiones);
            if (!res.succes)
            {
                MessageBox.Show($"Error al guardar la comision{Environment.NewLine}" +
                    $"{res.message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            MessageBox.Show($"Comision guardada exitosamente", "Exito", MessageBoxButton.OK, MessageBoxImage.Information);
            VM.GettinData = false;
        }
    }
}
