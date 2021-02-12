using Facturacion_Tostatronic.ViewModels.Commands;
using Facturacion_Tostatronic.ViewModels.Commands.PDVCommands.SaleCommands;
using Facturacion_Tostatronic.ViewModels.Commands.ProductsCommands;
using Facturacion_Tostatronic.Views;
using Facturacion_Tostatronic.Views.PDV.Sales;
using Facturacion_Tostatronic.Views.Products;
using System;
using Excel = Microsoft.Office.Interop.Excel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Facturacion_Tostatronic.Services;
using System.Security.Policy;
using Facturacion_Tostatronic.Models;
using RestSharp.Serialization.Json;
using Newtonsoft.Json;
using System.Windows;
using Facturacion_Tostatronic.Models.Products;
using Facturacion_Tostatronic.ViewModels.Commands.PricesCommands;
using Facturacion_Tostatronic.ViewModels.Commands.ImagesCommands;
using Microsoft.Win32;
using GroupDocs.Conversion;
using GroupDocs.Conversion.Options.Convert;
using GroupDocs.Conversion.FileTypes;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Media.Media3D;
using System.Drawing.Imaging;
using nQuant;
using System.Text.RegularExpressions;
using Facturacion_Tostatronic.ViewModels.Commands.MenuCommands;
using Facturacion_Tostatronic.ViewModels.Commands.PDVCommands;

namespace Facturacion_Tostatronic.ViewModels
{
    public class MenuVM : BaseNotifyPropertyChanged
    {
        #region declaracion De Comandos
        public CreateInvoiceCommand CreateInvoiceCommand { get; set; }
        public InvoiceConfigCommand InvoiceConfigCommand { get; set; }
        public SeeInvoiceCommand SeeInvoiceCommand { get; set; }
        public PrinterConfigCommand PrinterConfigCommand { get; set; }
        public CreateSaleCommand CreateSaleCommand { get; set; }
        public CreateProductsCommand CreateProductsCommand { get; set; }
        public CreateProductsListCommand CreateProductsListCommand { get; set; }
        public CreateNewProductsListCommand CreateNewProductsListCommand { get; set; }
        public SetPSIDCommand SetPSIDCommand { get; set; }
        public UpdateDistributorPriceNPCommand UpdateDistributorPriceNPCommand { get; set; }
        public CreatePagePricesCommand CreatePagePricesCommand { get; set; }
        public ConvertImageToPNGCommand ConvertImageToPNGCommand { get; set; }
        public CreateImagesCommand CreateImagesCommand { get; set; }
        public UpdatePublicPriceCommand UpdatePublicPriceCommand { get; set; }
        public MenuPageTwoCommand MenuPageTwoCommand { get; set; }
        public MenuToPageOneCommand MenuToPageOneCommand { get; set; }
        public UpdateDistributorPriceCommand UpdateDistributorPriceCommand { get; set; }
        public GetOrdersFromPSCommand GetOrdersFromPSCommand { get; set; }
        public CreateMLPricesCommand CreateMLPricesCommand { get; set; }
        public SetBarCodeCommand SetBarCodeCommand { get; set; }
        public ViewWarehouseMenuCommand ViewWarehouseMenuCommand { get; set; }
        public ViewUpdateImageCommand ViewUpdateImageCommand { get; set; }
        #endregion
        #region menuPages
        private Visibility menuPageOne;

        public Visibility MenuPageOne
        {
            get { return menuPageOne; }
            set { SetValue(ref menuPageOne,value); }
        }
        private Visibility menuPageTwo;

        public Visibility MenuPageTwo
        {
            get { return menuPageTwo; }
            set { SetValue(ref menuPageTwo, value); }
        }
        #endregion
        public MenuVM()
        {
            #region Inicializacion de comando
            CreateInvoiceCommand = new CreateInvoiceCommand(this);
            InvoiceConfigCommand = new InvoiceConfigCommand(this);
            SeeInvoiceCommand = new SeeInvoiceCommand(this);
            PrinterConfigCommand = new PrinterConfigCommand(this);
            CreateSaleCommand = new CreateSaleCommand(this);
            CreateProductsCommand = new CreateProductsCommand(this);
            CreateProductsListCommand = new CreateProductsListCommand(this);
            SetPSIDCommand = new SetPSIDCommand(this);
            CreateNewProductsListCommand = new CreateNewProductsListCommand(this);
            UpdateDistributorPriceNPCommand = new UpdateDistributorPriceNPCommand(this);
            CreatePagePricesCommand = new CreatePagePricesCommand(this);
            ConvertImageToPNGCommand = new ConvertImageToPNGCommand(this);
            CreateImagesCommand = new CreateImagesCommand(this);
            UpdatePublicPriceCommand = new UpdatePublicPriceCommand(this);
            MenuPageTwoCommand = new MenuPageTwoCommand(this);
            MenuToPageOneCommand = new MenuToPageOneCommand(this);
            UpdateDistributorPriceCommand = new UpdateDistributorPriceCommand(this);
            GetOrdersFromPSCommand = new GetOrdersFromPSCommand(this);
            CreateMLPricesCommand = new CreateMLPricesCommand(this);
            SetBarCodeCommand = new SetBarCodeCommand(this);
            ViewWarehouseMenuCommand = new ViewWarehouseMenuCommand(this);
            ViewUpdateImageCommand = new ViewUpdateImageCommand(this);
            #endregion
            MenuPageOne = Visibility.Visible;
            MenuPageTwo = Visibility.Hidden;
        }

        public void CreateInvoice()
        {
            CreateInvoiceV cI = new CreateInvoiceV();
            cI.ShowDialog();
        }
        public void SetInvoiceConfiguration()
        {
            InvoiceConfig iC = new InvoiceConfig();
            iC.ShowDialog();
        }
        public void CallSeeInvoiceView()
        {
            SeeInvoiceV sIV = new SeeInvoiceV();
            sIV.ShowDialog();
        }
        public void CallPrinterConfig()
        {
            PrinterConfigV pC = new PrinterConfigV();
            pC.ShowDialog();
        }
        public void CallSale()
        {
            SaleV sV = new SaleV();
            sV.ShowDialog();
        }
        public void CallProductAddingV()
        {
            AddProductV aPV = new AddProductV();
            aPV.ShowDialog();
        }

        public async void GenerateProductList()
        {
            int progress = 0;
            ProgressWindow pw = new ProgressWindow();
            pw.Show();
            Response res = await WebService.GetDataForInvoice(URLData.product_list);
            if (!res.succes)
            {
                MessageBox.Show(res.message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                pw.Close();
                return;
            }
            List<ProductList> products = JsonConvert.DeserializeObject<List<ProductList>>(res.data.ToString());
            bool finish = await Task.Run(() => GenerateProductListExcel(products, ref progress, pw));
            pw.Close();

        }

        bool GenerateProductListExcel(List<ProductList> products, ref int progress, ProgressWindow pw)
        {
            var excelApp = new Excel.Application();
            // Make the object visible.
            excelApp.Visible = false;
            excelApp.Workbooks.Add();
            Excel._Worksheet workSheet = (Excel.Worksheet)excelApp.ActiveSheet;
            workSheet.Cells[1, "A"] = "Referencia";
            workSheet.Cells[1, "B"] = "Nombre Del producto";
            workSheet.Cells[1, "C"] = "Precio Publico";
            workSheet.Cells[1, "D"] = "Precio Distribuidor";
            workSheet.Cells[1, "E"] = "Precio Minimo";
            workSheet.Cells[1, "F"] = "Image";
            var row = 1;
            Microsoft.Office.Interop.Excel.Range oRange;
            float left;
            float top;
            int count = 0;
            int total = products.Count;
            int previos = progress;

            foreach (var producto in products)
            {
                row++;
                count++;
                workSheet.Cells[row, "A"] = producto.idProduct;
                workSheet.Cells[row, "B"] = producto.name;
                workSheet.Cells[row, "C"] = producto.publico;
                workSheet.Cells[row, "D"] = producto.distribuidor;
                workSheet.Cells[row, "E"] = producto.minimo;
                oRange = (Microsoft.Office.Interop.Excel.Range)workSheet.Cells[row, 6];
                left = (float)((double)oRange.Left);
                top = (float)((double)oRange.Top);
                string basePathForMega= Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                basePathForMega = Path.Combine(basePathForMega, @"MEGAsync\Imagenes\");
                try
                {
                    workSheet.Shapes.AddPicture(basePathForMega + producto.image, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, left + 5, top + 5, 120, 120);
                }
                catch (Exception ex)
                {
                    workSheet.Shapes.AddPicture(basePathForMega + "no_image.png", Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, left + 5, top + 5, 120, 120);
                }
                previos = (count * 100) / total;
                if (progress != previos)
                {
                    progress = previos;
                    pw.Dispatcher.Invoke(() =>
                    {
                        pw.changeProgress();
                    });


                }

            }
            workSheet.Rows.RowHeight = 135;
            workSheet.Columns[1].AutoFit();
            workSheet.Columns[2].AutoFit();
            workSheet.Columns[3].AutoFit();
            workSheet.Columns[5].AutoFit();
            progress = 100;
            excelApp.Visible = true;

            return true;
        }

        #region SetPSID
        public async void SetPSID()
        {
            WaitPlease wp = new WaitPlease();
            wp.Show();

            Response res = await WebService.GetDataForInvoice(URLData.product_ps);
            List<ProductPS> products = JsonConvert.DeserializeObject<List<ProductPS>>(res.data.ToString());
            string result =await Task.Run(() => PrestashopService.GetWOUPCProducts(products));
            if (string.IsNullOrEmpty(result))
                MessageBox.Show("Exito al actualizar cosigos upc");
            else
                MessageBox.Show("Error: " + Environment.NewLine + result, "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            //Response res = await WebService.GetDataForInvoice(URLData.product_ps);
            //List<ProductPS> products = JsonConvert.DeserializeObject<List<ProductPS>>(res.data.ToString());
            //products = await Task.Run(() => SetFinalList(products));
            //res = await WebService.GetDataForInvoice(URLData.product_ps_web);
            //List<ProductPS> productsWeb = JsonConvert.DeserializeObject<List<ProductPS>>(res.data.ToString());
            //foreach (ProductPS p in products)
            //{
            //    var item = productsWeb.FirstOrDefault(o => o.idProduct == p.idProduct);
            //    if (item != null)
            //        p.ps = item.ps;
            //}
            //products = await Task.Run(() => RemoveNullProducts(products));
            //if (products.Count > 0)
            //{
            //    res = await WebService.InsertData(products, URLData.product_ps);
            //    if (res.succes)
            //        MessageBox.Show("Registros agregados correctamente", "Exito", MessageBoxButton.OK, MessageBoxImage.Information);
            //    else
            //        MessageBox.Show("Error: " + res.message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            //}
            //else
            //    MessageBox.Show("Sin registros a agregar", "Exito", MessageBoxButton.OK, MessageBoxImage.Information);

            wp.Close();

        }

        List<ProductPS> SetFinalList(List<ProductPS> products)
        {
            List<ProductPS> eliminados = new List<ProductPS>();
            foreach (ProductPS p in products)
            {
                if (!string.IsNullOrEmpty(p.ps))
                    eliminados.Add(p);
            }
            foreach (ProductPS p in eliminados)
            {
                products.Remove(p);
            }
            return products;
        }
        List<ProductPS> RemoveNullProducts(List<ProductPS> products)
        {
            List<ProductPS> eliminados = new List<ProductPS>();
            foreach (ProductPS p in products)
            {
                if (string.IsNullOrEmpty(p.ps))
                    eliminados.Add(p);
            }
            foreach (ProductPS p in eliminados)
            {
                products.Remove(p);
            }
            return products;
        }
        #endregion

        public async void GenerateNewProductsList()
        {

            int progress = 0;
            ProgressWindow pw = new ProgressWindow();
            pw.Show();
            Response res = await WebService.GetData("limit", "100", URLData.product_new_product_list);
            if (!res.succes)
            {
                MessageBox.Show(res.message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                pw.Close();
                return;
            }
            List<ProductList> products = JsonConvert.DeserializeObject<List<ProductList>>(res.data.ToString());
            bool finish = await Task.Run(() => GenerateProductListExcel(products, ref progress, pw));
            pw.Close();
        }

        public async void UpdateDistributorPriceNP()
        {
            WaitPlease pw = new WaitPlease();
            pw.Show();
            Response res = await WebService.GetDataForInvoice(URLData.product_distributor_price);
            if(!res.succes)
            {
                MessageBox.Show(res.message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                pw.Close();
                return;
            }
            List<DistributorPrice> products = JsonConvert.DeserializeObject<List<DistributorPrice>>(res.data.ToString());
            var printers = products.Where(producto => producto.name.Contains("Creality"));
            List<DistributorPrice> aux = new List<DistributorPrice>();
            foreach(DistributorPrice product in printers)
            {
                aux.Add(product);
            }
            foreach(DistributorPrice p in aux)
            {
                products.Remove(p);
            }
            res = await WebService.InsertData(products, URLData.updateProductsPriceTosta);
            if (res.succes)
                MessageBox.Show("Precios actualizados correctamente" + res.message, "Exito", MessageBoxButton.OK, MessageBoxImage.Information);
            else
                MessageBox.Show("Error al actualizar precios: " + res.message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            pw.Close();
        }

        public async void UpdatePublicPrice()
        {
            WaitPlease pw = new WaitPlease();
            pw.Show();
            Response res = await WebService.GetDataForInvoice(URLData.product_public_price);
            if (!res.succes)
            {
                MessageBox.Show(res.message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                pw.Close();
                return;
            }
            List<DistributorPrice> products = JsonConvert.DeserializeObject<List<DistributorPrice>>(res.data.ToString());
            res = await WebService.InsertData(products, URLData.updateProductsPriceTosta);
            if (res.succes)
                MessageBox.Show("Precios actualizados correctamente" + res.message, "Exito", MessageBoxButton.OK, MessageBoxImage.Information);
            else
                MessageBox.Show("Error al actualizar precios: " + res.message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            pw.Close();
        }

        public async void UpdateDistributorPrice()
        {
            WaitPlease pw = new WaitPlease();
            pw.Show();
            Response res = await WebService.GetDataForInvoice(URLData.product_distributor_price);
            if (!res.succes)
            {
                MessageBox.Show(res.message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                pw.Close();
                return;
            }
            List<DistributorPrice> products = JsonConvert.DeserializeObject<List<DistributorPrice>>(res.data.ToString());
            res = await WebService.InsertData(products, URLData.updateProductsPriceTosta);
            if (res.succes)
                MessageBox.Show("Precios actualizados correctamente" + res.message, "Exito", MessageBoxButton.OK, MessageBoxImage.Information);
            else
                MessageBox.Show("Error al actualizar precios: " + res.message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            pw.Close();
        }

        public async void GetPagePricesExcel()
        {
            ProgressWindow pw = new ProgressWindow();
            pw.Show();
            Response res = await WebService.GetDataForInvoice(URLData.product_page_prices);
            if (!res.succes)
            {
                MessageBox.Show(res.message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                pw.Close();
                return;
            }
            List<PageProduct> products = JsonConvert.DeserializeObject<List<PageProduct>>(res.data.ToString());
            bool finish = await Task.Run(() => GeneratePagePricesExcel(products, pw));
            pw.Close();
        }

        bool GeneratePagePricesExcel(List<PageProduct> products, ProgressWindow pw)
        {
            var excelApp = new Excel.Application();
            // Make the object visible.
            excelApp.Visible = false;
            excelApp.Workbooks.Add();
            Excel._Worksheet workSheet = (Excel.Worksheet)excelApp.ActiveSheet;
            workSheet.Cells[1, "A"] = "Referencia";
            workSheet.Cells[1, "B"] = "Precio Publico";
            workSheet.Cells[1, "C"] = "Precio Distribuidor";
            workSheet.Cells[1, "D"] = "Precio Minimo";
            workSheet.Cells[1, "E"] = "Costo";
            workSheet.Cells[1, "F"] = "Precio 1";
            workSheet.Cells[1, "G"] = "Precio 2";
            workSheet.Cells[1, "H"] = "Precio 3";
            workSheet.Cells[1, "I"] = "Precio 4";
            workSheet.Cells[1, "J"] = "Precio Min. Pag.";
            var row = 1;
            int progress = 0, previos = 0;
            int count = 0;
            int total = products.Count;

            foreach (var producto in products)
            {
                row++;
                count++;
                producto.MinPrice = float.Parse(producto.minimo) * 1.06f;
                float div = (float.Parse(producto.publico) - producto.MinPrice) / 5f;
                producto.PriceOne = float.Parse(producto.publico) - div;
                producto.PriceTwo = float.Parse(producto.publico) - (2*div);
                producto.Pricetrhee = float.Parse(producto.publico) - (3*div);
                producto.PriceFour = float.Parse(producto.publico) - (4*div);
                workSheet.Cells[row, "A"] = producto.idProduct;
                workSheet.Cells[row, "B"] = producto.publico;
                workSheet.Cells[row, "C"] = producto.distribuidor;
                workSheet.Cells[row, "D"] = producto.minimo;
                workSheet.Cells[row, "E"] = producto.costo;
                workSheet.Cells[row, "F"] = producto.PriceOne;
                workSheet.Cells[row, "G"] = producto.PriceTwo;
                workSheet.Cells[row, "H"] = producto.Pricetrhee;
                workSheet.Cells[row, "I"] = producto.PriceFour;
                workSheet.Cells[row, "J"] = producto.MinPrice;

                previos = (count * 100) / total;
                if (progress != previos)
                {
                    progress = previos;
                    pw.Dispatcher.Invoke(() =>
                    {
                        pw.changeProgress();
                    });


                }

            }
            workSheet.Columns[1].AutoFit();
            workSheet.Columns[2].AutoFit();
            workSheet.Columns[3].AutoFit();
            workSheet.Columns[5].AutoFit();
            progress = 100;
            excelApp.Visible = true;
            return true;
        }

        public async void GetMLPricesExcel()
        {
            ProgressWindow pw = new ProgressWindow();
            pw.Show();
            Response res = await WebService.GetDataForInvoice(URLData.product_page_prices);
            List<PageProduct> products = JsonConvert.DeserializeObject<List<PageProduct>>(res.data.ToString());
            bool finish = await Task.Run(() => GenerateMLPricesExcel(products, pw));
            pw.Close();
        }

        bool GenerateMLPricesExcel(List<PageProduct> products, ProgressWindow pw)
        {
            var excelApp = new Excel.Application();
            // Make the object visible.
            excelApp.Visible = false;
            excelApp.Workbooks.Add();
            Excel._Worksheet workSheet = (Excel.Worksheet)excelApp.ActiveSheet;
            workSheet.Cells[1, "A"] = "Referencia";
            workSheet.Cells[1, "B"] = "Precio Publico";
            workSheet.Cells[1, "C"] = "Precio Distribuidor";
            workSheet.Cells[1, "D"] = "Precio Minimo";
            workSheet.Cells[1, "E"] = "Costo";
            workSheet.Cells[1, "F"] = "Precio PB";
            workSheet.Cells[1, "G"] = "Precio PB C Envio";
            workSheet.Cells[1, "H"] = "Precio PP";
            workSheet.Cells[1, "I"] = "Precio PP C Envio";
            var row = 1;
            int progress = 0, previos = 0;
            int count = 0;
            int total = products.Count;

            foreach (var producto in products)
            {
                row++;
                count++;
                producto.PriceOne = ((float.Parse(producto.minimo)+15)*1.135f)*1.14f;
                producto.PriceTwo = producto.PriceOne + 85;
                producto.Pricetrhee = ((float.Parse(producto.minimo) + 15) * 1.175f) * 1.14f;
                producto.PriceFour = producto.Pricetrhee + 85;
                workSheet.Cells[row, "A"] = producto.idProduct;
                workSheet.Cells[row, "B"] = producto.publico;
                workSheet.Cells[row, "C"] = producto.distribuidor;
                workSheet.Cells[row, "D"] = producto.minimo;
                workSheet.Cells[row, "E"] = producto.costo;
                workSheet.Cells[row, "F"] = producto.PriceOne;
                workSheet.Cells[row, "G"] = producto.PriceTwo;
                workSheet.Cells[row, "H"] = producto.Pricetrhee;
                workSheet.Cells[row, "I"] = producto.PriceFour;

                previos = (count * 100) / total;
                if (progress != previos)
                {
                    progress = previos;
                    pw.Dispatcher.Invoke(() =>
                    {
                        pw.changeProgress();
                    });


                }

            }
            workSheet.Columns[1].AutoFit();
            workSheet.Columns[2].AutoFit();
            workSheet.Columns[3].AutoFit();
            workSheet.Columns[5].AutoFit();
            progress = 100;
            excelApp.Visible = true;
            return true;
        }

        //Seccion de Imagenes

        #region ConvertToPNG

        public async void ChnageImagesToPNG()
        {
            ProgressWindow pw = new ProgressWindow();
            OpenFileDialog images = new OpenFileDialog();
            images.Filter =
                "Images (*.BMP;*.JPG;*.GIF; *.WEBP)|*.BMP;*.JPG;*.GIF;*.WEBP|" +
                "All files (*.*)|*.*";

            // Allow the user to select multiple images.
            images.Multiselect = true;
            images.Title = "Imagenes a convertir";
            images.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string errMessage=string.Empty;
            if (images.ShowDialog() == true)
            {
                if(images.FileNames.Length > 0)
                {
                    string fullPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    try
                    {
                        fullPath = System.IO.Path.Combine(fullPath, "Tostatronic");
                        fullPath = System.IO.Path.Combine(fullPath, "PNG");
                        if (!Directory.Exists(fullPath))
                        {
                            Directory.CreateDirectory(fullPath);
                        }
                    }
                    catch(Exception e)
                    {
                        MessageBox.Show("Error al crear directorio: " + e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    pw.Show();
                    errMessage = await Task.Run(() => ConvertImages(images.FileNames, fullPath, pw));
                }
                
            }
            pw.Close();
            if(!string.IsNullOrEmpty(errMessage))
                MessageBox.Show("Errores en el proceso: " + errMessage, "Errores en el contenido", MessageBoxButton.OK, MessageBoxImage.Error);
            else
                MessageBox.Show("Exito al convertir las "+ images.FileNames.Length.ToString()+" imagenes seleccionadas", "Proceso terminado", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        string ConvertImages(string[] fileNames, string fullPath, ProgressWindow pw)
        {
            string name;
            string errMessage = "";
            int progress = 0, previos = 0;
            int count = 0;
            int total = fileNames.Length;
            foreach (string filename in fileNames)
            {
                try
                {
                    count++;
                    Converter converter = new Converter(filename);
                    ImageConvertOptions options = new ImageConvertOptions
                    { // Set the conversion format to JPG
                        Format = ImageFileType.Png
                    };
                    name = Path.Combine(fullPath, Path.GetFileNameWithoutExtension(filename));
                    name += ".png";
                    converter.Convert(name, options);
                    previos = (count * 100) / total;
                    if (progress != previos)
                    {
                        progress = previos;
                        pw.Dispatcher.Invoke(() =>
                        {
                            pw.changeProgress(progress);
                        });


                    }

                }
                catch (Exception e)
                {
                    errMessage += "Error al convertir "+Path.GetFileName(filename) + ": " + e.Message + Environment.NewLine;
                }
            }
            return errMessage;
        }
        #endregion

        #region CreateImages
        public async void CreateImages()
        {
            ProgressWindow pw = new ProgressWindow();
            OpenFileDialog images = new OpenFileDialog();
            images.Filter =
                "Images (*.PNG)|*.PNG|" +
                "All files (*.*)|*.*";

            // Allow the user to select multiple images.
            images.Multiselect = true;
            images.Title = "Imagenes a crear";
            images.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string errMessage = string.Empty;
            if (images.ShowDialog() == true)
            {
                if (images.FileNames.Length > 0)
                {
                    string fullPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    try
                    {
                        fullPath = System.IO.Path.Combine(fullPath, "MEGAsync");
                        fullPath = System.IO.Path.Combine(fullPath, @"Imagenes Nuevas Pagina\NI");
                        if (!Directory.Exists(fullPath))
                        {
                            MessageBox.Show("Error en la carpeta de destino", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Error al establcer destino de salida: " + e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    pw.Show();
                    errMessage = await Task.Run(() => CreateImages(images.FileNames, fullPath, pw));
                }

            }
            pw.Close();
            if (!string.IsNullOrEmpty(errMessage))
                MessageBox.Show("Errores en el proceso: " + errMessage, "Errores en el contenido", MessageBoxButton.OK, MessageBoxImage.Error);
            else
                MessageBox.Show("Exito al convertir las " + images.FileNames.Length.ToString() + " imagenes seleccionadas", "Proceso terminado", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        string CreateImages(string[] fileNames, string fullPath, ProgressWindow pw)
        {
            string name;
            string errMessage = "";
            int progress = 0, previos = 0;
            int count = 0;
            int total = fileNames.Length;
            string basePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string baseImagePath = Path.Combine(basePath, @"MEGAsync\Imagenes Nuevas Pagina\NI\Base Sin Logo.png");
            string baseImageWithLogoPath = Path.Combine(basePath, @"MEGAsync\Imagenes Nuevas Pagina\NI\Base Con Logo.png");
            string tempPath = Path.Combine(fullPath, "Temp");
            string logoPath = Path.Combine(fullPath, "Acomodar");
            string lowQualityPath = Path.Combine(fullPath, "Acomodar WEB");
            foreach (string filename in fileNames)
            {
                try
                {
                    count++;
                    errMessage += MergeImages(baseImagePath, filename, tempPath);
                    //newImage.Save(@"C:\Users\Tostatronic\Documents\MEGAsync\Imagenes Nuevas Pagina\imagen sin fondo\1.png", System.Drawing.Imaging.ImageFormat.Png);
                    previos = (count * 100) / total;
                    previos /= 4;
                    if (progress != previos)
                    {
                        progress = previos;
                        pw.Dispatcher.Invoke(() =>
                        {
                            pw.changeProgress(progress);
                        });
                    }

                }
                catch (Exception e)
                {
                    errMessage += "Error al crear " + Path.GetFileName(filename) + ": " + e.Message + Environment.NewLine;
                }
            }
            foreach (string filename in fileNames)
            {
                try
                {
                    count++;
                    errMessage += MergeImages(baseImageWithLogoPath, filename, logoPath);
                    previos = (count * 100) / total;
                    previos /= 4;
                    if (progress != previos)
                    {
                        progress = previos;
                        pw.Dispatcher.Invoke(() =>
                        {
                            pw.changeProgress(progress);
                        });
                    }

                }
                catch (Exception e)
                {
                    errMessage += "Error al crear " + Path.GetFileName(filename) + ": " + e.Message + Environment.NewLine;
                }
            }
            string[] fileEntries = Directory.GetFiles(logoPath);
            foreach (string filename in fileEntries)
            {
                try
                {
                    count++;
                    string imageNameA = Path.Combine(lowQualityPath, Path.GetFileName(filename));
                    VaryQualityLevel(filename, imageNameA);
                    previos = (count * 100) / total;
                    previos /= 4;
                    if (progress != previos)
                    {
                        progress = previos;
                        pw.Dispatcher.Invoke(() =>
                        {
                            pw.changeProgress(progress);
                        });
                    }

                }
                catch (Exception e)
                {
                    errMessage += "Error al crear " + Path.GetFileName(filename) + ": " + e.Message + Environment.NewLine;
                }
            }
            //Seccion para organizar las imagenes en carpetas
            //Variables para la organizacion
            string pathForBaseImages = Path.Combine(basePath, @"MEGAsync\Imagenes Nuevas Pagina\NI\Temp");
            string pathForHQImages = Path.Combine(basePath, @"MEGAsync\Imagenes Nuevas Pagina\NI\Acomodar");
            string pathForLQImages = Path.Combine(basePath, @"MEGAsync\Imagenes Nuevas Pagina\NI\Acomodar WEB");
            string finalProductPath = Path.Combine(basePath, @"MEGAsync\Imagenes Nuevas Pagina\NI\NP");
            string ImageWOBPath = Path.Combine(basePath, @"MEGAsync\Imagenes Nuevas Pagina\Producto final");
            string baseProductFolder="", lqProductFolder="", completeProductFolder="", imageWOBFolder="";
            string imageName, auxImageName=string.Empty; 
            foreach (string filename in fileEntries)
            {
                try
                {
                    count++;
                    imageName = Path.GetFileNameWithoutExtension(filename);
                    imageName = getFolderName(imageName);
                    if (!imageName.Equals(auxImageName))
                    {
                        auxImageName = imageName;
                        completeProductFolder = Path.Combine(finalProductPath, imageName);
                        baseProductFolder = Path.Combine(completeProductFolder, "Base");
                        lqProductFolder = Path.Combine(completeProductFolder, "LQ");
                        imageWOBFolder = Path.Combine(completeProductFolder, "WOB");
                        if(!Directory.Exists(completeProductFolder))
                        {
                            Directory.CreateDirectory(completeProductFolder);
                            Directory.CreateDirectory(baseProductFolder);
                            Directory.CreateDirectory(lqProductFolder);
                            Directory.CreateDirectory(imageWOBFolder);
                        }
                    }
                    File.Copy(Path.Combine(pathForBaseImages, Path.GetFileName(filename)), Path.Combine(baseProductFolder, Path.GetFileName(filename)), true);
                    File.Copy(Path.Combine(pathForHQImages, Path.GetFileName(filename)), Path.Combine(completeProductFolder, Path.GetFileName(filename)), true);
                    File.Copy(Path.Combine(pathForLQImages, Path.GetFileName(filename)), Path.Combine(lqProductFolder, Path.GetFileName(filename)), true);
                    File.Copy(Path.Combine(ImageWOBPath, Path.GetFileName(filename)), Path.Combine(imageWOBFolder, Path.GetFileName(filename)), true);
                   
                    previos = (count * 100) / total;
                    previos /= 4;
                    if (progress != previos)
                    {
                        progress = previos;
                        pw.Dispatcher.Invoke(() =>
                        {
                            pw.changeProgress(progress);
                        });
                    }

                }
                catch (Exception e)
                {
                    errMessage += "Error al crear " + Path.GetFileName(filename) + ": " + e.Message + Environment.NewLine;
                }
            }
            return errMessage;
        }

        string MergeImages(string baseImagePath, string productImagePath, string fullPath)
        {
            Image baseImageI;
            Image productImage;
            int width=2200, height=2200;
            try
            {
                baseImageI = Image.FromFile(baseImagePath);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            try
            {
                productImage = Image.FromFile(productImagePath);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            using (productImage)
            {
                using (var bitmap = new Bitmap(width, height))
                {
                    using (var canvas = Graphics.FromImage(bitmap))
                    {
                        canvas.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        canvas.DrawImage(baseImageI,
                                         new Rectangle(0,
                                                       0,
                                                       width,
                                                       height),
                                         new Rectangle(0,
                                                       0,
                                                       productImage.Width,
                                                       productImage.Height),
                                         GraphicsUnit.Pixel);
                        canvas.DrawImage(productImage,
                                         (bitmap.Width / 2) - (baseImageI.Width / 2),
                                         (bitmap.Height / 2) - (baseImageI.Height / 2));
                        canvas.Save();
                    }
                    try
                    {
                        string name = Path.Combine(fullPath, Path.GetFileNameWithoutExtension(productImagePath));
                        name += ".png";
                        bitmap.Save(name, System.Drawing.Imaging.ImageFormat.Png);
                        bitmap.Dispose();
                        return string.Empty;
                    }
                    catch (Exception ex) { return ex.Message; }
                }
            }
        }

        private void VaryQualityLevel(string imagePath, string savePath)
        {
            var quantizer = new WuQuantizer();
            Bitmap bmp1 = new Bitmap(imagePath);
            using (var quantized = quantizer.QuantizeImage(bmp1))
            {
                quantized.Save(savePath, ImageFormat.Png);
            }
            bmp1.Dispose();
        }

        string getFolderName(string fileName)
        {
            char[] chars = fileName.ToCharArray();
            int startPoint = 0, numberOfCharacters = 0;
            for(int i=0; i< fileName.Length; i++)
            {
                if (chars[i].Equals('_'))
                    startPoint = i;
                if (startPoint != 0)
                    numberOfCharacters++;
            }
            return fileName.Remove(startPoint, numberOfCharacters);
        }

        #endregion
    }
}
