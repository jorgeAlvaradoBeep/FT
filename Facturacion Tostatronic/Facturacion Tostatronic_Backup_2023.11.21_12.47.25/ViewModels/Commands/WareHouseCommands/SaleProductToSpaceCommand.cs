using Facturacion_Tostatronic.Models.WareHouse;
using Facturacion_Tostatronic.ViewModels.WareHouse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Facturacion_Tostatronic.ViewModels.Commands.WareHouseCommands
{
    public class SaleProductToSpaceCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public AddProductsToSpaceVM VM { get; set; }
        public SaleProductToSpaceCommand(AddProductsToSpaceVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            ProductInSpace p = (ProductInSpace)parameter;
            if (p == null)
                return;
            List<ProductInSpace> repited = VM.SelectedSpace.Products.Where(x => x.Product.Code.Equals(p.Product.Code)).ToList();
            if (repited.Count == 0)
            {
                VM.SelectedSpace.Products.Add(p);
                VM.NewProductInspace.Add(p);
            }
        }
    }
}
