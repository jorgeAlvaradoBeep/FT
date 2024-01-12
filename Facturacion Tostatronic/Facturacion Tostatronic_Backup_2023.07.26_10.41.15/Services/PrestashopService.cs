using Bukimedia.PrestaSharp.Factories;
using Facturacion_Tostatronic.Models;
using System;
using System.Collections.Generic;
using Bukimedia.PrestaSharp.Entities;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Facturacion_Tostatronic.Models.Products;
using Bukimedia.PrestaSharp;

namespace Facturacion_Tostatronic.Services
{
    public static class PrestashopService
    {
        public static string GetUPC(string reference)
        {
            ProductFactory product = new ProductFactory(URLData.psBaseUrl, URLData.psAccount, URLData.psPassword);
            Dictionary<string, string> dtn = new Dictionary<string, string>();
            dtn.Add("reference", reference);
            List<product> aux = product.GetByFilter(dtn, null, null);
            if (aux.Count > 0)
            {
                return aux[0].ean13;
            }
            return string.Empty;
        }
        public static string GetWOUPCProducts(List<ProductPS> products)
        {
            ProductFactory product = new ProductFactory(URLData.psBaseUrl, URLData.psAccount, URLData.psPassword);
            List<product> aux = product.GetAll();
            string errorMSG = string.Empty;
            if (aux.Count > 0)
            {
                foreach (ProductPS p in products)
                {
                    int index = aux.FindIndex(item => item.id.ToString() == p.ps);
                    if (index >= 0)
                    {
                        if(string.IsNullOrEmpty(aux[index].ean13))
                        {
                            //Actualizamos los UPC en la pagina
                            aux[index].ean13 = p.upc;

                            try
                            {
                                product.Update(aux[index]);
                            }
                            catch (PrestaSharpException e)
                            {
                                errorMSG += "Error al agregar el UPC en ps, se debera de agregar manualmente: " + e.ResponseErrorMessage + Environment.NewLine;
                            }
                        }
                    }
                }
                
            }
            return errorMSG;
        }
    }
}
