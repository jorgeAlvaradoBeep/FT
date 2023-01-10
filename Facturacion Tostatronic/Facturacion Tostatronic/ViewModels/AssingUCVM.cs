using Facturacion_Tostatronic.Models;
using Facturacion_Tostatronic.Models.Products;
using Facturacion_Tostatronic.Services;
using Facturacion_Tostatronic.ViewModels.Commands.AssingUCVCommands;
using Facturacion_Tostatronic.ViewModels.Commands.ProductsCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion_Tostatronic.ViewModels
{
    public class AssingUCVM : BaseNotifyPropertyChanged, IPageViewModel
    {
        public string Name { get; set; } = "AssingUCVM";
        public SearchProductCommand SearchProductCommand { get; set; }
        public SelectedProductCommand SelectedProductCommand { get; set; }
        public SaveUPCCommand SaveUPCCommand { get; set; }
        public CancelUPCCommand CancelUPCCommand { get; set; }
        private List<ProductCodes> products;

        public List<ProductCodes> Products
        {
            get { return products; }
            set { SetValue(ref products,value); }
        }

        private ProductCodes selectedProduct;

        public ProductCodes SelectedProduct
        {
            get { return selectedProduct; }
            set { SetValue( ref selectedProduct,value); }
        }



        public string ProductID { get; set; }
        public AssingUCVM()
        {
            Products = new List<ProductCodes>();
            SearchProductCommand = new SearchProductCommand(this);
            SelectedProductCommand = new SelectedProductCommand(this);
            SaveUPCCommand = new SaveUPCCommand(this);
            CancelUPCCommand = new CancelUPCCommand(this);
        }
    }
}
