using Facturacion_Tostatronic.Models.EF_Models.EF_Orders;
using Facturacion_Tostatronic.Models;
using Facturacion_Tostatronic.Services;
using Facturacion_Tostatronic.ViewModels.Orders;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using Facturacion_Tostatronic.Views.OrdersV;
using System.ComponentModel;

namespace Facturacion_Tostatronic.ViewModels.Commands.OrderCommands.OrderCheckCommands
{
    public class ImportExcelFileCommand : ICommand
    {
        public OrderCheckVM VM { get; set; }
        public event EventHandler CanExecuteChanged;

        public ImportExcelFileCommand(OrderCheckVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            ImportFromExcelV iE = new ImportFromExcelV();
            iE.Closing += SetOrder;
            iE.ShowDialog();
            

        }

        private async void SetOrder(object sender, CancelEventArgs e)
        {
            await Task.Run(() => setNewOrderInfo());
            Response res = await WebService.GetDataNode(URLData.OrderProductsDeleteAll, VM.ComlpleteOrder.OrdenID.ToString());
            if (!res.succes)
            {
                MessageBox.Show($"ERROR-> Error al eliminar los productos nuevos de la orden #{VM.ComlpleteOrder.OrdenID}." +
                    $"{Environment.NewLine}Razon: {res.message}{Environment.NewLine}","Error", MessageBoxButton.OK,MessageBoxImage.Error);
            }
            else
            {
                MessageBox.Show($"Exito al agregar los productos nuevos{Environment.NewLine}" +
                    $"Favor de presionar el boton de GUARDAR, para guardar los datos nuevos.");
            }
        }

        void setNewOrderInfo()
        {
            VM.GettingData = true;
            List<ProductOrderComplete> newProducts = (List<ProductOrderComplete>)Application.Current.Properties["NewOrderInfo"];
            if (newProducts != null)
            {
                if (newProducts.Count > 0)
                {
                    VM.ComlpleteOrder.ProductosDeOrdenesNavigation.Clear();
                    foreach (ProductOrderComplete item in newProducts)
                    {
                        if (VM.productInformationList.Where(x => x.CodigoProducto.Equals(item.CodigoProducto)).Count() > 0)
                            item.ProductInfoExist = true;
                        else
                            item.ProductInfoExist = false;
                        item.Nuevo = true;
                        item.Modificado = false;
                        item.ModificadoProducto = false;
                        VM.ComlpleteOrder.ProductosDeOrdenesNavigation.Add(item);
                    }
                    //Si esto funciona, lo que se tiene que hacer es mandar a eliminar los productos de la orden y dar 
                    //de alta los nuevos productos.

                    //Primero eliminamos Todos los productos de la BD
                    //List<ProductOrderComplete> productosAgregados = newProducts.Except(VM.ComlpleteOrder.ProductosDeOrdenesNavigation.ToList()).ToList();
                    /*
                    if (productosAgregados != null)
                    {
                        if (productosAgregados.Count > 0)
                        {
                            foreach (ProductOrderComplete item in productosAgregados)
                            {
                                if (VM.productInformationList.Where(x => x.CodigoProducto.Equals(item.CodigoProducto)).Count() > 0)
                                    item.ProductInfoExist = true;
                                else
                                    item.ProductInfoExist = false;
                                item.Nuevo = true;
                                item.Modificado = false;
                                item.ModificadoProducto = false;
                                VM.ComlpleteOrder.ProductosDeOrdenesNavigation.Add(item);
                            }
                        }
                    }
                    foreach (ProductOrderComplete item in VM.ComlpleteOrder.ProductosDeOrdenesNavigation.ToList())
                    {
                        var aux = newProducts.Where(x => x.CodigoProducto == item.CodigoProducto).ToList();
                        if (aux != null)
                        {
                            if (aux.Count > 0)
                            {
                                item.Cantidad = aux[0].Cantidad;
                                item.Precio = aux[0].Precio;
                                item.SubTotal = aux[0].SubTotal;
                                item.TargetPrice = aux[0].TargetPrice;
                            }
                        }
                    }*/
                }
            }
            VM.GettingData = false;
        }
    }
}
