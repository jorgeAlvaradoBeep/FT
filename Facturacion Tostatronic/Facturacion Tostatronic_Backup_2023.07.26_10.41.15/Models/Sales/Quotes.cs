using Facturacion_Tostatronic.Models.Products;
using Facturacion_Tostatronic.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion_Tostatronic.Models.Sales
{
    public class Quotes : BaseNotifyPropertyChanged
    {
        private int idQuote;

        public int id_cotizacion
        {
            get { return idQuote; }
            set { idQuote = value; }
        }

        public string nombres { get; set; }
        public string apellido_paterno { get; set; }
        public string apellido_materno { get; set; }
        public string fecha_cotizacion { get; set; }
        public float impuesto { get; set; }
        public string rfc { get; set; }
        public int id_cliente { get; set; }
        public int id_tipo_cliente { get; set; }

        public string CompleteName 
        { 
            get { return nombres + " " + apellido_paterno + " " + apellido_materno; }
        }

        private ObservableCollection<ProductSaleSaled> saleSearches;

        public ObservableCollection<ProductSaleSaled> SaledProducts
        {
            get { return saleSearches; }
            set { SetValue(ref saleSearches, value); }
        }


    }
}
