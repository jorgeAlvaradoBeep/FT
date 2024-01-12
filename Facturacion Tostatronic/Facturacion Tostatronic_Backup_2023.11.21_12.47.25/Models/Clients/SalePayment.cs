using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion_Tostatronic.Models.Clients
{
    class SalePayment
    {
        public int ID { get; set; }
        public float Payment { get; set; }
        public string Date { get; set; }
        public bool Payed { get; set; }

        public SalePayment(int id, float payment, string date, bool payed)
        {
            ID = id;
            Payment = payment;
            Date = date;
            Payed = payed;
        }
    }
}
