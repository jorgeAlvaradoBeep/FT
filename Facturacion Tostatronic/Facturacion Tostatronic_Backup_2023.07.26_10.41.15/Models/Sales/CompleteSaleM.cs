using Facturacion_Tostatronic.Models.Clients;
using Facturacion_Tostatronic.Models.Products;
using Facturacion_Tostatronic.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Facturacion_Tostatronic.Models.Sales
{
    public class CompleteSaleM : BaseNotifyPropertyChanged
    {
        public int IDSale { get; set; }
        private ClientSale clientSale;

        public ClientSale ClientSale
        {
            get { return clientSale; }
            set 
            { 
                SetValue(ref clientSale, value);
            }
        }
        private int priceType;

        public int PriceType
        {
            get { return priceType; }
            set 
            {
                if (priceType.Equals(value))
                    return;
                SetValue(ref priceType, value); 
                if(SearchedProducts.Count>0)
                {
                    foreach(ProductSaleSearch product in SearchedProducts)
                    {
                        product.DisplayPrice = product.PublicPrice;
                    }
                    switch (PriceType)
                    {
                        case 0:
                            foreach (ProductSaleSearch product in SearchedProducts)
                            {
                                product.DisplayPrice = product.MinimumPrice;
                            }
                            break;
                        case 1:
                            foreach (ProductSaleSearch product in SearchedProducts)
                            {
                                product.DisplayPrice = product.DistributorPrice;
                            }
                            break;
                        case 2:
                            foreach (ProductSaleSearch product in SearchedProducts)
                            {
                                product.DisplayPrice = product.PublicPrice;
                            }
                            break;
                    }
                }
                if (SaledProducts.Count > 0)
                {
                    switch (PriceType)
                    {
                        case 0:
                            foreach (ProductSaleSaled product in SaledProducts)
                            {
                                product.DisplayPrice = product.MinimumPrice;
                            }
                            break;
                        case 1:
                            foreach (ProductSaleSaled product in SaledProducts)
                            {
                                product.DisplayPrice = product.DistributorPrice;
                            }
                            break;
                        case 2:
                            foreach (ProductSaleSaled product in SaledProducts)
                            {
                                product.DisplayPrice = product.PublicPrice;
                            }
                            break;
                    }
                }
                GetSubtotal();
            }
        }

        private List<ProductSaleSearch> searchedProducts;

        public List<ProductSaleSearch> SearchedProducts
        {
            get { return searchedProducts; }
            set { SetValue(ref searchedProducts , value); }
        }

        private ObservableCollection<ProductSaleSaled> saleSearches;

        public ObservableCollection<ProductSaleSaled> SaledProducts
        {
            get { return saleSearches; }
            set { SetValue(ref saleSearches , value); }
        }

        private float subTotal;

        public float SubTotal
        {
            get { return subTotal; }
            set 
            { 
                SetValue(ref subTotal, value);
                if (NeedFactura)
                    IVA = SubTotal * .16f;
                else
                    IVA = 0;
                Total = SubTotal + IVA;
            }
        }

        private float iva;

        public float IVA
        {
            get { return iva; }
            set { SetValue(ref iva, value); }
        }

        private float total;

        public float Total
        {
            get { return total; }
            set { SetValue(ref total, value); }
        }

        public void GetSubtotal()
        {
            float sub = 0;
            foreach (ProductSaleSaled p in SaledProducts)
            {
                p.Subtotal = p.DisplayPrice * p.SaledQuantity;
                sub += p.Subtotal;
            }
            SubTotal = sub;
        }

        private bool needFactura;

        public bool NeedFactura
        {
            get { return needFactura; }
            set 
            { 
                SetValue(ref needFactura, value);
                GetSubtotal();
            }
        }

        private int salerID;

        public int SalerID
        {
            get { return salerID; }
            set { SetValue(ref salerID, value); }
        }

        public PaymentM Payment { get; set; }

        public bool SetIVAPrices()
        {
            if(SaledProducts.Count>0)
            {
                string message = "";
                foreach (ProductSaleSaled product in SaledProducts)
                {
                    if ((product.DisplayPrice / 1.16f) >= (product.MinimumPrice / 1.16f))
                        product.DisplayPrice /= 1.16f;
                    else
                        message += $"El precio actual ({product.DisplayPrice}) del producto {product.Name} ya esta en su minimo permitido{Environment.NewLine}";
                }
                GetSubtotal();
                NeedFactura = true;
                if (!message.Equals(""))
                    MessageBox.Show(message, "Algunos precios no se cambiaron", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            return true;   
        }

        public bool UndoIVAPrices()
        {
            if (SaledProducts.Count > 0)
            {
                foreach (ProductSaleSaled product in SaledProducts)
                {
                    product.DisplayPrice *= 1.16f;
                }
                GetSubtotal();
                NeedFactura = false;
            }
            return true;
        }

    }
}
