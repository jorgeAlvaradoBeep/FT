﻿using Bukimedia.PrestaSharp.Factories;
using Bukimedia.PrestaSharp.Entities;
using Facturacion_Tostatronic.Models;
using Facturacion_Tostatronic.ViewModels.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Facturacion_Tostatronic.Views;
using System.IO;
using Facturacion_Tostatronic.Services;
using Facturacion_Tostatronic.Models.Products;
using Bukimedia.PrestaSharp;
using Facturacion_Tostatronic.Models.EF_Models.EFProduct;

namespace Facturacion_Tostatronic.ViewModels.Commands.ProductsCommands
{
    public class AddNewProductCommand : ICommand
    {
        public AddProductVM VM { get; set; }
        public event EventHandler CanExecuteChanged;

        public AddNewProductCommand(AddProductVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            string errorMessage = string.Empty;
            if (VM.EnableProductBase)
            {
                if (string.IsNullOrEmpty(VM.Product.Code))
                    errorMessage += "El codigo del producto no puede ir vacio" + Environment.NewLine;
                if (string.IsNullOrEmpty(VM.Product.Name))
                    errorMessage += "El Nombre del producto no puede ir vacio" + Environment.NewLine;
                if (VM.Product.Existence <= 0)
                    errorMessage += "La cantidad de existencia no puede ser menor o igual a 0" + Environment.NewLine;
                if (VM.Product.MinimumQuantity <= 0)
                    errorMessage += "La cantidad minima no puede ser menor o igual a 0" + Environment.NewLine;
                if (VM.Product.BuyPrice <= 0)
                    errorMessage += "El costo de compra de un producto no puede ser menor o igual a 0" + Environment.NewLine;
                if (VM.Product.MinimumPrice <= VM.Product.BuyPrice)
                    errorMessage += "El costo de precio minimo de un producto no puede ser menor o igual al precio de compra" + Environment.NewLine;
                if (VM.Product.DistributorPrice <= VM.Product.MinimumPrice)
                    errorMessage += "El costo de precio distribuidor de un producto no puede ser menor o igual al precio minimo" + Environment.NewLine;
                if (VM.Product.PublicPrice <= VM.Product.DistributorPrice)
                    errorMessage += "El costo de precio publico de un producto no puede ser menor o igual al precio de distribuidor" + Environment.NewLine;
                if (string.IsNullOrEmpty(VM.Product.UPC))
                    errorMessage +=  "El producto debe tener un codigo universal"+ Environment.NewLine;
                if(VM.SelectedCode== null)
                {
                    errorMessage += "Debe Seleccionar un codigo de producto" + Environment.NewLine;
                }
                if (string.IsNullOrEmpty(VM.ImagePath))
                    VM.Product.Image = "No_image.png";
                else
                {
                    if (Path.GetExtension(VM.ImagePath).Equals(".png"))
                        VM.Product.Image = VM.Product.Code + ".png";
                    else
                    {
                        VM.Product.Image = "No_image.png";
                        errorMessage += "El archivo seleccionado no es una imagen PNG valida";
                    }

                }
                

                //En esta seccion va la validacion del codigo sat

                if (!string.IsNullOrEmpty(errorMessage))
                {
                    MessageBox.Show(errorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    VM.GettingData = false;
                }
                else
                {
                    VM.GettingData=true;
                    //Tenemos que validar si existe un codigo ya en el sistema
                    Response rmp = await WebService.GetDataNode(URLData.productExistNET, VM.Product.Code);
                    if(!rmp.succes)
                    {
                        MessageBox.Show(rmp.message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        VM.GettingData = false;
                        return;
                    }
                    else
                    {
                        if((bool)rmp.data)
                        {
                            MessageBox.Show($"El codigo que intenta introducir, ya existe.{Environment.NewLine}Favor de verificarlo", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            VM.GettingData = false;
                            return;
                        }
                    }
                    //Si no existe, procedemos con la adicion del producto.
                    Response response = await WebService.InsertData(VM.Product, URLData.product_add_new);
                    string errMsg = string.Empty;
                    if (response.succes)
                    {
                        //Aqui copiamos la imagen a la carpeta de megasync
                        if (!string.IsNullOrEmpty(VM.ImagePath))
                        {
                            try
                            {
                                string basePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                                string baseImagePath = Path.Combine(basePath, @"MEGAsync\Imagenes");
                                File.Copy(VM.ImagePath, Path.Combine(baseImagePath, VM.Product.Image), true);
                            }
                            catch (Exception e)
                            {
                                errMsg += "Error al copial la imagen" + Environment.NewLine;
                            }
                        }

                        //Ahora damos de alta los codigos de los productos
                        EFUniversalCodes pc = new EFUniversalCodes()
                        {
                            Referencia = VM.Product.Code,
                            Upc = VM.Product.UPC
                        };
                        Response res = await WebService.InsertData(pc, URLData.addProductCodesNET);
                        if (!res.succes)
                        {
                            errMsg += "Error al insertar UPC: " + res.message + Environment.NewLine;
                        }
                        //Ahora insertamos el codigo del SAT
                        EFCodigoSAT codigoSAT = new EFCodigoSAT()
                        {
                            Codigo = VM.Product.Code,
                            CodigoPro = VM.SelectedCode.CClaveProdServ
                        };
                        res = await WebService.InsertData(codigoSAT, URLData.addProductSatCodeNET);
                        if (!res.succes)
                        {
                            errMsg += "Error al modificar Codigo SAT: " + res.message + Environment.NewLine;
                        }
                        //Ahora validamos si se quiere guaradr en la pagina web.


                        //Aqui vemos los errores generados
                        if(!string.IsNullOrEmpty(errMsg)) { MessageBox.Show($"Producto Insertado Con Errores:{Environment.NewLine}{errMsg}",
                            "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
                        else
                            MessageBox.Show("Producto Insertado Correctamente",
                            "Exito", MessageBoxButton.OK, MessageBoxImage.Information);
                        VM.initialize();
                    }
                    VM.GettingData= false;
                }

            
            }
        }
        async Task<bool> InsertProductPrestashop()
        {
            ProductFactory ArticuloFactory = new ProductFactory(URLData.psBaseUrl, URLData.psAccount, URLData.psPassword);
            Bukimedia.PrestaSharp.Entities.AuxEntities.language Lenguaje = new Bukimedia.PrestaSharp.Entities.AuxEntities.language();

            Lenguaje.id = 1;
            var MiArticulo = new product();

            MiArticulo.reference = VM.Product.Code;
            MiArticulo.active = 1;
            MiArticulo.minimal_quantity = 1;
            MiArticulo.show_price = 1;
            MiArticulo.available_for_order = 1;
            MiArticulo.is_virtual = 0;
            MiArticulo.visibility = "both";
            MiArticulo.advanced_stock_management = 0;
            MiArticulo.state = 1;
            MiArticulo.id_tax_rules_group = 0;
            MiArticulo.id_category_default = 30;
            MiArticulo.pack_stock_type = 3;
            MiArticulo.redirect_type = "301-category";
            MiArticulo.id_shop_default = 1;
            MiArticulo.additional_delivery_times = 1;
            MiArticulo.price = Convert.ToDecimal(VM.Product.PublicPrice);
            MiArticulo.wholesale_price = Convert.ToDecimal(VM.Product.BuyPrice);
            MiArticulo.name.Add(new Bukimedia.PrestaSharp.Entities.AuxEntities.language((long)1, VM.Product.Name));
            Lenguaje.Value = VM.Product.Name;
            MiArticulo.AddLinkRewrite(Lenguaje);

            MiArticulo.associations.categories.Add(new Bukimedia.PrestaSharp.Entities.AuxEntities.category((long)1));

            try 
            { 
                MiArticulo = await ArticuloFactory.AddAsync(MiArticulo);
                VM.Product.PrestashopID = MiArticulo.id.ToString();
                try
                {
                    StockAvailableFactory StockAvailableFactory = new StockAvailableFactory(URLData.psBaseUrl, URLData.psAccount, URLData.psPassword);
                    long stockAvailableId = MiArticulo.associations.stock_availables[0].id;
                    stock_available MyStockAvailable = StockAvailableFactory.Get(stockAvailableId);
                    MyStockAvailable.quantity = (int)VM.Product.Existence; // Number of available products
                    MyStockAvailable.out_of_stock = 2; // Must enable orders
                    await StockAvailableFactory.UpdateAsync(MyStockAvailable);
                    return true;
                }
                catch(Exception d)
                {
                    MessageBox.Show("El producto se agrego con exito, la existencia no se pudo actualizar", "Agregado", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                return true;
            }
            catch (System.NullReferenceException e) 
            {
                MessageBoxResult result = MessageBox.Show("El producto no se pudo agregar a Prestashop, desea reintentar?", "Error", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.No)
                {
                    result = MessageBox.Show("Si no reintenta, el producto solo se guardara" + Environment.NewLine + "en el sistema pero no en la web, debera de agregarlo" + Environment.NewLine + "manuealmente despues." + Environment.NewLine + "Esta Seguro?", "Advertensia", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.Yes)
                        return false;
                    else
                        return await InsertProductPrestashop();
                }
                else
                    return await InsertProductPrestashop();
            }
        }
        List<SpecificPrice> GetSpecificPrices()
        {
            float minPricePage = VM.Product.MinimumPrice * 1.06f;
            float div = (VM.Product.PublicPrice - minPricePage) / 5f;
            float qtyDiv = VM.Product.Existence / 5f;
            List<SpecificPrice> specPrices = new List<SpecificPrice>();
            specPrices.Add(new SpecificPrice(qtyDiv, VM.Product.PublicPrice - div));
            specPrices.Add(new SpecificPrice(qtyDiv*2, VM.Product.PublicPrice - div*2));
            specPrices.Add(new SpecificPrice(qtyDiv*3, VM.Product.PublicPrice - div*3));
            specPrices.Add(new SpecificPrice(qtyDiv*4, VM.Product.PublicPrice - div*4));
            specPrices.Add(new SpecificPrice(qtyDiv*5, VM.Product.PublicPrice - div*5));
            return specPrices;
        }

        public async Task<string> SetSpecificPrices()
        {
            specific_price specialPriceRule = null;
            List<specific_price> specificPrices = new List<specific_price>();
            SpecificPriceFactory specificPriceFactory = new SpecificPriceFactory(URLData.psBaseUrl, URLData.psAccount, URLData.psPassword);
            foreach (SpecificPrice sp in VM.Product.SpecificPrices)
            {
                specialPriceRule = new specific_price
                {
                    id_product = long.Parse(VM.Product.PrestashopID),
                    reduction = 0,
                    reduction_tax = 1,
                    reduction_type = "amount",
                    id_shop = 1,
                    id_shop_group = 0,
                    id_product_attribute = 0,
                    id_cart = 0,
                    id_currency = 0,
                    id_country = 0,
                    id_group = 0,
                    id_customer = 0,
                    from_quantity = (int)sp.Quantity,
                    price = Convert.ToDecimal(sp.Price),
                };
                specificPrices.Add(specialPriceRule);
            }

            try
            {
                await specificPriceFactory.AddListAsync(specificPrices);
                return "Precios Especificos Dados de alta";
            }
            catch(PrestaSharpException e)
            {
                return "Error al dar de alta los precios especificos: ." + e.ResponseErrorMessage;
                //MessageBoxResult result = MessageBox.Show("No se pudieron agregar los precios especificos, desea reintentar?", "Error", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                //if (result == MessageBoxResult.No)
                //{
                //    result = MessageBox.Show("Si no reintenta, no se guardaran los precios especificos" + Environment.NewLine +  "Esta Seguro?", "Advertencia", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                //    if (result == MessageBoxResult.Yes)
                //        return ;
                //    else
                //        SetSpecificPrices();
                //}
                //else
                //    SetSpecificPrices();
            }
        }
    }
}