using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion_Tostatronic.Models.Products
{
    public class ProductComplete : ProductBase
    {
        private string prestashopID;

        public string PrestashopID
        {
            get { return prestashopID; }
            set { SetValue(ref prestashopID, value); }
        }
        private string upc;

        public string UPC
        {
            get { return upc; }
            set { SetValue(ref upc, value); }
        }
        private List<SpecificPrice> specificPrices;

        public List<SpecificPrice> SpecificPrices
        {
            get { return specificPrices; }
            set { SetValue(ref specificPrices, value); }
        }
        private string description;

        public string Description
        {
            get { return description; }
            set { SetValue(ref description, value); }
        }

        public static explicit operator ProductComplete(ProductSaleSearch v)
        {
            throw new NotImplementedException();
        }

        public ProductComplete()
        {

        }
        public ProductComplete(string code, string name, float existence, float minimumQuantity, float buyPrice, float minimumPrice,
            float distributorPrice, float publicPrice, string image, string prestashopID, string upc, List<SpecificPrice> specificPrices) : 
            base(code, name, existence, minimumQuantity, buyPrice, minimumPrice, distributorPrice, publicPrice, image)
        {
            Code = code;
            Existence = existence;
            MinimumQuantity = minimumQuantity;
            BuyPrice = buyPrice;
            MinimumPrice = minimumPrice;
            DistributorPrice = distributorPrice;
            PublicPrice = publicPrice;
            Image = image;
            PrestashopID = prestashopID;
            UPC = upc;
            SpecificPrices = specificPrices;
        }
    }
}
