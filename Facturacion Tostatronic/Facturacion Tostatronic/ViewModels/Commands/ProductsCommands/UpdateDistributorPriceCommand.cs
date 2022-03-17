using Bukimedia.PrestaSharp.Entities;
using Bukimedia.PrestaSharp.Factories;
using Facturacion_Tostatronic.Models;
using Facturacion_Tostatronic.Models.Products;
using Facturacion_Tostatronic.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Facturacion_Tostatronic.ViewModels.Commands.ProductsCommands
{
    public class UpdateDistributorPriceCommand : ICommand
    {
        public MenuVM VM { get; set; }
        public event EventHandler CanExecuteChanged;

        public UpdateDistributorPriceCommand(MenuVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += this.OnBackgroundWorkerDoWork;
            backgroundWorker.RunWorkerCompleted += OnBackgroundWorkerRunWorkerCompleted;
            backgroundWorker.RunWorkerAsync();
        }

        private void OnBackgroundWorkerRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var backgroundWorker = sender as BackgroundWorker;
            backgroundWorker.DoWork -= this.OnBackgroundWorkerDoWork;
            backgroundWorker.RunWorkerCompleted -= OnBackgroundWorkerRunWorkerCompleted;
        }

        private async void OnBackgroundWorkerDoWork(object sender, DoWorkEventArgs e)
        {
            VM.GettingData = true;
            Response res = await WebService.GetDataForInvoice(URLData.product_distributor_price);
            if (!res.succes)
            {
                MessageBox.Show(res.message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                VM.GettingData = false;
                return;
            }
            List<DistributorPrice> products = JsonConvert.DeserializeObject<List<DistributorPrice>>(res.data.ToString());
            ProductFactory ArticuloFactory = new ProductFactory(URLData.psBaseUrl, URLData.psAccount, URLData.psPassword);
            List<product> productList = new List<product>();
            productList = await ArticuloFactory.GetAllAsync();

            List<product> updatedList = new List<product>();
            //Seccion de actualizacion de precios por paginacion, asi evitamos una sobre carga al server y se hacenn varias
            //solicitudes de actualizacion de maximo 200 productos por tanda
            //int numberOfPages;
            //numberOfPages = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(products.Count / 200)));

            int productCount = 0;
            string error = "";
            int aux = 0;
            int totalProduct = products.Count;
            int pricesChnaged = 0;

            foreach (DistributorPrice p in products)
            {
                pricesChnaged++; ;
                if (productCount != 200)
                {
                    var obj = productList.FirstOrDefault(x => x.reference == p.idProduct);
                    if (obj != null)
                    {
                        if (obj.price != Convert.ToDecimal(p.distribuidor))
                        {
                            obj.price = Convert.ToDecimal(p.distribuidor);
                            updatedList.Add(obj);
                            productCount++;
                            aux++;
                            if (productCount == 20)
                            {
                                productCount = 0;
                                error += await VM.UpdateDistributorPricePage(updatedList, ArticuloFactory);
                                updatedList = new List<product>();
                                VM.ProgressVal = "Porcentaje de carga: "+Environment.NewLine+((pricesChnaged / totalProduct) * 100).ToString();
                            }
                        }
                    }
                }
            }
            if (updatedList.Count > 0)
            {
                error += await VM.UpdateDistributorPricePage(updatedList, ArticuloFactory);
            }
            if (error.Equals(""))
                MessageBox.Show("Precios Actualizados Correctamente", "Exito", MessageBoxButton.OK, MessageBoxImage.Information);
            else

                MessageBox.Show("Error: " + Environment.NewLine + error, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            VM.GettingData = false;
        }
    }
}
