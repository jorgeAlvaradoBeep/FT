using Facturacion_Tostatronic.Models.Products;
using Facturacion_Tostatronic.ViewModels.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Facturacion_Tostatronic.ViewModels.Commands.PDVCommands.SaleCommands
{
    public class SetSelectedPriceCommand : ICommand
    {
        public SaleVM VM { get; set; }
        public event EventHandler CanExecuteChanged;
        public SetSelectedPriceCommand(SaleVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            //if (VM.CompleteSale.PriceType == (int)parameter)
            //    return;
            VM.CompleteSale.PriceType = (int)parameter;
            if (VM.CompleteSale.SearchedProducts.Count > 0)
            {
                switch (VM.CompleteSale.PriceType)
                {
                    case 0:
                        foreach (ProductSaleSearch product in VM.CompleteSale.SearchedProducts)
                        {
                            product.DisplayPrice = product.MinimumPrice;
                        }
                        break;
                    case 1:
                        foreach (ProductSaleSearch product in VM.CompleteSale.SearchedProducts)
                        {
                            product.DisplayPrice = product.DistributorPrice;
                        }
                        break;
                    case 2:
                        foreach (ProductSaleSearch product in VM.CompleteSale.SearchedProducts)
                        {
                            product.DisplayPrice = product.PublicPrice;
                        }
                        break;
                }
            }
            if (VM.CompleteSale.SaledProducts.Count > 0)
            {
                switch (VM.CompleteSale.PriceType)
                {
                    case 0:
                        foreach (ProductSaleSaled product in VM.CompleteSale.SaledProducts)
                        {
                            product.DisplayPrice = product.MinimumPrice;
                        }
                        break;
                    case 1:
                        foreach (ProductSaleSaled product in VM.CompleteSale.SaledProducts)
                        {
                            product.DisplayPrice = product.DistributorPrice;
                        }
                        break;
                    case 2:
                        foreach (ProductSaleSaled product in VM.CompleteSale.SaledProducts)
                        {
                            product.DisplayPrice = product.PublicPrice;
                        }
                        break;
                }
            }
        }
    }
}
