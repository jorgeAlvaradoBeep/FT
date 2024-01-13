using Facturacion_Tostatronic.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion_Tostatronic.Models.EF_Models.EF_Orders
{
    public class EFOrder: BaseNotifyPropertyChanged
    {
		private ObservableCollection<EFOrderProduct> products;

		public ObservableCollection<EFOrderProduct> Products
		{
			get { return products; }
			set { SetValue(ref products, value); }
		}
		private float subTotal;

		public float SubTotal
		{
			get { return subTotal; }
			set { SetValue(ref subTotal, value); }
		}
		private float shippingFee;

		public float ShippingFee
		{
			get { return shippingFee; }
			set { SetValue(ref shippingFee, value); }
		}
		private float total;

		public float Total
		{
			get { return total; }
			set { SetValue(ref total, value); }
		}


	}
}
