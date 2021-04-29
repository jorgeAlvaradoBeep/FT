using Facturacion_Tostatronic.Models.WareHouse;
using Facturacion_Tostatronic.Services;
using Facturacion_Tostatronic.ViewModels.Commands.SubMenuCommands.WareHouseCommands;
using Facturacion_Tostatronic.ViewModels.Commands.WareHouseCommands;
using System;
using System.Collections.Generic;
using System.Windows;

namespace Facturacion_Tostatronic.ViewModels.WareHouse
{
    public class AddProductsToSpaceVM : BaseNotifyPropertyChanged
    {
        #region Propiedades
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
            }
        }

        private FornitureSpaces selectedSpace;

        public FornitureSpaces SelectedSpace
        {
            get { return selectedSpace; }
            set 
            { 
                SetValue(ref selectedSpace, value);
                SelectedSpace.Products = new System.Collections.ObjectModel.ObservableCollection<ProductInSpace>();
                IsSelectionAvailable = false;
                ProductsGrid = Visibility.Visible;
            }
        }

        public List<WareHouseM> WareHouseList { get; set; }

        private bool isSelectionAvailable;

        public bool IsSelectionAvailable
        {
            get { return isSelectionAvailable; }
            set { SetValue(ref isSelectionAvailable, value); }
        }

        private string productCriterialSearch;

        public string ProductCriterialSearch
        {
            get { return productCriterialSearch; }
            set { SetValue(ref productCriterialSearch, value); }
        }
        private List<ProductInSpace> serchedProducts;

        public List<ProductInSpace> SerchedProducts
        {
            get { return serchedProducts; }
            set { SetValue(ref serchedProducts, value); }
        }

        private Visibility productsGrid;

        public Visibility ProductsGrid
        {
            get { return productsGrid; }
            set { SetValue(ref productsGrid, value); }
        }


        #endregion
        #region Commands
        public SearchProductsForSpacecommand SearchProductsForSpacecommand { get; set; }
        #endregion


        public AddProductsToSpaceVM()
        {
            SearchProductsForSpacecommand = new SearchProductsForSpacecommand(this);
            ProductsGrid = Visibility.Hidden;
            WareHouseList = (List<WareHouseM>)Application.Current.Properties["WareHouses"];
            Application.Current.Properties["WareHouses"] = null;
            IsSelectionAvailable = true;
        }
    }
}
