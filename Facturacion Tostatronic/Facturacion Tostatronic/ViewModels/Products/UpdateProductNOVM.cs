using Facturacion_Tostatronic.Models.Products;
using Facturacion_Tostatronic.ViewModels.Commands.ProductsCommands.LoadNewPricesCommands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion_Tostatronic.ViewModels.Products
{
    public class UpdateProductNOVM : INotifyPropertyChanged
    {

        private bool gettingData;

        public bool GettingData
        {
            get { return gettingData; }
            set
            {
                if (gettingData != value)
                {
                    gettingData = value;
                    OnPropertyChanged();
                }
            }
        }
        private ProductCompleteNewArrival productoAActualizar;

        public ProductCompleteNewArrival ProductoAActualizar
        {
            get { return productoAActualizar; }
            set
            {
                if (productoAActualizar != value)
                {
                    productoAActualizar = value;
                    OnPropertyChanged();
                }
            }
        }
        public UpdateProductInfoCommand UpdateProductInfoCommand { get; set; }
        public UpdateProductNOVM(ProductCompleteNewArrival updateProduct)
        {
            GettingData = false;
            ProductoAActualizar = updateProduct;
            UpdateProductInfoCommand = new UpdateProductInfoCommand(this);  
        }


        public event PropertyChangedEventHandler PropertyChanged;

        // Método auxiliar para levantar el evento. 
        // [CallerMemberName] permite no tener que especificar el nombre de la propiedad manualmente.
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
