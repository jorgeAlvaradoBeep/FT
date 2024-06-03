using Facturacion_Tostatronic.Models.Products;
using Facturacion_Tostatronic.Models;
using Facturacion_Tostatronic.Services;
using Facturacion_Tostatronic.ViewModels.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace Facturacion_Tostatronic.ViewModels.Commands.ProductsCommands.SeeProductsCommands
{
    public class SeeProductsLoadedCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public SeeProductVM VM { get; set; }
        public SeeProductsLoadedCommand(SeeProductVM vM)
        {
            VM = vM;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {

            VM.GettingData = true;
            Response rmp = await WebService.GetDataForInvoice(URLData.getProductsNet);
            if (rmp.succes)
            {
                await Task.Run(() => GetProductsToList(rmp));
            }
            else
                MessageBox.Show("Error al traer la información solicitada", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            VM.GettingData = false;
        }
        void GetProductsToList(Response res)
        {
            VM._allProducts = null;
            VM._allProducts = JsonConvert.DeserializeObject<IReadOnlyList<UpdateProductM>>(res.data.ToString());
            VM.Products.Clear();
            VM.Products = new ObservableCollection<UpdateProductM>(VM._allProducts.ToList());
            //foreach (UpdateProductM a in VM._allProducts)
            //{
            //    VM.Products.Add(a);
            //}
            // Crear índices
            foreach (var product in VM._allProducts)
            {
                VM.AddToIndex(VM._nombreIndex, product.Nombre.ToLower(), product);
                VM.AddToIndex(VM._codigoIndex, product.Codigo.ToLower(), product);
            }
        }
        
    }
}
