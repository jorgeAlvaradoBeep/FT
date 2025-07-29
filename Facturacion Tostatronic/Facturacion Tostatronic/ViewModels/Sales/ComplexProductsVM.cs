using Facturacion_Tostatronic.Models.Clients;
using Facturacion_Tostatronic.Models.EF_Models.EFEarnings;
using Facturacion_Tostatronic.Models.Sales;
using Facturacion_Tostatronic.Services;
using Facturacion_Tostatronic.ViewModels.Commands.PDVCommands.ComplexProductsCommands;
using Facturacion_Tostatronic.ViewModels.Commands.PDVCommands.SaleCommands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Facturacion_Tostatronic.ViewModels.Sales
{
    public class ComplexProductsVM : BaseNotifyPropertyChanged, IDataErrorInfo
    {
        #region Comandos
        public CPCallSearchClientCommand CallSearchClientViewCommand { get; set; }
        public CPSearchProductsCommand SearchProductsForSalecommand { get; set; }
        public CPSaleProductCommand SaleProductCommand { get; set; }
        public CPChangeSaledQuantityCommand ChangeSaledQuantityCommand { get; set; }
        public CPDeleteProductCommand DeleteProductFromSalecommand { get; set; }
        public SaveComplexProductCommand SaveComplexProductCommand { get; set; }
        public CPCancelCommand CancelCommand { get; set; }
        public CPSearchProductFromSaleCommand SearchProductFromSalecommand { get; set; }
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

        private bool showComplexProductFields;
        public bool ShowComplexProductFields
        {
            get { return showComplexProductFields; }
            set { SetValue(ref showComplexProductFields, value); }
        }

        private int idCotizacion;
        public int IdCotizacion
        {
            get { return idCotizacion; }
            set { SetValue(ref idCotizacion, value); }
        }

        private string nombre;
        public string Nombre
        {
            get { return nombre; }
            set { SetValue(ref nombre, value); }
        }

        private ObservableCollection<Plataforma> plataformas;
        public ObservableCollection<Plataforma> Plataformas
        {
            get { return plataformas; }
            set { SetValue(ref plataformas, value); }
        }

        private Plataforma selectedPlataforma;
        public Plataforma SelectedPlataforma
        {
            get { return selectedPlataforma; }
            set { SetValue(ref selectedPlataforma, value); }
        }

        private string codigoPlataforma;
        public string CodigoPlataforma
        {
            get { return codigoPlataforma; }
            set { SetValue(ref codigoPlataforma, value); }
        }

        private string sku;
        public string Sku
        {
            get { return sku; }
            set { SetValue(ref sku, value); }
        }

        public bool IvaPricesSet { get; set; }
        #endregion

        #region IDataErrorInfo
        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                string error = null;

                switch (columnName)
                {
                    case nameof(Nombre):
                        if (ShowComplexProductFields && string.IsNullOrWhiteSpace(Nombre))
                            error = "El nombre es requerido";
                        else if (ShowComplexProductFields && Nombre?.Length > 200)
                            error = "El nombre no puede exceder 200 caracteres";
                        break;

                    case nameof(CodigoPlataforma):
                        if (ShowComplexProductFields && string.IsNullOrWhiteSpace(CodigoPlataforma))
                            error = "El código de plataforma es requerido";
                        else if (ShowComplexProductFields && !Regex.IsMatch(CodigoPlataforma ?? "", @"^[a-zA-Z0-9]+$"))
                            error = "El código de plataforma solo puede contener caracteres alfanuméricos";
                        else if (ShowComplexProductFields && CodigoPlataforma?.Length > 350)
                            error = "El código de plataforma no puede exceder 350 caracteres";
                        break;

                    case nameof(Sku):
                        if (ShowComplexProductFields && string.IsNullOrWhiteSpace(Sku))
                            error = "El SKU es requerido";
                        else if (ShowComplexProductFields && !Sku?.StartsWith("CMPLX") == true)
                            error = "El SKU debe comenzar con 'CMPLX'";
                        else if (ShowComplexProductFields && !Regex.IsMatch(Sku ?? "", @"^CMPLX[a-zA-Z0-9]*$"))
                            error = "El SKU debe comenzar con 'CMPLX' seguido solo de caracteres alfanuméricos";
                        else if (ShowComplexProductFields && Sku?.Length > 60)
                            error = "El SKU no puede exceder 60 caracteres";
                        break;

                    case nameof(SelectedPlataforma):
                        if (ShowComplexProductFields && SelectedPlataforma == null)
                            error = "Debe seleccionar una plataforma";
                        break;
                }

                return error;
            }
        }
        #endregion

        public ComplexProductsVM()
        {
            ClientType = new List<string>()
            {
                "Minimo",
                "Distribuidor",
                "Publico"
            };
            InitializeCompleteSale();
            IvaPricesSet = false;
            ShowComplexProductFields = false;
            Plataformas = new ObservableCollection<Plataforma>();
        }

        public void InitializeCompleteSale()
        {
            GettingData = false;
            CallSearchClientViewCommand = new CPCallSearchClientCommand(this);
            SearchProductsForSalecommand = new CPSearchProductsCommand(this);
            SaleProductCommand = new CPSaleProductCommand(this);
            ChangeSaledQuantityCommand = new CPChangeSaledQuantityCommand(this);
            DeleteProductFromSalecommand = new CPDeleteProductCommand(this);
            SaveComplexProductCommand = new SaveComplexProductCommand(this);
            CancelCommand = new CPCancelCommand(this);
            SearchProductFromSalecommand = new CPSearchProductFromSaleCommand(this);

            //Objetos
            CompleteSale = new CompleteSaleM();
            CompleteSale.ClientSale = new ClientSale();
            CompleteSale.SearchedProducts = new List<Models.Products.ProductSaleSearch>();
            CompleteSale.SaledProducts = new System.Collections.ObjectModel.ObservableCollection<Models.Products.ProductSaleSaled>();
            CompleteSale.NeedFactura = false;
            CompleteSale.SalerID = 1;
            ProductCriterialSearch = "";
            IvaPricesSet = false;
        }

        public void ResetComplexProductFields()
        {
            Nombre = "";
            CodigoPlataforma = "";
            Sku = "CMPLX";
            SelectedPlataforma = null;
            IdCotizacion = 0;
            ShowComplexProductFields = false;
        }
    }
}