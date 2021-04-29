namespace Facturacion_Tostatronic.Models
{
    public class URLData
    {
        const string baseURL = "http://192.168.3.15:8012/factura_api/controller/";//Enlace de prueba
        //const string baseURL = "http://192.168.100.29/factura_api/controller/";//Enlace de produccion
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
        public static string product_public_price = baseURL+ "Products/PublicPrice.php";
        public static string product_page_prices = baseURL+ "Products/ProductExcel.php";
        public static string product_complete_codes = baseURL+ "Products/ProductCodes.php";
        public static string product_add_new = baseURL+ "Products/CompleteProduct.php";
        public static string product_add_ps_single = baseURL+ "Products/productPSSingle.php";
        public static string product_sale_search = baseURL+ "Products/ProductSaleSearch.php";
        public static string product_updtae_image = baseURL+ "Products/ProductImages.php";
        public static string new_pi = baseURL+ "Products/NewOrderPI.php";
        //Parte de ventas
        public static string sales_client_search = baseURL+ "Sales/saleClient.php";
        public static string sales_save = baseURL+ "Sales/SaleSave.php";
        public static string quote_save = baseURL+ "Sales/PreSaleSave.php";
        //Variables para los almacenes
        public static string save_new_warehouse = baseURL + "Warehouse/Warehouse.php";
        public static string save_new_spaces = baseURL + "Warehouse/SaveSpaces.php";
        public static string save_new_product_to_space = baseURL + "Warehouse/SaveProductsToSpace.php";

        //Variables para el servidor comercial
        const string baseURLTosta = "https://tostatronic.com/store/NewWBST/";//Enlace de prueba
        public static string updateProductsPriceTosta = baseURLTosta+"updatePrice.php";//Enlace de prueba


        

        //Variables del WebServices de Prestashop
        public static string psBaseUrl = "https://tostatronic.com/store/api";
        public static string psAccount = "TJNTQKNTUELZU2V86CGIBCSJMB92LCQI";
        public static string psPassword = "";
    }
}