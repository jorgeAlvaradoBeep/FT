using Facturacion_Tostatronic.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion_Tostatronic.Models.WareHouse
{
    public class WareHouseM : BaseNotifyPropertyChanged
    {
        public int ID { get; set; }
        private string name;
        public string Name
        {
            get { return name; }
            set 
            {
                SetValue(ref name, value); 
            }
        }

        private ObservableCollection<LocalizationM> localizations;

        public ObservableCollection<LocalizationM> Localizations
        {
            get { return localizations; }
            set { SetValue(ref localizations, value); }
        }

    }
}
