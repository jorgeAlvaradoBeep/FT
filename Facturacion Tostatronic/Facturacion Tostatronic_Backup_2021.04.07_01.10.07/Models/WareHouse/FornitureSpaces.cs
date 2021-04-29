using Facturacion_Tostatronic.Services;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Facturacion_Tostatronic.Models.WareHouse
{
    public class FornitureSpaces : BaseNotifyPropertyChanged
    {
        public int ID { get; set; }
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
                if (!string.IsNullOrEmpty(name))
                    Edited = true;
                SetValue(ref name, value); 
            }
        }
        public bool Edited { get; set; } = false;
        public int IDForniture { get; set; }

        private ObservableCollection<ProductInSpace> products;

        public ObservableCollection<ProductInSpace> Products
        {
            get { return products; }
            set { SetValue(ref products, value); }
        }

    }
}
