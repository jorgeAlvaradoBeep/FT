using Facturacion_Tostatronic.Models.EF_Models.EF_Orders;
using Facturacion_Tostatronic.ViewModels.Orders;
using GalaSoft.MvvmLight.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using Facturacion_Tostatronic.Models.Clients;

namespace Facturacion_Tostatronic.ViewModels.Commands.OrderCommands.ImportFromExcelCommands
{
    public class UpdateOrderFromExcelCommand : ICommand
    {
        public ImportFromExcelVM VM { get; set; }
        public event EventHandler CanExecuteChanged;

        public UpdateOrderFromExcelCommand(ImportFromExcelVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            Application.Current.Properties["NewOrderInfo"] = VM.Productos.ToList();
            MessageBox.Show("Información asignada con exito, cierre esta ventana", "Exito",MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
