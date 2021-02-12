using Facturacion_Tostatronic.Models;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using System.Xml.Serialization;
using XSDToXML.Utils;

namespace Facturacion_Tostatronic.Services
{
    class Facturacion
    {
        static private string prePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        static private string path = Path.Combine(prePath, "Tostatronic");
        static string pathXML = path + @"/XML.xml";
        static string pathPDF = path + @"/PDF.pdf";
        string realXMLName = path;
        string realPDFName = path;
        
        public static void CreaFactura()
        {
            string pathCer = Directory.GetCurrentDirectory() + @"/Fiel/Certifiado.cer";
            string pathKey = Directory.GetCurrentDirectory() + @"/Fiel/Key.key";
            string clavePrivada = "12345678a";

            //Obtenemos el numero
            string numeroCertificado, aa, b, c;
            SelloDigital.leerCER(pathCer, out aa, out b, out c, out numeroCertificado);


            //Llenamos la clase COMPROBANTE--------------------------------------------------------
            Comprobante oComprobante = new Comprobante();
            oComprobante.Version = "3.3";
            oComprobante.Serie = "H";
            oComprobante.Folio = "1";
            oComprobante.Fecha = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
            // oComprobante.Sello = "faltante"; //sig video
            oComprobante.FormaPago = "1";
            oComprobante.NoCertificado = numeroCertificado;
            // oComprobante.Certificado = ""; //sig video
            oComprobante.SubTotal = 10m;
            oComprobante.Moneda = "MXN";
            oComprobante.Total = 10;
            oComprobante.TipoDeComprobante = "I";
            oComprobante.MetodoPago = "PUE";
            oComprobante.LugarExpedicion = "44860";



            ComprobanteEmisor oEmisor = new ComprobanteEmisor();

            oEmisor.Rfc = "AATJ9502061EA";
            oEmisor.Nombre = "Jorge Humberto Alvarado Tostado";
            oEmisor.RegimenFiscal = "612";

            ComprobanteReceptor oReceptor = new ComprobanteReceptor();
            oReceptor.Nombre = "Pepe SA DE CV";
            oReceptor.Rfc = "BIO091204LB1";
            oReceptor.UsoCFDI = "G03";

            //asigno emisor y receptor
            oComprobante.Emisor = oEmisor;
            oComprobante.Receptor = oReceptor;


            List<ComprobanteConcepto> lstConceptos = new List<ComprobanteConcepto>();
            ComprobanteConcepto oConcepto = new ComprobanteConcepto();
            oConcepto.Importe = 10m;
            oConcepto.ClaveProdServ = "92111704";
            oConcepto.Cantidad = 1;
            oConcepto.ClaveUnidad = "H87";
            oConcepto.Descripcion = "Un misil para la guerra";
            oConcepto.ValorUnitario = 10m;


            lstConceptos.Add(oConcepto);

            oComprobante.Conceptos = lstConceptos.ToArray();


            //Creamos el xml
            CreateXML(oComprobante);

            string cadenaOriginal = "";
            string pathxsl = Directory.GetCurrentDirectory() + @"/Fiel/cadenaoriginal_3_3.xslt";
            System.Xml.Xsl.XslCompiledTransform transformador = new System.Xml.Xsl.XslCompiledTransform(true);
            transformador.Load(pathxsl);

            using (StringWriter sw = new StringWriter())
            using (XmlWriter xwo = XmlWriter.Create(sw, transformador.OutputSettings))
            {

                transformador.Transform(pathXML, xwo);
                cadenaOriginal = sw.ToString();
            }


            SelloDigital oSelloDigital = new SelloDigital();
            oComprobante.Certificado = oSelloDigital.Certificado(pathCer);
            oComprobante.Sello = oSelloDigital.Sellar(cadenaOriginal, pathKey, clavePrivada);

            CreateXML(oComprobante);

            ////TIMBRE DEL XML
            ServiceReference1.RespuestaCFDi respuestaCFDI = new ServiceReference1.RespuestaCFDi();

            byte[] bXML = System.IO.File.ReadAllBytes(pathXML);

            ServiceReference1.TimbradoClient oTimbrado = new ServiceReference1.TimbradoClient();

            respuestaCFDI = oTimbrado.TimbrarTest("TEST010101ST1", "a", bXML);

            if (respuestaCFDI.Documento == null)
            {
                Console.WriteLine(respuestaCFDI.Mensaje);
            }
            else
            {

                System.IO.File.WriteAllBytes(pathXML, respuestaCFDI.Documento);
            }
        }

        public async static Task<string> CreaFactura(string folio, string formaPago, string metodoDePago, List<ProductoSat> productos, float subtotal, string rfc, string rz, string usoCFDI, string mail, float iva, float total)
        {
            string pathCer = Directory.GetCurrentDirectory() + @"/Fiel/Certifiado.cer";
            string pathKey = Directory.GetCurrentDirectory() + @"/Fiel/Key.key";
            string clavePrivada = "Jorge1995";

            //Obtenemos el numero
            string numeroCertificado, aa, b, c;
            SelloDigital.leerCER(pathCer, out aa, out b, out c, out numeroCertificado);


            //Llenamos la clase COMPROBANTE--------------------------------------------------------
            string subt = subtotal.ToString();
            string impuetosImporte = iva.ToString();
            float t = total;
            string ts = t.ToString();
            Comprobante oComprobante = new Comprobante();
            oComprobante.Version = "3.3";
            oComprobante.Serie = "H";
            oComprobante.Folio = folio;
            oComprobante.Fecha = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
            //oComprobante.Fecha = "2019-06-11T10:52:20";
            //oComprobante.Sello = "faltante"; //sig video
            oComprobante.FormaPago = formaPago;
            oComprobante.NoCertificado = numeroCertificado;
            // oComprobante.Certificado = ""; //sig video
            oComprobante.SubTotal = decimal.Parse(subt);
            oComprobante.Moneda = "MXN";
            oComprobante.Total = decimal.Parse(ts);
            oComprobante.TipoDeComprobante = "I";
            oComprobante.MetodoPago = metodoDePago;
            oComprobante.LugarExpedicion = "44860";



            ComprobanteEmisor oEmisor = new ComprobanteEmisor();

            oEmisor.Rfc = "AATJ9502061EA";
            oEmisor.Nombre = "Jorge Humberto Alvarado Tostado";
            oEmisor.RegimenFiscal = "612";

            ComprobanteReceptor oReceptor = new ComprobanteReceptor();
            oReceptor.Nombre = rz;
            oReceptor.Rfc = rfc;
            oReceptor.UsoCFDI = usoCFDI;

            //asigno emisor y receptor
            oComprobante.Emisor = oEmisor;
            oComprobante.Receptor = oReceptor;


            List<ComprobanteConcepto> lstConceptos = new List<ComprobanteConcepto>();
            ComprobanteConcepto oConcepto;
            ComprobanteConceptoImpuestos impuestos;
            ComprobanteConceptoImpuestosTraslado imAux;
            ComprobanteConceptoImpuestosTraslado[] impuestosTrasladados;
            decimal impAux = 0;

            foreach (ProductoSat a in productos)
            {
                oConcepto = new ComprobanteConcepto();
                impuestos = new ComprobanteConceptoImpuestos();
                imAux = new ComprobanteConceptoImpuestosTraslado();
                impuestosTrasladados = new ComprobanteConceptoImpuestosTraslado[1];
                oConcepto.Importe = Math.Round(decimal.Parse((a.Subtotal).ToString()), 3);
                oConcepto.ClaveProdServ = a.CodigoSAT;
                oConcepto.Cantidad = decimal.Parse(a.Cantidad.ToString());
                oConcepto.ClaveUnidad = "H87";
                oConcepto.Descripcion = a.Descripcion;
                oConcepto.ValorUnitario = decimal.Parse((a.Precio).ToString());
                //Impuestos
                imAux.Base = decimal.Parse(a.Subtotal.ToString());
                imAux.ImporteSpecified = true;
                imAux.TasaOCuotaSpecified = true;
                imAux.TipoFactor = "Tasa";
                imAux.Importe = Math.Round(decimal.Parse((a.Subtotal * 0.16).ToString()), 2);
                impAux += imAux.Importe;
                imAux.TasaOCuota = decimal.Parse("0.160000");
                imAux.Impuesto = "002";
                impuestosTrasladados[0] = imAux;
                impuestos.Traslados = impuestosTrasladados;
                oConcepto.Impuestos = impuestos;
                lstConceptos.Add(oConcepto);
            }
            oComprobante.Conceptos = lstConceptos.ToArray();

            ComprobanteImpuestos imComprobante = new ComprobanteImpuestos();
            ComprobanteImpuestosTraslado imComprobanteTraladados = new ComprobanteImpuestosTraslado();
            ComprobanteImpuestosTraslado[] imComprobanteTraladadosArray = new ComprobanteImpuestosTraslado[1];

            imComprobanteTraladados.TipoFactor = "Tasa";
            imComprobanteTraladados.TasaOCuota = decimal.Parse("0.160000");
            imComprobanteTraladados.Impuesto = "002";
            imComprobanteTraladados.Importe = Math.Round(impAux, 2);
            oComprobante.Total = oComprobante.SubTotal + imComprobanteTraladados.Importe;

            imComprobanteTraladadosArray[0] = imComprobanteTraladados;

            imComprobante.Traslados = imComprobanteTraladadosArray;

            imComprobante.TotalImpuestosTrasladadosSpecified = true;
            imComprobante.TotalImpuestosTrasladados = Math.Round(impAux, 2);
            oComprobante.Impuestos = imComprobante;



            //Creamos el xml
            CreateXML(oComprobante);

            string cadenaOriginal = "";
            string pathxsl = Directory.GetCurrentDirectory() + @"/Fiel/cadenaoriginal_3_3.xslt";
            System.Xml.Xsl.XslCompiledTransform transformador = new System.Xml.Xsl.XslCompiledTransform(true);
            transformador.Load(pathxsl);

            using (StringWriter sw = new StringWriter())
            using (XmlWriter xwo = XmlWriter.Create(sw, transformador.OutputSettings))
            {

                transformador.Transform(pathXML, xwo);
                cadenaOriginal = sw.ToString();
            }


            SelloDigital oSelloDigital = new SelloDigital();
            oComprobante.Certificado = oSelloDigital.Certificado(pathCer);
            oComprobante.Sello = oSelloDigital.Sellar(cadenaOriginal, pathKey, clavePrivada);

            CreateXML(oComprobante);

            ////TIMBRE DEL XML
            string errorMessage="";
            ServiceReference1.RespuestaCFDi respuestaCFDI = new ServiceReference1.RespuestaCFDi();

            byte[] bXML = System.IO.File.ReadAllBytes(pathXML);

            ServiceReference1.TimbradoClient oTimbrado = new ServiceReference1.TimbradoClient();
            var ob=oTimbrado.State;
            respuestaCFDI = await oTimbrado.TimbrarAsync("AATJ9502061EA", "827984aaddd4126c9c67", bXML);

            if (respuestaCFDI.Documento == null)
            {
                return respuestaCFDI.Mensaje;
            }
            else
            {
                System.IO.File.WriteAllBytes(pathXML, respuestaCFDI.Documento);
                ServiceReference1.TimbradoClient pdf = new ServiceReference1.TimbradoClient();
                bXML = System.IO.File.ReadAllBytes(pathXML);
                respuestaCFDI = await pdf.PDFAsync("AATJ9502061EA", "827984aaddd4126c9c67", bXML, null);
                System.IO.File.WriteAllBytes(pathPDF, respuestaCFDI.Documento);
                SaveInvoice si = new SaveInvoice();
                si.folio = folio;
                si.xml = Convert.ToBase64String(bXML, 0, bXML.Length);
                si.rfc = rfc;
                si.razonSocial = rz;
                Response r= await WebService.InsertData(si, URLData.invoice_save);
                if (!r.succes)
                    errorMessage = $"Factura Creada correctamente {Environment.NewLine}Error al insertar en la BD: {r.message}{Environment.NewLine}";
                string pXMl = @path + "\\" + folio + ".xml";
                string pPDF = @path + "\\" + folio + ".pdf";
                File.Move(pathXML, pXMl);
                File.Move(pathPDF, pPDF);
                try
                {
                    errorMessage = await Task.Run(() => Email(new string[] { mail }, pXMl, pPDF));
                }
                catch (Exception e)
                {
                    errorMessage += $"Error al enviar correo electronico: {e.Message}{Environment.NewLine}";
                }

                File.Delete(pXMl);
                File.Delete(pPDF);
            }
            return errorMessage;
        }

        private static void CreateXML(Comprobante oComprobante)
        {
            //SERIALIZAMOS.-------------------------------------------------

            XmlSerializerNamespaces xmlNameSpace = new XmlSerializerNamespaces();
            xmlNameSpace.Add("cfdi", "http://www.sat.gob.mx/cfd/3");
            xmlNameSpace.Add("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
            xmlNameSpace.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");


            XmlSerializer oXmlSerializar = new XmlSerializer(typeof(Comprobante));

            string sXml = "";

            using (var sww = new StringWriterWithEncoding(Encoding.UTF8))
            {

                using (XmlWriter writter = XmlWriter.Create(sww))
                {

                    oXmlSerializar.Serialize(writter, oComprobante, xmlNameSpace);
                    sXml = sww.ToString();
                }

            }

            //guardamos el string en un archivo
            System.IO.File.WriteAllText(pathXML, sXml);
        }

        public static string Email(string[] emails, string pXMl, string pPDF)
        {
            MailMessage mail = new MailMessage();
            mail.Subject = "Facturacion Tostatronic";
            mail.Body = "Factura Creada por Tostatronic Software Desing";
            mail.From = new MailAddress("jorge.alvarado@tostatronic.com");
            mail.IsBodyHtml = true;
            mail.Attachments.Add(new Attachment(pXMl));
            mail.Attachments.Add(new Attachment(pPDF));
            for(int i=0; i<emails.Length;i++)
                mail.To.Add(new MailAddress(emails[i]));
            MimeKit.MimeMessage mimeMessage = MimeKit.MimeMessage.CreateFromMailMessage(mail);

            Message message = new Message();
            message.Raw = Base64UrlEncode(mimeMessage.ToString());
            //Gmail API credentials
            UserCredential credential;
            string[] Scope = { GmailService.Scope.GmailSend };
            using (var stream =
                new FileStream("MailResources/client_id.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = System.Environment.GetFolderPath(
                    System.Environment.SpecialFolder.Personal);
                credPath = Path.Combine(credPath, ".credentials/gmail-dotnet-quickstart2.json");

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scope,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }

            // Create Gmail API service.
            var service = new GmailService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "Facturacion Tostatronic",
            });
            //Send Email
            var result = service.Users.Messages.Send(message, "jorge.alvarado@tostatronic.com").Execute();
            mail.Dispose();
            return "";
        }
        private static string Base64UrlEncode(string input)
        {
            var inputBytes = System.Text.Encoding.UTF8.GetBytes(input);
            return Convert.ToBase64String(inputBytes)
              .Replace('+', '-')
              .Replace('/', '_')
              .Replace("=", "");
        }

        public async static Task<bool> GenerateInvoiceFiles(byte[] cXml, string folio)
        {
            try
            {
                ServiceReference1.RespuestaCFDi respuestaCFDI = new ServiceReference1.RespuestaCFDi();
                ServiceReference1.TimbradoClient pdf = new ServiceReference1.TimbradoClient();
                respuestaCFDI = await pdf.PDFAsync("AATJ9502061EA", "827984aaddd4126c9c67", cXml, null);
                System.IO.File.WriteAllBytes(pathPDF, respuestaCFDI.Documento);
                System.IO.File.WriteAllBytes(pathXML, cXml);
                string pXMl = @path + "\\" + folio + ".xml";
                string pPDF = @path + "\\" + folio + ".pdf";
                File.Move(pathXML, pXMl);
                File.Move(pathPDF, pPDF);
                return true;
            }
            catch(Exception e)
            {
                MessageBox.Show($"Error: {e.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return false;
        }
    }
}
