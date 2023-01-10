using Facturacion_Tostatronic.Models;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WooCommerceNET.WooCommerce.v3;

namespace Facturacion_Tostatronic.Services
{
    class WebService
    {
        public static async Task<Response> InsertData(object newPI, string url)
        {
            // En el caso de Sandbox http://localhost/costs_api/controller/pi/pi.php
            var client = new RestClient(url);
            client.Timeout = 15000;

            var request = new RestRequest(Method.POST);

            //Generamos el archivo JSON que necesitaremos
            string data = JsonConvert.SerializeObject(newPI);

            // En el caso de JSON
            request.AddParameter("application/json", data, ParameterType.RequestBody);

            request.AddHeader("Content-Type", "application/json"); // Tambien puede ser application/json o application/xml
            IRestResponse response;
            try
            {
                response = await client.ExecuteAsync(request);
                Response result;
                if (response.IsSuccessful)
                    result = JsonConvert.DeserializeObject<Response>(response.Content);
                else
                    
                    result = new Response() { succes = false, message = response.ErrorMessage, statusCode = 404 };
                return result;
            }
            catch (TimeoutException e)
            {
                Response result = new Response();
                result.succes = false;
                result.message = "Tiempo de espera agotado... " + e.Message;
                return result;
            }
            catch (Exception e)
            {
                Response result = new Response();
                result.succes = false;
                result.message = "Error... " + e.Message;
                return result;
            }
        }

        public static async Task<Response> DeleteData(int idPI)
        {
            // En el caso de Sandbox 
            var client = new RestClient("http://localhost/costs_api/controller/pi/pi.php");
            client.Timeout = 15000;

            var request = new RestRequest(Method.DELETE);
            request.AddParameter("id_pi", idPI);
            IRestResponse response;
            try
            {
                response = await client.ExecuteAsync(request);
                Response result;
                if (response.IsSuccessful)
                    result = JsonConvert.DeserializeObject<Response>(response.Content);
                else
                    result = new Response() { succes = false, message = response.ErrorMessage, statusCode = 404 };
                return result;
            }
            catch (TimeoutException e)
            {
                Response result = new Response();
                result.succes = false;
                result.message = "Tiempo de espera agotado... " + e.Message;
                return result;
            }
            catch (Exception e)
            {
                Response result = new Response();
                result.succes = false;
                result.message = "Error... " + e.Message;
                return result;
            }
        }

        public static async Task<Response> GetData(string objectName, string findKey, string url)
        {
            // En el caso de Sandbox 
            var client = new RestClient(url);
            client.Timeout = 15000;

            var request = new RestRequest(Method.GET);
            request.AddParameter(objectName, findKey);
            IRestResponse response;
            try
            {
                response = await client.ExecuteAsync(request);
                Response result;
                if (response.IsSuccessful)
                    result = JsonConvert.DeserializeObject<Response>(response.Content);
                else
                    result = new Response() { succes = false, message = response.ErrorMessage, statusCode = 404 };
                return result;
            }
            catch (TimeoutException e)
            {
                Response result = new Response();
                result.succes = false;
                result.message = "Tiempo de espera agotado... " + e.Message;
                return result;
            }
            catch (Exception e)
            {
                Response result = new Response();
                result.succes = false;
                result.message = "Error... " + e.Message;
                return result;
            }
        }

        public static Response GetDataNoasync(string objectName, string findKey, string url)
        {
            // En el caso de Sandbox 
            var client = new RestClient(url);
            client.Timeout = 15000;

            var request = new RestRequest(Method.GET);
            request.AddParameter(objectName, findKey);
            IRestResponse response;
            try
            {
                response = client.Execute(request);
                Response result;
                if (response.IsSuccessful)
                    result = JsonConvert.DeserializeObject<Response>(response.Content);
                else
                    result = new Response() { succes = false, message = response.ErrorMessage, statusCode = 404 };
                return result;
            }
            catch (TimeoutException e)
            {
                Response result = new Response();
                result.succes = false;
                result.message = "Tiempo de espera agotado... " + e.Message;
                return result;
            }
            catch (Exception e)
            {
                Response result = new Response();
                result.succes = false;
                result.message = "Error... " + e.Message;
                return result;
            }
        }

        public static async Task<Response> GetDataForInvoice(string url)
        {
            // En el caso de Sandbox 
            var client = new RestClient(url);
            client.Timeout = 15000;

            var request = new RestRequest(Method.GET);
            IRestResponse response;
            try
            {
                response = await client.ExecuteAsync(request);
                Response result;
                if (response.IsSuccessful)
                    result = JsonConvert.DeserializeObject<Response>(response.Content);
                else
                    result = new Response() { succes = false, message = response.ErrorMessage, statusCode = 404 };
                return result;
            }
            catch (TimeoutException e)
            {
                Response result = new Response();
                result.succes = false;
                result.message = "Tiempo de espera agotado... " + e.Message;
                return result;
            }
            catch (Exception e)
            {
                Response result = new Response();
                result.succes = false;
                result.message = "Error... " + e.Message;
                return result;
            }
        }

        public static async Task<IRestResponse> GetDataWooCommercer(string url, string fields, string page)
        {
            // En el caso de Sandbox 
            var client = new RestClient();
            client.Timeout = 15000;
            client.BaseUrl = new System.Uri(url);
            client.Authenticator =
                OAuth1Authenticator.ForProtectedResource(URLData.wcKey, URLData.wcSecret, string.Empty, string.Empty);
            client.AddDefaultQueryParameter("fields", fields);
            client.AddDefaultQueryParameter("orderby", "id");
            client.AddDefaultQueryParameter("per_page", "100");
            client.AddDefaultQueryParameter("page", page);
            
            var request = new RestRequest(Method.GET);
            IRestResponse response;
            try
            {
                response = await client.ExecuteAsync(request);
                return response;
            }
            catch (TimeoutException e)
            {
                Response result = new Response();
                result.succes = false;
                result.message = "Tiempo de espera agotado... " + e.Message;
                return null;
            }
            catch (Exception e)
            {
                Response result = new Response();
                result.succes = false;
                result.message = "Error... " + e.Message;
                return null;
            }
        }

        public static async Task<IRestResponse> InsertDataWooCommercer(string url, string jsonToInsert)
        {
            // En el caso de Sandbox 
            var client = new RestClient();
            client.Timeout = 35000;
            client.BaseUrl = new System.Uri(url);
            client.Authenticator =
                OAuth1Authenticator.ForProtectedResource(URLData.wcKey, URLData.wcSecret, string.Empty, string.Empty);

            var request = new RestRequest(Method.POST);
            request.AddParameter("application/json", jsonToInsert, ParameterType.RequestBody);

            request.AddHeader("Content-Type", "application/json");
            IRestResponse response;
            try
            {
                response = await client.ExecuteAsync(request);
                return response;
            }
            catch (TimeoutException e)
            {
                Response result = new Response();
                result.succes = false;
                result.message = "Tiempo de espera agotado... " + e.Message;
                return null;
            }
            catch (Exception e)
            {
                Response result = new Response();
                result.succes = false;
                result.message = "Error... " + e.Message;
                return null;
            }
        }

        public static Response GetDataForInvoiceNoAsync(string url)
        {
            // En el caso de Sandbox 
            var client = new RestClient(url);
            client.Timeout = 15000;

            var request = new RestRequest(Method.GET);
            IRestResponse response;
            try
            {
                response = client.Execute(request);
                Response result;
                if (response.IsSuccessful)
                    result = JsonConvert.DeserializeObject<Response>(response.Content);
                else
                    result = new Response() { succes = false, message = response.ErrorMessage, statusCode = 404 };
                return result;
            }
            catch (TimeoutException e)
            {
                Response result = new Response();
                result.succes = false;
                result.message = "Tiempo de espera agotado... " + e.Message;
                return result;
            }
            catch (Exception e)
            {
                Response result = new Response();
                result.succes = false;
                result.message = "Error... " + e.Message;
                return result;
            }
        }
        public static async Task<Response> GetData(object newPI, string url)
        {
            // En el caso de Sandbox http://localhost/costs_api/controller/pi/pi.php
            var client = new RestClient(url);
            client.Timeout = 15000;

            var request = new RestRequest(Method.GET);

            //Generamos el archivo JSON que necesitaremos
            string data = JsonConvert.SerializeObject(newPI);

            // En el caso de JSON
            request.AddParameter("application/json", data, ParameterType.RequestBody);

            request.AddHeader("Content-Type", "application/json"); // Tambien puede ser application/json o application/xml
            IRestResponse response;
            try
            {
                response = await client.ExecuteAsync(request);
                Response result;
                if (response.IsSuccessful)
                    result = JsonConvert.DeserializeObject<Response>(response.Content);
                else
                    result = new Response() { succes = false, message = response.ErrorMessage, statusCode = 404 };
                return result;
            }
            catch (TimeoutException e)
            {
                Response result = new Response();
                result.succes = false;
                result.message = "Tiempo de espera agotado... " + e.Message;
                return result;
            }
            catch (Exception e)
            {
                Response result = new Response();
                result.succes = false;
                result.message = "Error... " + e.Message;
                return result;
            }
        }

        public static async Task<Response> GetDataNode(string url, string criterialSearch)
        {
            url += criterialSearch;
            var client = new RestClient(url);
            client.Timeout = 15000;

            var request = new RestRequest(Method.GET);

            
            IRestResponse response;
            try
            {
                response = await client.ExecuteAsync(request);
                Response result;
                if (response.IsSuccessful)
                    result = JsonConvert.DeserializeObject<Response>(response.Content);
                else
                    result = new Response() { succes = false, message = response.ErrorMessage, statusCode = 404 };
                return result;
            }
            catch (TimeoutException e)
            {
                Response result = new Response();
                result.succes = false;
                result.message = "Tiempo de espera agotado... " + e.Message;
                return result;
            }
            catch (Exception e)
            {
                Response result = new Response();
                result.succes = false;
                result.message = "Error... " + e.Message;
                return result;
            }
        }

        public static Response GetDataNodeNoAsync(string url, string criterialSearch)
        {
            url += criterialSearch;
            var client = new RestClient(url);
            client.Timeout = 15000;

            var request = new RestRequest(Method.GET);


            IRestResponse response;
            try
            {
                response = client.Execute(request);
                Response result;
                if (response.IsSuccessful)
                    result = JsonConvert.DeserializeObject<Response>(response.Content);
                else
                    result = new Response() { succes = false, message = response.ErrorMessage, statusCode = 404 };
                return result;
            }
            catch (TimeoutException e)
            {
                Response result = new Response();
                result.succes = false;
                result.message = "Tiempo de espera agotado... " + e.Message;
                return result;
            }
            catch (Exception e)
            {
                Response result = new Response();
                result.succes = false;
                result.message = "Error... " + e.Message;
                return result;
            }
        }

        public static async Task<Response> ModifyData(object newPI, string url)
        {
            // En el caso de Sandbox http://localhost/costs_api/controller/pi/pi.php
            var client = new RestClient(url);
            client.Timeout = 15000;

            var request = new RestRequest(Method.PUT);

            //Generamos el archivo JSON que necesitaremos
            string data = JsonConvert.SerializeObject(newPI);

            // En el caso de JSON
            request.AddParameter("application/json", data, ParameterType.RequestBody);

            request.AddHeader("Content-Type", "application/json"); // Tambien puede ser application/json o application/xml
            IRestResponse response;
            try
            {
                response = await client.ExecuteAsync(request);
                Response result;
                if (response.IsSuccessful)
                    result = JsonConvert.DeserializeObject<Response>(response.Content);
                else

                    result = new Response() { succes = false, message = response.ErrorMessage, statusCode = 404 };
                return result;
            }
            catch (TimeoutException e)
            {
                Response result = new Response();
                result.succes = false;
                result.message = "Tiempo de espera agotado... " + e.Message;
                return result;
            }
            catch (Exception e)
            {
                Response result = new Response();
                result.succes = false;
                result.message = "Error... " + e.Message;
                return result;
            }
        }

        public static async Task<Response> DeleteDataEF(string url, string[] ids)
        {
            foreach(string id in ids) 
            {
                url += id+"/";
            }
            
            var client = new RestClient(url);
            client.Timeout = 15000;

            var request = new RestRequest(Method.DELETE);


            IRestResponse response;
            try
            {
                response = await client.ExecuteAsync(request);
                Response result;
                if (response.IsSuccessful)
                    result = JsonConvert.DeserializeObject<Response>(response.Content);
                else
                    result = new Response() { succes = false, message = response.ErrorMessage, statusCode = 404 };
                return result;
            }
            catch (TimeoutException e)
            {
                Response result = new Response();
                result.succes = false;
                result.message = "Tiempo de espera agotado... " + e.Message;
                return result;
            }
            catch (Exception e)
            {
                Response result = new Response();
                result.succes = false;
                result.message = "Error... " + e.Message;
                return result;
            }
        }
    }
}
