using System;
using Microsoft.Win32;
using Facturacion_Tostatronic.ViewModels.Orders;

namespace Facturacion_Tostatronic.ViewModels.Commands.OrderCommands.ImportFromExcelCommands
{
    public class LoadNewOrderInfoFromExcel
    {
        public event EventHandler CanExecuteChanged;
        public ImportFromExcelVM VM { get; set; }
        public LoadNewOrderInfoFromExcel(ImportFromExcelVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var dialog = new OpenFileDialog
            {
                Filter = "Archivos Excel|*.xlsx;*.xls",
                Title = "Seleccionar archivo Excel"
            };

            if (dialog.ShowDialog() == true)
            {
                VM.ImagePath = dialog.FileName;
            }
        }
    }
}
