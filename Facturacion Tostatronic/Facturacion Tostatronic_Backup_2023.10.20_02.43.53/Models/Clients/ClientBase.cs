using Facturacion_Tostatronic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion_Tostatronic.Models.Clients
{
    public class ClientBase: BaseNotifyPropertyChanged
    {
        public int ID { get; set; }
        public int ClientType { get; set; }
        public string Names { get; set; }
        public string FatherLastName { get; set; }
        public string MotherLastName { get; set; }
        public string RFC { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public int ZipCode { get; set; }
        public string Mail { get; set; }
        public string CellphoneNumber { get; set; }


    }
}
