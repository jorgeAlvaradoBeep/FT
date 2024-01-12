using Facturacion_Tostatronic.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion_Tostatronic.Models.WareHouse
{
    public class LocalizationM : BaseNotifyPropertyChanged
    {
        private int id;

        public int ID
        {
            get { return id; }
            set { SetValue(ref id, value); }
        }

        private string name;
        [Required]
        public string Name
        {
            get { return name; }
            set 
            {
                ValidationContext validationContext = new ValidationContext(this, null, null);
                validationContext.MemberName = "Name";
                Validator.ValidateProperty(value, validationContext);
                if (string.IsNullOrEmpty(value))
                    return;
                SetValue(ref name, value);
                OrganizationFornitures = new ObservableCollection<OrganizationForniture>();
            }
        }

        private ObservableCollection<OrganizationForniture> organizationFornitures;

        public ObservableCollection<OrganizationForniture> OrganizationFornitures
        {
            get { return organizationFornitures; }
            set { SetValue(ref organizationFornitures, value); }
        }
    }
}
