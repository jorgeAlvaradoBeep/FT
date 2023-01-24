using Bukimedia.PrestaSharp.Entities;
using Bukimedia.PrestaSharp.Entities.AuxEntities;
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
using System.Diagnostics;
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
            //Sección para crear la lista de tabla de descuentos

            Response res = await WebService.GetDataNode(URLData.getProductsNet, "");
            if (!res.succes)
            {
                MessageBox.Show("Error: " + res.message + Environment.NewLine + "Error al extraer productos", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                VM.GettingData = false;
                return;
            }
            List<UpdateProductM> productos = JsonConvert.DeserializeObject<List<UpdateProductM>>(res.data.ToString());
            res = await WebService.GetDataNode(URLData.getProductCodesNET, "");
            if (!res.succes)
            {
                MessageBox.Show("Error: " + res.message + Environment.NewLine + "Error al extraer codigo", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                VM.GettingData = false;
                return;
            }
            List<ProductCodesEF> productCodes = JsonConvert.DeserializeObject<List<ProductCodesEF>>(res.data.ToString());
            DispatcherHelper.CheckBeginInvokeOnUI(
                () =>
                {
                    // Dispatch back to the main thread
                    VM.ProgressVal = "Obteniendo datos desde WEB.";
                });

            #region excel
            var excelApp = new Excel.Application();
            // Make the object visible.
            excelApp.Visible = false;
            excelApp.Workbooks.Add();
            Excel._Worksheet workSheet = (Excel.Worksheet)excelApp.ActiveSheet;
            workSheet.Cells[1, "A"] = "Referencia";
            workSheet.Cells[1, "B"] = "Nombre Del producto";
            workSheet.Cells[1, "C"] = "Precio Minimo";
            workSheet.Cells[1, "D"] = "Precio Publico";
            workSheet.Cells[1, "E"] = "Precio Minimo Tabla";
            workSheet.Cells[1, "F"] = "Diferencia";
            workSheet.Cells[1, "G"] = "Cantidad";
            workSheet.Cells[1, "H"] = "CD";
            workSheet.Cells[1, "I"] = "C1";
            workSheet.Cells[1, "J"] = "P1";
            workSheet.Cells[1, "K"] = "C2";
            workSheet.Cells[1, "L"] = "P2";
            workSheet.Cells[1, "M"] = "C3";
            workSheet.Cells[1, "N"] = "P3";
            workSheet.Cells[1, "O"] = "C4";
            workSheet.Cells[1, "P"] = "P4";
            workSheet.Cells[1, "Q"] = "C5";
            workSheet.Cells[1, "R"] = "P5";
            workSheet.Cells[1, "S"] = "Codigo Woo";
            var row = 1;
            Microsoft.Office.Interop.Excel.Range oRange;
            float left;
            float top;
            int count = 0;
            int total = productos.Count;
            int progress = 0;
            int previos = progress;
            float pmt, dif;
            int cc, cp;
            foreach (var producto in productos)
            {
                if(producto.Existencia>0)
                {
                    row++;
                    count++;
                    workSheet.Cells[row, "A"] = producto.Codigo;
                    workSheet.Cells[row, "B"] = producto.Nombre;
                    workSheet.Cells[row, "C"] = producto.PrecioMinimo;
                    workSheet.Cells[row, "D"] = producto.PrecioPublico;
                    pmt = (producto.PrecioMinimo / 0.94f);
                    workSheet.Cells[row, "E"] = pmt.ToString("#.##");
                    dif = producto.PrecioPublico - pmt;
                    workSheet.Cells[row, "F"] = dif.ToString("#.##");
                    dif = dif / 5;
                    workSheet.Cells[row, "G"] = producto.Existencia;
                    cc = (int)(producto.Existencia / 5);
                    workSheet.Cells[row, "H"] = cc;
                    workSheet.Cells[row, "I"] = cc;
                    workSheet.Cells[row, "J"] = producto.PrecioPublico - dif;
                    workSheet.Cells[row, "K"] = 2 * cc;
                    workSheet.Cells[row, "L"] = producto.PrecioPublico - (2 * dif);
                    workSheet.Cells[row, "M"] = 3 * cc;
                    workSheet.Cells[row, "N"] = producto.PrecioPublico - (3 * dif);
                    workSheet.Cells[row, "O"] = 4 * cc;
                    workSheet.Cells[row, "P"] = producto.PrecioPublico - (4 * dif);
                    workSheet.Cells[row, "Q"] = 5 * cc;
                    workSheet.Cells[row, "R"] = producto.PrecioPublico - (5 * dif);
                    ProductCodesEF paux = productCodes.Where(a => a.Referencia == producto.Codigo).FirstOrDefault();
                    if (paux != null)
                        workSheet.Cells[row, "S"] = paux.Prestashop;
                }
                previos = (count * 100) / total;
                if (progress != previos)
                {
                    progress = previos;
                    DispatcherHelper.CheckBeginInvokeOnUI(
                    () =>
                    {
                        // Dispatch back to the main thread
                        VM.ProgressVal = $"Progreso: {progress}%";
                    });


                }

            }
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
            workSheet.Columns[12].AutoFit();
            workSheet.Columns[13].AutoFit();
            workSheet.Columns[14].AutoFit();
            workSheet.Columns[15].AutoFit();
            workSheet.Columns[16].AutoFit();
            workSheet.Columns[17].AutoFit();
            workSheet.Columns[18].AutoFit();
            workSheet.Columns[19].AutoFit();
            progress = 100;
            excelApp.Visible = true;
            #endregion

            //Seccón de actualización de codigos PS
            /*
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
            }*/
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
