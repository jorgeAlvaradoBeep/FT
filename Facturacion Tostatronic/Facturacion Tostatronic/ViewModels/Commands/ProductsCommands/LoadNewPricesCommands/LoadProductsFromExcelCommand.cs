using Facturacion_Tostatronic.Models.EF_Models.EF_Orders;
using Facturacion_Tostatronic.ViewModels.Products;
using GalaSoft.MvvmLight.Threading;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using Excel = Microsoft.Office.Interop.Excel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Facturacion_Tostatronic.ViewModels.Commands.ProductsCommands.LoadNewPricesCommands
{
    public class LoadProductsFromExcelCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public LoadNewPricesVM VM { get; set; }
        public LoadProductsFromExcelCommand(LoadNewPricesVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            if(VM.FilaInicio == 0 || VM.FilaFin == 0)
            {
                MessageBox.Show("Error: No se establecio el valor de alguna columna. Verifique",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }   
            var dialog = new OpenFileDialog
            {
                Filter = "Archivos Excel|*.xlsx;*.xls",
                Title = "Seleccionar archivo Excel"
            };

            if (dialog.ShowDialog() == true)
            {
               VM.ProductosActualizar = await Task.Run(() => GetDataFromExcel(dialog.FileName));
                VM.IsBusy = false;
            }
        }
        List<ProductOrderComplete> GetDataFromExcel(string path)
        {
            VM.IsBusy = true;
            int porcentaje = 0;
            Excel.Application xlApp = new Excel.Application();
            if (xlApp == null)
                return new List<ProductOrderComplete>();
            Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(path);
            Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
            Excel.Range xlRange = xlWorksheet.UsedRange;

            int rowCount = VM.FilaFin-VM.FilaInicio;
            List<ProductOrderComplete> products = new List<ProductOrderComplete>();
            ProductOrderComplete adicion;
            try
            {
                float aux = 0;
                for (int i = VM.FilaInicio; i <= VM.FilaFin; i++)
                {
                    adicion = new ProductOrderComplete();
                    // Leer el valor de la celda actual
                    if (xlRange.Cells[i,"B"] != null && xlRange.Cells[i, "B"].Value2 != null)
                        adicion.CodigoProducto = xlRange.Cells[i, "B"].Value2.ToString();

                    if (xlRange.Cells[i, "C"] != null && xlRange.Cells[i, "C"].Value2 != null)
                        adicion.NombreEs = xlRange.Cells[i, "C"].Value2.ToString();

                    if (xlRange.Cells[i, "D"] != null && xlRange.Cells[i, "D"].Value2 != null)
                        adicion.Cantidad = int.Parse(xlRange.Cells[i, "D"].Value2.ToString());

                    if (xlRange.Cells[i, "L"] != null && xlRange.Cells[i, "L"].Value2 != null)
                        adicion.Costo = decimal.Parse(xlRange.Cells[i, "D"].Value2.ToString());

                    if (xlRange.Cells[i, "P"] != null && xlRange.Cells[i, "P"].Value2 != null)
                        adicion.Minimo = decimal.Parse(xlRange.Cells[i, "P"].Value2.ToString());
                    if (xlRange.Cells[i, "T"] != null && xlRange.Cells[i, "T"].Value2 != null)
                        adicion.Distribuidor = decimal.Parse(xlRange.Cells[i, "T"].Value2.ToString());
                    if (xlRange.Cells[i, "X"] != null && xlRange.Cells[i, "X"].Value2 != null)
                        adicion.Publico = decimal.Parse(xlRange.Cells[i, "X"].Value2.ToString());

                   

                    products.Add(adicion);
                    porcentaje = (products.Count / rowCount) * 100;
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
                    $"Razon: {e.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
