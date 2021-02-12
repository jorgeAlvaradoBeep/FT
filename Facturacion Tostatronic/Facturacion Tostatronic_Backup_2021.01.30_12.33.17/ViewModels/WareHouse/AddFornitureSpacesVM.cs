using Facturacion_Tostatronic.Models.WareHouse;
using Facturacion_Tostatronic.Services;
using Facturacion_Tostatronic.ViewModels.Commands.WareHouseCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Facturacion_Tostatronic.ViewModels.WareHouse
{
    public class AddFornitureSpacesVM : BaseNotifyPropertyChanged
    {
        #region Comandos
        public SaveSpacesCommand SaveSpacesCommand { get; set; }
        #endregion
        #region Propiedades
        private WareHouseM swh;

        public WareHouseM SWH
        {
            get { return swh; }
            set { SetValue(ref swh, value); }
        }

        private bool isBusy;

        public bool IsBusy
        {
            get { return isBusy; }
            set { SetValue(ref isBusy, value); }
        }

        private WareHouseM selectedWareHouse;

        public WareHouseM SelectedWareHouse
        {
            get { return selectedWareHouse; }
            set 
            { 
                SetValue(ref selectedWareHouse, value);
            }
        }

        private LocalizationM selectedLocalization;

        public LocalizationM SelectedLocalization
        {
            get { return selectedLocalization; }
            set { SetValue(ref selectedLocalization, value); }
        }

        private OrganizationForniture selectedForniture;

        public OrganizationForniture SelectedForniture
        {
            get { return selectedForniture; }
            set 
            { 
                SetValue(ref selectedForniture, value);
                if (selectedForniture != null)
                {
                    AddSpacesGridVisible = Visibility.Visible;
                    if(SelectedForniture.Spaces == null)
                        SelectedForniture.Spaces = new List<FornitureSpaces>();
                }
                else
                    AddSpacesGridVisible = Visibility.Hidden;
            }
        }


        private Visibility addSpacesGridVisible;

        public Visibility AddSpacesGridVisible
        {
            get { return addSpacesGridVisible; }
            set { SetValue(ref addSpacesGridVisible, value); }
        }



        public List<WareHouseM> WareHouseList { get; set; }

        #endregion
        public AddFornitureSpacesVM()
        {
            SaveSpacesCommand = new SaveSpacesCommand(this);
            WareHouseList = (List<WareHouseM>)Application.Current.Properties["WareHouses"];
            Application.Current.Properties["WareHouses"] = null;
            AddSpacesGridVisible = Visibility.Hidden;
        }
    }
}
