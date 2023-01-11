using Facturacion_Tostatronic.Models;
using Facturacion_Tostatronic.Models.Clients;
using Facturacion_Tostatronic.Models.Products;
using Facturacion_Tostatronic.Models.SAT;
using Facturacion_Tostatronic.Services;
using Facturacion_Tostatronic.ViewModels.Commands.ProductsCommands;
using Newtonsoft.Json;
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

        #region Properties
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

        private List<SATCode> codigoSat;

        public List<SATCode> CodigoSat
        {
            get { return codigoSat; }
            set { SetValue(ref codigoSat, value); }
        }

        private SATCode selectedCode;

        public SATCode SelectedCode
        {
            get { return selectedCode; }
            set { SetValue(ref selectedCode, value); }
        }


        #endregion


        public AddProductVM()
        {
            initialize();
            AddNewProductCommand = new AddNewProductCommand(this);
            CodigoSat = new List<SATCode>();
            GettingData = true;  
            Task.Run(() =>
            {
                Response rmp = WebService.GetDataForInvoiceNoAsync(URLData.getCodigosSatNET);
                if (rmp.succes)
                {
                    CodigoSat = JsonConvert.DeserializeObject<List<SATCode>>(rmp.data.ToString());
                }
                else
                    MessageBox.Show("Error al traer la información solicitada", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                GettingData = false;
            });

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
            SelectedCode = null;
        }
    }
}
