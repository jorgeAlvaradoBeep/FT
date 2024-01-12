using Facturacion_Tostatronic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion_Tostatronic.Models.Sales
{
    public class PaymentM : BaseNotifyPropertyChanged
    {
        private float total;

        public float Total
        {
            get { return total; }
            set { total = value; }
        }
        private float payment;

        public float Payment
        {
            get { return payment; }
            set 
            { 
                SetValue(ref payment, value);
                float r = Total - Payment;
                if (r >= 0)
                    Remaining = r;
                else
                    Remaining = 0;
                float c = Payment - Total;
                if (c < 0)
                    Change = 0;
                else
                    Change = c;
            }
        }
        private float remaining;

        public float Remaining
        {
            get { return remaining; }
            set { SetValue(ref remaining, value); }
        }
        private float change;

        public float Change
        {
            get { return change; }
            set { SetValue(ref change, value); }
        }
        private bool pagado;

        public bool Pagado
        {
            get { return pagado; }
            set { SetValue(ref pagado, value); }
        }
        private bool cancel;

        public bool Cancel
        {
            get { return cancel; }
            set { SetValue(ref cancel, value); }
        }

        public PaymentM()
        {
            Pagado = false;
            Cancel = false;
        }
        public PaymentM(float total)
        {
            Total = total;
            Pagado = false;
            Cancel = false;
        }
    }
}
