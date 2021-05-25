using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Facturacion_Tostatronic.ViewModels.Commands
{
    public class SelectedDateChangedCommand : ICommand
    {
        public CreateInvoiceVM VM { get; set; }

        public event EventHandler CanExecuteChanged;
        public SelectedDateChangedCommand(CreateInvoiceVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            VM.GetSalesDataByDate("sale_date", VM.SelectedDate.Date.ToString("yyyy-MM-dd"));
        }
    }
}
