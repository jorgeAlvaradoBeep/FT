using Bukimedia.PrestaSharp;
using Bukimedia.PrestaSharp.Entities;
using Bukimedia.PrestaSharp.Factories;
using Chilkat;
using Facturacion_Tostatronic.Models;
using Facturacion_Tostatronic.Models.Products;
using Facturacion_Tostatronic.Models.WooCommerceModels;
using Facturacion_Tostatronic.Services;
using Facturacion_Tostatronic.Views;
using GalaSoft.MvvmLight.Threading;
using Microsoft.Office.Interop.Excel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WooCommerceNET;
using WooCommerceNET.WooCommerce.Legacy;

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
            VM.GettingData = true;
            DispatcherHelper.CheckBeginInvokeOnUI(
                () =>
                {
                    // Dispatch back to the main thread
                    VM.ProgressVal = "Cargando Datos de existencia actuales.";
                });
            
            Response res = await WebService.GetData("cs", "", URLData.product_update);
            if (!res.succes)
            {
                MessageBox.Show("Error: " + res.message + Environment.NewLine + "No se encontrarion coincidencias", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                VM.GettingData = false;
                return;
            }
            DispatcherHelper.CheckBeginInvokeOnUI(
                () =>
                {
                    // Dispatch back to the main thread
                    VM.ProgressVal = $"Cargando de datos terminada.{Environment.NewLine}Obteniendo datos desde WEB.";
                });
            
            List<ProductComplete> aux = JsonConvert.DeserializeObject<List<ProductComplete>>(res.data.ToString());
            List<WooCommerceProduct> productsTemp = new List<WooCommerceProduct>();
            int pageNumber1 = 1;

            bool endWhile1 = false;
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
            while (!endWhile1)
            {
                var res2 =  await WebService.GetDataWooCommercer(URLData.wcProducts, "id,sku,name,stock_quantity", pageNumber1.ToString());
                if (!res2.IsSuccessful)
                {
                    MessageBox.Show(res2.ErrorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    
                    List<WooCommerceProduct> products2 = JsonConvert.DeserializeObject<List<WooCommerceProduct>>(res2.Content.ToString(), settings);
                    if (products2.Count > 0)
                    {
                        productsTemp.AddRange(products2);
                        pageNumber1++;
                    }
                    else
                    {
                        endWhile1 = true;
                    }
                    DispatcherHelper.CheckBeginInvokeOnUI(
                    () =>
                    {
                        // Dispatch back to the main thread
                        VM.ProgressVal = $"Productos Cargados: {productsTemp.Count}";
                    });
                    
                }
                
            }
            DispatcherHelper.CheckBeginInvokeOnUI(
            () =>
            {
                // Dispatch back to the main thread
                VM.ProgressVal = $"Productos extraidos desde la web.{Environment.NewLine}Validando existencias.";
            });
            
            List<Update> updatedList = new List<Update>();
            foreach (ProductComplete p in aux)
            {
                try
                {
                    var obj = productsTemp.FirstOrDefault(x => x.Sku == p.Code);
                    if (obj != null)
                    {
                        if (obj.Stock_quantity != (int)p.Existence)
                        {
                            obj.Stock_quantity = (int)p.Existence;
                            updatedList.Add
                                (new Update() { id=obj.Id, stock_quantity=obj.Stock_quantity});
                        }

                    }
                }
                catch(Exception)
                {

                }
            }
            DispatcherHelper.CheckBeginInvokeOnUI(() => { VM.ProgressVal = $"Productos a actualizar: {updatedList.Count}{Environment.NewLine}Obteniendo Listas de actualizacion."; });
            //Aqui dividimos la lista en listas maximo de 85 elementos
            List<List<Update>> litsToUpdate = SplitList(updatedList);
            //Ahora efectuamos el proceso de actualizacion de cada lista
            int cont = 1;
            bool error = false;
            DispatcherHelper.CheckBeginInvokeOnUI(() => { VM.ProgressVal = $"Listas Obtenidas: {litsToUpdate.Count}{Environment.NewLine}Actualizando Listas."; });
            foreach (List<Update> listToUpdate in litsToUpdate)
            {
                CRUDActionClass crud = new CRUDActionClass() { create = new List<Create>(), delete = new List<Delete>(), update = listToUpdate };
                string json = JsonConvert.SerializeObject(crud, Newtonsoft.Json.Formatting.None,
                                new JsonSerializerSettings
                                {
                                    NullValueHandling = NullValueHandling.Ignore
                                }); 
                var res2 = await WebService.InsertDataWooCommercer(URLData.wcBatchProduct, json);
                if (!res2.IsSuccessful)
                {
                    MessageBox.Show($"Error en la lista #{cont}{Environment.NewLine}Error: "+res2.ErrorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    error = true;
                }
                else
                    DispatcherHelper.CheckBeginInvokeOnUI(
                    () =>
                    {
                        // Dispatch back to the main thread
                        VM.ProgressVal = $"Lista #{cont}/{litsToUpdate.Count} actualizada exitosamente.";
                    });
                
                cont++;
            }
            //Ahora hacemos el procveso para los productos con variaciones
            DispatcherHelper.CheckBeginInvokeOnUI(
            () =>
            {
                // Dispatch back to the main thread
                VM.ProgressVal = $"Actualizando Productos con variantes.";
            });
            List<WooCommerceProduct> variationList = new List<WooCommerceProduct>();
            foreach(WooCommerceProduct p in productsTemp)
            {
                if (string.IsNullOrEmpty(p.Sku))
                    variationList.Add(p);
            }
            //Extraemos las Variantes de Cada Producto
            cont=0;
            foreach (WooCommerceProduct p in variationList)
            {
                cont++;
                var res2 = await WebService.GetProductVariationsWooCommercer(URLData.wcProducts, p.Id.ToString());
                if(string.IsNullOrEmpty(res2.Content))
                {
                    DispatcherHelper.CheckBeginInvokeOnUI(
                    () =>
                    {
                        // Dispatch back to the main thread
                        VM.ProgressVal = $"Sin Variantes por obtener" +
                        $"{Environment.NewLine}Productos analizados {cont}/{variationList.Count}";
                    });
                }
                else
                {
                    List<WooCommerceProduct> products2 = JsonConvert.DeserializeObject<List<WooCommerceProduct>>(res2.Content.ToString(), settings);
                    p.Variantes = products2;
                    DispatcherHelper.CheckBeginInvokeOnUI(
                    () =>
                    {
                        // Dispatch back to the main thread
                        VM.ProgressVal = $"Variantes Obtenidas de {p.Id}: {p.Variantes.Count}" +
                        $"{Environment.NewLine}Productos analizados {cont}/{variationList.Count}";
                    });
                }
                
            }
            cont=0; 
            updatedList.Clear();
            foreach (WooCommerceProduct p in variationList)
            {
                cont++;
                if(p.Variantes!=null)
                {
                    if (p.Variantes.Count > 0)
                    {
                        foreach (WooCommerceProduct q in p.Variantes)
                        {
                            var obj = aux.FirstOrDefault(x => x.Code == q.Sku);
                            if (obj != null)
                            {
                                if (obj.Existence != (int)q.Stock_quantity)
                                {
                                    q.Stock_quantity = (int)obj.Existence;
                                    updatedList.Add
                                        (new Update() { id = q.Id, stock_quantity = q.Stock_quantity });
                                }

                            }
                        }
                        if (!await UpdateProductVariants(updatedList, cont, p.Id))
                        {
                            DispatcherHelper.CheckBeginInvokeOnUI(
                            () =>
                            {
                                // Dispatch back to the main thread
                                VM.ProgressVal = $"Variantes de Producto({p.Id}) #{cont}/{variationList.Count} actualizadas exitosamente.";
                            });
                        }
                    }
                }
            }
            

            VM.ProgressVal = string.Empty;
            VM.GettingData = false;
            if(error)
                MessageBox.Show("Existencias actualizadas pero con errores","Error",MessageBoxButton.OK,MessageBoxImage.Error);
            else
                MessageBox.Show("Existencias actualizadas Correctamente", "Error", MessageBoxButton.OK, MessageBoxImage.Information);
            /*
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
            */
        }

        private async Task<bool> UpdateProductVariants(List<Update> variantsToBeUpdated, int cont, int idProduct)
        {
            bool error;
            CRUDActionClass crud = new CRUDActionClass() { create = new List<Create>(), delete = new List<Delete>(), update = variantsToBeUpdated };
            string json = JsonConvert.SerializeObject(crud, Newtonsoft.Json.Formatting.None,
                            new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore
                            });
            var res2 = await WebService.InsertDataWooCommercer($"{URLData.wcProducts}/{idProduct}/variations/batch", json);
            if (!res2.IsSuccessful)
            {
                MessageBox.Show($"Error en la lista #{cont}{Environment.NewLine}Error: " + res2.ErrorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                error = true;
            }
            else
                error = false;
            return error;
        }
        private async Task<List<stock_available>> GetStocks(StockAvailableFactory factory)
        {
            var stockList = await factory.GetAllAsync();
            return stockList;
        }

        public static List<List<Update>> SplitList(List<Update> toUpdateProducts, int nSize = 85)
        {
            var list = new List<List<Update>>();

            for (int i = 0; i < toUpdateProducts.Count; i += nSize)
            {
                list.Add(toUpdateProducts.GetRange(i, Math.Min(nSize, toUpdateProducts.Count - i)));
            }

            return list;
        }
    }
}
