using Chilkat;
using Facturacion_Tostatronic.Models;
using Facturacion_Tostatronic.Models.EF_Models.EFProduct;
using Facturacion_Tostatronic.Models.EF_Models.EFSale;
using Facturacion_Tostatronic.Services;
using Facturacion_Tostatronic.ViewModels.Sales;
using Newtonsoft.Json;
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
            Response r = await WebService.GetDataNode(URLData.getDatedOrders, VM.SelectedDate.Date.ToString("yyyy-MM-dd"));
            if (!r.succes)
            {
                if (!string.IsNullOrEmpty(r.message))
                    MessageBox.Show(r.message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                else
                    MessageBox.Show("Error al traer datos de ventas", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                VM.GettinData = false;
                VM.Sales = new ObservableCollection<CompleteSaleEF>();
                return;
            }

            VM.Sales = JsonConvert.DeserializeObject<ObservableCollection<CompleteSaleEF>>(r.data.ToString());
            VM.GettinData = false;
        }
    }
}
