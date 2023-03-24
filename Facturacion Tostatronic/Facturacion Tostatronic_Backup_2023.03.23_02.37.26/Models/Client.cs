using Facturacion_Tostatronic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion_Tostatronic.Models
{
    public class Client: BaseNotifyPropertyChanged
    {
        private string rfc;

        public string Rfc
        {
            get { return rfc; }
            set { SetValue(ref rfc, value); }
        }

        private string completeName;

        public string CompleteName
        {
            get { return completeName; }
            set { SetValue(ref completeName, value); }
        }
        private string email;

        public string Email
        {
            get { return email; }
            set { SetValue(ref email, value); }
        }
        private string cp;

        public string CP
        {
            get { return cp; }
            set { SetValue(ref cp, value); }
        }
    }
}
