using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion_Tostatronic.Models.EF_Models.EFClientF
{
    public class EFClient
    {
        public int idCliente { get; set; }
        public int idTipoCliente { get; set; }
        public string nombres { get; set; }
        public string apellidoPaterno { get; set; }
        public string apellidoMaterno { get; set; }
        public string rfc { get; set; }
        public string telefono { get; set; }
        public string domicilio { get; set; }
        public int codigoPostal { get; set; }
        public string colonia { get; set; }
        public string correoElectronico { get; set; }
        public string celular { get; set; }
        public string descripcion { get; set; }
        public string regimenFiscal { get; set; }
        public bool eliminado { get; set; }

        public string CompleteName
        {
            get
            {
                if(!string.IsNullOrEmpty(apellidoPaterno))
                {
                    if (!string.IsNullOrEmpty(apellidoMaterno))
                        return $"{nombres} {apellidoPaterno} {apellidoMaterno}";
                    else
                        return $"{nombres} {apellidoPaterno}";
                }
                else
                    return $"{nombres}";
            }
        }
    }
}
