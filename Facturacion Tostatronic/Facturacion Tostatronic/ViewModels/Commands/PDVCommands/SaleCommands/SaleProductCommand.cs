using Facturacion_Tostatronic.Models.Products;
using Facturacion_Tostatronic.ViewModels.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Facturacion_Tostatronic.ViewModels.Commands.PDVCommands.SaleCommands
{
    public class SaleProductCommand : ICommand
    {
        public SaleVM VM { get; set; }
        public event EventHandler CanExecuteChanged;
        public SaleProductCommand(SaleVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            ProductSaleSearch p = (ProductSaleSearch)parameter;
            if (p == null)
                return;
            ProductSaleSaled product = new ProductSaleSaled(p.Code,p.Name, p.Existence, p.MinimumPrice, p.DistributorPrice, p.PublicPrice,
                                                            p.DisplayPrice, 1);
            List< ProductSaleSaled> repited = VM.CompleteSale.SaledProducts.Where(x => x.Code.Equals(product.Code)).ToList();
            if(repited.Count>0)
            {
                var item = VM.CompleteSale.SaledProducts.FirstOrDefault(i => i.Code == product.Code);
                if (item != null)
                {
                    item.SaledQuantity++;
                }
            }
            else
                VM.CompleteSale.SaledProducts.Add(product);
            VM.CompleteSale.SubTotal += product.DisplayPrice;

        }
    }
}
