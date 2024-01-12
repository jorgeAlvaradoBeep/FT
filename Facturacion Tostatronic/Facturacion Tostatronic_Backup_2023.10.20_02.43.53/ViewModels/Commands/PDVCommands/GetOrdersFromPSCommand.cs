using Bukimedia.PrestaSharp.Entities;
using Bukimedia.PrestaSharp.Factories;
using Facturacion_Tostatronic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Facturacion_Tostatronic.ViewModels.Commands.PDVCommands
{
    public class GetOrdersFromPSCommand : ICommand
    {
        public MenuVM VM { get; set; }
        public event EventHandler CanExecuteChanged;

        public GetOrdersFromPSCommand(MenuVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            

            //ManufacturerFactory ManufacturerFactory = new ManufacturerFactory(BaseUrl, Account, Password);

            //List<manufacturer> manufacturers = ManufacturerFactory.GetAll();

            OrderFactory Orders = new OrderFactory(URLData.psBaseUrl, URLData.psAccount, URLData.psPassword);

            //Dictionary<string, string> dtn = new Dictionary<string, string>();
            //dtn.Add("name", "Metallica");

            List<order> AllOrders = Orders.GetAll();

            //OrderHistoryFactory OrderHis = new OrderHistoryFactory(BaseUrl, Account, Password);

            //List<order_history> AllOrderHis = OrderHis.GetAll();
        }
    }
}
