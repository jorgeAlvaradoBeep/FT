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
using Facturacion_Tostatronic.Views.OrdersV;

namespace Facturacion_Tostatronic.ViewModels.Commands.OrderCommands.OrderCheckCommands
{
    public class ImportExcelFileCommand : ICommand
    {
        public OrderCheckVM VM { get; set; }
        public event EventHandler CanExecuteChanged;

        public ImportExcelFileCommand(OrderCheckVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            ImportFromExcelV iE = new ImportFromExcelV();
            iE.ShowDialog();
        }
    }
}
