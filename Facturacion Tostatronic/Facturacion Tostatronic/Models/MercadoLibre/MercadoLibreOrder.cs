using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion_Tostatronic.Models.MercadoLibre
{
    public class MercadoLibreOrder
    {
        [JsonProperty("id")]
        public long OrderId { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("paid_amount")]
        public decimal PaidAmount { get; set; }

        [JsonProperty("total_amount")]
        public decimal TotalAmount { get; set; }

        [JsonProperty("currency_id")]
        public string CurrencyId { get; set; }

        [JsonProperty("date_created")]
        public DateTime DateCreated { get; set; }

        [JsonProperty("date_closed")]
        public DateTime? DateClosed { get; set; }

        [JsonProperty("fulfilled")]
        public bool Fulfilled { get; set; }

        [JsonProperty("tags")]
        public List<string> Tags { get; set; }

        [JsonProperty("seller")]
        public User Seller { get; set; }

        [JsonProperty("buyer")]
        public User Buyer { get; set; }

        [JsonProperty("payments")]
        public List<Payment> Payments { get; set; }

        [JsonProperty("order_items")]
        public List<OrderItem> OrderItems { get; set; }

        [JsonProperty("shipping")]
        public Shipping Shipping { get; set; }

        [JsonProperty("context")]
        public Context Context { get; set; }
        public decimal SaleFee => OrderItems?.Where(i => i != null).Sum(i => i.SaleFee * i.Quantity) ?? 0;
    }

    public class User
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("nickname")]
        public string Nickname { get; set; }
    }

    public class Payment
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("status_detail")]
        public string StatusDetail { get; set; }

        [JsonProperty("payment_type")]
        public string PaymentType { get; set; }

        [JsonProperty("payment_method_id")]
        public string PaymentMethodId { get; set; }

        [JsonProperty("transaction_amount")]
        public decimal TransactionAmount { get; set; }

        [JsonProperty("shipping_cost")]
        public decimal ShippingCost { get; set; }

        [JsonProperty("total_paid_amount")]
        public decimal TotalPaidAmount { get; set; }

        [JsonProperty("date_approved")]
        public DateTime DateApproved { get; set; }

        [JsonProperty("installments")]
        public int Installments { get; set; }

        [JsonProperty("issuer_id")]
        public string IssuerId { get; set; }

        [JsonProperty("payer_id")]
        public long PayerId { get; set; }

        [JsonProperty("authorization_code")]
        public string AuthorizationCode { get; set; }

        [JsonProperty("currency_id")]
        public string CurrencyId { get; set; }
    }

    public class OrderItem
    {
        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        [JsonProperty("unit_price")]
        public decimal UnitPrice { get; set; }

        [JsonProperty("sale_fee")]
        public decimal SaleFee { get; set; }

        [JsonProperty("item")]
        public Item Item { get; set; }
    }

    public class Item
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("category_id")]
        public string CategoryId { get; set; }

        [JsonProperty("warranty")]
        public string Warranty { get; set; }

        [JsonProperty("condition")]
        public string Condition { get; set; }

        [JsonProperty("seller_sku")]
        public string SellerSku { get; set; }
    }

    public class Shipping
    {
        [JsonProperty("id")]
        public long Id { get; set; }
    }

    public class Context
    {
        [JsonProperty("channel")]
        public string Channel { get; set; }

        [JsonProperty("site")]
        public string Site { get; set; }

        [JsonProperty("flows")]
        public List<string> Flows { get; set; }
    }
}
