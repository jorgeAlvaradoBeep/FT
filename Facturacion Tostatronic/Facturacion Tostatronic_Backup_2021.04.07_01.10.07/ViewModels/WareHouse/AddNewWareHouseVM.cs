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
    public class AddNewWareHouseVM : BaseNotifyPropertyChanged
    {
        #region Commands
        public SaveNameCommand SaveNameCommand { get; set; }
        public SaveNewWHCommand SaveNewWHCommand { get; set; }
        public SaveLocalizationFornituresCommand SaveLocalizationFornituresCommand { get; set; }
        #endregion
        #region Properties
        private WareHouseM wh;

        public WareHouseM WH
        {
            get { return wh; }
            set { SetValue(ref wh, value); }
        }

        private bool enableEditingName;

        public bool EnableEditingName
        {
            get { return enableEditingName; }
            set { SetValue(ref enableEditingName, value); }
        }

        private bool enabledEditingLocalizationName;

        public bool EnabledEditingLocalizationName
        {
            get { return enabledEditingLocalizationName; }
            set { SetValue(ref enabledEditingLocalizationName, value); }
        }


        private Visibility localizationVisibility;

        public Visibility LocalizationVisibility
        {
            get { return localizationVisibility; }
            set { SetValue(ref localizationVisibility, value); }
        }

        private Visibility fornitureVisibility;

        public Visibility FornitureVisibility
        {
            get { return fornitureVisibility; }
            set { SetValue(ref fornitureVisibility, value); }
        }

        //private LocalizationM selectedLocalization;

        //public LocalizationM SelectedLocalization
        //{
        //    get { return selectedLocalization; }
        //    set 
        //    {
        //        if (EnabledEditingLocalizationName)
        //        {
        //            SetValue(ref selectedLocalization, value);
        //            selectedLocalization.OrganizationFornitures = new System.Collections.ObjectModel.ObservableCollection<OrganizationForniture>();
        //            //WH.Localizations.Where(x => x == selectedLocalization).FirstOrDefault().OrganizationFornitures = new System.Collections.ObjectModel.ObservableCollection<OrganizationForniture>();
        //        }
        //    }
        //}
        private int selectedLocalizationIndex;

        public int SelectedLocalizationIndex
        {
            get { return selectedLocalizationIndex; }
            set 
            { 
                SetValue(ref selectedLocalizationIndex, value); 
            }
        }

        private LocalizationM selectedLocalizationItem;

        public LocalizationM SelectedLocalizationItem
        {
            get { return selectedLocalizationItem; }
            set
            {
                if (EnabledEditingLocalizationName)
                {
                    SetValue(ref selectedLocalizationItem, value);
                    FornitureVisibility = Visibility.Visible;
                    //selectedLocalization.OrganizationFornitures = new System.Collections.ObjectModel.ObservableCollection<OrganizationForniture>();
                    //WH.Localizations.Where(x => x == selectedLocalization).FirstOrDefault().OrganizationFornitures = new System.Collections.ObjectModel.ObservableCollection<OrganizationForniture>();
                }
            }
        }
        public int SavePosition { get; set; }

        private bool isBusy;

        public bool IsBusy
        {
            get { return isBusy; }
            set { SetValue(ref isBusy, value); }
        }


        #endregion
        public AddNewWareHouseVM()
        {
            Initilizate();
        }
        public void Initilizate()
        {
            SaveNameCommand = new SaveNameCommand(this);
            SaveNewWHCommand = new SaveNewWHCommand(this);
            SaveLocalizationFornituresCommand = new SaveLocalizationFornituresCommand(this);
            WH = new WareHouseM();
            WH.Localizations = new System.Collections.ObjectModel.ObservableCollection<LocalizationM>();
            EnableEditingName = true;
            LocalizationVisibility = Visibility.Hidden;
            FornitureVisibility = Visibility.Hidden;
            EnabledEditingLocalizationName = false;
            SavePosition = 0;
            IsBusy = false;
        }
    }
}
