using Facturacion_Tostatronic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion_Tostatronic.Models.Products
{
    public class MLProductData : BaseNotifyPropertyChanged
    {
        #region Properties
        //General configuration
        private int numberOfPiecesOfPackage;

        public int NumberOfPiecesOfPackage
        {
            get { return numberOfPiecesOfPackage; }
            set 
            { 
                SetValue(ref numberOfPiecesOfPackage, value);
                UpdatePrice();
            }
        }

        private float priceToCalculate;

        public float PriceToCalculate
        {
            get { return priceToCalculate; }
            set 
            { 
                SetValue(ref priceToCalculate, value);
                UpdatePrice();
            }
        }

        private float publicationPrice;

        public float PublicationPrice
        {
            get { return publicationPrice; }
            set 
            { 
                SetValue(ref publicationPrice, value);
                UpdatePrice();
            }
        }


        //classic Publication configuration

        private float classicPublicationComission;

        public float ClassicPublicationComission
        {
            get { return classicPublicationComission; }
            set 
            { 
                SetValue(ref classicPublicationComission, value);
                UpdatePrice();
            }
        }

        private float classicPublicationShippingCost;

        public float ClassicPublicationShippingCost
        {
            get { return classicPublicationShippingCost; }
            set 
            { 
                SetValue(ref classicPublicationShippingCost, value);
                UpdatePrice();
            }
        }

        private float classicPriceWOS;

        public float ClassicPriceWOS
        {
            get { return classicPriceWOS; }
            set 
            { 
                SetValue(ref classicPriceWOS, value);
                
            }
        }
        private float classicPriceWS;

        public float ClassicPriceWS
        {
            get { return classicPriceWS; }
            set { SetValue(ref classicPriceWS, value); }
        }


        //Premium Publication configuration

        private float premiumPublicationComission;

        public float PremiumublicationComission
        {
            get { return premiumPublicationComission; }
            set { SetValue(ref premiumPublicationComission, value); UpdatePrice(); }
        }

        private float premiumPublicationShippingCost;

        public float PremiumPublicationShippingCost
        {
            get { return premiumPublicationShippingCost; }
            set { SetValue(ref premiumPublicationShippingCost, value); UpdatePrice(); }
        }

        private float premiumPriceWOS;

        public float PremiumPriceWOS
        {
            get { return premiumPriceWOS; }
            set { SetValue(ref premiumPriceWOS, value); }
        }
        private float premiumPriceWS;

        public float PremiumPriceWS
        {
            get { return premiumPriceWS; }
            set { SetValue(ref premiumPriceWS, value); }
        }

        public float MinPrice { get; set; }


        #endregion

        void UpdatePrice()
        {
            if (PriceToCalculate == 0 || NumberOfPiecesOfPackage == 0 || PublicationPrice == 0 ||
               ClassicPublicationComission == 0 || ClassicPublicationShippingCost == 0 || PremiumublicationComission == 0)
                return;
            float p = PriceToCalculate;
            p = p * NumberOfPiecesOfPackage;
            p += PublicationPrice;
            p = p / (1 - (ClassicPublicationComission / 100));
            p /= 0.9f;
            ClassicPriceWOS = p;
            p = PriceToCalculate;
            p = p * NumberOfPiecesOfPackage;
            p += PublicationPrice;
            p += ClassicPublicationShippingCost;
            p = p / (1 - (ClassicPublicationComission / 100));
            p /= 0.9f;
            ClassicPriceWS = p;

            p = PriceToCalculate;
            p = p * NumberOfPiecesOfPackage;
            p += PublicationPrice;
            p = p / (1 - (PremiumublicationComission / 100));
            p /= 0.9f;

            PremiumPriceWOS = p;
            p = PriceToCalculate;
            p = p * NumberOfPiecesOfPackage;
            p += PublicationPrice;
            p += PremiumPublicationShippingCost;
            p = p / (1 - (PremiumublicationComission / 100));
            p /= 0.9f;
            PremiumPriceWS = p + PremiumPublicationShippingCost;
        }

    }
}
