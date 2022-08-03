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
            Application.Current.Dispatcher.Invoke(new Action(() =>
            { VM.ProgressVal = "Cargando datos inciales, espere un momento."; }));
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



            VM.GettingData = false;
        }
    }
}
