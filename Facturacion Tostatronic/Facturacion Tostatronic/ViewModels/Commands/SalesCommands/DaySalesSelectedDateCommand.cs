using Facturacion_Tostatronic.Models.EF_Models.EFSale;
using Facturacion_Tostatronic.Models;
using Facturacion_Tostatronic.Services;
using Facturacion_Tostatronic.ViewModels.Sales;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using Facturacion_Tostatronic.Models.EF_Models.EFProduct;
using Facturacion_Tostatronic.Models.EF_Models.EFEarnings;

namespace Facturacion_Tostatronic.ViewModels.Commands.SalesCommands
{
    public class DaySalesSelectedDateCommand : ICommand
    {
        public EarningsVM VM { get; set; }
        public event EventHandler CanExecuteChanged;
        public DaySalesSelectedDateCommand(EarningsVM vm)
        {
            VM = vm;
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            VM.GettinData = true;
            Response r = await WebService.GetDataNode(URLData.getDayOrdersWT, VM.SelectedDate.Date.ToString("yyyy-MM-dd"));
            if (!r.succes)
            {
                if (!string.IsNullOrEmpty(r.message))
                    MessageBox.Show(r.message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                else
                    MessageBox.Show("Error al traer datos de ventas", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                VM.GettinData = false;
                VM.Sales = new List<EarningSale>();
                return;
            }
            else
            {
                try
                {
                    VM.Sales = JsonConvert.DeserializeObject<List<EarningSale>>(r.data.ToString());
                    float total = 0;
                    float totalSales = 0;
                    //Ahora extraemos las comisiones si existen de las ventas del dia seleccionado
                    var ids = VM.Sales.Select(sale => sale.idVenta).ToList();
                    if(ids!=null || ids.Count!=0)
                    {
                        var idsQueryString = string.Join("&", ids.Select(id => $"ids={id}"));
                        r = await WebService.GetDataNode(URLData.ComisionesDeVentas, idsQueryString);
                        if (!r.succes)
                        {
                            if (!string.IsNullOrEmpty(r.message))
                                MessageBox.Show(r.message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            else
                                MessageBox.Show($"Error al traer las comisiones" +
                                    $"{Environment.NewLine}Motivo: {r.message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                        }
                        else
                        {
                            if (r.data != null)
                            {
                                var comisiones = JsonConvert.DeserializeObject<List<EFComisiones>>(r.data.ToString());
                                foreach (EarningSale s in VM.Sales)
                                {
                                    var comision = comisiones.FirstOrDefault(c => c.VentaId == s.idVenta);
                                    if (comision != null)
                                    {
                                        s.Comisiones = comision;
                                    }
                                    else
                                    {
                                        s.Comisiones = new EFComisiones();
                                    }
                                }
                            }
                        }
                    }
                    
                    foreach (EarningSale s in VM.Sales)
                    {
                        if (s.iva == 0)
                            s.iva = s.total - (s.total / 1.16f);
                        float ct = 0;
                        s.Comisiones.VentaId = s.idVenta;   
                        foreach(EFSaleProducts sp in s.ProductosDeVenta)
                        {
                            if(sp.productoNavigation!=null)
                            {
                                if (!sp.productoNavigation.nombre.Contains("Envio"))
                                    ct += (float)sp.productoNavigation.precioCompra * sp.cantidadComprada;
                            }
                            else
                            {
                                r = await WebService.GetDataNode(URLData.getProductsNet, sp.idProducto);
                                if (!r.succes)
                                {
                                    if (!string.IsNullOrEmpty(r.message))
                                        MessageBox.Show(r.message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                    else
                                        MessageBox.Show($"Error al traer datos del producto {sp.idProducto}" +
                                            $"{Environment.NewLine}El calculo de costos puede no ser exacto,", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                
                                }
                                else
                                {
                                    sp.productoNavigation = JsonConvert.DeserializeObject<EFProduct>(r.data.ToString());
                                    if (!sp.productoNavigation.nombre.Contains("Envio"))
                                        ct += (float)sp.productoNavigation.precioCompra * sp.cantidadComprada;
                                }
                            }
                        }

                        //Seccion para calcular comisiones si no existieron
                        s.ivaPagada = ct-(ct/1.16f);
                        s.ivaAPagar = s.iva - s.ivaPagada;
                        s.IVARetenido = 0;
                        s.ISRRetenido = 0;
                        s.Costototal = ct;
                        s.getTotal();
                        total += s.Comisiones.Ganancia ?? 0.0f;
                        totalSales += s.total;
                    }
                    VM.TotalEarnings = total;
                    VM.TotalVentas = totalSales;
                    VM.NumberOfSales = VM.Sales.Count;
                }
                catch(Exception ex) 
                {
                    MessageBox.Show("Error al convertir datos de venta:" +
                        $"{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                
            }
            VM.GettinData = false;
        }
    }
}
