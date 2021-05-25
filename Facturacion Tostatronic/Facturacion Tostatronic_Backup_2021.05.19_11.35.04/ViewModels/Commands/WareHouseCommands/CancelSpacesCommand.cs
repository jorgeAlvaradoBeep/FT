using Facturacion_Tostatronic.ViewModels.WareHouse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Facturacion_Tostatronic.ViewModels.Commands.WareHouseCommands
{
    public class CancelSpacesCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public AddProductsToSpaceVM VM { get; set; }
        public CancelSpacesCommand(AddProductsToSpaceVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            VM.SelectedWareHouse = null;
            VM.IsSelectionAvailable = true;
            VM.ProductsGrid = System.Windows.Visibility.Hidden;
            VM.NewProductInspace.Clear();
            VM.ProductCriterialSearch = string.Empty;
            VM.SerchedProducts = new List<Models.WareHouse.ProductInSpace>();
            
        }
    }
}
