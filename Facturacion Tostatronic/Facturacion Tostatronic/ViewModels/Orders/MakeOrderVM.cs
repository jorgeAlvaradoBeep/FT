using Facturacion_Tostatronic.Models.EF_Models.EF_Orders;
using Facturacion_Tostatronic.Models.Products;
using Facturacion_Tostatronic.Services;
using Facturacion_Tostatronic.ViewModels.Commands.OrderCommands;
using GalaSoft.MvvmLight.Threading;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Facturacion_Tostatronic.ViewModels.Orders
{
    public class MakeOrderVM : BaseNotifyPropertyChanged, IPageViewModel
    {
        #region Properties
        public string Name { get; set; } = "MakeOrderVM";
        private bool gettinData;

        public bool GettingData
        {
            get { return gettinData; }
            set { SetValue(ref gettinData, value); }
        }
        private bool isProductExtractionenabled;

        public bool IsProductExtractionenabled
        {
            get { return isProductExtractionenabled; }
            set { SetValue(ref isProductExtractionenabled, value); }
        }


        public EFOrder Order { get; set; }

        private ObservableCollection<UpdateProductM> allProducts;

        public ObservableCollection<UpdateProductM> AllProducts
        {
            get { return allProducts; }
            set { SetValue(ref allProducts, value); }
        }

        private UpdateProductM selectedProduct;

        public UpdateProductM SelectedProduct
        {
            get { return selectedProduct; }
            set 
            {
                if(value != null)
                {
                    if(value != selectedProduct)
                    {
                        SetValue(ref selectedProduct, value);
                        if (Order.Products.Where(x=> x.nombreES.Equals(selectedProduct.Nombre)).Count() == 0)
                        {
                            EFOrderProduct aux = new EFOrderProduct(selectedProduct.Codigo, selectedProduct.Nombre, selectedProduct.Imagen);
                            Order.Products.Add(aux);
                        }
                        SelectedProduct = null;
                    }
                }
                
            }
        }
        private string progressVal;

        public string ProgressVal
        {
            get { return progressVal; }
            set { SetValue(ref progressVal, value); }
        }
        public readonly SynchronizationContext _syncContext;

        #endregion
        #region CommandsDeclarations
        public GetNewOrderCommand GetNewOrderCommand { get; set; }
        public SaveOrderCommand SaveOrderCommand { get; set; }
        public AddNewRowCommand AddNewRowCommand { get; set; }
        #endregion

        public MakeOrderVM()
        {
            DispatcherHelper.Initialize();
            _syncContext = SynchronizationContext.Current;
            Order = new EFOrder();
            Order.Products = new ObservableCollection<EFOrderProduct>();
            GettingData = false;
            isProductExtractionenabled = true;
            AllProducts = new ObservableCollection<UpdateProductM>();
            GetNewOrderCommand = new GetNewOrderCommand(this);
            SaveOrderCommand = new SaveOrderCommand(this);
            AddNewRowCommand = new AddNewRowCommand(this);
        }
    }
}
