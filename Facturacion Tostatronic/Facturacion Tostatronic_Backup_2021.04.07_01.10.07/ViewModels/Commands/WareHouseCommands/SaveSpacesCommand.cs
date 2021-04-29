using Facturacion_Tostatronic.Models;
using Facturacion_Tostatronic.Services;
using Facturacion_Tostatronic.ViewModels.WareHouse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Facturacion_Tostatronic.ViewModels.Commands.WareHouseCommands
{
    public class SaveSpacesCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public AddFornitureSpacesVM VM { get; set; }
        public SaveSpacesCommand(AddFornitureSpacesVM vm)
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
            Task.Run(() => SaveData());
            VM.IsBusy = false;
        }
        async void SaveData()
        {
            List<Models.WareHouse.FornitureSpaces> spaces = new List<Models.WareHouse.FornitureSpaces>();
            List<Models.WareHouse.FornitureSpaces> edited = new List<Models.WareHouse.FornitureSpaces>();
            foreach (Models.WareHouse.WareHouseM wh in VM.WareHouseList)
            {
                foreach (Models.WareHouse.LocalizationM localization in wh.Localizations)
                {
                    foreach (Models.WareHouse.OrganizationForniture of in localization.OrganizationFornitures)
                    {
                        if (of.Spaces != null)
                        {
                            if (of.Spaces.Count > 0)
                            {
                                foreach (Models.WareHouse.FornitureSpaces space in of.Spaces)
                                {
                                    if (space.IDForniture == 0)
                                    {
                                        space.IDForniture = of.ID;
                                        spaces.Add(space);
                                    }
                                    else if (space.Edited)
                                    {
                                        edited.Add(space);
                                    }
                                }

                            }
                        }
                    }
                }
            }
            if (spaces.Count.Equals(0))
            {
                MessageBox.Show("Debe guardar a lo menos un espacio de almacenamiento", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                VM.IsBusy = false;
                return;
            }
            Response res = await WebService.InsertData(spaces, URLData.save_new_spaces);
            MessageBox.Show("Agrega espacios");
        }
    }
}
