using Facturacion_Tostatronic.Models;
using Facturacion_Tostatronic.Services;
using Facturacion_Tostatronic.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Excel = Microsoft.Office.Interop.Excel;

namespace Facturacion_Tostatronic.ViewModels.Commands.ProductsCommands
{
    public class GetNewPICommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            int progress = 0;
            ProgressWindow pw = new ProgressWindow();
            pw.Show();
            Response res = await WebService.GetDataForInvoice(URLData.new_pi);
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
            workSheet.Cells[1, "C"] = "Image";
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
                oRange = (Microsoft.Office.Interop.Excel.Range)workSheet.Cells[row, 3];
                left = (float)((double)oRange.Left);
                top = (float)((double)oRange.Top);
                string basePathForMega = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
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
            progress = 100;
            excelApp.Visible = true;

            return true;
        }
    }
}
