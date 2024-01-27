using Facturacion_Tostatronic.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Facturacion_Tostatronic.Models.EF_Models.EF_Orders
{
    public class ExcelParametersM:BaseNotifyPropertyChanged, INotifyDataErrorInfo
    {
        #region IDataError
        private Dictionary<string, List<string>> _errors = new Dictionary<string, List<string>>();

        public bool HasErrors => _errors.Count > 0;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName) || !_errors.ContainsKey(propertyName))
            {
                return null;
            }
            return _errors[propertyName];
        }

        protected virtual void OnErrorsChanged([CallerMemberName] string propertyName = "")
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }
        #endregion
        public short FilaInicio { get; set; }
        public short FilaFin { get; set; }
        private string columnCodigo;

        public string ColumnCodigo
        {
            get { return columnCodigo; }
            set 
            {
                SetValue(ref columnCodigo, value);
                ValidarPropiedad(value, nameof(ColumnCodigo));
            }
        }

        public string ColumnNombreEs { get; set; }
        public string ColumnNombreEn { get; set; }
        public string ColumnQty { get; set; }
        public string ColumnPrecio { get; set; }
        public string ColumnTargetPrice { get; set; }
        public string ColumnLink { get; set; }

        private void ValidarPropiedad(string valor, string propiedad)
        {
            if (!Regex.IsMatch(valor, @"^[a-zA-Z]{1,2}$"))
            {
                _errors[propiedad] = new List<string> { "Debe contener 1 o 2 letras." };
                OnErrorsChanged(propiedad);
            }
            else if (_errors.ContainsKey(propiedad))
            {
                _errors.Remove(propiedad);
                OnErrorsChanged(propiedad);
            }
        }
    }
}
