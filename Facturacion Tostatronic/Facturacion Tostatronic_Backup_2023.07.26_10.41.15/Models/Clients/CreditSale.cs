using Facturacion_Tostatronic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion_Tostatronic.Models.Clients
{
    public class CreditSale : BaseNotifyPropertyChanged
    {
        public int IDSale { get; set; }
        public string Date { get; set; }
        public float SubTotalAmount { get; set; }
        public float TAX { get; set; }
        public float Total
        {
            get { return SubTotalAmount*TAX; }
        }
        private float amountPayed;

        public float AmountPayed
        {
            get { return amountPayed; }
            set 
            { 
                SetValue(ref amountPayed, value);
                AmountToPay = Total - AmountPayed;
            }

        }

        private float amountToPay;
        public float AmountToPay
        {
            get { return amountToPay; }
            set { SetValue(ref amountToPay, value); }
        }


    }
}
