using Facturacion_Tostatronic.Models;
using Facturacion_Tostatronic.Models.EF_Models.EFProduct;
using Facturacion_Tostatronic.Models.Products;
using Facturacion_Tostatronic.Services;
using Facturacion_Tostatronic.ViewModels.Commands.AssingUCVCommands;
using Facturacion_Tostatronic.ViewModels.Commands.ProductsCommands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Facturacion_Tostatronic.ViewModels
{
    public class AssingUCVM : BaseNotifyPropertyChanged, IPageViewModel
    {
        public string Name { get; set; } = "AssingUCVM";
        private bool gettinData;

        public bool GettingData
        {
            get { return gettinData; }
            set { SetValue(ref gettinData, value); }
        }

        public SearchProductCommand SearchProductCommand { get; set; }
        public SelectedProductCommand SelectedProductCommand { get; set; }
        public SaveUPCCommand SaveUPCCommand { get; set; }
        public CancelUPCCommand CancelUPCCommand { get; set; }
        private List<EFProduct> products;

        public List<EFProduct> Products
        {
            get { return products; }
            set { SetValue(ref products,value); }
        }

        private EFProduct selectedProduct;

        public EFProduct SelectedProduct
        {
            get { return selectedProduct; }
            set 
            { 
                SetValue( ref selectedProduct,value); 
                if(SelectedProduct!=null)
                {
                    GettingData= true;
                    Task.Run(() =>
                    {
                        Response rmp = WebService.GetDataNodeNoAsync(URLData.getProductCodesIDNET, SelectedProduct.codigo);
                        if (rmp.succes)
                        {
                            ProductToModify = JsonConvert.DeserializeObject<ProductCodesEF>(rmp.data.ToString());
                        }
                        else
                            MessageBox.Show($"Error al traer la información solicitada" +
                                $"{Environment.NewLine}{rmp.message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        GettingData = false;
                    });
                }
            }
        }

        private ProductCodesEF productToModify;

        public ProductCodesEF ProductToModify
        {
            get { return productToModify; }
            set { SetValue(ref productToModify, value); }
        }


        public string ProductID { get; set; }
        public AssingUCVM()
        {
            GettingData = false;
            ProductToModify = new ProductCodesEF();
            Products = new List<EFProduct>();
            SearchProductCommand = new SearchProductCommand(this);
            SelectedProductCommand = new SelectedProductCommand(this);
            SaveUPCCommand = new SaveUPCCommand(this);
            CancelUPCCommand = new CancelUPCCommand(this);
        }
    }
}
