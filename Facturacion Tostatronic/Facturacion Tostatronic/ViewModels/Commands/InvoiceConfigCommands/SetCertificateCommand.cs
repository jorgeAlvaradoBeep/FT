using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Facturacion_Tostatronic.ViewModels.Commands.InvoiceConfigCommands
{
    public class SetCertificateCommand : ICommand
    {
        public InvoiceConfigVM VM { get; set; }
        public event EventHandler CanExecuteChanged;

        public SetCertificateCommand(InvoiceConfigVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (VM.SetCertificate())
                MessageBox.Show("Certificado Copiado con exito", "Existo!", MessageBoxButton.OK, MessageBoxImage.Information);
            else
                MessageBox.Show("Error al copiar, intentelo mas tarde", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
