namespace Facturacion_Tostatronic.Models
{
    public class URLData
    {
        //const string baseURL = "http://192.168.3.15:8012/factura_api/controller/";//Enlace de prueba
        //const string baseURL = "http://192.168.100.29/factura_api/controller/";//Enlace de produccion
        const string baseURL = "http://143.198.173.21/factura_api/controller/";//Prueba de enlace con servidor dedicado
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
        public static string product_update = baseURL+ "Products/ProductUpdate.php";
        public static string product_updtae_image = baseURL+ "Products/ProductImages.php";
        public static string new_pi = baseURL+ "Products/NewOrderPI.php";
        public static string search_product_localization = baseURL+ "Products/ShowProductPlaceC.php";
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

        //Variables del WebServices de WooCommerce
        public static string wcBaseUrl = "https://www.tostatronic.com/wp-json/wc/v3/";
        public static string wcKey = "ck_365d249e5d95320bac719698750403b22310e114";
        public static string wcSecret = "cs_d870c7cc05e2ef673d1c695497439a9c27e8d509";
        public static string wcProducts = wcBaseUrl+ "products";
        public static string wcBatchProduct = wcBaseUrl + "products/batch";



        //URL Para NodeJS
        const string baseURLNode = "http://143.198.173.21:3000/";
        //const string baseURLNode = "http://localhost:3000/";
        public static string quotes = baseURLNode + "short_quotes/";
        public static string quotesByDate = baseURLNode + "short_quotes_date/";
        public static string productsOfQuote = baseURLNode + "quote_products/";
        public static string creditSales = baseURLNode + "credit_sales/";
        public static string creditPayments = baseURLNode + "sales_payments/";
        
        public static string addPaymentToSale = baseURLNode + "add_payment/";
        public static string setSucursalPrice = baseURLNode + "existe_sucursal_price/";
        public static string addSucursalPrice = baseURLNode + "add_sucursal_price/";
        public static string regimenFiscal = baseURLNode + "Regimen/";

        //URL Para API de .net
        //const string baseURLNET = "http://192.168.3.15:5249/api/";
        const string baseURLNET = "http://192.168.68.102:5249/api/";
        //const string baseURLNET = "http://143.198.173.21:5249/api/";

        public static string addClient = baseURLNET + "Clientes/Create";
        public static string rfcExist = baseURLNET + "Clientes/RFCExist/";
        public static string editClient = baseURLNET + "Clientes/Edit/";
        public static string getClientByRFC = baseURLNET + "Clientes/ClientRFC/";
        public static string getClients = baseURLNET + "Clientes";
        public static string getClientOrders = baseURLNET + "Ventas/ClientOrdersWT/";
        public static string getDatedOrders = baseURLNET + "Ventas/SeeDatedOrder/";
        public static string getSpecificOrder = baseURLNET + "Ventas/GetSpecificSale/";
        public static string getDayOrdersWT = baseURLNET + "Ventas/DayOrdersWT/";
        public static string getOrderComplete = baseURLNET + "Ventas/OrderWT/";
        public static string regimenFiscalesNet = baseURLNET + "RegimenFiscal";
        public static string editRegimenFiscalesNet = baseURLNET + "Clientes/EditRegimen";
        public static string getProductsNet = baseURLNET + "Products/";
        public static string editProductsNet = baseURLNET + "Products/Edit/";
        public static string searchProductsNet = baseURLNET + "Products/SearchProduct/";
        public static string getTotalForSale = baseURLNET + "ProductosDeVenta/SaleTotal/";
        public static string getOrderData = baseURLNET + "ProductosDeVenta/";
        public static string productoEnEspacio = baseURLNET + "ProductoEnEspacio/";
        public static string getCodigosSatNET = baseURLNET + "CodigoSat/";
        public static string productExistNET = baseURLNET + "Products/ProductExist/";
        public static string addProductCodesNET = baseURLNET + "CodigosUniversales/Create/";
        public static string updateProductCodesListNET = baseURLNET + "CodigosUniversales/EditList/";
        public static string createProductCodesListNET = baseURLNET + "CodigosUniversales/CreateList/";
        public static string getProductTypeNET = baseURLNET + "CodigosUniversales/ProductType/";
        public static string getProductCodesNET = baseURLNET + "CodigosUniversales/";
        public static string editProductCodesNET = baseURLNET + "CodigosUniversales/Edit/";
        public static string getProductCodesIDNET = baseURLNET + "CodigosUniversales/GetUniversalCodes/";
        public static string addProductSatCodeNET = baseURLNET + "ProductSatCode/Create/";
        public static string getProductSatCodeNET = baseURLNET + "ProductSatCode/";
        public static string getDatedQuotesNET = baseURLNET + "Cotizacion/SeeDatedQoute/";
        public static string getSpecificQuoteNET = baseURLNET + "Cotizacion/GetSpecificQoute/";
        public static string getProductsForQuote = baseURLNET + "ProductosDeCotizacion/";
        public static string Orders = baseURLNET + "Ordenes/";
        public static string OpenOrders = baseURLNET + "Ordenes/AvailableOrders/";
        public static string ProductOrderInfo = baseURLNET + "ProductOrderInfo/";
        public static string ProductOrderInfoEdit = baseURLNET + "ProductOrderInfo/Edit/";
        public static string InserProductOrderInfoList = baseURLNET + "ProductOrderInfo/Create/Bulk";
        public static string OrderProducts = baseURLNET + "ProductosOrdenes/";
        public static string OrderProductsDelete = baseURLNET + "ProductosOrdenes/Delete/";
        public static string InserOrderProductsList = baseURLNET + "ProductosOrdenes/Create/Bulk";
        public static string OrderProductsEditList = baseURLNET + "ProductosOrdenes/Edit/";


    }
}