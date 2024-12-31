using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion_Tostatronic.Models.Products
{
    public class ProductCompleteNewArrival:ProductComplete
    {
        private int oldStock;

        public int OldStock
        {
            get { return oldStock; }
            set { SetValue(ref oldStock, value); }
        }
        private int newStock;

        public int NewStock
        {
            get { return newStock; }
            set { SetValue(ref newStock, value); }
        }
        private bool newProduct;

        public bool NewProduct
        {
            get { return newProduct; }
            set { SetValue(ref newProduct, value); }
        }
        private bool procesado;

        public bool Procesado
        {
            get { return procesado; }
            set { SetValue(ref procesado, value); }
        }
        public int WooID { get; set; }
        public int WooParentID { get; set; }

        public ProductCompleteNewArrival()
        {
        }
        public ProductCompleteNewArrival(string code, string name, float existence, float minimumQuantity, float buyPrice, float minimumPrice,
            float distributorPrice, float publicPrice, string image, string prestashopID, string upc, List<SpecificPrice> specificPrices, int oldStock, int newStock) :
            base(code, name, existence, minimumQuantity, buyPrice, minimumPrice, distributorPrice, publicPrice, image, prestashopID, upc, specificPrices)
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
            OldStock = oldStock;
            NewStock = newStock;
        }
    }
}
