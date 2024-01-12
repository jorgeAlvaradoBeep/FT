using Facturacion_Tostatronic.Models;
using Facturacion_Tostatronic.Models.Products;
using Facturacion_Tostatronic.Services;
using Facturacion_Tostatronic.ViewModels.Commands.ProductsCommands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace Facturacion_Tostatronic.ViewModels.Products
{
    public class MLPriceVM : BaseNotifyPropertyChanged, IPageViewModel
    {
        #region commands
        public SearchProductsForMLPricecommand SearchProductsForMLPricecommand { get; set; }
        #endregion
        #region Propiedades

        public string Name { get; set; } = "MLPriceVM";
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
                Task.Run(() => SelectedProduct.UPC = GetUPC(SelectedProduct.Code));
                ProductData.NumberOfPiecesOfPackage = 1;
                ProductData.PriceToCalculate = SelectedProduct.MinimumPrice;
                ProductData.PublicationPrice = 25;
                ProductData.ClassicPublicationComission = 14.5f;
                ProductData.ClassicPublicationShippingCost = 110;
                ProductData.PremiumublicationComission = 19.5f;
                ProductData.PremiumPublicationShippingCost = 110;

                /*
                float p = SelectedProduct.MinimumPrice;
                p = p * ProductData.NumberOfPiecesOfPackage;
                p += ProductData.PublicationPrice;
                p = p / (1 - (ProductData.ClassicPublicationComission / 100));
                p /= 1.1f;
                //p *= 1.04f;
                ProductData.ClassicPriceWOS = p;
                ProductData.ClassicPriceWS = p + ProductData.ClassicPublicationShippingCost;

                p = SelectedProduct.MinimumPrice;
                p = p * ProductData.NumberOfPiecesOfPackage;
                p += ProductData.PublicationPrice;
                p = p / (1 - (ProductData.PremiumublicationComission / 100));
                p /= 1.1f;

                ProductData.PremiumPriceWOS = p;
                ProductData.PremiumPriceWS = p + ProductData.PremiumPublicationShippingCost;*/
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

        public string GetUPC(string code)
        {
            GettingData = true;
            Response res = WebService.GetDataNodeNoAsync(URLData.getProductCodesIDNET, code);
            if (!res.succes)
            {
                MessageBox.Show("Error: " + res.message + Environment.NewLine + "No se encontrarion coincidencias", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                GettingData = false;
                return string.Empty;
            }
            ProductCodesEF productoCode = JsonConvert.DeserializeObject<ProductCodesEF>(res.data.ToString());
            GettingData = false;
            return productoCode.Upc;
        }

    }
}
