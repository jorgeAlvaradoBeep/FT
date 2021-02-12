using Facturacion_Tostatronic.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                SetValue(ref name, value); 
            }
        }
        public bool Edited { get; set; } = false;
        public int IDForniture { get; set; }
    }
}
