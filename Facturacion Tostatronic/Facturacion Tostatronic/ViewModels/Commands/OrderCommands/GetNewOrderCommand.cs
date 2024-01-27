using Facturacion_Tostatronic.Models;
using Facturacion_Tostatronic.Services;
using System;
using System.Windows.Input;
using System.Windows;
using Facturacion_Tostatronic.ViewModels.Orders;
using Newtonsoft.Json;
using System.Collections.Generic;
using Facturacion_Tostatronic.Models.EF_Models.EF_Orders;
using System.Threading.Tasks;
using Facturacion_Tostatronic.Models.Products;
using System.Collections.ObjectModel;
using System.Linq;

namespace Facturacion_Tostatronic.ViewModels.Commands.OrderCommands
{
    public class GetNewOrderCommand : ICommand
    {
        public MakeOrderVM VM { get; set; }
        public event EventHandler CanExecuteChanged;

        public GetNewOrderCommand(MakeOrderVM vm)
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
            Response res = await WebService.GetDataForInvoice(URLData.new_pi);
            if (!res.succes)
            {
                MessageBox.Show(res.message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                VM.GettingData = false;
                return;
            }
            List<ProductList> products = JsonConvert.DeserializeObject<List<ProductList>>(res.data.ToString());
            await Task.Run((() => 
            {
                foreach (ProductList product in products)
                {
                    EFOrderProduct newProduct = new EFOrderProduct(product.idProduct, product.name, product.image);
                    VM.Order.Products.Add(newProduct);
                }
            }));
            res = await WebService.GetDataForInvoice(URLData.getProductsNet);
            if (res.succes)
            {
                VM.AllProducts = JsonConvert.DeserializeObject<ObservableCollection<UpdateProductM>>(res.data.ToString());
            }
            else
                MessageBox.Show("Error al traer la información solicitada", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            List<APIProductOrderInformation> productInformationList = new List<APIProductOrderInformation>();
            res = await WebService.GetDataForInvoice(URLData.ProductOrderInfo);
            if (res.succes)
            {
                productInformationList = JsonConvert.DeserializeObject<List<APIProductOrderInformation>>(res.data.ToString());
            }
            else
                MessageBox.Show("Error al traer la lista información de los productos.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            if (productInformationList.Count > 0)
            {
                await Task.Run((() =>
                {
                    foreach (EFOrderProduct product in VM.Order.Products)
                    {
                        var pt = productInformationList.Where(pr => pr.CodigoProducto == product.codigo).ToList();
                        if (pt.Count > 0) 
                        {
                            var p = pt[0];
                            if (p != null)
                            {
                                if (!string.IsNullOrEmpty(p.CodigoProducto))
                                {
                                    product.nombreES = p.NombreEs;
                                    product.nombreEN = p.NombreEn;
                                    product.Link = p.Link;
                                    product.ProductInfoExist = true;
                                    product.Modificado = false;
                                }
                            }
                        }
                        
                    }
                }));
            }
            VM.GettingData = false;
            VM.IsProductExtractionenabled = false;
        }
    }
}
