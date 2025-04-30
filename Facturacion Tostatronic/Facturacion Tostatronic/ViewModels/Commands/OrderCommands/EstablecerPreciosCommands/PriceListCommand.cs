using Facturacion_Tostatronic.Models.EF_Models.EF_Orders;
using Facturacion_Tostatronic.Models.EF_Models.EFProduct;
using Facturacion_Tostatronic.ViewModels.Orders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Excel = Microsoft.Office.Interop.Excel;

namespace Facturacion_Tostatronic.ViewModels.Commands.OrderCommands.EstablecerPreciosCommands
{
    public class PriceListCommand : ICommand
    {
        public EstablecerPreciosVM VM { get; set; }
        public PriceListCommand(EstablecerPreciosVM vm)
        {
            VM = vm;
        }
        public event EventHandler CanExecuteChanged;
        public bool CanExecute(object parameter)
        {
            return true;
        }
        public async void Execute(object parameter)
        {
            VM.GettingData = true;
            await Task.Run(() => GenerateProductListExcel(VM.DetallesOrden.ToList()));
            VM.GettingData = false;
        }

        bool GenerateProductListExcel(List<DetalleOrdenExtendido> products)
        {
            var excelApp = new Excel.Application();
            // Make the object visible.
            excelApp.Visible = false;
            excelApp.Workbooks.Add();
            Excel._Worksheet workSheet = (Excel.Worksheet)excelApp.ActiveSheet;
            workSheet.Cells[1, "A"] = "Código";
            workSheet.Cells[1, "B"] = "Nombre";
            workSheet.Cells[1, "C"] = "Precio Minimo";
            workSheet.Cells[1, "D"] = "Imagen";
            var row = 1;
            Microsoft.Office.Interop.Excel.Range oRange;
            float left;
            float top;
            int count = 0;
            int total = products.Count;
            int progress = 0;
            int previos = progress;

            foreach (var producto in products)
            {
                row++;
                count++;
                workSheet.Cells[row, "A"] = producto.codigoProducto;
                workSheet.Cells[row, "B"] = producto.Nombre;
                workSheet.Cells[row, "C"] = producto.minimo;
                oRange = (Microsoft.Office.Interop.Excel.Range)workSheet.Cells[row, 4];
                left = (float)((double)oRange.Left);
                top = (float)((double)oRange.Top);
                string basePathForMega = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                basePathForMega = Path.Combine(basePathForMega, @"MEGAsync\Imagenes\");
                try
                {
                    workSheet.Shapes.AddPicture($"{basePathForMega}{producto.codigoProducto}.png", Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, left + 5, top + 5, 120, 120);
                }
                catch (Exception ex)
                {
                    //workSheet.Shapes.AddPicture(basePathForMega + "no_image.png", Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, left + 5, top + 5, 120, 120);
                }
            }
            if(row != 1)
                workSheet.Rows.RowHeight = 135;
            workSheet.Columns[1].AutoFit();
            workSheet.Columns[2].AutoFit();
            workSheet.Columns[3].AutoFit();
            progress = 100;
            excelApp.Visible = true;

            return true;
        }
    }
}
