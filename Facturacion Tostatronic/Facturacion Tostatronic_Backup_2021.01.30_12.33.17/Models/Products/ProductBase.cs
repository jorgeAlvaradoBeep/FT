using Facturacion_Tostatronic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion_Tostatronic.Models.Products
{
    public class ProductBase : BaseNotifyPropertyChanged
    {
        private string code;

        public string Code
        {
            get { return code; }
            set { code = value; }
        }
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private float existence;

        public float Existence
        {
            get { return existence; }
            set { existence = value; }
        }
        private float minimumQuantity;

        public float MinimumQuantity
        {
            get { return minimumQuantity; }
            set { minimumQuantity = value; }
        }
        private float buyPrice;

        public float BuyPrice
        {
            get { return buyPrice; }
            set { buyPrice = value; }
        }
        private float minimumPrice;

        public float MinimumPrice
        {
            get { return minimumPrice; }
            set { minimumPrice = value; }
        }
        private float distributorPrice;

        public float DistributorPrice
        {
            get { return distributorPrice; }
            set { distributorPrice = value; }
        }
        private float publicPrice;

        public float PublicPrice
        {
            get { return publicPrice; }
            set { publicPrice = value; }
        }
        private string image;

        public string Image
        {
            get { return image; }
            set { image = value; }
        }
        public ProductBase()
        {

        }

        public ProductBase(string code, string name, float existence, float minimumQuantity, float buyPrice, float minimumPrice,
            float distributorPrice, float publicPrice, string image)
        {
            Code = code;
            Name = name;
            Existence = existence;
            MinimumQuantity = minimumQuantity;
            BuyPrice = buyPrice;
            MinimumPrice = minimumPrice;
            DistributorPrice = distributorPrice;
            PublicPrice = publicPrice;
            Image = image;
        }
    }
}
