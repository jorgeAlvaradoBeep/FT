using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion_Tostatronic.Models.MercadoLibre
{
    public class MercadoLibreOrdersRoot
    {
        [JsonProperty("results")]
        public List<MercadoLibreOrder> results { get; set; }

        [JsonProperty("paging")]
        public PagingInfo paging { get; set; }
    }

    public class PagingInfo
    {
        [JsonProperty("total")]
        public int Total { get; set; }

        [JsonProperty("offset")]
        public int Offset { get; set; }

        [JsonProperty("limit")]
        public int Limit { get; set; }

        public int TotalPages => Limit == 0 ? 0 : (int)Math.Ceiling((double)Total / Limit);
    }
}
