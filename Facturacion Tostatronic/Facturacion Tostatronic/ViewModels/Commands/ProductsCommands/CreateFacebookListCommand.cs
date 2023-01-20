using Bukimedia.PrestaSharp.Entities;
using Bukimedia.PrestaSharp.Factories;
using Facturacion_Tostatronic.Models;
using Facturacion_Tostatronic.Models.Products;
using Facturacion_Tostatronic.Models.WooCommerceModels;
using Facturacion_Tostatronic.Services;
using GalaSoft.MvvmLight.Threading;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Excel = Microsoft.Office.Interop.Excel;

namespace Facturacion_Tostatronic.ViewModels.Commands.ProductsCommands
{
    public class CreateFacebookListCommand : ICommand
    {
        public MenuVM VM { get; set; }
        public event EventHandler CanExecuteChanged;

        public CreateFacebookListCommand(MenuVM vm)
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
            VM.GettingData = true;
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
            //Sección que se uso para actualizar los codigos de woocommerce
            
            DispatcherHelper.CheckBeginInvokeOnUI(
                () =>
                {
                    // Dispatch back to the main thread
                    VM.ProgressVal = "Cargando Datos de existencia actuales.";
                });

            Response res = await WebService.GetDataNode(URLData.getProductCodesNET,"");
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

            List<ProductCodesEF> aux = JsonConvert.DeserializeObject<List<ProductCodesEF>>(res.data.ToString());
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
                var res2 = await WebService.GetDataWooCommercer(URLData.wcProducts, "id,sku,name,stock_quantity", pageNumber1.ToString());
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
                VM.ProgressVal = $"Productos extraidos desde la web.{Environment.NewLine}Validando Codigos.";
            });

            List<ProductCodesEF> updatedList = new List<ProductCodesEF>();
            List<ProductCodesEF> insertList = new List<ProductCodesEF>();
            List<WooCommerceProduct> variationList = new List<WooCommerceProduct>();
            foreach (WooCommerceProduct p in productsTemp)
            {
                if(!string.IsNullOrEmpty(p.Sku))
                {
                    try
                    {
                        var obj = aux.FirstOrDefault(x => x.Referencia == p.Sku);
                        if (obj != null)
                        {
                            if (obj.Prestashop != (int)p.Id)
                            {
                                obj.Prestashop = (int)p.Id;
                                updatedList.Add(obj);
                            }

                        }
                        else
                        {
                            insertList.Add(new ProductCodesEF() { Referencia = p.Sku, Prestashop= (int)p.Id });
                        }
                    }
                    catch (Exception)
                    {

                    }
                }
                else
                {
                    variationList.Add(p);
                }
            }
            string data = JsonConvert.SerializeObject(insertList);
            DispatcherHelper.CheckBeginInvokeOnUI(() => { VM.ProgressVal = $"Productos a actualizar: {updatedList.Count}" +
                $"{Environment.NewLine}Productos a insertar: {insertList.Count}" +
                $"{Environment.NewLine}Productos con variantes: {variationList.Count}"; });
            int cont = 1;
            bool error = false;
            //DispatcherHelper.CheckBeginInvokeOnUI(() => { VM.ProgressVal = $"Actualizando Productos..."; });
            //res = await WebService.ModifyData(updatedList, URLData.updateProductCodesListNET);
            //if (!res.succes)
            //{
            //    MessageBox.Show("Error: " + res.message + Environment.NewLine + "No se encontrarion coincidencias", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            //    VM.GettingData = false;
            //    return;
            //}
            //DispatcherHelper.CheckBeginInvokeOnUI(() => { VM.ProgressVal = $"Producto Actualizados"; });
            cont = 0;
            foreach (WooCommerceProduct p in variationList)
            {
                cont++;
                var res2 = await WebService.GetProductVariationsWooCommercer(URLData.wcProducts, p.Id.ToString());
                if (string.IsNullOrEmpty(res2.Content))
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
            cont = 0;
            updatedList.Clear();
            foreach (WooCommerceProduct p in variationList)
            {
                cont++;
                if (p.Variantes != null)
                {
                    if (p.Variantes.Count > 0)
                    {
                        foreach (WooCommerceProduct q in p.Variantes)
                        {
                            var obj = aux.FirstOrDefault(x => x.Referencia == q.Sku);
                            if (obj != null)
                            {
                                if (obj.Prestashop == q.Id)
                                {
                                    obj.Prestashop = q.Id;
                                    obj.InternationalSku = p.Id.ToString();
                                    updatedList.Add(obj);
                                }

                            }
                        }
                        string d = JsonConvert.SerializeObject(updatedList);
                        res = await WebService.ModifyData(updatedList, URLData.updateProductCodesListNET);
                        if (!res.succes)
                        {
                            DispatcherHelper.CheckBeginInvokeOnUI(() => { VM.ProgressVal = "Error: " + res.message + Environment.NewLine + "No se encontrarion coincidencias"; });
                            
                        }
                        DispatcherHelper.CheckBeginInvokeOnUI(
                        () =>
                        {
                            // Dispatch back to the main thread
                            VM.ProgressVal = $"Variantes de Producto({p.Id}) #{cont}/{variationList.Count} actualizadas exitosamente.";
                        });
                        updatedList.Clear();
                    }
                }
            }
            /*
            VM.ProgressVal = $"Variantes actualizadas exitosamente.{Environment.NewLine}" +
                $"Insertando Productos Faltantes.";
            res = await WebService.InsertData(insertList, URLData.createProductCodesListNET);
            if (!res.succes)
            {
                MessageBox.Show($"Error al insertar:{res.message}","Error",MessageBoxButton.OK, MessageBoxImage.Error);
            }*/

            VM.ProgressVal = string.Empty;
            VM.GettingData = false;
            if (error)
                MessageBox.Show("Codigos actualizadas pero con errores", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            else
                MessageBox.Show("Codigos actualizadas Correctamente", "Error", MessageBoxButton.OK, MessageBoxImage.Information);
            /*
        Response res = await WebService.GetDataForInvoice(URLData.product_public_price);
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

        List<product> facebookList = new List<product>();
        Application.Current.Dispatcher.Invoke(new Action(() =>
        { VM.ProgressVal = "Carga inicial terminada, en breve comenzara la creación de la lista..."; }));

        ImageFactory img = new ImageFactory(URLData.psBaseUrl, URLData.psAccount, URLData.psPassword);
        CategoryFactory category = new CategoryFactory(URLData.psBaseUrl, URLData.psAccount, URLData.psPassword);


        #region ExcelFile
        var excelApp = new Excel.Application();
        // Make the object visible.
        excelApp.Visible = false;
        excelApp.Workbooks.Add();
        Excel._Worksheet workSheet = (Excel.Worksheet)excelApp.ActiveSheet;
        workSheet.Cells[1, "A"] = "ID";
        workSheet.Cells[1, "B"] = "Nombre Del producto";
        workSheet.Cells[1, "C"] = "Descrpción";
        workSheet.Cells[1, "D"] = "Stock";
        workSheet.Cells[1, "E"] = "Condicion";
        workSheet.Cells[1, "F"] = "Price";
        workSheet.Cells[1, "G"] = "Link";
        workSheet.Cells[1, "H"] = "Imagen";
        workSheet.Cells[1, "I"] = "Brand";
        workSheet.Cells[1, "J"] = "Google";
        workSheet.Cells[1, "k"] = "Facebook";
        var row = 1;
        int totalProducts;
        int count = 0;
        string link;
        string linkImage;
        totalProducts = products.Count;
        float progresssTotal;
        #endregion
        foreach (DistributorPrice p in products)
        {
            var obj = productList.FirstOrDefault(x => x.reference == p.idProduct);
            if (obj != null)
            {

                count++;
                try
                {
                    Bukimedia.PrestaSharp.Entities.FilterEntities.declination imageses = img.GetProductImages((long)obj.id)[0];
                    category cat = await category.GetAsync((long)obj.id_category_default);
                    link = "https://tostatronic.com/store/" + cat.link_rewrite[0].Value + "/" + obj.id + "-" + obj.link_rewrite[0].Value + ".html";
                    linkImage = "https://tostatronic.com/store/" + obj.id_default_image + "-large_default/" + obj.link_rewrite[0].Value + ".jpg";
                    row++;
                    workSheet.Cells[row, "A"] = obj.reference;
                    workSheet.Cells[row, "B"] = obj.name[0].Value;
                    if (String.IsNullOrEmpty(obj.description_short[0].Value))
                        workSheet.Cells[row, "C"] = "Sin Descripción";
                    else
                        workSheet.Cells[row, "C"] = HtmlUtilities.ConvertToPlainText(obj.description_short[0].Value);
                    workSheet.Cells[row, "D"] = "in stock";
                    workSheet.Cells[row, "E"] = "new";
                    workSheet.Cells[row, "F"] = obj.price.ToString("#.##") + " MXN";
                    workSheet.Cells[row, "G"] = link;
                    workSheet.Cells[row, "H"] = linkImage;
                    workSheet.Cells[row, "I"] = "Tostatronic";
                    workSheet.Cells[row, "J"] = "Electronics > Electronics Accessories";
                    workSheet.Cells[row, "K"] = "Electronics > Accessories";
                    if(row%50==0)
                    {
                        progresssTotal = (count * 100) / totalProducts;
                        Application.Current.Dispatcher.Invoke(new Action(() =>
                        { VM.ProgressVal = "Progereso: " + progresssTotal; }));
                    }
                }
                catch(Exception gy)
                {

                }
            }
        }
        workSheet.Rows.RowHeight = 25;
        workSheet.Columns[1].AutoFit();
        workSheet.Columns[2].AutoFit();
        workSheet.Columns[3].AutoFit();
        workSheet.Columns[4].AutoFit();
        workSheet.Columns[5].AutoFit();
        workSheet.Columns[6].AutoFit();
        workSheet.Columns[7].AutoFit();
        workSheet.Columns[8].AutoFit();
        workSheet.Columns[9].AutoFit();
        workSheet.Columns[10].AutoFit();
        workSheet.Columns[11].AutoFit();
        excelApp.Visible = true;
        */



            VM.GettingData = false;
        }
    }
}
