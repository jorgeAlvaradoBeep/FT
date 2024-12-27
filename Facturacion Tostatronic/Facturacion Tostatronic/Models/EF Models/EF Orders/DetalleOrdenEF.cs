using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion_Tostatronic.Models.EF_Models.EF_Orders
{
    public class DetalleOrdenEF : INotifyPropertyChanged
    {
        // 1) Estas propiedades NO lanzan OnPropertyChanged 
        //    (según tu solicitud).
        public int ordenId { get; set; }
        public string codigoProducto { get; set; }
        public int cantidad { get; set; }
        public float precio { get; set; }

        // 2) Las siguientes propiedades SI lanzan OnPropertyChanged.

        private decimal _porcentaje;
        public decimal porcentaje
        {
            get => _porcentaje;
            set
            {
                if (_porcentaje != value)
                {
                    _porcentaje = value;
                    OnPropertyChanged();
                }
            }
        }

        private decimal _minimo;
        public decimal minimo
        {
            get => _minimo;
            set
            {
                if (_minimo != value)
                {
                    _minimo = value;
                    OnPropertyChanged();
                }
            }
        }

        private decimal _distribuidor;
        public decimal distribuidor
        {
            get => _distribuidor;
            set
            {
                if (_distribuidor != value)
                {
                    _distribuidor = value;
                    OnPropertyChanged();
                }
            }
        }

        private decimal _publico;
        public decimal publico
        {
            get => _publico;
            set
            {
                if (_publico != value)
                {
                    _publico = value;
                    OnPropertyChanged();
                }
            }
        }

        public static DetalleOrdenEF ToDetalleOrden(ProductOrderComplete source)
        {
            if (source == null) return null;

            return new DetalleOrdenEF
            {
                // Mapeo simple de propiedades "idénticas"
                ordenId = source.IDOrden,
                codigoProducto = source.CodigoProducto,
                cantidad = source.Cantidad,
                precio = (float)source.Costo,

                // Mapeo de las propiedades que notificarán cambios
                // Asumimos que "porcentaje" se obtiene de "PorcentajeOrden"
                porcentaje = (decimal)source.PorcentajeOrden,
                minimo = source.Minimo,
                distribuidor = source.Distribuidor,
                publico = source.Publico
            };
        }

        // 3) Implementación de INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
