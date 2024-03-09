using Facturacion_Tostatronic.Models.EF_Models.EF_Orders;
using Facturacion_Tostatronic.ViewModels.Orders;
using Facturacion_Tostatronic.Views;
using Facturacion_Tostatronic.Views.OrdersV;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using System.Windows.Input;
using System.IO;
using GalaSoft.MvvmLight.Threading;
using Bukimedia.PrestaSharp.Entities.AuxEntities;
using System.Diagnostics;

namespace Facturacion_Tostatronic.ViewModels.Commands.OrderCommands.OrderCheckCommands
{
    public class ExportOrderToExcelCommand : ICommand
    {
        public OrderCheckVM VM { get; set; }
        public event EventHandler CanExecuteChanged;

        public ExportOrderToExcelCommand(OrderCheckVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {

            VM.GettingData=true;
            bool finish = await Task.Run(() => GenerateProductListExcel(VM.ComlpleteOrder.ProductosDeOrdenesNavigation.ToList()));
            VM.GettingData = false;
        }
        bool GenerateProductListExcel(List<ProductOrderComplete> products)
        {
            var excelApp = new Excel.Application();
            // Make the object visible.
            excelApp.Visible = false;
            excelApp.Workbooks.Add();
            Excel._Worksheet workSheet = (Excel.Worksheet)excelApp.ActiveSheet;
            workSheet.Cells[1, "A"] = "Code";
            workSheet.Cells[1, "B"] = "Spanish Name";
            workSheet.Cells[1, "C"] = "English Name";
            workSheet.Cells[1, "D"] = "QTY";
            workSheet.Cells[1, "E"] = "Price";
            workSheet.Cells[1, "F"] = "SubTotal";
            workSheet.Cells[1, "G"] = "Target Price";
            workSheet.Cells[1, "H"] = "Link";
            workSheet.Cells[1, "I"] = "Image";
            var row = 1;
            Microsoft.Office.Interop.Excel.Range oRange;
            float left;
            float top;
            int count = 0;
            int total = products.Count;
            int progress =0;
            int previos = progress;

            foreach (var producto in products)
            {
                row++;
                count++;
                workSheet.Cells[row, "A"] = producto.CodigoProducto;
                workSheet.Cells[row, "B"] = producto.NombreEs;
                workSheet.Cells[row, "C"] = producto.NombreEn;
                workSheet.Cells[row, "D"] = producto.Cantidad;
                workSheet.Cells[row, "E"] = producto.Precio;
                workSheet.Cells[row, "F"] = producto.SubTotal;
                workSheet.Cells[row, "G"] = producto.TargetPrice;
                workSheet.Cells[row, "H"] = producto.Link;
                oRange = (Microsoft.Office.Interop.Excel.Range)workSheet.Cells[row, 9];
                left = (float)((double)oRange.Left);
                top = (float)((double)oRange.Top);
                string basePathForMega = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                basePathForMega = Path.Combine(basePathForMega, @"MEGAsync\Imagenes\");
                try
                {
                    workSheet.Shapes.AddPicture($"{basePathForMega}{producto.CodigoProducto}.png", Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, left + 5, top + 5, 120, 120);
                }
                catch (Exception ex)
                {
                    workSheet.Shapes.AddPicture(basePathForMega + "no_image.png", Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, left + 5, top + 5, 120, 120);
                }
                previos = (count * 100) / total;
                if (progress != previos)
                {
                    progress = previos;
                    DispatcherHelper.CheckBeginInvokeOnUI(
                    () =>
                    {
                        VM.ProgressVal = $"Progreso: {progress}";
                    });
                }

            }
            workSheet.Rows.RowHeight = 135;
            workSheet.Columns[1].AutoFit();
            workSheet.Columns[2].AutoFit();
            workSheet.Columns[3].AutoFit();
            workSheet.Columns[4].AutoFit();
            workSheet.Columns[5].AutoFit();
            workSheet.Columns[6].AutoFit();
            workSheet.Columns[7].AutoFit();
            workSheet.Columns[8].AutoFit();
            progress = 100;
            excelApp.Visible = true;

            return true;
        }
    }
}
