using Facturacion_Tostatronic.ViewModels.Orders;
using GalaSoft.MvvmLight.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Facturacion_Tostatronic.ViewModels.Commands.OrderCommands
{
    public class AddNewRowCommand : ICommand
    {
        public MakeOrderVM VM { get; set; }
        public event EventHandler CanExecuteChanged;

        public AddNewRowCommand(MakeOrderVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            VM.Order.Products.Add(new Models.EF_Models.EF_Orders.EFOrderProduct(false));
        }
    }
}
