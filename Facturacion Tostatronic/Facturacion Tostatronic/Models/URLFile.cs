namespace Facturacion_Tostatronic.Models
{
    public class URLData
    {
        //const string baseURL = "http://192.168.100.7/factura_api/controller/";//Enlace de prueba
        const string baseURL = "http://192.168.100.29/factura_api/controller/";//Enlace de produccion
        public static string factured_sales = baseURL+"Sales/factured_sales.php";
        public static string invoice_save = baseURL+ "Sales/invoice_save.php";
        public static string sales = baseURL+ "Sales/sales.php";
        public static string products = baseURL+ "Sales/products.php";
        public static string clients = baseURL+ "Sales/clients.php";
        public static string payment_method = baseURL+ "CFDI/payment_method.php";
        public static string cfdi_use = baseURL+ "CFDI/cfdi_use.php";
        public static string payment_form = baseURL+ "CFDI/payment_form.php";
        public static string product_list = baseURL+ "Products/products.php";
        public static string product_ps = baseURL+ "Products/productsPS.php";
        public static string product_ps_web = baseURL+ "Products/prodcutFromPS.php";
        public static string product_new_product_list = baseURL+ "Products/newProductsList.php";
        public static string product_distributor_price = baseURL+ "Products/DistributorPrice.php";
        public static string product_distributor_price_np = baseURL+ "Products/DistributorPriceNP.php";
    }
}