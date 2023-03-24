using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using Facturacion_Tostatronic.ViewModels.Sales;
using Facturacion_Tostatronic.Models;
using Facturacion_Tostatronic.Services;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.Security.AccessControl;

namespace Facturacion_Tostatronic.ViewModels.Commands.SalesCommands
{
    public class SearchSaleSSVCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public SeeSalesVM VM { get; set; }
        public SearchSaleSSVCommand(SeeSalesVM vm)
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
                Response r = await WebService.GetData("sale_number", VM.Folio, URLData.sales);
                if (r.statusCode == 404)
                {
                    if (!string.IsNullOrEmpty(r.message))
                        MessageBox.Show(r.message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    else
                        MessageBox.Show("No existen registros de ese folio", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    VM.GettinData = false;
                    VM.Sales = new ObservableCollection<Sale>();
                    return;
                }
                var t = r.data;
                List<Sale> v = ((JArray)t).Select(x => new Sale
                {
                    idSale = (int)x["idSale"],
                    rfc = (string)x["rfc"],
                    date = (string)x["date"]
                }).ToList();
                VM.Sales = new ObservableCollection<Sale>(v);
                VM.GettinData = false;
            }
            else
            {
                if(VM.Folio==null) 
                {
                    VM.SelectedDateChangedSaleCommand.Execute(null);
                }
                else if(String.IsNullOrEmpty(VM.Folio))
                    VM.SelectedDateChangedSaleCommand.Execute(null);
                else
                    MessageBox.Show("El campo de busqueda esta con letras.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
                
        }
    }
}
