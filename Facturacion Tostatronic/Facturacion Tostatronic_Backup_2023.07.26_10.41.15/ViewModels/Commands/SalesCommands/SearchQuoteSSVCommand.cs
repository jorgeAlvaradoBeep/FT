using Facturacion_Tostatronic.Models.EF_Models.EFSale;
using Facturacion_Tostatronic.Models;
using Facturacion_Tostatronic.Services;
using Facturacion_Tostatronic.ViewModels.Sales;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace Facturacion_Tostatronic.ViewModels.Commands.SalesCommands
{
    public class SearchQuoteSSVCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public SeeQuatitionsVM VM { get; set; }
        public SearchQuoteSSVCommand(SeeQuatitionsVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            //if ((bool)parameter)
            //{
            //    MessageBox.Show("El folio que introddujo es incorrecto", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            //    return;
            //}
            int invoiceNumber;
            if (int.TryParse(VM.Folio, out invoiceNumber))
            {
                VM.GettinData = true;
                Response r = await WebService.GetDataNode(URLData.getSpecificQuoteNET, VM.Folio);
                if (!r.succes)
                {
                    if (!string.IsNullOrEmpty(r.message))
                        MessageBox.Show(r.message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    else
                        MessageBox.Show("Error al traer datos de ventas", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    VM.GettinData = false;
                    VM.Quatition = new ObservableCollection<CotizacionEF>();
                    return;
                }

                VM.Quatition = JsonConvert.DeserializeObject<ObservableCollection<CotizacionEF>>(r.data.ToString());
                VM.GettinData = false;
            }
            else
            {
                if (VM.Folio == null)
                {
                    VM.SelectedDateChangedQuoteCommand.Execute(null);
                }
                else if (String.IsNullOrEmpty(VM.Folio))
                    VM.SelectedDateChangedQuoteCommand.Execute(null);
                else
                    MessageBox.Show("El campo de busqueda esta con letras.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
    }
}
