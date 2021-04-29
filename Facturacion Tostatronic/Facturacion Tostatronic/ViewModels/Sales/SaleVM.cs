using Facturacion_Tostatronic.Models.Clients;
using Facturacion_Tostatronic.Models.Sales;
using Facturacion_Tostatronic.Services;
using Facturacion_Tostatronic.ViewModels.Commands.PDVCommands.SaleCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion_Tostatronic.ViewModels.Sales
{
    public class SaleVM : BaseNotifyPropertyChanged
    {
        #region Comandos
        public CallSearchClientViewCommand CallSearchClientViewCommand { get; set; }
        public SaveSaleCommand SaveSaleCommand { get; set; }
        public SearchProductsForSalecommand SearchProductsForSalecommand { get; set; }
        public SetSelectedPriceCommand SetSelectedPriceCommand { get; set; }
        public SaleProductCommand SaleProductCommand { get; set; }
        public ChangeSaledQuantityCommand ChangeSaledQuantityCommand { get; set; }
        public NeedFacturaCommand NeedFacturaCommand { get; set; }
        public DeleteProductFromSalecommand DeleteProductFromSalecommand { get; set; }
        public SaveQuateCommand SaveQuateCommand { get; set; }
        public CancelCommand CancelCommand { get; set; }
        #endregion

        #region Propiedades
        private List<string> clientType;
        public List<string> ClientType
        {
            get { return clientType; }
            set { SetValue(ref clientType, value); }
        }
        private bool gettingData;

        public bool GettingData
        {
            get { return gettingData; }
            set { SetValue(ref gettingData, value); }
        }

        private CompleteSaleM completeSale;

        public CompleteSaleM CompleteSale
        {
            get { return completeSale; }
            set { SetValue(ref completeSale, value); }
        }

        private string productCriterialSearch;

        public string ProductCriterialSearch
        {
            get { return productCriterialSearch; }
            set { SetValue(ref productCriterialSearch, value); }
        }


        #endregion
        public SaleVM()
        {
            ClientType = new List<string>()
                {
                    "Minimo",
                    "Distribuidor",
                    "Publico"
                };
            InitializeCompleteSale();
        }
        public void InitializeCompleteSale()
        {
            GettingData = false;
            CallSearchClientViewCommand = new CallSearchClientViewCommand(this);
            SaveSaleCommand = new SaveSaleCommand(this);
            SearchProductsForSalecommand = new SearchProductsForSalecommand(this);
            SetSelectedPriceCommand = new SetSelectedPriceCommand(this);
            SaleProductCommand = new SaleProductCommand(this);
            ChangeSaledQuantityCommand = new ChangeSaledQuantityCommand(this);
            NeedFacturaCommand = new NeedFacturaCommand(this);
            DeleteProductFromSalecommand = new DeleteProductFromSalecommand(this);
            SaveQuateCommand = new SaveQuateCommand(this);
            CancelCommand = new CancelCommand(this);
            CompleteSale = new CompleteSaleM();
            CompleteSale.ClientSale = new ClientSale();
            CompleteSale.SearchedProducts = new List<Models.Products.ProductSaleSearch>();
            CompleteSale.SaledProducts = new System.Collections.ObjectModel.ObservableCollection<Models.Products.ProductSaleSaled>();
            CompleteSale.NeedFactura = false;
            CompleteSale.SalerID = 1;
            ProductCriterialSearch = "";
        }
    }
}
