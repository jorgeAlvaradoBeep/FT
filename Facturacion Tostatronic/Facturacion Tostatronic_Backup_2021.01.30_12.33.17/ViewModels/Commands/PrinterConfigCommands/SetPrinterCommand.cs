using Facturacion_Tostatronic.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace Facturacion_Tostatronic.ViewModels.Commands.PrinterConfigCommands
{
    public class SetPrinterCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public PrinterConfigVM VM { get; set; }
        public SetPrinterCommand(PrinterConfigVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            Printer printer = (Printer)parameter;
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            configuration.AppSettings.Settings["PrinterName"].Value = printer.PrinterName;
            ConfigurationManager.RefreshSection("appSettings");
            configuration.Save(ConfigurationSaveMode.Modified);
            MessageBox.Show($"Imprsora {ConfigurationManager.AppSettings.Get("PrinterName")} guardada correctamente", "Configuracion Guardada", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //VM.SetInvoiceConfiguration();
        }
    }
}
