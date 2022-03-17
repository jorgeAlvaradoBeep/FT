using Bukimedia.PrestaSharp;
using Bukimedia.PrestaSharp.Entities;
using Bukimedia.PrestaSharp.Factories;
using Facturacion_Tostatronic.Models;
using Facturacion_Tostatronic.Models.Products;
using Facturacion_Tostatronic.Services;
using Facturacion_Tostatronic.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Facturacion_Tostatronic.ViewModels.Commands.ProductsCommands
{
    public class UpdateQuantitiesViewCommand : ICommand
    {
        public MenuVM VM { get; set; }
        public event EventHandler CanExecuteChanged;

        public UpdateQuantitiesViewCommand(MenuVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            WaitPlease pw = new WaitPlease();
            pw.Show();
            Response res = await WebService.GetData("cs", "", URLData.product_update);
            if (!res.succes)
            {
                MessageBox.Show("Error: " + res.message + Environment.NewLine + "No se encontrarion coincidencias", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                pw.Close();
                return;
            }
            List<ProductComplete> aux = JsonConvert.DeserializeObject<List<ProductComplete>>(res.data.ToString());
            StockAvailableFactory stockAvailableFactory = new StockAvailableFactory(URLData.psBaseUrl, URLData.psAccount, URLData.psPassword);
            List<stock_available> stockList = await GetStocks(stockAvailableFactory);
            List<stock_available> updatedList = new List<stock_available>();
            foreach (ProductComplete p in aux)
            {
                if(p.PrestashopID!= null)
                {
                    var obj = stockList.FirstOrDefault(x => x.id_product == long.Parse(p.PrestashopID));
                    if (obj != null)
                    {
                        if (obj.quantity != (int)p.Existence)
                        {
                            obj.quantity = (int)p.Existence;
                            obj.out_of_stock = 2;
                            updatedList.Add(obj);
                        }

                    }
                        
                }
            }
            try
            {
                await stockAvailableFactory.UpdateListAsync(updatedList);
                MessageBox.Show("Cantidades actualizados correctamente" + res.message, "Exito", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (PrestaSharpException e)
            {
                MessageBox.Show("Error al actualizar cantidades: " + res.message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                pw.Close();
                return;
            }

            pw.Close();
        }

        private async Task<List<stock_available>> GetStocks(StockAvailableFactory factory)
        {
            var stockList = await factory.GetAllAsync();
            return stockList;
        }
    }
}
