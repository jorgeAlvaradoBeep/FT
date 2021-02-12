using Facturacion_Tostatronic.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Facturacion_Tostatronic.ViewModels.Commands.SeeInvoiceCommands
{
    public class CloseFormCommand : ICommand
    {
        public SeeInvoiceVM VM { get; set; }
        public event EventHandler CanExecuteChanged;
        public CloseFormCommand(SeeInvoiceVM vm)
        {
            VM = vm;
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            SeeInvoiceV v = (SeeInvoiceV)parameter;
            string prePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string path = Path.Combine(prePath, "Tostatronic");
            string pdfPath = @path + "\\" + VM.Folio + ".pdf";
            string xmlPath = @path + "\\" + VM.Folio + ".xml";
            if (File.Exists(pdfPath))
                File.Delete(pdfPath);
            if (File.Exists(xmlPath))
                File.Delete(xmlPath);
            v.Close();
        }
    }
}
