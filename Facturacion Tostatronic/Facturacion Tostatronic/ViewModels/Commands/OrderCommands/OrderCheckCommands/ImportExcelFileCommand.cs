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
            VM.GettingData = true;
            await Task.Run(() => setNewOrderInfo());
            Response res = await WebService.GetDataNode(URLData.OrderProductsDeleteAll, VM.ComlpleteOrder.OrdenID.ToString());
            if (!res.succes)
            {
                MessageBox.Show($"ERROR-> Error al eliminar los productos nuevos de la orden #{VM.ComlpleteOrder.OrdenID}." +
                    $"{Environment.NewLine}Razon: {res.message}{Environment.NewLine}" +
                    $"Favor de presionar el boton guardar para intentar guardar los cambios de nuevo.","Error", MessageBoxButton.OK,MessageBoxImage.Error);
                VM.GettingData = false;
                return;
            }
            else
            {
                res= await InsertaNewProductsToOrder();
                if (!res.succes)
                {
                    MessageBox.Show($"ERROR-> Error al insertar los productos nuevos de la orden #{VM.ComlpleteOrder.OrdenID}." +
                        $"{Environment.NewLine}Razon: {res.message}{Environment.NewLine}" +
                        $"Favor de presionar el boton guardar para intentar guardar los cambios de nuevo.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    VM.GettingData = false;
                    return;
                }
                else
                {
                    //Asignamos los nuevos Totales
                    //await GetSubTotalMxn();
                }
                MessageBox.Show($"Exito al actualizar la orden{Environment.NewLine}", "Exito", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            VM.GettingData = false;
        }

        #region SetNewOrderInfo
        void setNewOrderInfo()
        {
            List<ProductOrderComplete> newProducts = (List<ProductOrderComplete>)Application.Current.Properties["NewOrderInfo"];
            if (newProducts != null)
            {
                if (newProducts.Count > 0)
                {
                    VM.ComlpleteOrder.ProductosDeOrdenesNavigation.Clear();
                    decimal subTotal = 0;
                    string repitedProducts = "";
                    foreach (ProductOrderComplete item in newProducts)
                    {
                        if (VM.ComlpleteOrder.ProductosDeOrdenesNavigation.Where(x => x.CodigoProducto == item.CodigoProducto).ToArray().Length > 0)
                        {
                            repitedProducts += $"{item.CodigoProducto}{Environment.NewLine} ";
                            continue;
                        }
                        var aux =  VM.productInformationList.Where(x => x.CodigoProducto == item.CodigoProducto).FirstOrDefault();
                        if (aux!=null)
                        {
                            item.NombreEn = aux.NombreEn;
                            item.ProductInfoExist = true;
                        }
                        else
                        {
                            item.ProductInfoExist = false;
                        }
                        subTotal += item.SubTotal;
                        VM.ComlpleteOrder.ProductosDeOrdenesNavigation.Add(item);
                    }
                    if(!string.IsNullOrEmpty(repitedProducts))
                        Console.WriteLine(repitedProducts);
                    VM.ComlpleteOrder.SubTotal = (float)subTotal;
                    VM.ComlpleteOrder.TotalUSD = (decimal)(VM.ComlpleteOrder.SubTotal + (float)VM.ComlpleteOrder.CostoEnvio);
                    VM.TotalProductos = VM.ComlpleteOrder.ProductosDeOrdenesNavigation.Count;
                    VM.ComlpleteOrder.GetSubTotalMxn();
                }
            }
        }
        async Task<Response> InsertaNewProductsToOrder()
        {
            List<APIProductosOrdenes>  ProductosDeOrdenesNuevos = new List<APIProductosOrdenes>();
            List<APIProductOrderInformation> ProductInfo = new List<APIProductOrderInformation>();
            foreach (var item in VM.ComlpleteOrder.ProductosDeOrdenesNavigation)
            {
                ProductosDeOrdenesNuevos.Add(new APIProductosOrdenes(VM.ComlpleteOrder.OrdenID, item.CodigoProducto, item.Cantidad, item.Precio, item.TargetPrice));
                if (!item.ProductInfoExist)
                    ProductInfo.Add(new APIProductOrderInformation(item.CodigoProducto, item.NombreEs, item.NombreEn, item.Link));
                    
                //if (!item.ProductInfoExist)//sE DESCOMENTA SU SE VE NECESARIO QUE SE ACTUALICEN LOS COMENTARIOS
                //    ProductInfo.Add(new APIProductOrderInformation(item.CodigoProducto, item.NombreEs, item.NombreEn, item.Link));
            }
            Response res = await WebService.InsertData(ProductosDeOrdenesNuevos, URLData.InserOrderProductsList);
            if(res.succes)
            {
                if(ProductInfo.Count>0)
                {
                    res = await WebService.InsertData(ProductInfo, URLData.InserProductOrderInfoList);
                    if(res.succes)
                    {
                        foreach (var item in ProductInfo)
                        {
                            VM.productInformationList.Add(item);
                        }
                    }
                }   
            }
            return res;
        }
        #endregion
        #region CodigoAntiguoSetNewOrderInfo
        /*
        void setNewOrderInfo()
        {
            VM.GettingData = true;
            List<ProductOrderComplete> newProducts = (List<ProductOrderComplete>)Application.Current.Properties["NewOrderInfo"];
            if (newProducts != null)
            {
                if (newProducts.Count > 0)
                {
                    ObservableCollection<ProductOrderComplete> productosDeOrdenActualizada = new ObservableCollection<ProductOrderComplete>();
                    var productosExistentes = VM.ComlpleteOrder.ProductosDeOrdenesNavigation.ToList();

                    //Primero damos de alta los productos que no existen en la orden
                    var productosNuevos = newProducts.Where(a => !productosExistentes.Any(b => b.CodigoProducto == a.CodigoProducto)).ToList();
                    foreach (ProductOrderComplete item in productosNuevos)
                    {
                        if (VM.productInformationList.Where(x => x.CodigoProducto.Equals(item.CodigoProducto)).Count() > 0)
                            item.ProductInfoExist = true;
                        else
                            item.ProductInfoExist = false;

                        item.Nuevo = true;
                        item.Modificado = false;
                        item.ModificadoProducto = false;
                        productosDeOrdenActualizada.Add(item);
                    }

                    //Ahora actualizamos los productos que ya existen en la orden
                    foreach (ProductOrderComplete item in productosExistentes)
                    {
                        var aux = newProducts.Where(x => x.CodigoProducto == item.CodigoProducto).ToList();
                        if (aux != null)
                        {
                            if (aux.Count > 0)
                            {
                                if (item.Cantidad != aux[0].Cantidad || item.Precio != aux[0].Precio
                                    || item.TargetPrice != aux[0].TargetPrice)
                                {
                                    item.Cantidad = aux[0].Cantidad;
                                    item.Precio = aux[0].Precio;
                                    item.SubTotal = aux[0].SubTotal;
                                    item.TargetPrice = aux[0].TargetPrice;
                                    item.Modificado = true;
                                }
                                if(item.NombreEs != aux[0].NombreEs || item.NombreEn != aux[0].NombreEn || item.Link != aux[0].Link)
                                {
                                    item.NombreEs = aux[0].NombreEs;
                                    item.NombreEn = aux[0].NombreEn;    
                                    item.Link = aux[0].Link;
                                    item.ModificadoProducto = true;
                                }
                                    
                            }
                        }
                        productosDeOrdenActualizada.Add(item);
                    }
                    VM.ComlpleteOrder.ProductosDeOrdenesNavigation.Clear();
                    foreach (ProductOrderComplete item in productosDeOrdenActualizada)
                    {
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
                    }
                }
            }
            VM.GettingData = false;
        }*/

        #endregion
    }
}
