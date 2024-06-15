using Facturacion_Tostatronic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion_Tostatronic.Models.EF_Models.EFEarnings
{
    public  class EFComisiones: BaseNotifyPropertyChanged
    {
        private int id;
        public int Id
        {
            get { return id; }
            set { SetValue(ref id, value); }
        }

        private int ventaId;
        public int VentaId
        {
            get { return ventaId; }
            set { SetValue(ref ventaId, value); }
        }

        private string folioPlataforma;
        public string FolioPlataforma
        {
            get { return folioPlataforma; }
            set { SetValue(ref folioPlataforma, value); }
        }
        private int plataformaId;

        public int PlataformaId
        {
            get { return plataformaId; }
            set { SetValue(ref plataformaId, value); }
        }

        private Plataforma plataforma;
        public Plataforma Plataforma
        {
            get { return plataforma; }
            set 
            { 
                SetValue(ref plataforma, value); 
                if(Plataforma!=null)
                {
                    if (string.IsNullOrEmpty(Plataforma.Nombre))
                        return;
                    if(Plataforma.Nombre.Equals("Tienda"))
                    {
                        FolioPlataforma = VentaId.ToString();
                    }
                }
            }
        }
        private int metodoPagoId;

        public int MetodoPagoId
        {
            get { return metodoPagoId; }
            set { SetValue(ref metodoPagoId, value); }
        }
        private EFMetodoPago metodoPago;
        public EFMetodoPago MetodoPago
        {
            get { return metodoPago; }
            set 
            { 
                SetValue(ref metodoPago, value); 
            }
        }

        private float comision;
        public float Comision
        {
            get { return comision; }
            set { SetValue(ref comision, value); }
        }

        private float envio;
        public float Envio
        {
            get { return envio; }
            set { SetValue(ref envio, value); }
        }

        private float? ganancia;
        public float? Ganancia
        {
            get { return ganancia; }
            set { SetValue(ref ganancia, value); }
        }
    }
}
