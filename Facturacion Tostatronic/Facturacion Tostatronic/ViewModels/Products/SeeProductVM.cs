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
using Facturacion_Tostatronic.ViewModels.Commands.ProductsCommands.SeeProductsCommands;

namespace Facturacion_Tostatronic.ViewModels.Products
{
    public class SeeProductVM : BaseNotifyPropertyChanged, IPageViewModel
    {
        public string Name { get; set; } = "SeeProductVM";
        private string searchCriterial;

        //Algoritmo de busqueda para productos por indices
        public readonly Dictionary<string, List<UpdateProductM>> _nombreIndex;
        public readonly Dictionary<string, List<UpdateProductM>> _codigoIndex;

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
        public SaveProductInfoCommand SaveProductInfoCommand { get; set; }
        public DeleteProductCommand DeleteProductCommand { get; set; }
        public SeeProductsLoadedCommand SeeProductsLoadedCommand { get; set; }

        public SeeProductVM()
        {
            GettingData = false;
            Products = new ObservableCollection<UpdateProductM>();
            ModifyProductCommand = new ModifyProductCommand(this);
            SaveProductInfoCommand = new SaveProductInfoCommand(this);
            DeleteProductCommand = new DeleteProductCommand(this);
            SeeProductsLoadedCommand = new SeeProductsLoadedCommand(this);
            SelectedProduct = null;
            _nombreIndex = new Dictionary<string, List<UpdateProductM>>();
            _codigoIndex = new Dictionary<string, List<UpdateProductM>>();
        }
        //private void ApplyFilter(string textToSearch)
        //{
        //    if (this._allProducts == null)
        //    {
        //        return;
        //    }

        //    var filteredEmployees = string.IsNullOrEmpty(textToSearch)
        //        ? this._allProducts
        //        : this._allProducts.Where(p => p.Nombre.ToLower().Contains(textToSearch.ToLower()) || p.Codigo.ToLower().Contains(textToSearch.ToLower()));

        //    this.Products.Clear();

        //    foreach (var employee in filteredEmployees)
        //    {
        //        this.Products.Add(employee);
        //    }
        //}
        public void AddToIndex(Dictionary<string, List<UpdateProductM>> index, string key, UpdateProductM product)
        {
            if (!index.ContainsKey(key))
            {
                index[key] = new List<UpdateProductM>();
            }
            index[key].Add(product);
        }
        public void ApplyFilter(string textToSearch)
        {
            if (_allProducts == null)
            {
                return;
            }

            IEnumerable<UpdateProductM> filteredProducts;
            if (string.IsNullOrEmpty(textToSearch))
            {
                filteredProducts = _allProducts;
            }
            else
            {
                textToSearch = textToSearch.ToLower();
                var filteredByName = _nombreIndex.Where(kv => kv.Key.Contains(textToSearch)).SelectMany(kv => kv.Value);
                var filteredByCodigo = _codigoIndex.Where(kv => kv.Key.Contains(textToSearch)).SelectMany(kv => kv.Value);

                filteredProducts = filteredByName.Concat(filteredByCodigo).Distinct();
            }

            Products.Clear();
            foreach (var product in filteredProducts)
            {
                Products.Add(product);
            }
        }
    }
}
