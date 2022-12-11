using Facturacion_Tostatronic.Models;
using Facturacion_Tostatronic.Models.WareHouse;
using Facturacion_Tostatronic.Services;
using Facturacion_Tostatronic.ViewModels.Commands.SubMenuCommands.WareHouseCommands;
using Facturacion_Tostatronic.ViewModels.Commands.WareHouseCommands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
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
                if (SelectedSpace == null)
                    return;
                SelectedSpace.Products = Task.Run(() => GetProductsFromspace(SelectedSpace.ID)).Result;
                //else
                //    SelectedSpace.Products = new ObservableCollection<ProductInSpace>();
                NewProductInspace = new List<ProductInSpace>();
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

        public List<ProductInSpace> NewProductInspace { get; set; }

        #endregion
        #region Commands
        public SearchProductsForSpacecommand SearchProductsForSpacecommand { get; set; }
        public SaleProductToSpaceCommand SaleProductToSpaceCommand { get; set; }
        public SaveProductsTospaceCommand SaveProductsTospaceCommand { get; set; }
        public CancelSpacesCommand CancelSpacesCommand { get; set; }
        public DeleteProductFromForniturecommand DeleteProductFromForniturecommand { get; set; }
        #endregion


        public AddProductsToSpaceVM()
        {
            SearchProductsForSpacecommand = new SearchProductsForSpacecommand(this);
            SaleProductToSpaceCommand = new SaleProductToSpaceCommand(this);
            SaveProductsTospaceCommand = new SaveProductsTospaceCommand(this);
            CancelSpacesCommand = new CancelSpacesCommand(this);
            DeleteProductFromForniturecommand = new DeleteProductFromForniturecommand(this);
            ProductsGrid = Visibility.Hidden;
            WareHouseList = (List<WareHouseM>)Application.Current.Properties["WareHouses"];
            Application.Current.Properties["WareHouses"] = null;
            IsSelectionAvailable = true;
        }
        ObservableCollection<ProductInSpace> GetProductsFromspace(int idSpace)
        {
            ObservableCollection<ProductInSpace> nL;
            IsBusy = true;
            Response res = WebService.GetDataNoasync("idSpace", idSpace.ToString(), URLData.save_new_product_to_space);
            if (!res.succes)
            {
                //MessageBox.Show("Error: " + res.message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                IsBusy = false;
                return new ObservableCollection<ProductInSpace>();
            }
            nL = JsonConvert.DeserializeObject<ObservableCollection<ProductInSpace>>(res.data.ToString());
            IsBusy = false;
            if(nL.Count==0)
                return new ObservableCollection<ProductInSpace>();
            return nL;
        }
    }
}
