using Google.Apis.Gmail.v1.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Facturacion_Tostatronic.ViewModels.Commands.SeeInvoiceCommands
{
    public class PrintInvoiceCommand : ICommand
    {
        public SeeInvoiceVM VM { get; set; }
        public event EventHandler CanExecuteChanged;
        public PrintInvoiceCommand(SeeInvoiceVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (VM.PrintInvoice())
                MessageBox.Show("Impreso Correctamente", "Impresion Correcta", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
