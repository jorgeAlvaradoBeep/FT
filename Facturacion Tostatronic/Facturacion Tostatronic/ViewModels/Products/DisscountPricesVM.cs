using Facturacion_Tostatronic.Models.Products;
using Facturacion_Tostatronic.Services;
using Facturacion_Tostatronic.ViewModels.Commands.ProductsCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Facturacion_Tostatronic.ViewModels.Products
{
    
    public class DisscountPricesVM : BaseNotifyPropertyChanged
    {
        #region Properties
        private ProductComplete product;

        public ProductComplete Product
        {
            get { return product; }
            set { SetValue(ref product, value); }
        }

        private Visibility baseProductVisibility;

        public Visibility BaseProductVisibility
        {
            get { return baseProductVisibility; }
            set { SetValue(ref baseProductVisibility, value); }
        }

        private Visibility specificProductVisibility;

        public Visibility SpecificProductVisibility
        {
            get { return specificProductVisibility; }
            set { SetValue(ref specificProductVisibility, value); }
        }

        private Visibility searchProductVisibility;

        public Visibility SearchProductVisibility
        {
            get { return searchProductVisibility; }
            set { SetValue(ref searchProductVisibility, value); }
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

        private bool dataUpdated;

        public bool DataUpdated
        {
            get { return dataUpdated; }
            set { SetValue(ref dataUpdated, value); }
        }

        #endregion

        #region Commands
        public SearchProductsForDisscountPricecommand SearchProductsForDisscountPricecommand { get; set; }
        public SelectDisscountPriceProductCommand SelectDisscountPriceProductCommand { get; set; }
        public UpdateProductCommand UpdateProductCommand { get; set; }
        #endregion

        public DisscountPricesVM()
        {
            BaseProductVisibility = Visibility.Hidden;
            SpecificProductVisibility = Visibility.Hidden;
            SearchProductVisibility = Visibility.Visible;
            DataUpdated = true;
            GettingData = false;
            SearchProductsForDisscountPricecommand = new SearchProductsForDisscountPricecommand(this);
            SelectDisscountPriceProductCommand = new SelectDisscountPriceProductCommand(this);
            UpdateProductCommand = new UpdateProductCommand(this);
        }

        public void Initialize()
        {
            BaseProductVisibility = Visibility.Hidden;
            SpecificProductVisibility = Visibility.Hidden;
            SearchProductVisibility = Visibility.Visible;
            DataUpdated = true;
            GettingData = false;
            Product.SpecificPrices = new List<SpecificPrice>();
            Product = new ProductComplete();

        }
    }
}
