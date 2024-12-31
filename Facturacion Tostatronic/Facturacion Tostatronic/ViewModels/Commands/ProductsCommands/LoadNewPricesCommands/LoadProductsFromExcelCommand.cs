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
using Facturacion_Tostatronic.Models.Products;

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
        {  VM.IsBusy = false;
        }
        //List<ProductCompleteNewArrival> GetDataFromExcel(string path)
        //{
        //    VM.IsBusy = true;
        //    int porcentaje = 0;
        //    Excel.Application xlApp = new Excel.Application();
        //    if (xlApp == null)
        //        return new List<ProductCompleteNewArrival>();
        //    Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(path);
        //    Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
        //    Excel.Range xlRange = xlWorksheet.UsedRange;

        //    int rowCount = VM.FilaFin-VM.FilaInicio;
        //    List<ProductCompleteNewArrival> products = new List<ProductCompleteNewArrival>();
        //    ProductCompleteNewArrival adicion;
        //    try
        //    {
        //        float aux = 0;
        //        for (int i = VM.FilaInicio; i <= VM.FilaFin; i++)
        //        {
        //            adicion = new ProductCompleteNewArrival();
        //            // Leer el valor de la celda actual
        //            if (xlRange.Cells[i,"B"] != null && xlRange.Cells[i, "B"].Value2 != null)
        //                adicion.Code = xlRange.Cells[i, "B"].Value2.ToString();

        //            if (xlRange.Cells[i, "C"] != null && xlRange.Cells[i, "C"].Value2 != null)
        //                adicion.Name = xlRange.Cells[i, "C"].Value2.ToString();

        //            if (xlRange.Cells[i, "D"] != null && xlRange.Cells[i, "D"].Value2 != null)
        //                adicion.NewStock = int.Parse(xlRange.Cells[i, "D"].Value2.ToString());

        //            if (xlRange.Cells[i, "L"] != null && xlRange.Cells[i, "L"].Value2 != null)
        //                adicion.BuyPrice = float.Parse(xlRange.Cells[i, "L"].Value2.ToString());

        //            if (xlRange.Cells[i, "P"] != null && xlRange.Cells[i, "P"].Value2 != null)
        //                adicion.MinimumPrice = float.Parse(xlRange.Cells[i, "P"].Value2.ToString());
        //            if (xlRange.Cells[i, "T"] != null && xlRange.Cells[i, "T"].Value2 != null)
        //                adicion.DistributorPrice = float.Parse(xlRange.Cells[i, "T"].Value2.ToString());
        //            if (xlRange.Cells[i, "X"] != null && xlRange.Cells[i, "X"].Value2 != null)
        //                adicion.PublicPrice = float.Parse(xlRange.Cells[i, "X"].Value2.ToString());

                   

        //            products.Add(adicion);
        //            porcentaje = (products.Count / rowCount) * 100;
        //            DispatcherHelper.CheckBeginInvokeOnUI(
        //            () =>
        //            {
        //                VM.PorcentajeDeAvance = $"Progreso: {porcentaje}%";
        //            });
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        MessageBox.Show($"Error No se pudo completar el proceso de lectura.{Environment.NewLine}" +
        //            $"Razon: {e.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        //        GC.Collect();
        //        GC.WaitForPendingFinalizers();

        //        // Cerrar y liberar
        //        xlWorkbook.Close();
        //        xlApp.Quit();

        //        Marshal.ReleaseComObject(xlRange);
        //        Marshal.ReleaseComObject(xlWorksheet);
        //        Marshal.ReleaseComObject(xlWorkbook);
        //        Marshal.ReleaseComObject(xlApp);
        //        return products;
        //    }
        //    GC.Collect();
        //    GC.WaitForPendingFinalizers();

        //    // Cerrar y liberar
        //    xlWorkbook.Close();
        //    xlApp.Quit();

        //    Marshal.ReleaseComObject(xlRange);
        //    Marshal.ReleaseComObject(xlWorksheet);
        //    Marshal.ReleaseComObject(xlWorkbook);
        //    Marshal.ReleaseComObject(xlApp);
        //    return products;
        //}
    }
}
