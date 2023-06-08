using Facturacion_Tostatronic.Models;
using Facturacion_Tostatronic.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Facturacion_Tostatronic.ViewModels.Commands.SeeInvoiceCommands
{
    public class SaleSelectedSICommand : ICommand
    {
        public SeeInvoiceVM VM { get; set; }
        public event EventHandler CanExecuteChanged;
        public SaleSelectedSICommand(SeeInvoiceVM vm)
        {
            VM = vm;
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            EndSale s = (EndSale)parameter;
            VM.Rfc = s.rfc;
            VM.RazonSocial = s.razonSocial;
            VM.Folio = s.idSale.ToString();
            VM.Email = s.Email;
            VM.IsDataLoaded = false;
            VM.Sales = null;
            VM.GettingData= true;
            bool result = await VM.GenerateInvoice(s.XmlFull, s.idSale.ToString());
            VM.DataEntranceSavailable = true;
            VM.GettingData = false;
        }
    }
}
