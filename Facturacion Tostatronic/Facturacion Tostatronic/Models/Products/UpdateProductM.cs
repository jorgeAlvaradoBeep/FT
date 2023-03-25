using Facturacion_Tostatronic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion_Tostatronic.Models.Products
{
    public class UpdateProductM : BaseNotifyPropertyChanged
    {
        private string code;

        public string Codigo
        {
            get { return code; }
            set { SetValue(ref code, value); }
        }
        private string name;

        public string Nombre
        {
            get { return name; }
            set { name = value; }
        }
        private float existence;

        public float Existencia
        {
            get { return existence; }
            set { SetValue(ref existence, value); }
        }
        private float minimumQuantity;

        public float CantidadMinima
        {
            get { return minimumQuantity; }
            set { minimumQuantity = value; }
        }
        private float buyPrice;

        public float PrecioCompra
        {
            get { return buyPrice; }
            set 
            { 
                buyPrice = value;
                BuyPriceModified = true;
            }
        }
        private float minimumPrice;

        public float PrecioMinimo
        {
            get { return minimumPrice; }
            set { minimumPrice = value; }
        }
        private float distributorPrice;

        public float PrecioDistribuidor
        {
            get { return distributorPrice; }
            set { distributorPrice = value; }
        }
        private float publicPrice;

        public float PrecioPublico
        {
            get { return publicPrice; }
            set 
            { 
                publicPrice = value;
                PublicPriceModified=true;
            }
        }
        private string image;

        public string Imagen
        {
            get { return image; }
            set { image = value; }
        }
        private string prestashopID;

        public string PrestashopID
        {
            get { return prestashopID; }
            set { SetValue(ref prestashopID, value); }
        }

        public bool PublicPriceModified { get; set; }
        public bool BuyPriceModified { get; set; }
        public UpdateProductM()
        {
            BuyPriceModified=false;
            PublicPriceModified = false;
        }

        public UpdateProductM(string code, string name, float existence, float minimumQuantity, float buyPrice, float minimumPrice,
            float distributorPrice, float publicPrice, string image, string prestashopID)
        {
            Codigo = code;
            Nombre = name;
            Existencia = existence;
            CantidadMinima = minimumQuantity;
            PrecioCompra = buyPrice;
            PrecioMinimo = minimumPrice;
            PrecioDistribuidor = distributorPrice;
            PrecioPublico = publicPrice;
            Imagen = image;
            PrestashopID = prestashopID;
            BuyPriceModified = false;
            PublicPriceModified = false;
        }
    }
}
