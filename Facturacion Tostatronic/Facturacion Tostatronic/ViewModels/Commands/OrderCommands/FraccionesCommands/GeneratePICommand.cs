using Facturacion_Tostatronic.Models.EF_Models.EFProduct;
using Facturacion_Tostatronic.ViewModels.Orders;
using GalaSoft.MvvmLight.Threading;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Excel = Microsoft.Office.Interop.Excel;

namespace Facturacion_Tostatronic.ViewModels.Commands.OrderCommands.FraccionesCommands
{
    public class GeneratePICommand : ICommand
    {
        public FraccionesArancelariasVM VM { get; set; }
        public GeneratePICommand(FraccionesArancelariasVM vm)
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
            await Task.Run(() => GenerateProductListExcel(VM.FraccionesOrden.ToList()));
            VM.GettingData = false;
        }

        bool GenerateProductListExcel(List<FraccionArancelaria> products)
        {
            var excelApp = new Excel.Application();
            // Make the object visible.
            excelApp.Visible = false;
            excelApp.Workbooks.Add();
            Excel._Worksheet workSheet = (Excel.Worksheet)excelApp.ActiveSheet;
            workSheet.Cells[1, "B"] = "Codigo";
            workSheet.Cells[1, "C"] = "Nombre";
            workSheet.Cells[1, "D"] = "Imagen";
            workSheet.Cells[1, "E"] = "Precio";
            workSheet.Cells[1, "F"] = "Cantidad";
            workSheet.Cells[1, "G"] = "SubTotal";
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
                var pro =  VM.sl.ProductosDeOrdenesNavigation.FirstOrDefault(p => p.CodigoProducto == producto.Codigo);
                workSheet.Cells[row, "B"] = producto.Codigo;
                workSheet.Cells[row, "C"] = producto.Descripcion;
                workSheet.Cells[row, "E"] = producto.ValorDeclarado;
                workSheet.Cells[row, "F"] = pro.Cantidad;
                workSheet.Cells[row, "G"] = producto.ValorDeclarado * pro.Cantidad;
                oRange = (Microsoft.Office.Interop.Excel.Range)workSheet.Cells[row, 4];
                left = (float)((double)oRange.Left);
                top = (float)((double)oRange.Top);
                string basePathForMega = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                basePathForMega = Path.Combine(basePathForMega, @"MEGAsync\Imagenes\");
                try
                {
                    workSheet.Shapes.AddPicture($"{basePathForMega}{producto.Codigo}.png", Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, left + 5, top + 5, 90, 90);
                }
                catch (Exception ex)
                {
                    //workSheet.Shapes.AddPicture(basePathForMega + "no_image.png", Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, left + 5, top + 5, 120, 120);
                }
                progress = (count * 100) / total;
                if (progress != (previos+2))
                {
                    previos = progress;
                    VM.ProgresVal = $"Progreso: {progress}";
                }
            }
            workSheet.Rows.RowHeight = 100;
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
