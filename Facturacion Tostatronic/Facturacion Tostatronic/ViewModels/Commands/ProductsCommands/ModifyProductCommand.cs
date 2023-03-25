using Facturacion_Tostatronic.Models.Clients;
using Facturacion_Tostatronic.Models;
using Facturacion_Tostatronic.Services;
using Facturacion_Tostatronic.ViewModels.Clients;
using Facturacion_Tostatronic.ViewModels.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Telerik.Windows.Controls;
using Facturacion_Tostatronic.Models.Products;
using Facturacion_Tostatronic.Models.WooCommerceModels;
using GalaSoft.MvvmLight.Threading;
using Microsoft.Office.Interop.Excel;
using Newtonsoft.Json;
using WooCommerceNET.WooCommerce.v3;
using Chilkat;

namespace Facturacion_Tostatronic.ViewModels.Commands.ProductsCommands
{
    public class ModifyProductCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public SeeProductVM VM { get; set; }
        public ModifyProductCommand(SeeProductVM vM)
        {
            VM = vM;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            UpdateProductM cC = (UpdateProductM)((RadTabItem)parameter).DataContext;
            if (cC != null)
            {
                if (string.IsNullOrEmpty(cC.Nombre))
                {
                    MessageBox.Show("El nombre del producto no puede ir vacio", "Error De dato", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                VM.GettingData = true;
                Response res = await WebService.ModifyData(cC, URLData.editProductsNet + cC.Codigo);
                if (!res.succes)
                {
                    MessageBox.Show("Error: " + res.message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    VM.GettingData = false;
                    return;
                }
                else
                {
                    //Vamos a actualizar los datos en la pagina
                    //Primero Validamos si el producto es variacion o simple
                    res = await WebService.GetDataNode(URLData.getProductTypeNET, cC.Codigo);
                    if (!res.succes)
                    {
                        MessageBox.Show("Error: " + res.message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        VM.GettingData = false;
                        return;
                    }
                    List<Update> produtToUpdate = new List<Update>();
                    int ID;
                    if (res.data == null)
                        ID = 0;
                    else
                        int.TryParse(res.data.ToString(), out ID);
                    produtToUpdate.Add(new Update()
                    {
                        id = ID,
                        stock_quantity = (int)cC.Existencia,
                        regular_price = cC.PrecioPublico.ToString(),
                    });
                    if (produtToUpdate[0].id== 0) 
                    {
                        MessageBox.Show($"Error: No se pudo obtener el Codigo WooCommerc.{Environment.NewLine}" +
                            $"Producto Actualizado SOLO Localmente", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        VM.GettingData = false;
                        return;
                    }
                    CRUDActionClass crud = new CRUDActionClass() { create = new List<Create>(), delete = new List<Delete>(), update = produtToUpdate };
                    string json = JsonConvert.SerializeObject(crud, Newtonsoft.Json.Formatting.None,
                                    new JsonSerializerSettings
                                    {
                                        NullValueHandling = NullValueHandling.Ignore
                                    });
                    if (res.message.Equals("Single"))
                    {
                        var res2 = await WebService.InsertDataWooCommercer(URLData.wcBatchProduct, json);
                        if (!res2.IsSuccessful)
                        {
                            MessageBox.Show($"Error: {res2.ErrorMessage}.{Environment.NewLine}" +
                            $"Producto Actualizado SOLO Localmente", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            VM.GettingData = false;
                            return;
                        }
                        else
                            MessageBox.Show($"producto Modificado Con Exito", "Correcto", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        int parentID;
                        if (int.TryParse(res.message, out parentID))
                        {
                            var res2 = await WebService.InsertDataWooCommercer($"{URLData.wcProducts}/{parentID}/variations/batch", json);
                            if (!res2.IsSuccessful)
                            {
                                MessageBox.Show($"Error: {res2.ErrorMessage}.{Environment.NewLine}" +
                                $"Producto Actualizado SOLO Localmente", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                VM.GettingData = false;
                                return;
                            }
                            else
                            {
                                VM.Products = VM.Products;
                                MessageBox.Show("Producto Actualizado correctamente", "Exito", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                        }
                        else
                        {
                            MessageBox.Show($"Error: {ID} No tiene registro.{Environment.NewLine}" +
                                $"Producto Actualizado SOLO Localmente", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        
                    }

                    VM.GettingData = false;
                    
                }
            }
        }
    }
}
