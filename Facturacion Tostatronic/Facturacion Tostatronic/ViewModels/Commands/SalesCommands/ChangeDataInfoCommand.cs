using Facturacion_Tostatronic.Models.EF_Models.EFProduct;
using Facturacion_Tostatronic.Models.EF_Models.EFSale;
using Facturacion_Tostatronic.Models;
using Facturacion_Tostatronic.Services;
using Facturacion_Tostatronic.ViewModels.Sales;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace Facturacion_Tostatronic.ViewModels.Commands.SalesCommands
{
    public class ChangeDataInfoCommand : ICommand
    {
        public EarningsVM VM { get; set; }
        public event EventHandler CanExecuteChanged;
        public ChangeDataInfoCommand(EarningsVM vm)
        {
            VM = vm;
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            VM.GettinData = true;
            await Task.Run(() => GetTotalEarnings());
            VM.GettinData = false;
        }

        void GetTotalEarnings()
        {
            float total = 0;
            foreach (EarningSale s in VM.Sales)
            {
                s.getTotal();
                total += s.Ganancia;
            }
            VM.TotalEarnings = total;
        }
    }
}
