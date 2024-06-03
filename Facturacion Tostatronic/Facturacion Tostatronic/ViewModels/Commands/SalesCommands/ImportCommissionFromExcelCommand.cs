using Facturacion_Tostatronic.Models.EF_Models.EF_Orders;
using Facturacion_Tostatronic.ViewModels.Orders;
using GalaSoft.MvvmLight.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using Facturacion_Tostatronic.ViewModels.Sales;
using Excel = Microsoft.Office.Interop.Excel;
using Facturacion_Tostatronic.Models.EF_Models.EFSale;
using OfficeOpenXml;
using System.Globalization;
using System.IO;

namespace Facturacion_Tostatronic.ViewModels.Commands.SalesCommands
{

    public class ImportCommissionFromExcelCommand : ICommand
    {
        public EarningsVM VM { get; set; }
        public event EventHandler CanExecuteChanged;

        public ImportCommissionFromExcelCommand(EarningsVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            if (VM.FilaInicio == 0)
            {
                MessageBox.Show("Error: La Fila de inicio no puede ser la 1. Verifique",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else if (VM.FilaFin == 0)
            {
                MessageBox.Show("Error: La Fila de Fin no puede ser la 1. Verifique",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            VM.GettinData = true;
            List<EFExcelComissionsM> finish = await Task.Run(() => GetDataFromExcel());
            if (finish.Count > 0)
            {
                await Task.Run(() => InsertProductToGrid(finish));
            }
            VM.ChangeDataInfoCommand.Execute(VM);
            InsertEarningExcel();
            VM.GettinData = false;
            //Aqui haremos lo que continua para actualizar la orden.
        }
        void InsertProductToGrid(List<EFExcelComissionsM> finishList)
        {
            int cont = 0;
            foreach (var item in VM.Sales)
            {
                var data = finishList.Where(x => x.SaleID==item.idVenta).FirstOrDefault();
                if(data != null)
                {
                    item.Comision = data.Comission;
                    item.Envio = data.Shipping;
                }
            }
        }
        void InsertEarningExcel() 
        {
            int rowCount = VM.FilaFin;
            List<EFExcelComissionsM> products = new List<EFExcelComissionsM>();
            EFExcelComissionsM adicion;
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                FileInfo fileInfo = new FileInfo(VM.ImagePath);

                using (ExcelPackage package = new ExcelPackage(fileInfo))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[2]; // Asume que es la primera hoja
                    Dictionary<int, EarningSale> diccionarioVentas = VM.Sales.ToDictionary(v => v.idVenta);
                    for (int row = VM.FilaInicio; row <= VM.FilaFin; row++)
                    {
                        string valueA = worksheet.Cells[row, 1].Text; //Codigo
                        if(int.TryParse(valueA, NumberStyles.Any, CultureInfo.InvariantCulture, out int resultA))
                        {
                            if (VM.Sales.Any(x => x.idVenta == resultA))
                            {
                                worksheet.Cells[row, 1].Value = VM.Sales.Where(x => x.idVenta == resultA).FirstOrDefault().Ganancia;
                            }
                        }
                        
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error No se pudo completar el proceso de lectura.{Environment.NewLine}" +
                    $"Razon: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
        List<EFExcelComissionsM> GetDataFromExcel()
        {
            int porcentaje = 0;
            Excel.Application xlApp = new Excel.Application();
            if (xlApp == null)
                return new List<EFExcelComissionsM>();
            if(string.IsNullOrEmpty(VM.ImagePath))
            {
                MessageBox.Show("Error, debe seleccionar un archivo primero", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                return new List<EFExcelComissionsM>();
            }
            Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(VM.ImagePath);
            Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
            Excel.Range xlRange = xlWorksheet.UsedRange;

            int rowCount = VM.FilaFin;
            List<EFExcelComissionsM> products = new List<EFExcelComissionsM>();
            EFExcelComissionsM adicion;
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                FileInfo fileInfo = new FileInfo(VM.ImagePath);

                using (ExcelPackage package = new ExcelPackage(fileInfo))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[2]; // Asume que es la primera hoja

                    for (int row = VM.FilaInicio; row <= VM.FilaFin; row++)
                    {
                        string valueF = worksheet.Cells[row, 6].Text; // Columna F
                        string valueG = worksheet.Cells[row, 7].Text; // Columna G
                        string valueA = worksheet.Cells[row, 1].Text; // Columna G
                        adicion = new EFExcelComissionsM();
                        // Eliminar símbolos de pesos y comas
                        valueF = valueF.Replace("$", "").Replace(",", "");
                        valueG = valueG.Replace("$", "").Replace(",", "");

                        // Convertir a float
                        if (float.TryParse(valueF, NumberStyles.Any, CultureInfo.InvariantCulture, out float resultF) &&
                            float.TryParse(valueG, NumberStyles.Any, CultureInfo.InvariantCulture, out float resultG) &&
                            int.TryParse(valueA, NumberStyles.Any, CultureInfo.InvariantCulture, out int resultA))
                        {
                            adicion.Comission = resultF;
                            adicion.Shipping = resultG;
                            adicion.SaleID = resultA;
                            products.Add(adicion);
                        }
                        else
                        {
                            Console.WriteLine($"No se pudo convertir los valores en la fila {row}");
                        }
                    }
                }
            }
            catch (Exception ex) 
            {
                MessageBox.Show($"Error No se pudo completar el proceso de lectura.{Environment.NewLine}" +
                    $"Razon: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                GC.Collect();
                GC.WaitForPendingFinalizers();

                // Cerrar y liberar
                xlWorkbook.Close();
                xlApp.Quit();

                Marshal.ReleaseComObject(xlRange);
                Marshal.ReleaseComObject(xlWorksheet);
                Marshal.ReleaseComObject(xlWorkbook);
                Marshal.ReleaseComObject(xlApp);
                GC.Collect();
                GC.WaitForPendingFinalizers();
                // Cerrar y liberar
                xlWorkbook.Close();
                xlApp.Quit();
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
