using Facturacion_Tostatronic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion_Tostatronic.Models.EF_Models.EF_Orders
{
    public class EFOrderProduct: BaseNotifyPropertyChanged
    {
        private string _codigo;

        public string codigo
        {
            get { return _codigo; }
            set { SetValue(ref _codigo, value); Modificado = true; }
        }
        private string _nombreES;

        public string nombreES
        {
            get { return _nombreES; }
            set { SetValue(ref _nombreES, value); Modificado = true; }
        }
        private string _nombreEN;

        public string nombreEN
        {
            get { return _nombreEN; }
            set { SetValue(ref _nombreEN, value); Modificado = true; }
        }
        public int cantidad { get; set; }
        public float precio { get; set; }
        public float subTotal { get; set; }
        public float targetPrice { get; set; }
        public bool Modificado { get; set; }
        public bool Nuevo { get; set; }
        public string imagen { get; set; }
    }
}
