using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion_Tostatronic.Services
{
    public class NavigationViewItemModel
    {
        public string Icon { get; set; }
        public string Title { get; set; }
        public ObservableCollection<NavigationViewItemModel> SubItems { get; set; }
    }
}
