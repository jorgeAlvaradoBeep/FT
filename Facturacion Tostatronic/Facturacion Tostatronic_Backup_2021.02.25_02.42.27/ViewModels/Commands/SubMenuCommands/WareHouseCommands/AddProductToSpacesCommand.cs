using Facturacion_Tostatronic.Models;
using Facturacion_Tostatronic.Models.WareHouse;
using Facturacion_Tostatronic.Services;
using Facturacion_Tostatronic.ViewModels.Menus;
using Facturacion_Tostatronic.Views.WareHouseViews;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Facturacion_Tostatronic.ViewModels.Commands.SubMenuCommands.WareHouseCommands
{
    public class AddProductToSpacesCommand : ICommand
    {
        public WareHouseMenuVM VM { get; set; }
        public event EventHandler CanExecuteChanged;

        public AddProductToSpacesCommand(WareHouseMenuVM vm)
        {
            VM = vm;
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            VM.IsBusy = true;
            Response res = await WebService.GetData("wh", null, URLData.save_new_warehouse);
            if (!res.succes)
            {
                MessageBox.Show("Error al consultar en la BD." +
                        Environment.NewLine + "Motivos: " + res.message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                VM.IsBusy = false;
                return;
            }
            List<WareHouseM> wh = JsonConvert.DeserializeObject<List<WareHouseM>>(res.data.ToString());
            Application.Current.Properties["WareHouses"] = wh;
            VM.IsBusy = false;
            WareHouseContetView wHCV = new WareHouseContetView(new AddProductsToSpaceV());
            wHCV.ShowDialog();
        }
    }
}
