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
                    foreach (EarningSale s in VM.Sales)
                    {
                        if (s.iva == 0)
                            s.iva = s.total - (s.total / 1.16f);
                        float ct = 0;
                        foreach(EFSaleProducts sp in s.ProductosDeVenta)
                        {
                            if(sp.productoNavigation.nombre.Contains("Envio"))
                            {
                                s.Envio = (float)sp.productoNavigation.precioCompra;
                            }else
                                ct += (float)sp.productoNavigation.precioCompra*sp.cantidadComprada;
                        }
                        s.ivaPagada = ct-(ct/1.16f);
                        s.ivaAPagar = s.iva - s.ivaPagada;
                        s.IVARetenido = 0;
                        s.ISRRetenido = 0;
                        s.Comision = 0;
                        s.Costototal = ct;
                        s.getTotal();
                        total += s.Ganancia;   
                    }
                    VM.TotalEarnings = total;
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
