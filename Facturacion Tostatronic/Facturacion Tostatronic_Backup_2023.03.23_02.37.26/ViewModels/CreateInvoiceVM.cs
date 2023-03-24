using Facturacion_Tostatronic.Models;
using Facturacion_Tostatronic.Models.CFDI;
using Facturacion_Tostatronic.Models.Clients;
using Facturacion_Tostatronic.Services;
using Facturacion_Tostatronic.ViewModels.Commands;
using Facturacion_Tostatronic.Views;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Facturacion_Tostatronic.ViewModels
{
    public class CreateInvoiceVM : BaseNotifyPropertyChanged, IPageViewModel
    {
        public string Name { get; set; } = "CreateInvoiceVM";

        private bool gettingData;

        public bool GettingData
        {
            get { return gettingData; }
            set { SetValue(ref gettingData, value); }
        }

        private string invoiceNumber;

        public string InvoiceNumber
        {
            get { return invoiceNumber; }
            set { SetValue(ref invoiceNumber, value); }
        }

        private DateTime selectedDate;

        public DateTime SelectedDate
        {
            get { return selectedDate; }
            set { selectedDate = value; }
        }

        private ObservableCollection<Sale> sales;

        public ObservableCollection<Sale> Sales
        {
            get { return sales; }
            set { SetValue(ref sales, value); }
        }
        //En esta propiedad validaremos si los controles de seleccion estan disponibles o no
        private bool dataEntranceAvailable;

        public bool DataEntranceSavailable
        {
            get { return dataEntranceAvailable; }
            set { SetValue(ref dataEntranceAvailable, value); }
        }


        public int SelectedSale { get; set; }

        private List<PaymentMethod> paymentMethod;

        public List<PaymentMethod> PaymentMethod
        {
            get { return paymentMethod; }
            set { SetValue(ref paymentMethod,value); }
        }
        private List<PaymentForm>paymentForm;

        public List<PaymentForm> PaymentForm
        {
            get { return paymentForm; }
            set { SetValue(ref paymentForm, value); }
        }

        private List<RegimenFiscal> regimenFiscal;

        public List<RegimenFiscal> RegimenFiscal
        {
            get { return regimenFiscal; }
            set { SetValue(ref regimenFiscal, value); }
        }
        private List<CFDIUse> cFDIUse;

        public List<CFDIUse> CFDIUse
        {
            get { return cFDIUse; }
            set { SetValue(ref cFDIUse, value); }
        }
        private RegimenFiscal selectedRegimen;

        public RegimenFiscal SelectedRegimen
        {
            get { return selectedRegimen; }
            set 
            {
                SetValue(ref selectedRegimen, value);
                if (value != null)
                {
                    CompleteSale.InvoiceData.RegimenFiscal = SelectedRegimen.RegimenFiscalP;
                }
            }
        }
        private CFDIUse selectedCFDIUse;

        public CFDIUse SelectedCFDIUse
        {
            get { return selectedCFDIUse; }
            set
            {
                if (value != null)
                {
                    SetValue(ref selectedCFDIUse, value);
                    if (value != null)
                        CompleteSale.InvoiceData.UsoCFDI = SelectedCFDIUse.CFDIUseP;
                }
            }
        }

        private PaymentMethod selectedPaymentMethod;

        public PaymentMethod SelectedPaymentMethod
        {
            get { return selectedPaymentMethod; }
            set
            {
                if (value != null)
                {
                    SetValue(ref selectedPaymentMethod, value);
                    if(value != null)
                        CompleteSale.InvoiceData.MetodoDePago = SelectedPaymentMethod.PaymentMethodP;
                }
            }
        }

        private PaymentForm selectedPaymentForm;

        public PaymentForm SelectedPaymentForm
        {
            get { return selectedPaymentForm; }
            set { SetValue(ref selectedPaymentForm, value); }
        }


        public CompleteSale CompleteSale { get; set; }

        public SearchSaleCommand SearchSaleCommand { get; set; }
        public SaleSelectedCommand SaleSelectedCommand { get; set; }
        public SelectedDateChangedCommand SelectedDateChangedCommand { get; set; }
        public CreateNewInvoiceCommand CreateNewInvoiceCommand { get; set; }
        public SetSelectedCFDIUseCommando SetSelectedCFDIUseCommando { get; set; }

        public CreateInvoiceVM()
        {
            InitialiceObjects();
            //Aqui inicializamos los comandos
            SearchSaleCommand = new SearchSaleCommand(this);
            SaleSelectedCommand = new SaleSelectedCommand(this);
            SelectedDateChangedCommand = new SelectedDateChangedCommand(this);
            CreateNewInvoiceCommand = new CreateNewInvoiceCommand(this);
            SetSelectedCFDIUseCommando = new SetSelectedCFDIUseCommando(this);
            SelectedDateChangedCommand.Execute(null);
        }

        public void InitialiceObjects() 
        {
            Sales = new ObservableCollection<Sale>();
            SelectedDate = DateTime.Now;
            CompleteSale = new CompleteSale
            {
                Products = new List<Product>(),
                InvoiceData = new InvoiceData(),
                Client = new Client()
            };
            //Aqui inicializamos los datos de las diferentes opciones de la factura
            CFDIUse = new List<CFDIUse>();
            PaymentForm = new List<PaymentForm>();
            PaymentMethod = new List<PaymentMethod>();
            RegimenFiscal = new List<RegimenFiscal>();
            //Al inicio no se permite la seleccion de datos
            DataEntranceSavailable = false;
        }

        public async void GetSalesData(string objectName, string keyString)
        {
            int invoiceNumber;
            if(int.TryParse(InvoiceNumber, out invoiceNumber))
            {
                GettingData = true;
                Response r = await WebService.GetData(objectName, keyString, URLData.sales);
                if (r.statusCode == 404)
                {
                    if (!string.IsNullOrEmpty(r.message))
                        MessageBox.Show(r.message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    else
                        MessageBox.Show("No existen registros de ese folio", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    GettingData = false;
                    Sales = new ObservableCollection<Sale>();
                    return;
                }
                var t = r.data;
                List<Sale> v = ((JArray)t).Select(x => new Sale
                {
                    idSale = (int)x["idSale"],
                    rfc = (string)x["rfc"],
                    date = (string)x["date"]
                }).ToList();
                Sales = new ObservableCollection<Sale>(v);
                GettingData = false;
            }
            else
                MessageBox.Show("El campo de busqueda esta vacio.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public async void GetSalesDataByDate(string objectName, string keyString)
        {
            GettingData = true;
            Response r = await WebService.GetData(objectName, keyString, URLData.sales);
            InvoiceNumber = string.Empty;
            if(r.statusCode==404)
            {
                if (!string.IsNullOrEmpty(r.message))
                    MessageBox.Show(r.message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                GettingData = false;
                Sales = new ObservableCollection<Sale>();
                return;
            }
            var t = r.data;
            List<Sale> v = ((JArray)t).Select(x => new Sale
            {
                idSale = (int)x["idSale"],
                rfc = (string)x["rfc"],
                date = (string)x["date"]
            }).ToList();
            Sales = new ObservableCollection<Sale>(v);
            GettingData = false;
        }

        public async void GetProductsFromSale(string objectName, string keyString, string rfc)
        {
            GettingData = true;
            Response r = await WebService.GetData(objectName, keyString, URLData.products);
            Response rc = await WebService.GetData("rfc", rfc, URLData.clients);
            if(PaymentMethod.Count==0)
            {
                Response rmp = await WebService.GetDataForInvoice(URLData.payment_method);
                if (rmp.succes)
                {
                    PaymentMethod = JsonConvert.DeserializeObject<List<PaymentMethod>>(rmp.data.ToString());
                }
            }
            if (CFDIUse.Count == 0)
            {
                Response rmp = await WebService.GetDataForInvoice(URLData.cfdi_use);
                if (rmp.succes)
                {
                    CFDIUse = JsonConvert.DeserializeObject<List<CFDIUse>>(rmp.data.ToString());
                }
            }
            if (PaymentForm.Count == 0)
            {
                Response rmp = await WebService.GetDataForInvoice(URLData.payment_form);
                if (rmp.succes)
                {
                    PaymentForm = JsonConvert.DeserializeObject<List<PaymentForm>>(rmp.data.ToString());
                }
            }
            if (RegimenFiscal.Count == 0)
            {
                Response rmp = await WebService.GetDataForInvoice(URLData.regimenFiscalesNet);
                if (rmp.succes)
                {
                    RegimenFiscal = JsonConvert.DeserializeObject<List<RegimenFiscal>>(rmp.data.ToString());
                }
            }
            InvoiceNumber = string.Empty;
            if (r.statusCode == 404)
            {
                if(!string.IsNullOrEmpty(r.message))
                    MessageBox.Show(r.message,"Error", MessageBoxButton.OK, MessageBoxImage.Error);
                else
                    MessageBox.Show("No existen registros de esa venta", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                GettingData = false;
                Sales = new ObservableCollection<Sale>();
                return;
            }
            var t = r.data;
            CompleteSale.Products = ((JArray)t).Select(x => new Product
            {
                idProduct = (string)x["idProduct"],
                name = (string)x["name"],
                quantity = (string)x["quantity"],
                priceAtMoment = (string)x["priceAtMoment"],
                satCode = (string)x["satCode"]
            }).ToList();
            CompleteSale.Client = JsonConvert.DeserializeObject<Client>(rc.data.ToString());
            //Aqui Seleccionamos el regimen fiscal
            Response rmps = await WebService.GetDataForInvoice(URLData.getClientByRFC+CompleteSale.Client.Rfc);
            if (rmps.succes)
            {
                ClientComplete clientComplete = JsonConvert.DeserializeObject<List<ClientComplete>>(rmps.data.ToString())[0];
                if(clientComplete != null)
                {
                    if(!string.IsNullOrEmpty(clientComplete.RegimenFiscal))
                    {
                        SelectedRegimen = RegimenFiscal.Where(x => x.RegimenFiscalP == clientComplete.RegimenFiscal).First();
                    }
                    else
                        SelectedRegimen = null;
                }
            }
            else
                SelectedRegimen=null;
            GettingData = false;
            DataEntranceSavailable = true;
        }

        public async Task<bool> CreateAndInsertInvoice()
        {
            GettingData = true;
            List<ProductoSat> articulos = new List<ProductoSat>();
            foreach(Product a in CompleteSale.Products)
            {
                if (string.IsNullOrEmpty(a.satCode))
                    a.satCode = "01010101";
                ProductoSat ps = new ProductoSat(a.name,a.satCode,float.Parse(a.quantity),float.Parse(a.priceAtMoment),float.Parse(a.SubTotal));
                articulos.Add(ps);
            }

            string error = await Facturacion.CreaFactura(CompleteSale.Folio, CompleteSale.InvoiceData.FormaDePago, 
                CompleteSale.InvoiceData.MetodoDePago,articulos,CompleteSale.SubTotal, CompleteSale.Client.Rfc, 
                CompleteSale.Client.CompleteName, CompleteSale.InvoiceData.UsoCFDI, CompleteSale.Client.Email, 
                CompleteSale.Tax, CompleteSale.Total, CompleteSale.InvoiceData.RegimenFiscal, CompleteSale.Client.CP);
            GettingData = false;
            if (error == "")
                return true;
            MessageBox.Show(error, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }
    }
}
