using Facturacion_Tostatronic.Models;
using Facturacion_Tostatronic.Services;
using System;
using System.Windows.Input;
using System.Windows;
using Facturacion_Tostatronic.ViewModels.Orders;
using Newtonsoft.Json;
using System.Collections.Generic;
using Facturacion_Tostatronic.Models.EF_Models.EF_Orders;

namespace Facturacion_Tostatronic.ViewModels.Commands.OrderCommands
{
    public class GetNewOrderCommand : ICommand
    {
        public MakeOrderVM VM { get; set; }
        public event EventHandler CanExecuteChanged;

        public GetNewOrderCommand(MakeOrderVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            VM.GettingData = true;
            Response res = await WebService.GetDataForInvoice(URLData.new_pi);
            if (!res.succes)
            {
                MessageBox.Show(res.message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                VM.GettingData = false;
                return;
            }
            List<ProductList> products = JsonConvert.DeserializeObject<List<ProductList>>(res.data.ToString());
            foreach (ProductList product in products) 
            {
                EFOrderProduct newProduct = new EFOrderProduct();

            }
            VM.GettingData = false;
        }
    }
}
