using Facturacion_Tostatronic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion_Tostatronic.Models.EF_Models.EFFactura
{
    public class FacturaCompleta: BaseNotifyPropertyChanged
    {
        public string Folio { get; set; }
        private Client client;

        public Client Client
        {
            get { return client; }
            set { SetValue(ref client, value); }
        }
        private List<ProductFactura> products;

        public List<ProductFactura> Products
        {
            get { return products; }
            set
            {
                SetValue(ref products, value);
                SubTotal = (float)Math.Round((double)GetSubTotal(), 2);
                Tax = (float)Math.Round((double)SubTotal * .16f, 2);
                Total = (float)Math.Round((double)SubTotal * 1.16f, 2);
            }
        }

        private float subTotal;

        public float SubTotal
        {
            get { return subTotal; }
            set { SetValue(ref subTotal, value); }
        }

        private float tax;

        public float Tax
        {
            get { return tax; }
            set { SetValue(ref tax, value); }
        }

        private float total;

        public float Total
        {
            get { return total; }
            set { SetValue(ref total, value); }
        }
        public InvoiceData InvoiceData { get; set; }

        decimal GetSubTotal()
        {
            decimal total = 0;
            foreach (ProductFactura a in Products)
            {
                total += a.SubTotal;
            }
            return total;
        }
    }
}
