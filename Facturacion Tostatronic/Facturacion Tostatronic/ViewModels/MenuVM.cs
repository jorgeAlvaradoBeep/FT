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

namespace Facturacion_Tostatronic.ViewModels
{
    public class MenuVM
    {
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
        public MenuVM()
        {
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
                try
                {
                    workSheet.Shapes.AddPicture(@"C:\Users\Jorge\Documents\MEGAsync\Imagenes\" + producto.image, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, left + 5, top + 5, 120, 120);
                }
                catch (Exception ex)
                {
                    workSheet.Shapes.AddPicture(@"C:\Users\Jorge\Documents\MEGAsync\Imagenes\no_image.png", Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, left + 5, top + 5, 120, 120);
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

        public async void SetPSID()
        {
            WaitPlease wp = new WaitPlease();
            wp.Show();
            Response res = await WebService.GetDataForInvoice(URLData.product_ps);
            List<ProductPS> products = JsonConvert.DeserializeObject<List<ProductPS>>(res.data.ToString());
            products = await Task.Run(() => SetFinalList(products));
            res = await WebService.GetDataForInvoice(URLData.product_ps_web);
            List<ProductPS> productsWeb = JsonConvert.DeserializeObject<List<ProductPS>>(res.data.ToString());
            foreach (ProductPS p in products)
            {
                var item = productsWeb.FirstOrDefault(o => o.idProduct == p.idProduct);
                if (item != null)
                    p.ps = item.ps;
            }
            products = await Task.Run(() => RemoveNullProducts(products));
            if (products.Count > 0)
            {
                res = await WebService.InsertData(products, URLData.product_ps);
                if (res.succes)
                    MessageBox.Show("Registros agregados correctamente", "Exito", MessageBoxButton.OK, MessageBoxImage.Information);
                else
                    MessageBox.Show("Error: " + res.message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
                MessageBox.Show("Sin registros a agregar", "Exito", MessageBoxButton.OK, MessageBoxImage.Information);

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

        public async void GenerateNewProductsList()
        {

            int progress = 0;
            ProgressWindow pw = new ProgressWindow();
            pw.Show();
            Response res = await WebService.GetData("limit", "100", URLData.product_new_product_list);
            List<ProductList> products = JsonConvert.DeserializeObject<List<ProductList>>(res.data.ToString());
            bool finish = await Task.Run(() => GenerateProductListExcel(products, ref progress, pw));
            pw.Close();
        }

        public async void UpdateDistributorPriceNP()
        {
            WaitPlease pw = new WaitPlease();
            pw.Show();
            Response res = await WebService.GetDataForInvoice(URLData.product_distributor_price);
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
            res = await WebService.GetData(products, URLData.product_distributor_price_np);
            if (res.succes)
                MessageBox.Show("Precios actualizados correctamente" + res.message, "Exito", MessageBoxButton.OK, MessageBoxImage.Information);
            else
                MessageBox.Show("Error al actualizar precios: " + res.message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            pw.Close();
        }
    }
}
