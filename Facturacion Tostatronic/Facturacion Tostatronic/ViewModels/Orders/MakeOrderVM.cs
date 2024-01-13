using Facturacion_Tostatronic.Models.EF_Models.EF_Orders;
using Facturacion_Tostatronic.Services;
using Facturacion_Tostatronic.ViewModels.Commands.OrderCommands;
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
        private bool isProductExtractionenabled;

        public bool IsProductExtractionenabled
        {
            get { return isProductExtractionenabled; }
            set { SetValue(ref isProductExtractionenabled, value); }
        }


        public EFOrder Order { get; set; }
        #endregion
        #region CommandsDeclarations
        public GetNewOrderCommand GetNewOrderCommand { get; set; }
        #endregion

        public MakeOrderVM()
        {
            Order = new EFOrder();
            Order.Products = new ObservableCollection<EFOrderProduct>();
            GettingData = false;
            isProductExtractionenabled = true;
            GetNewOrderCommand = new GetNewOrderCommand(this);
        }
    }
}
