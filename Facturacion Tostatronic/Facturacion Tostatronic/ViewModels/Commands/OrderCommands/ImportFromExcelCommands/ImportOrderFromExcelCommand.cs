using Facturacion_Tostatronic.ViewModels.Orders;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using System.Windows;
using System.Windows.Input;
using Facturacion_Tostatronic.Models.EF_Models.EF_Orders;
using GalaSoft.MvvmLight.Threading;
using System.Runtime.InteropServices;

namespace Facturacion_Tostatronic.ViewModels.Commands.OrderCommands.ImportFromExcelCommands
{
    public class ImportOrderFromExcelCommand : ICommand
    {
        public ImportFromExcelVM VM { get; set; }
        public event EventHandler CanExecuteChanged;

        public ImportOrderFromExcelCommand(ImportFromExcelVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            if(string.IsNullOrEmpty(VM.ExcelFile.ColumnCodigo) || string.IsNullOrEmpty(VM.ExcelFile.ColumnNombreEs)
                 || string.IsNullOrEmpty(VM.ExcelFile.ColumnNombreEn) || string.IsNullOrEmpty(VM.ExcelFile.ColumnQty)
                 || string.IsNullOrEmpty(VM.ExcelFile.ColumnPrecio) || string.IsNullOrEmpty(VM.ExcelFile.ColumnTargetPrice)
                || string.IsNullOrEmpty(VM.ExcelFile.ColumnLink))
            {
                MessageBox.Show("Error: No se establecio el valor de alguna columna. Verifique", 
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else if (VM.ExcelFile.FilaInicio==0)
            {
                MessageBox.Show("Error: La Fila de inicio no puede ser la 1. Verifique",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else if (VM.ExcelFile.FilaFin == 0)
            {
                MessageBox.Show("Error: La Fila de Fin no puede ser la 1. Verifique",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            VM.GettingData = true;
            List<ProductOrderComplete> finish = await Task.Run(() => GetDataFromExcel());
            if (finish.Count > 0) 
            {
                await Task.Run(() => InsertProductToGrid(finish));
            }
            VM.GettingData = false;
            //Aqui haremos lo que continua para actualizar la orden.
        }
        void InsertProductToGrid(List<ProductOrderComplete> finishList)
        {
            foreach (var item in finishList)
            {
                VM.Productos.Add(item);
            }
        }
        List<ProductOrderComplete> GetDataFromExcel()
        {
            int porcentaje = 0;
            Excel.Application xlApp = new Excel.Application();
            if (xlApp == null)
                return new List<ProductOrderComplete>();
            Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(VM.ImagePath);
            Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
            Excel.Range xlRange = xlWorksheet.UsedRange;

            int rowCount = VM.ExcelFile.FilaFin;
            List<ProductOrderComplete> products = new List<ProductOrderComplete>();
            ProductOrderComplete adicion;
            try
            {
                float aux=0;
                for (int i = VM.ExcelFile.FilaInicio; i <= rowCount; i++)
                {
                    adicion = new ProductOrderComplete();
                    // Leer el valor de la celda actual
                    if (xlRange.Cells[i, VM.ExcelFile.ColumnCodigo] != null && xlRange.Cells[i, VM.ExcelFile.ColumnCodigo].Value2 != null)
                        adicion.CodigoProducto = xlRange.Cells[i, VM.ExcelFile.ColumnCodigo].Value2.ToString();

                    if (xlRange.Cells[i, VM.ExcelFile.ColumnNombreEs] != null && xlRange.Cells[i, VM.ExcelFile.ColumnNombreEs].Value2 != null)
                        adicion.NombreEs = xlRange.Cells[i, VM.ExcelFile.ColumnNombreEs].Value2.ToString();

                    if (xlRange.Cells[i, VM.ExcelFile.ColumnNombreEn] != null && xlRange.Cells[i, VM.ExcelFile.ColumnNombreEn].Value2 != null)
                        adicion.NombreEn = xlRange.Cells[i, VM.ExcelFile.ColumnNombreEn].Value2.ToString();

                    if (xlRange.Cells[i, VM.ExcelFile.ColumnQty] != null && xlRange.Cells[i, VM.ExcelFile.ColumnQty].Value2 != null)
                        adicion.Cantidad = int.Parse(xlRange.Cells[i, VM.ExcelFile.ColumnQty].Value2.ToString());

                    if (xlRange.Cells[i, VM.ExcelFile.ColumnPrecio] != null && xlRange.Cells[i, VM.ExcelFile.ColumnPrecio].Value2 != null)
                    {
                        float.TryParse(xlRange.Cells[i, VM.ExcelFile.ColumnPrecio].Value2.ToString(), out aux);
                        adicion.Precio = aux;
                        adicion.SubTotal = (decimal)(adicion.Precio * adicion.Cantidad);
                        aux = 0;
                    }

                    if (xlRange.Cells[i, VM.ExcelFile.ColumnTargetPrice] != null && xlRange.Cells[i, VM.ExcelFile.ColumnTargetPrice].Value2 != null)
                    {
                        float.TryParse(xlRange.Cells[i, VM.ExcelFile.ColumnTargetPrice].Value2.ToString(), out aux);
                        adicion.TargetPrice = aux;
                        aux = 0;
                    }
                     
                    if (xlRange.Cells[i, VM.ExcelFile.ColumnLink] != null && xlRange.Cells[i, VM.ExcelFile.ColumnLink].Value2 != null)
                        adicion.Link = xlRange.Cells[i, VM.ExcelFile.ColumnLink].Value2.ToString();

                    products.Add(adicion);
                    porcentaje = (products.Count / VM.ExcelFile.FilaFin) * 100;
                    DispatcherHelper.CheckBeginInvokeOnUI(
                    () =>
                    {
                        VM.PorcentajeDeAvance = $"Progreso: {porcentaje}%";
                    });
                }
            }
            catch (Exception e) 
            {
                MessageBox.Show($"Error No se pudo completar el proceso de lectura.{Environment.NewLine}" +
                    $"Razon: {e.Message}","Error",MessageBoxButton.OK, MessageBoxImage.Error);
                GC.Collect();
                GC.WaitForPendingFinalizers();

                // Cerrar y liberar
                xlWorkbook.Close();
                xlApp.Quit();

                Marshal.ReleaseComObject(xlRange);
                Marshal.ReleaseComObject(xlWorksheet);
                Marshal.ReleaseComObject(xlWorkbook);
                Marshal.ReleaseComObject(xlApp);
                return products;
            }
            GC.Collect();
            GC.WaitForPendingFinalizers();

            // Cerrar y liberar
            xlWorkbook.Close();
            xlApp.Quit();

            Marshal.ReleaseComObject(xlRange);
            Marshal.ReleaseComObject(xlWorksheet);
            Marshal.ReleaseComObject(xlWorkbook);
            Marshal.ReleaseComObject(xlApp);
            return products;
        }
    }
}
