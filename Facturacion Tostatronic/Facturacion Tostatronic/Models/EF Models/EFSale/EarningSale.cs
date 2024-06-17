using Facturacion_Tostatronic.Models.EF_Models.EFClientF;
using Facturacion_Tostatronic.Models.EF_Models.EFEarnings;
using Facturacion_Tostatronic.Models.EF_Models.EFProduct;
using Facturacion_Tostatronic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion_Tostatronic.Models.EF_Models.EFSale
{
    public class EarningSale: BaseNotifyPropertyChanged
    {
        public EarningSale()
        {
            Comisiones = new EFComisiones();
            Comisiones.Plataforma = new Plataforma();
            Comisiones.MetodoPago = new EFMetodoPago();
        }
        public int idVenta { get; set; }
        public int idUsuario { get; set; }
        public int idCliente { get; set; }
        public bool pagada { get; set; }
        public string fechaDeVenta { get; set; }
        public bool cancelada { get; set; }
        public double impuesto { get; set; }
        public int facturada { get; set; }
        public float subTotal { get; set; }
        public float iva { get; set; }
        public float total { get; set; }
        public float ivaPagada { get; set; }
        public float ivaAPagar { get; set; }
        private float ivaRetenido;

        public float IVARetenido
        {
            get { return ivaRetenido; }
            set 
            { 
                SetValue(ref ivaRetenido, value);
                IVAFinal = ivaAPagar - IVARetenido;
            }
        }
        private float ivaFinal;

        public float IVAFinal
        {
            get { return ivaFinal; }
            set { SetValue(ref ivaFinal, value); }
        }
        private float isrRetenido;

        public float ISRRetenido
        {
            get { return isrRetenido; }
            set { SetValue(ref isrRetenido, value); getTotal(); }
        }
        //Seccion de las comisiones
        public EFComisiones Comisiones { get; set; }


        private float pGanancia;

        public float PGanancia
        {
            get { return pGanancia; }
            set { SetValue(ref pGanancia, value); }
        }
        private float costoTotal;

        public float Costototal
        {
            get { return costoTotal; }
            set { SetValue(ref costoTotal, value); }
        }

        public List<EFSaleProducts> ProductosDeVenta { get; set; }
        public EFClient idClienteNavigation { get; set; }

        public void getTotal()
        {
            if(Costototal > 0)
            {
                Comisiones.Ganancia = total - Costototal - ivaAPagar - ISRRetenido - Comisiones.Comision - Comisiones.Envio - IVARetenido;
                PGanancia = ((Comisiones.Ganancia ?? 0.0f)/Costototal)*100;
            }
                
        }
    }
}
