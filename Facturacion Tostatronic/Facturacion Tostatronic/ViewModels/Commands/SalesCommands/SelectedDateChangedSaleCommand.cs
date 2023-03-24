using Facturacion_Tostatronic.Models;
using Facturacion_Tostatronic.Services;
using Facturacion_Tostatronic.ViewModels.Sales;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.AccessControl;
using System.Windows;
using System.Windows.Input;

namespace Facturacion_Tostatronic.ViewModels.Commands.SalesCommands
{
    public class SelectedDateChangedSaleCommand : ICommand
    {
        public SeeSalesVM VM { get; set; }

        public event EventHandler CanExecuteChanged;
        public SelectedDateChangedSaleCommand(SeeSalesVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            VM.GettinData = true;
            Response r = await WebService.GetData("sale_date", VM.SelectedDate.Date.ToString("yyyy-MM-dd"), URLData.sales);
            if (r.statusCode == 404)
            {
                if (!string.IsNullOrEmpty(r.message))
                    MessageBox.Show(r.message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
    }
}
