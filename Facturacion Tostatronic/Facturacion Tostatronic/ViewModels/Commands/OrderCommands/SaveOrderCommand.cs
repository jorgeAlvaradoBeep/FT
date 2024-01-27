using Facturacion_Tostatronic.ViewModels.Orders;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Bukimedia.PrestaSharp.Entities.AuxEntities;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;
using GalaSoft.MvvmLight.Threading;
using Facturacion_Tostatronic.Models.EF_Models.EF_Orders;
using System.Windows.Documents;
using System.Collections.Generic;
using Facturacion_Tostatronic.Models;
using Facturacion_Tostatronic.Services;
using System.Windows;
using System.Linq;

namespace Facturacion_Tostatronic.ViewModels.Commands.OrderCommands
{
    public class SaveOrderCommand : ICommand
    {
        public MakeOrderVM VM { get; set; }
        public event EventHandler CanExecuteChanged;

        public SaveOrderCommand(MakeOrderVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            VM.GettingData = true;
            //Convertimos la Orden para que cuadre con el objeto de ;a API
            APIOrder aO = new APIOrder();
            aO.OrdenID = ObtenerFechaActualComoEntero();
            DateTime fechaHoy = DateTime.Today;
            aO.FechaCreacion = fechaHoy.ToString("dd/MM/yyyy");
            //aO.FechaFin = "--";
            //aO.CostoAA = 0;
            //aO.CostoEnvio = 0;
            aO.ProductosDeOrdenesNavigation =  await Task.Run(() => SetNewProductList(aO.OrdenID));
            Response res = await WebService.InsertData(aO, URLData.Orders);
            if (!res.succes)
            {
                MessageBox.Show("Error: " + res.message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                VM.GettingData = false;
                return;
            }
            else
            {
                var aux = VM.Order.Products.Where(product => product.ProductInfoExist == false).ToList();
                if(aux!=null)
                {
                    if(aux.Count > 0)
                    {
                        List<APIProductOrderInformation> addProductInfo = new List<APIProductOrderInformation>();
                        await Task.Run((() =>
                        {
                            foreach (EFOrderProduct product in aux)
                            {
                                APIProductOrderInformation p = new APIProductOrderInformation(product.codigo,product.nombreES, product.nombreEN, product.Link);
                                addProductInfo.Add(p);
                            }
                        }));
                        if (addProductInfo.Count > 0)
                        {
                            res = await WebService.InsertData(addProductInfo, URLData.InserProductOrderInfoList);
                            if (!res.succes)
                            {
                                MessageBox.Show("Error: " + res.message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                VM.GettingData = false;
                                return;
                            }
                            else
                            {
                                VM.GettingData = false;
                                MessageBox.Show($"Orden Agregada Con Exito y{Environment.NewLine}" +
                                    $"{addProductInfo.Count} Productos Actualizados", "Correcto", MessageBoxButton.OK, MessageBoxImage.Information);
                                return;
                            }
                        }
                    }
                }
                
                MessageBox.Show($"Orden Agregada Con Exito", "Correcto", MessageBoxButton.OK, MessageBoxImage.Information);
                
            }

            //DispatcherHelper.CheckBeginInvokeOnUI(
            //() =>
            //{
            //    // Dispatch back to the main thread
            //    VM.ProgressVal = $"Creando Archivo Excel: 0% de Progreso";
            //});
            //await Task.Run(() => GenerateProductListExcel());
            VM.GettingData = false;
        }
        List<APIProductosOrdenes> SetNewProductList(int oID)
        {
            List<APIProductosOrdenes> listOfProducts = new List<APIProductosOrdenes>();
            foreach (EFOrderProduct p in VM.Order.Products)
            {
                APIProductosOrdenes aux = new APIProductosOrdenes();
                aux.IDOrden = oID;
                aux.CodigoProducto = p.codigo;
                aux.Cantidad = p.cantidad;
                aux.Precio = p.precio;
                aux.TargetPrice = p.targetPrice;
                listOfProducts.Add(aux);
            }
            return listOfProducts;
        }
        static int ObtenerFechaActualComoEntero()
        {
            // Obtener la fecha de hoy
            DateTime fechaHoy = DateTime.Today;

            // Convertir la fecha a formato yyyyMMdd
            string fechaComoCadena = fechaHoy.ToString("yyyyMMdd");
            Random random = new Random();

            // Generar el primer número aleatorio
            int numero1 = random.Next(10, 100); // Genera un número aleatorio entre 0 y Int32.MaxValue

            fechaComoCadena += numero1.ToString();

            // Convertir la cadena a entero
            int fechaComoEntero = int.Parse(fechaComoCadena);
            

            return fechaComoEntero;
        }
        private void GenerateProductListExcel()
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
            workSheet.Cells[1, "F"] = "Sub Total";
            workSheet.Cells[1, "G"] = "Target Price";
            workSheet.Cells[1, "H"] = "Link";
            workSheet.Cells[1, "I"] = "Image";
            var row = 1;
            Microsoft.Office.Interop.Excel.Range oRange;
            float left;
            float top;
            int count = 0;
            int total = VM.Order.Products.Count;
            int previos = 0;
            int progress = 0;

            foreach (var producto in VM.Order.Products)
            {
                row++;
                count++;
                workSheet.Cells[row, "A"] = producto.codigo;
                workSheet.Cells[row, "B"] = producto.nombreES;
                workSheet.Cells[row, "C"] = producto.nombreEN;
                workSheet.Cells[row, "D"] = producto.cantidad;
                workSheet.Cells[row, "E"] = producto.precio;
                workSheet.Cells[row, "G"] = producto.targetPrice;
                workSheet.Cells[row, "H"] = producto.Link;
                oRange = (Microsoft.Office.Interop.Excel.Range)workSheet.Cells[row, 9];
                left = (float)((double)oRange.Left);
                top = (float)((double)oRange.Top);
                string basePathForMega = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                basePathForMega = Path.Combine(basePathForMega, @"MEGAsync\Imagenes\");
                try
                {
                    workSheet.Shapes.AddPicture(basePathForMega + producto.Image, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, left + 5, top + 5, 120, 120);
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
                        // Dispatch back to the main thread
                        VM.ProgressVal = $"Creando Archivo Excel: {progress}% de Progreso";
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
        }
    }
}
