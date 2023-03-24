using Facturacion_Tostatronic.Models.Products;
using Facturacion_Tostatronic.Models.Products.SucursalProduct;
using Facturacion_Tostatronic.Services;
using Facturacion_Tostatronic.ViewModels.Commands.ProductsCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion_Tostatronic.ViewModels.Products
{
    public class SucursalPricesVM : BaseNotifyPropertyChanged
    {

        #region Properties
        private SucursalProduct product;

        public SucursalProduct Product
        {
            get { return product; }
            set { SetValue(ref product, value); }
        }
        private bool gettingData;

        public bool GettingData
        {
            get { return gettingData; }
            set { SetValue(ref gettingData, value); }
        }
        private string productCriterialSearch;

        public string ProductCriterialSearch
        {
            get { return productCriterialSearch; }
            set { SetValue(ref productCriterialSearch, value); }
        }
        private List<ProductComplete> searchedProducts;

        public List<ProductComplete> SearchedProducts
        {
            get { return searchedProducts; }
            set { SetValue(ref searchedProducts, value); }
        }
        #endregion
        #region Commands
        public SearchProductForSucursalPrice SearchProductForSucursalPrice { get; set; }
        public SelectSucursalPriceProductCommand SelectSucursalPriceProductCommand { get; set; }
        public UpdateSucursalPriceCommand UpdateSucursalPriceCommand { get; set; }

        #endregion
        public SucursalPricesVM()
        {
            //inicialización de propiedades
            GettingData = false;
            SearchedProducts = new List<ProductComplete>();


            //inicialización de comandos
            SearchProductForSucursalPrice = new SearchProductForSucursalPrice(this);
            SelectSucursalPriceProductCommand = new SelectSucursalPriceProductCommand(this);
            UpdateSucursalPriceCommand = new UpdateSucursalPriceCommand(this);

        }
    }
}
