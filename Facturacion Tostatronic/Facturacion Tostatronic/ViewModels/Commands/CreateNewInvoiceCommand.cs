using Facturacion_Tostatronic.Services;
using Facturacion_Tostatronic.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Facturacion_Tostatronic.ViewModels.Commands
{
    public class CreateNewInvoiceCommand : ICommand
    {
        public CreateInvoiceVM VM { get; set; }
        public event EventHandler CanExecuteChanged;
        public CreateNewInvoiceCommand(CreateInvoiceVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {

            var values = (object[])parameter;
            bool aux = true;
            for (int i = 0; i < 3; i++)
            {
                aux &= !(bool)values[i];
            }
            //CreateInvoiceV vAux = (CreateInvoiceV)values[3];
            
            if(!aux)
            {
                MessageBox.Show("Tiene Errores En el Formulario.");
                return;
            }
            else if (string.IsNullOrEmpty(VM.CompleteSale.InvoiceData.UsoCFDI))
            {
                MessageBox.Show("Debe seleccionar un uso de CFDI");
                return;
            }else if(string.IsNullOrEmpty(VM.CompleteSale.InvoiceData.MetodoDePago))
            {
                MessageBox.Show("Debe seleccionar un metodo de pago");
                return;
            }
            else if (string.IsNullOrEmpty(VM.CompleteSale.InvoiceData.FormaDePago))
            {
                MessageBox.Show("Debe seleccionar una forma de pago");
                return;
            }
            else if(string.IsNullOrEmpty(VM.CompleteSale.Client.CompleteName))
            {
                MessageBox.Show("La razon social no puede esta vacia.");
                return;

            }
            else if (string.IsNullOrEmpty(VM.CompleteSale.Client.Rfc))
            {
                MessageBox.Show("El RFC no puede estar vacio.");
                return;

            }
            else if (string.IsNullOrEmpty(VM.CompleteSale.Client.Email))
            {
                MessageBox.Show("El correo electronico es necesario.");
                return;

            }
            else if (string.IsNullOrEmpty(VM.CompleteSale.Client.CP))
            {
                MessageBox.Show("El CP es necesario.");
                return;

            }
            else if (string.IsNullOrEmpty(VM.CompleteSale.InvoiceData.RegimenFiscal))
            {
                MessageBox.Show("El Regimen Fiscal es necesario.");
                return;

            }
            if (await VM.CreateAndInsertInvoice())
            {
                MessageBox.Show("Factura creada con existo", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            VM.CompleteSale.Client = new Models.Client();
            VM.CompleteSale.Products = new List<Models.Product>();
            VM.SelectedCFDIUse = null;
            VM.SelectedPaymentMethod= null;
            VM.SelectedRegimen=null;
            VM.SelectedPaymentForm = null;
            VM.DataEntranceSavailable = false;
            VM.SelectedDateChangedCommand.Equals(null);//Validar que funcione correctamente. 

        }
    }
}
