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
        public EFOrderProduct(string code, string name)
        {
            _codigo = code;
            _nombreES = name;   
        }

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
        private int _cantidad;

        public int cantidad
        {
            get { return _cantidad; }
            set { SetValue(ref _cantidad, value); Modificado = true; }
        }
        private float _precio;

        public float precio
        {
            get { return _precio; }
            set { SetValue(ref _precio, value); }
        }

        private float _subTotal;

        public float subTotal
        {
            get { return _subTotal; }
            set { SetValue(ref _subTotal, value); }
        }
        private float _targetPrice;

        public float targetPrice
        {
            get { return _targetPrice; }
            set { SetValue(ref _targetPrice, value); }
        }
        private bool modificado;

        public bool Modificado
        {
            get { return modificado; }
            set { SetValue(ref modificado, value); }
        }

        private bool nuevo;

        public bool Nuevo
        {
            get { return nuevo; }
            set { SetValue(ref nuevo, value); }
        }
        private string _imagen;

        public string imagen
        {
            get { return _imagen; }
            set { SetValue(ref _imagen, value); Modificado = true; }
        }
    }
}
