using Bukimedia.PrestaSharp;
using Bukimedia.PrestaSharp.Entities;
using Bukimedia.PrestaSharp.Entities.AuxEntities;
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
            MessageBoxResult result = MessageBox.Show("Estás por actualizar precios y cantidades, ¿Estás seguro?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.No)
            {
                MessageBox.Show("Operación Cancelada por el Usuario", "Abortado", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
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
                DispatcherHelper.CheckBeginInvokeOnUI(
                () =>
                {
                    // Dispatch back to the main thread
                    VM.ProgressVal = $"Pagína #{pageNumber1-1}{Environment.NewLine}" +
                    $"Productos Cargados: {productsTemp.Count}";
                });
                var res2 =  await WebService.GetDataWooCommercer(URLData.wcProducts, "id,sku,name,stock_quantity", pageNumber1.ToString());
                if (!res2.IsSuccessful)
                {
                    DispatcherHelper.CheckBeginInvokeOnUI(
                    () =>
                    {
                        // Dispatch back to the main thread
                        VM.ProgressVal = $"Pagína #{pageNumber1 - 1}{Environment.NewLine}" +
                        $"Productos Cargados: {productsTemp.Count}" +
                        $"Error en la descarga de los productos de la Pagína{Environment.NewLine}" +
                        $"Motivos: {res2.ErrorMessage}";
                    });
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
                    
                    
                }
                
            }
            DispatcherHelper.CheckBeginInvokeOnUI(
            () =>
            {
                // Dispatch back to the main thread
                VM.ProgressVal = $"Productos Totales Obtenidos: {productsTemp.Count}.{Environment.NewLine}" +
                $"Comparando precios...";
            });
            string resultText = string.Empty;
            List<Update> lisUpdate = new List<Update>();
            List<WooCommerceProduct> lisVariationsFathers = new List<WooCommerceProduct>();
            if (productsTemp.Count > 0)
            {
                foreach (WooCommerceProduct p in productsTemp)
                {
                    if (p.Sku != null)
                    {
                        if (p.Sku != string.Empty)
                        {
                            var obj = aux.FirstOrDefault(x => x.Code == p.Sku);
                            if (obj != null)
                            {
                                //if(obj.Existence!= p.Stock_quantity)
                                //{
                                    //p.Sale_price = obj.DistributorPrice.ToString();
                                    p.Sale_price = "";
                                    p.Stock_quantity = (int)obj.Existence;
                                    p.Regular_price = obj.PublicPrice.ToString();
                                    lisUpdate.Add(new Update()
                                    {
                                        id = p.Id,
                                        sku = p.Sku,
                                        sale_price = p.Sale_price,
                                        regular_price = p.Regular_price,
                                        stock_quantity = p.Stock_quantity
                                    });
                                //}                                
                            }
                        }
                        else
                            lisVariationsFathers.Add(p);
                    }
                }
            }
            //Aqui hacemos la seción de generar las listas de actualización.
            //Aqui dividimos la lista en listas maximo de 85 elementos
            List<List<Update>> litsToUpdate = SplitList(lisUpdate);
            int cont = 1;
            bool error = false;
            foreach (List<Update> listToUpdate in litsToUpdate)
            {
                DispatcherHelper.CheckBeginInvokeOnUI(
                () =>
                {
                    // Dispatch back to the main thread
                    VM.ProgressVal = $"Listas De Productos a Actualizar {litsToUpdate.Count}{Environment.NewLine}" +
                    $"Listas Actualizadas: {cont-1}/{litsToUpdate.Count}";
                });
                CRUDActionClass crud = new CRUDActionClass()
                {
                    create = new List<Create>(),
                    delete = new List<Delete>(),
                    update = listToUpdate
                };
                string json = JsonConvert.SerializeObject(crud,
               Newtonsoft.Json.Formatting.None,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
                var res2 = await WebService.InsertDataWooCommercer(URLData.wcBatchProduct, json);
                if (!res2.IsSuccessful)
                {
                    DispatcherHelper.CheckBeginInvokeOnUI(
                    () =>
                    {
                        // Dispatch back to the main thread
                        VM.ProgressVal = $"Listas De Productos a Actualizar {litsToUpdate.Count}{Environment.NewLine}" +
                        $"Listas Actualizadas: {cont - 1}/{litsToUpdate.Count}" +
                        $"Error en la actualización de las lista:  {cont}{Environment.NewLine}" +
                        $"Motivos: {res2.ErrorMessage}";
                    });
                    
                    error = true;
                }
                cont++;
            }

            //Seccion de variantes

            //Aqu[i obtenemos las listas de las variantes
            DispatcherHelper.CheckBeginInvokeOnUI(
            () =>
            {
                // Dispatch back to the main thread
                VM.ProgressVal = $"Productos individuales Actualizados{Environment.NewLine}" +
                $"Obteniendo {lisVariationsFathers.Count} productos de variantes";
            });
            endWhile1 = false;
            pageNumber1 = 1;
            List<List<WooCommerceProduct>> variationProducts = new List<List<WooCommerceProduct>>();
            List<Update> listToUpdateVariantes = new List<Update>();
            DispatcherHelper.CheckBeginInvokeOnUI(
            () =>
            {
                // Dispatch back to the main thread
                VM.ProgressVal = $"Variantes Actualizadas: {pageNumber1 - 1}/{lisVariationsFathers.Count}" +
                $"Extrayendo datos de: {lisVariationsFathers[pageNumber1 - 1].Name}";
            });
            int errorCounter = 0;
            while (!endWhile1)
            {
                var res2 = await WebService.GetSingleDataWooCommercer($"{URLData.wcProducts}/{lisVariationsFathers[pageNumber1 - 1].Id}/variations", new List <Fields>());
                if (!res2.IsSuccessful)
                {
                    DispatcherHelper.CheckBeginInvokeOnUI(
                    () =>
                    {
                        // Dispatch back to the main thread
                        VM.ProgressVal = $"Pagína #{pageNumber1 - 1}{Environment.NewLine}" +
                        $"Productos Cargados: {productsTemp.Count}" +
                        $"Error en la descarga de los productos de la Pagína{Environment.NewLine}" +
                        $"Motivos: {res2.ErrorMessage}";
                    });
                }
                else
                {
                    List<WooCommerceProduct> products2 = JsonConvert.DeserializeObject<List<WooCommerceProduct>>(res2.Content.ToString(), settings);
                    if(products2!=null)
                    {
                        if(products2.Count>0)
                        {
                            //variationProducts.Add(products2);
                            //Aqui actualkizamos las variantes
                            DispatcherHelper.CheckBeginInvokeOnUI(
                            () =>
                            {
                                // Dispatch back to the main thread
                                VM.ProgressVal = $"Variantes Actualizadas: {pageNumber1 - 1}/{lisVariationsFathers.Count}" +
                                $"Extrayendo datos de: {lisVariationsFathers[pageNumber1 - 1].Name}" +
                                $"Productos a actualizar: {products2.Count}";
                            });
                            foreach (WooCommerceProduct product in products2)
                            {
                                var obj = aux.FirstOrDefault(x => x.Code == product.Sku);
                                if (obj != null)
                                {
                                    //if (obj.Existence != product.Stock_quantity)
                                    //{
                                        //product.Sale_price = obj.DistributorPrice.ToString();
                                        product.Sale_price = "";
                                        product.Stock_quantity = (int)obj.Existence;
                                        product.Regular_price = obj.PublicPrice.ToString();
                                        listToUpdateVariantes.Add(new Update()
                                        {
                                            id = product.Id,
                                            sku = product.Sku,
                                            sale_price = product.Sale_price,
                                            regular_price = product.Regular_price,
                                            stock_quantity = product.Stock_quantity
                                        });


                                    //}
                                }
                                
                            }
                            DispatcherHelper.CheckBeginInvokeOnUI(
                            () =>
                            {
                                // Dispatch back to the main thread
                                VM.ProgressVal = $"Variantes Actualizadas: {pageNumber1 - 1}/{lisVariationsFathers.Count}{Environment.NewLine}" +
                                $"Datos extraidos de: {lisVariationsFathers[pageNumber1 - 1].Name}{Environment.NewLine}" +
                                $"Actualizando {products2.Count} productos.";
                            });
                            if(listToUpdateVariantes.Count >0)
                            {
                                if (!await UpdateProductVariants(listToUpdateVariantes, pageNumber1 - 1, lisVariationsFathers[pageNumber1 - 1].Id))
                                {
                                    pageNumber1++;
                                    if (pageNumber1 >= lisVariationsFathers.Count)
                                        endWhile1 = true;
                                }
                                else
                                {
                                    errorCounter++;
                                    DispatcherHelper.CheckBeginInvokeOnUI(
                                    () =>
                                    {
                                        // Dispatch back to the main thread
                                        VM.ProgressVal = $"Variantes Actualizadas: {pageNumber1 - 1}/{lisVariationsFathers.Count}{Environment.NewLine}" +
                                        $"Error al Actualizar a: {lisVariationsFathers[pageNumber1 - 1].Name}{Environment.NewLine}" +
                                        $"Reintentando...";
                                    });
                                    if (errorCounter >= 3)
                                    {
                                        errorCounter = 0;
                                        DispatcherHelper.CheckBeginInvokeOnUI(
                                        () =>
                                        {
                                            // Dispatch back to the main thread
                                            VM.ProgressVal = $"Variantes Actualizadas: {pageNumber1 - 1}/{lisVariationsFathers.Count}{Environment.NewLine}" +
                                            $"Error al Actualizar a: {lisVariationsFathers[pageNumber1 - 1].Name}{Environment.NewLine}" +
                                            $"Limite excedido de intentos...";
                                        });
                                        pageNumber1++;
                                        if (pageNumber1 >= lisVariationsFathers.Count)
                                            endWhile1 = true;
                                    }
                                }
                            }
                            else
                            {
                                pageNumber1++;
                                if (pageNumber1 >= lisVariationsFathers.Count)
                                    endWhile1 = true;
                            }
                            listToUpdateVariantes.Clear();
                        }
                        else
                        {
                            pageNumber1++;
                            if (pageNumber1 >= lisVariationsFathers.Count)
                                endWhile1 = true;
                        }
                    }
                }

            }
            //Finaliza la seccion de variantes
            VM.ProgressVal = string.Empty;
            VM.GettingData = false;
            if (error)
                MessageBox.Show("Existencias actualizadas pero con errores", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            else
                MessageBox.Show("Existencias actualizadas Correctamente", "Error", MessageBoxButton.OK, MessageBoxImage.Information);

            //Version anterior de actualizacion de cantidades
            /*
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
            */
        }

        private async Task<bool> UpdateProductVariants(List<Update> variantsToBeUpdated, int cont, int idProduct)
        {
            bool error;
            CRUDActionVarationsClass crud = new CRUDActionVarationsClass() { create = new List<Create>(), delete = new List<Delete>(), update = variantsToBeUpdated };
            string json = JsonConvert.SerializeObject(crud, Newtonsoft.Json.Formatting.None,
                            new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore
                            });
            var res2 = await WebService.InsertDataWooCommercer($"{URLData.wcProducts}/{idProduct}/variations/batch", json);
            if (!res2.IsSuccessful)
            {
                DispatcherHelper.CheckBeginInvokeOnUI(
                    () =>
                    {
                        // Dispatch back to the main thread
                        VM.ProgressVal = $"Error en la lista #{cont}{Environment.NewLine}Error: {res2.ErrorMessage}";
                    });
                error = true;
            }
            else
                error = false;
            return error;
        }
        /*
        private async Task<List<stock_available>> GetStocks(StockAvailableFactory factory)
        {
            var stockList = await factory.GetAllAsync();
            return stockList;
        }*/

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
