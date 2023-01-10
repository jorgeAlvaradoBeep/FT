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
    public class AddProductVM : BaseNotifyPropertyChanged, IPageViewModel
    {
        public AddNewProductCommand AddNewProductCommand { get; set; }
        public string Name { get; set; } = "AddProductVM";
        private ProductComplete product;

        public ProductComplete Product
        {
            get { return product; }
            set { SetValue(ref product, value); }
        }

        private string imagePath;

        public string ImagePath
        {
            get { return imagePath; }
            set { SetValue(ref imagePath, value); }
        }
        private Visibility baseProductVisibility;

        public Visibility BaseProductVisibility
        {
            get { return baseProductVisibility; }
            set { SetValue(ref baseProductVisibility, value); }
        }
        private bool enableProductBase;

        public bool EnableProductBase
        {
            get { return enableProductBase; }
            set { SetValue(ref enableProductBase, value); }
        }
        private bool gettinData;

        public bool GettingData
        {
            get { return gettinData; }
            set { SetValue(ref gettinData,value); }
        }
        private string progressVal;

        public string ProgressVall
        {
            get { return progressVal; }
            set { SetValue(ref progressVal, value); }
        }


        public AddProductVM()
        {
            initialize();
            AddNewProductCommand = new AddNewProductCommand(this);
            
        }
        public void initialize()
        {
            Product = new ProductComplete();
            Product.SpecificPrices = new List<SpecificPrice>();
            ImagePath = string.Empty;
            BaseProductVisibility = Visibility.Hidden;
            EnableProductBase = true;
            GettingData= false;
            ProgressVall = string.Empty;
        }
    }
}
