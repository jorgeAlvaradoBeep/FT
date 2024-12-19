using Facturacion_Tostatronic.Models.EF_Models.EF_Orders;
using Facturacion_Tostatronic.Services;
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
    public class LoadNewPricesVM: INotifyPropertyChanged, IPageViewModel
    {
        public string Name { get; set; } = "LoadNewPricesVM";
        private bool isBusy;

		public bool IsBusy
        {
			get { return isBusy; }
			set 
            {
                if (isBusy != value)
                {
                    isBusy = value;
                    OnPropertyChanged(); // Notifica cambio en "Nombre"
                }
            }
		}

        private string porcentajeDeAvance;

        public string PorcentajeDeAvance
        {
            get { return porcentajeDeAvance; }
            set
            {
                if (porcentajeDeAvance != value)
                {
                    porcentajeDeAvance = value;
                    OnPropertyChanged(); // Notifica cambio en "Nombre"
                }
            }
        }
        private int filaFinal;

        public int FilaFin
        {
            get { return filaFinal; }
            set
            {
                if (filaFinal != value)
                {
                    filaFinal = value;
                    OnPropertyChanged(); // Notifica cambio en "Nombre"
                }
            }
        }
        private int filaInicio;

        public int FilaInicio
        {
            get { return filaInicio; }
            set
            {
                if (filaInicio != value)
                {
                    filaInicio = value;
                    OnPropertyChanged(); // Notifica cambio en "Nombre"
                }
            }
        }

        private List<ProductOrderComplete> productosActualizar;

        public List<ProductOrderComplete> ProductosActualizar
        {
            get { return productosActualizar; }
            set
            {
                if (productosActualizar != value)
                {
                    productosActualizar = value;
                    OnPropertyChanged(); // Notifica cambio en "Nombre"
                }
            }
        }
        private object _currentPage;
        public object CurrentPage
        {
            get => _currentPage;
            set
            {
                if (_currentPage != value)
                {
                    _currentPage = value;
                    OnPropertyChanged();
                }
            }
        }

        #region Commands
        public LoadProductsFromExcelCommand LoadProductsFromExcelCommand { get; set; }
        public SelectedProductToUpdateCommand SelectedProductToUpdateCommand { get; set; }
        #endregion
        public LoadNewPricesVM()
        {
            IsBusy = false;
            PorcentajeDeAvance = "0%";
            ProductosActualizar = new List<ProductOrderComplete>();
            CurrentPage = null;

            //Comandos
            LoadProductsFromExcelCommand = new LoadProductsFromExcelCommand(this);
            SelectedProductToUpdateCommand = new SelectedProductToUpdateCommand(this);
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
