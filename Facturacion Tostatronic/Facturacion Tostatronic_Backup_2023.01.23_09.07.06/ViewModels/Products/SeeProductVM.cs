using Facturacion_Tostatronic.Models.Clients;
using Facturacion_Tostatronic.Models;
using Facturacion_Tostatronic.Services;
using Facturacion_Tostatronic.ViewModels.Commands.Clients.ModifyClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Facturacion_Tostatronic.Models.Products;
using Facturacion_Tostatronic.ViewModels.Commands.ProductsCommands;
using SkiaSharp;

namespace Facturacion_Tostatronic.ViewModels.Products
{
    public class SeeProductVM : BaseNotifyPropertyChanged, IPageViewModel
    {
        public string Name { get; set; } = "SeeProductVM";
        private string searchCriterial;

        public string SearchCriterial
        {
            get { return searchCriterial; }
            set
            {
                SetValue(ref searchCriterial, value);
                ApplyFilter(searchCriterial);
            }
        }

        private bool gettingData;

        public bool GettingData
        {
            get { return gettingData; }
            set { SetValue(ref gettingData, value); }
        }

        private ObservableCollection<UpdateProductM> products;

        public ObservableCollection<UpdateProductM> Products
        {
            get { return products; }
            set { SetValue(ref products, value); }
        }
        public IReadOnlyList<UpdateProductM> _allProducts;

        private bool editing;

        public bool Editing
        {
            get { return editing; }
            set { SetValue(ref editing, value); }
        }
        private UpdateProductM selectedProduct;

        public UpdateProductM SelectedProduct
        {
            get { return selectedProduct; }
            set { SetValue(ref selectedProduct, value); }
        }

        public ModifyProductCommand ModifyProductCommand { get; set; }

        public SeeProductVM()
        {
            GettingData = false;
            Products = new ObservableCollection<UpdateProductM>();
            GettingData = true;
            ModifyProductCommand = new ModifyProductCommand(this);
            Task.Run(() =>
            {
                Response rmp = WebService.GetDataForInvoiceNoAsync(URLData.getProductsNet);
                if (rmp.succes)
                {
                    GetProductsToList(rmp);
                }
                else
                    MessageBox.Show("Error al traer la información solicitada", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                GettingData = false;
            });
            SelectedProduct = null;
        }

        void GetProductsToList(Response res)
        {
            _allProducts = JsonConvert.DeserializeObject<IReadOnlyList<UpdateProductM>>(res.data.ToString());
            foreach (UpdateProductM a in _allProducts)
            {
                Products.Add(a);
            }
        }

        private void ApplyFilter(string textToSearch)
        {
            if (this._allProducts == null)
            {
                return;
            }

            var filteredEmployees = string.IsNullOrEmpty(textToSearch)
                ? this._allProducts
                : this._allProducts.Where(p => p.Nombre.ToLower().Contains(textToSearch.ToLower()) || p.Codigo.ToLower().Contains(textToSearch.ToLower()));

            this.Products.Clear();

            foreach (var employee in filteredEmployees)
            {
                this.Products.Add(employee);
            }

        }
    }
}
