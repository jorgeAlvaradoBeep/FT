using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion_Tostatronic.Models.EF_Models.EF_Orders
{
    public class DetalleOrdenExtendido : DetalleOrdenEF
    {
        private string _nombre;
        public string Nombre
        {
            get => _nombre;
            set
            {
                if (_nombre != value)
                {
                    _nombre = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _nuevo;
        public bool Nuevo
        {
            get => _nuevo;
            set
            {
                if (_nuevo != value)
                {
                    _nuevo = value;
                    OnPropertyChanged();
                }
            }
        }
        //Seccion de calculos

        private decimal minimoRecomendado;

        public decimal MinimoRecomendado
        {
            get => minimoRecomendado;
            set
            {
                if (minimoRecomendado != value)
                {
                    minimoRecomendado = value;
                    OnPropertyChanged();
                }
            }
        }

        private decimal subMinimo;

        public decimal SubMinimo
        {
            get => subMinimo;
            set
            {
                if (subMinimo != value)
                {
                    subMinimo = value;
                    OnPropertyChanged();
                }
            }
        }

        private float porcentajeMinimo;

        public float PorcentajeMinimo
        {
            get => porcentajeMinimo;
            set
            {
                if (porcentajeMinimo != value)
                {
                    porcentajeMinimo = value;
                    OnPropertyChanged();
                }
            }
        }

        private decimal distribuidorRecomendado;

        public decimal DistribuidorRecomendado
        {
            get => distribuidorRecomendado;
            set
            {
                if (distribuidorRecomendado != value)
                {
                    distribuidorRecomendado = value;
                    OnPropertyChanged();
                }
            }
        }
        private float porcentajeDistribuidor;

        public float PorcentajeDistribuidor
        {
            get => porcentajeDistribuidor;
            set
            {
                if (porcentajeDistribuidor != value)
                {
                    porcentajeDistribuidor = value;
                    OnPropertyChanged();
                }
            }
        }

        private decimal subDistribuidor;

        public decimal SubDistribuidor
        {
            get => subDistribuidor;
            set
            {
                if (subDistribuidor != value)
                {
                    subDistribuidor = value;
                    OnPropertyChanged();
                }
            }
        }
        private float porcentajePublico;
        private decimal subPublico;
        private decimal publicoRecomendado;

        public float PorcentajePublico
        {
            get => porcentajePublico;
            set
            {
                if (porcentajePublico != value)
                {
                    porcentajePublico = value;
                    OnPropertyChanged();
                }
            }
        }

        public decimal SubPublico
        {
            get => subPublico;
            set
            {
                if (subPublico != value)
                {
                    subPublico = value;
                    OnPropertyChanged();
                }
            }
        }

        public decimal PublicoRecomendado
        {
            get => publicoRecomendado;
            set
            {
                if (publicoRecomendado != value)
                {
                    publicoRecomendado = value;
                    OnPropertyChanged();
                }
            }
        }

        public ProductOrderComplete ToProductOrderComplete()
        {
            return new ProductOrderComplete
            {
                // Mapeo simple de propiedades "idénticas"
                IDOrden = ordenId,
                CodigoProducto = codigoProducto,
                Cantidad = cantidad,
                Costo = (decimal)precio,
                NombreEs = Nombre,

                // Mapeo de las propiedades que notificarán cambios
                // Asumimos que "porcentaje" se obtiene de "PorcentajeOrden"
                Minimo = minimo,
                Distribuidor = distribuidor,
                Publico = publico
            };
        }
    }
}
