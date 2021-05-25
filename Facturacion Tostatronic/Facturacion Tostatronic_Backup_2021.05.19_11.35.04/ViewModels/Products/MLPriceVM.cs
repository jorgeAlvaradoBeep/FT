using Facturacion_Tostatronic.Models.Products;
using Facturacion_Tostatronic.Services;
using Facturacion_Tostatronic.ViewModels.Commands.ProductsCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion_Tostatronic.ViewModels.Products
{
    public class MLPriceVM : BaseNotifyPropertyChanged
    {
        #region commands
        public SearchProductsForMLPricecommand SearchProductsForMLPricecommand { get; set; }
        #endregion
        #region Propiedades
        private bool gettingData;

        public bool GettingData
        {
            get { return gettingData; }
            set { SetValue(ref gettingData, value); }
        }

        private List<ProductComplete> searchedProducts;

        public List<ProductComplete> SearchedProducts
        {
            get { return searchedProducts; }
            set { SetValue(ref searchedProducts, value); }
        }

        private ProductComplete selectedProduct;

        public ProductComplete SelectedProduct
        {
            get { return selectedProduct; }
            set 
            { 
                SetValue(ref selectedProduct, value);
                if (selectedProduct == null)
                    return;
                GettingData = true;
                Task.Run(() => SelectedProduct.UPC = PrestashopService.GetUPC(SelectedProduct.Code));
                ProductData.NumberOfPiecesOfPackage = 1;
                ProductData.PriceToCalculate = SelectedProduct.MinimumPrice;
                ProductData.PublicationPrice = 15;
                ProductData.ClassicPublicationComission = 13.5f;
                ProductData.ClassicPublicationShippingCost = 95;
                ProductData.PremiumublicationComission = 17;
                ProductData.PremiumPublicationShippingCost = 95;
                float p = SelectedProduct.MinimumPrice;
                p = p * ProductData.NumberOfPiecesOfPackage;
                p = p * (1 + (ProductData.ClassicPublicationComission / 100));
                p *= 1.1f;
                p += ProductData.PublicationPrice;
                p *= 1.04f;
                ProductData.ClassicPriceWOS = p;
                ProductData.ClassicPriceWS = p + ProductData.ClassicPublicationShippingCost;

                p = SelectedProduct.MinimumPrice;
                p = p * ProductData.NumberOfPiecesOfPackage;
                p = p * (1 + (ProductData.PremiumublicationComission / 100));
                p *= 1.1f;
                p += ProductData.PublicationPrice;
                p *= 1.04f;

                ProductData.PremiumPriceWOS = p;
                ProductData.PremiumPriceWS = p + ProductData.PremiumPublicationShippingCost;
                GettingData = false;
            }
        }

        private MLProductData productData;

        public MLProductData ProductData
        {
            get { return productData; }
            set { SetValue(ref productData, value); }
        }

        private string productCriterialSerach;

        public string ProductCriterialSearch
        {
            get { return productCriterialSerach; }
            set { SetValue(ref productCriterialSerach, value); }
        }



        #endregion

        public MLPriceVM()
        {
            SearchProductsForMLPricecommand = new SearchProductsForMLPricecommand(this);
            GettingData = false;
            ProductData = new MLProductData();
        }

    }
}
