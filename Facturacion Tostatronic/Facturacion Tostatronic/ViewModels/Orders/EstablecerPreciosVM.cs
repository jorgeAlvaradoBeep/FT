using Facturacion_Tostatronic.Models.EF_Models.EF_Orders;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion_Tostatronic.ViewModels.Orders
{
    public class EstablecerPreciosVM:INotifyPropertyChanged
    {
        #region Propiedades
        private bool gettingData;

        public bool GettingData
        {
            get { return gettingData; }
            set
            {
                if (gettingData != value)
                {
                    gettingData = value;
                    OnPropertyChanged(nameof(GettingData));
                }
            }
        }
        private OrderComplete orderComplete;

        public OrderComplete OrderComplete
        {
            get { return orderComplete; }
            set
            {
                if (orderComplete != value)
                {
                    orderComplete = value;
                    OnPropertyChanged(nameof(OrderComplete));
                }
            }
        }
        #endregion
        #region Comandos
        #endregion

        public EstablecerPreciosVM(OrderComplete _OrderComplete)
        {
            GettingData = false;
            this.OrderComplete = _OrderComplete;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
