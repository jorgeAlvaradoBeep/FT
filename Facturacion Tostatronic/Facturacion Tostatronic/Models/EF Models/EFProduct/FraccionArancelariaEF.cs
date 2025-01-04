using Facturacion_Tostatronic.Models.EF_Models.EF_Orders;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Facturacion_Tostatronic.Models.EF_Models.EFProduct
{
    public class FraccionArancelaria : INotifyPropertyChanged
    {
        private string _codigo;
        private string _descripcion;
        private string _composicion;
        private string _uso;
        private string _comentarios;
        private string _fraccion;
        private decimal? _valorDeclarado;
        private bool _nuevo;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Codigo
        {
            get => _codigo;
            set
            {
                if (_codigo != value)
                {
                    _codigo = value;
                    OnPropertyChanged(nameof(Codigo));
                }
            }
        }

        public string Descripcion
        {
            get => _descripcion;
            set
            {
                if (_descripcion != value)
                {
                    _descripcion = value;
                    OnPropertyChanged(nameof(Descripcion));
                }
            }
        }

        public string Composicion
        {
            get => _composicion;
            set
            {
                if (_composicion != value)
                {
                    _composicion = value;
                    OnPropertyChanged(nameof(Composicion));
                }
            }
        }

        public string Uso
        {
            get => _uso;
            set
            {
                if (_uso != value)
                {
                    _uso = value;
                    OnPropertyChanged(nameof(Uso));
                }
            }
        }

        public string Comentarios
        {
            get => _comentarios;
            set
            {
                if (_comentarios != value)
                {
                    _comentarios = value;
                    OnPropertyChanged(nameof(Comentarios));
                }
            }
        }

        public string Fraccion
        {
            get => _fraccion;
            set
            {
                if (_fraccion != value)
                {
                    _fraccion = value;
                    OnPropertyChanged(nameof(Fraccion));
                }
            }
        }

        public decimal? ValorDeclarado
        {
            get => _valorDeclarado;
            set
            {
                if (_valorDeclarado != value)
                {
                    _valorDeclarado = value;
                    OnPropertyChanged(nameof(ValorDeclarado));
                }
            }
        }

        public bool Nuevo
        {
            get => _nuevo;
            set
            {
                if (_nuevo != value)
                {
                    _nuevo = value;
                    OnPropertyChanged(nameof(Nuevo));
                }
            }
        }

        public static FraccionArancelaria FromOrderToFraccion(APIProductosOrdenes producto, string nombre)
        {
            if(!string.IsNullOrEmpty(nombre))
            {
                return new FraccionArancelaria
                {
                    Codigo = producto.CodigoProducto,
                    Descripcion = nombre
                };
            }
            return new FraccionArancelaria
            {
                Codigo = producto.CodigoProducto
            };
        }

        public bool Modificado { get; set; }
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
