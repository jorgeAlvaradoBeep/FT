using Facturacion_Tostatronic.Models.EF_Models.EF_Orders;
using Facturacion_Tostatronic.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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
        private ObservableCollection<EFOrderProduct> orderProducts;

        public ObservableCollection<EFOrderProduct> OrderProducts
        {
            get { return orderProducts; }
            set { SetValue(ref orderProducts, value); }
        }
        #endregion
        #region CommandsDeclarations
        #endregion

        public MakeOrderVM()
        {
            OrderProducts = new ObservableCollection<EFOrderProduct>();
            GettingData = false;
        }
    }
}
