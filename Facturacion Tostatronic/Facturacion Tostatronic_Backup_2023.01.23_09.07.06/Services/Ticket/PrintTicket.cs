using Facturacion_Tostatronic.Models.Products;
using Facturacion_Tostatronic.Models.Sales;
using RawPrint;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Facturacion_Tostatronic.Services.Ticket
{
    public static class PrintTicket
    {
        public static bool ImprimeTicket(CompleteSaleM sale)
        {
            try
            {
                double varEFECTIVO = 0;
                double varCAMBIO = 0;
                double varTOTAL = 0;
                double varIVA = 0;
                NewTicket ticket = new NewTicket();
                //Ticket ticket = new Ticket();
                //ticket.Path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                //ticket.Path = Path.Combine(ticket.Path, "Tostatronic");
                ticket.MaxChar = 40;
                ticket.MaxCharDescription = 15;
                //if (!Directory.Exists(ticket.Path))
                //    Directory.CreateDirectory(ticket.Path);
                //ticket.FileName = $"\\{sale.IDSale}.pdf";
                string imagePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"MEGAsync\Imagenes Nuevas Pagina\NI\Ticket.png");
                if (File.Exists(imagePath))
                    ticket.HeaderImage=Image.FromFile(imagePath);
                //ticket.HeaderImage = imagePath;
                ticket.AddHeaderLine(GetCenterText("TOSTATRONIC"));
                ticket.AddHeaderLine(GetCenterText("Venta de componentes electronicos"));
                ticket.AddSubHeaderLine("\n");
                ticket.AddSubHeaderLine("Folio: " + sale.IDSale);
                ticket.AddSubHeaderLine("Le atendió: Jorge alvarado");
                ticket.AddSubHeaderLine("Fecha y Hora: " +
                    DateTime.Now);
                ticket.AddSubHeaderLine("Cliente: " +
                    sale.ClientSale.Name);
                varEFECTIVO = Convert.ToDouble(sale.Payment.Payment);
                foreach ( ProductSaleSaled a in sale.SaledProducts)
                {
                    //ticket.AddItem(a.SaledQuantity.ToString(), a.Name, a.DisplayPrice.ToString(), a.Subtotal.ToString());
                    ticket.AddItem(a.SaledQuantity.ToString(), a.Name,a.DisplayPrice.ToString("$0.00"), a.Subtotal.ToString("$0.00"));
                }
                varTOTAL = Convert.ToDouble(sale.SubTotal);
                varIVA =
                    Convert.ToDouble(sale.IVA);
                varCAMBIO = sale.Payment.Change;

                //El metodo AddTotal requiere 2 parametros, 
                //la descripcion del total, y el precio 
                ticket.AddTotal("SUBTOTAL", varTOTAL.ToString("$0.00"));
                ticket.AddTotal("IVA", varIVA.ToString("$0.00"));
                ticket.AddTotal("TOTAL", (varTOTAL + varIVA).ToString("$0.00"));
                ticket.AddTotal("", "");//Ponemos un total 
                //en blanco que sirve de espacio 
                ticket.AddTotal("RECIBIDO", varEFECTIVO.ToString("$0.00"));
                if (sale.Payment.Remaining > 0)
                    ticket.AddTotal("Restante: ", sale.Payment.Remaining.ToString("$0.00"));
                else
                    ticket.AddTotal("Cambio: ", varCAMBIO.ToString("$0.00"));
                ticket.AddTotal("", "");//Ponemos un total 
                //en blanco que sirve de espacio 
                //El metodo AddFooterLine funciona igual que la cabecera 
                ticket.AddFooterLine(GetCenterText("Gracias por su preferencia"));
                ticket.AddFooterLine(GetCenterText("Tostatronic le desea un buen dia"));
                //Generamos
                try
                {
                    ticket.PrintTicket(ConfigurationManager.AppSettings.Get("PrinterName"));
                    //ticket.PrintTi();
                    return true;
                }
                catch(Exception e)
                {
                    MessageBox.Show("Error al imprimir: " + e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
            catch (Exception e) 
            {
                MessageBox.Show("Error Creado Ticket: " + e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false; }
        }

        static string GetCenterText(string text)
        {
            int spaces = 40 - text.Length;
            int padLeft = spaces / 2 + text.Length;
            return text.PadLeft(padLeft).PadRight(40, ' ');
        }
        
    }
}
