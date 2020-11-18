using Facturacion_Tostatronic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion_Tostatronic.Models
{
    public class Sale : BaseNotifyPropertyChanged
    {
        private int _idSale;

        public int idSale
        {
            get { return _idSale; }
            set { SetValue(ref _idSale,value); }
        }
        private string _rfc;

        public string rfc
        {
            get { return _rfc; }
            set { SetValue(ref _rfc, value); }
        }
        private string _date;

        public string date
        {
            get { return _date; }
            set { SetValue(ref _date, value); }
        }
    }
}
