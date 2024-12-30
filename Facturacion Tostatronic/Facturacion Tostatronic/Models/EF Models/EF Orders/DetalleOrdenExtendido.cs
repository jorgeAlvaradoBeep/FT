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
        private decimal _minimo;
        public new decimal minimo
        {
            get => _minimo;
            set
            {
                if (_minimo != value)
                {
                    _minimo = value;
                    OnPropertyChanged();
                    if (_minimo != 0)
                    {
                        // Realizar la operación deseada
                        SubMinimo = cantidad * _minimo;
                        var minAI = (float)_minimo / 1.16f;
                        minAI = (float)Math.Round(minAI, 2);
                        var costAI = (float)precio / 1.16f;
                        costAI = (float)Math.Round(costAI, 2);
                        PorcentajeMinimo = (minAI - costAI) / minAI;
                    }
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
        private decimal _distribuidor;
        public new decimal distribuidor
        {
            get => _distribuidor;
            set
            {
                if (_distribuidor != value)
                {
                    _distribuidor = value;
                    OnPropertyChanged();
                    if (_distribuidor != 0)
                    {
                        SubDistribuidor = cantidad * _distribuidor;
                        PorcentajeDistribuidor = (((float)_distribuidor / 1.16f) - (float)precio / 1.16f) / ((float)_distribuidor / 1.16f);
                    }
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
        private decimal _publico;
        public new decimal publico
        {
            get => _publico;
            set
            {
                if (_publico != value)
                {
                    _publico = value;
                    OnPropertyChanged();
                    if (_publico != 0)
                    {
                        SubPublico = cantidad * _publico;
                        PorcentajePublico = (((float)_publico / 1.16f) - (float)precio / 1.16f) / ((float)_publico / 1.16f);
                    }
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
