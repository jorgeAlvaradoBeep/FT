using Facturacion_Tostatronic.Models;
using Facturacion_Tostatronic.Models.EF_Models.EF_Orders;
using Facturacion_Tostatronic.Services;
using Facturacion_Tostatronic.ViewModels.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Facturacion_Tostatronic.ViewModels.Commands.OrderCommands.OrderCheckCommands
{
    public class SaveModifiedOrderCommand : ICommand
    {
        public OrderCheckVM VM { get; set; }
        public event EventHandler CanExecuteChanged;

        public SaveModifiedOrderCommand(OrderCheckVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            string message = string.Empty;
            List<ProductOrderComplete> productosNuevos = new List<ProductOrderComplete>();
            List<ProductOrderComplete> modificacinOrden = new List<ProductOrderComplete>();
            List<ProductOrderComplete> modificacionDatos = new List<ProductOrderComplete>();
            List<ProductOrderComplete> infoProductoNoExist = new List<ProductOrderComplete>();

            productosNuevos = VM.ComlpleteOrder.ProductosDeOrdenesNavigation.Where(x=> x.Nuevo==true).ToList();
            modificacinOrden = VM.ComlpleteOrder.ProductosDeOrdenesNavigation.Where(x => x.Nuevo == false && x.Modificado == true).ToList();
            modificacionDatos = VM.ComlpleteOrder.ProductosDeOrdenesNavigation.Where(x => x.Nuevo == false && x.ModificadoProducto == true).ToList();
            infoProductoNoExist = modificacionDatos.Where(x=> x.ProductInfoExist==false).ToList();
            modificacionDatos = modificacionDatos.Where(x=> x.ProductInfoExist==true).ToList();

            List<APIProductosOrdenes> ProductosDeOrdenesNuevos;
            List<APIProductOrderInformation> ProductInfo;
            VM.GettingData = true;
            //Seccion de productos nuevos
            if(productosNuevos.Count>0)
            {
                ProductosDeOrdenesNuevos = new List<APIProductosOrdenes>();
                ProductInfo =  new List<APIProductOrderInformation>();
                foreach (var item in productosNuevos)
                {
                    ProductosDeOrdenesNuevos.Add(new APIProductosOrdenes(VM.ComlpleteOrder.OrdenID, item.CodigoProducto, item.Cantidad, item.Precio, item.TargetPrice));
                    if (!item.ProductInfoExist)
                        ProductInfo.Add(new APIProductOrderInformation(item.CodigoProducto, item.NombreEs, item.NombreEn, item.Link));
                }
                Response res = await WebService.InsertData(ProductosDeOrdenesNuevos, URLData.InserOrderProductsList);
                if (!res.succes)
                {
                    message += $"ERROR-> Error al agregar los productos nuevos a la orden." +
                        $"{Environment.NewLine}Razon: {res.message}{Environment.NewLine}";
                }
                else
                {
                    message += $"Exito al agregar los productos nuevos{Environment.NewLine}";
                    foreach (var item in VM.ComlpleteOrder.ProductosDeOrdenesNavigation)
                    {
                        if(item.Nuevo)
                            item.Nuevo = false;
                    }
                }
                    

                res = await WebService.InsertData(ProductInfo, URLData.InserProductOrderInfoList);
                if (!res.succes)
                {
                    message += $"ERROR-> Error al agregar la informacion de los productos nuevos a la orden." +
                        $"{Environment.NewLine}Razon: {res.message}{Environment.NewLine}";
                }
                else
                    message += $"Exito al agregar la informacion de los productos nuevos{Environment.NewLine}";
            }
            //Seccion de modificacion de informacion de la orden
            if (modificacinOrden.Count > 0)
            {
                ProductosDeOrdenesNuevos = new List<APIProductosOrdenes>();
                foreach (var item in modificacinOrden)
                {
                    ProductosDeOrdenesNuevos.Add(new APIProductosOrdenes(VM.ComlpleteOrder.OrdenID, item.CodigoProducto, item.Cantidad, item.Precio, item.TargetPrice));
                }
                Response res = await WebService.ModifyData(ProductosDeOrdenesNuevos, URLData.OrderProductsEditList);
                if (!res.succes)
                {
                    message += $"ERROR-> Error al agregar los productos nuevos a la orden." +
                        $"{Environment.NewLine}Razon: {res.message}{Environment.NewLine}";
                }
                else
                {
                    message += $"Exito al modificar los datos de la orden{Environment.NewLine}";
                    foreach (var item in VM.ComlpleteOrder.ProductosDeOrdenesNavigation)
                    {
                        if (!item.Nuevo && item.Modificado)
                            item.Modificado = false;
                    }
                }
                    
            }
            //Seccion para editar informaci[on de productos nuevos.
            if (modificacionDatos.Count > 0)
            {
                ProductInfo = new List<APIProductOrderInformation>();
                foreach (var item in modificacionDatos)
                {
                    ProductInfo.Add(new APIProductOrderInformation(item.CodigoProducto, item.NombreEs, item.NombreEn, item.Link));
                }
                Response res = await WebService.ModifyData(ProductInfo, URLData.ProductOrderInfoEdit);
                if (!res.succes)
                {
                    message += $"ERROR-> Error al editar la informacion de los productos." +
                        $"{Environment.NewLine}Razon: {res.message}{Environment.NewLine}";
                }
                else
                {
                    foreach (var item in VM.ComlpleteOrder.ProductosDeOrdenesNavigation)
                    {
                        if (!item.Nuevo && item.ModificadoProducto)
                            item.ModificadoProducto = false;
                    }
                    message += $"Exito al editar los productos de la orden.{Environment.NewLine}";
                }
                    
            }

            //Seccion para insertar la informacion de productos nuevos
            if (infoProductoNoExist.Count > 0)
            {
                ProductInfo = new List<APIProductOrderInformation>();
                foreach (var item in infoProductoNoExist)
                {
                    ProductInfo.Add(new APIProductOrderInformation(item.CodigoProducto, item.NombreEs, item.NombreEn, item.Link));
                }
                Response res = await WebService.InsertData(ProductInfo, URLData.InserProductOrderInfoList);
                if (!res.succes)
                {
                    message += $"ERROR-> Error al Agregar la informacion de los productos." +
                        $"{Environment.NewLine}Razon: {res.message}{Environment.NewLine}";
                }
                else
                {
                    message += $"Exito al agregar la informacion de los productos{Environment.NewLine}";
                    foreach (var item in VM.ComlpleteOrder.ProductosDeOrdenesNavigation)
                    {
                        if (!item.ProductInfoExist)
                            item.ProductInfoExist = true;
                    }
                }
                    
            }
            //Seccion Para eliminar Productos de las ordenes
            if (VM.ProductosEliminados.Count > 0)
            {
                ProductosDeOrdenesNuevos = new List<APIProductosOrdenes>();
                foreach (var item in VM.ProductosEliminados)
                {
                    ProductosDeOrdenesNuevos.Add(new APIProductosOrdenes(VM.ComlpleteOrder.OrdenID,item.CodigoProducto,item.Cantidad, item.Precio, item.TargetPrice));
                }
                Response res = await WebService.InsertData(ProductosDeOrdenesNuevos, URLData.OrderProductsDelete);
                if (!res.succes)
                {
                    message += $"ERROR-> Error al Eliminar Productos de la orden." +
                        $"{Environment.NewLine}Razon: {res.message}{Environment.NewLine}";
                }
                else
                {
                    VM.ProductosEliminados = new List<ProductOrderComplete>();
                    message += $"Exito al eliminar los productos{Environment.NewLine}";
                }
                    
            }
            MessageBox.Show(message);
            VM.GettingData = false;
        }
    }
}
