using ESCPOS_NET;
using ESCPOS_NET.Emitters;
using ESCPOS_NET.Utilities;
using Facturacion_Tostatronic.Models;
using Facturacion_Tostatronic.Models.Products;
using Facturacion_Tostatronic.Models.Sales;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion_Tostatronic.Services.Ticket
{
    public static class TermalTicket
    {
        
        public static string PrintTicket(CompleteSaleM cs)
        {
            var printer = new SerialPrinter("USB1", 9600);
            var e = new EPSON();
            string imagePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"MEGAsync\Imagenes Nuevas Pagina\NI\TickerLogo.png");
            List<byte[]> fileTP = new List<byte[]>();
            fileTP.Add(ByteSplicer.Combine(
                e.CenterAlign(),
                e.PrintImage(File.ReadAllBytes(imagePath), true),
                e.PrintLine(""),
                e.PrintLine(printLine(80)),
                e.LeftAlign(),
                e.PrintLine($"Folio: {cs.IDSale}"),
                e.PrintLine(printLine(80)),
                e.SetStyles(PrintStyle.FontB),
                e.PrintLine("CANT      DESCRIPCION                                     PRECIO      IMPORTE "),
                e.PrintLine(printLine(80)),
                e.PrintLine("")
              ));
            foreach(ProductSaleSaled item in cs.SaledProducts)
            {
                fileTP.Add(e.PrintLine(printItem(item)));
            }
            fileTP.Add(ByteSplicer.Combine(
                e.PrintLine(printLine(80)),
                e.RightAlign(),
                e.PrintLine($"SUBTOTAL         {cs.SubTotal}"),
                e.PrintLine($"IVA              {cs.IVA}"),
                e.PrintLine($"Total            {cs.Total}"),
                e.PrintLine(""),
                e.PrintLine($"Pago con:        {cs.Payment.Payment}"),
                e.PrintLine($"Cambio:          {cs.Payment.Change}"),
                e.PrintLine(printLine(80)),
                e.CenterAlign(),
                e.SetStyles(PrintStyle.Bold | PrintStyle.FontB),
                e.PrintLine("Tostatronic"),
                e.PrintLine("Tejedores 680, Col. La paz"),
                e.PrintLine("Guadalajara, Jalisco, CP 44860"),
                e.PrintLine("33-1457-5853"),
                e.SetStyles(PrintStyle.Underline),
                e.PrintLine("www.Tostatronic.com"),
                e.PrintLine(""),
                e.PrintLine("")
              ));
            printer.Write(fileTP.ToArray());
            return "";
        }
        static string printLine(int numOfLines)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < numOfLines; i++)
            {
                sb.Append("-");
            }
            return sb.ToString();
        }
        static string printItem(ProductSaleSaled item)
        {
            StringBuilder sb = new StringBuilder();
            int whiteSpaces = 10-item.SaledQuantity.ToString().Length;
            sb.Append(item.SaledQuantity.ToString());
            for (int i = 0; i < whiteSpaces; i++)
            {
                sb.Append(" ");
            }
            if(item.Name.Length>46)
                item.Name = item.Name.Substring(0, 46);
            whiteSpaces = 48 - item.Name.Length;
            sb.Append(item.Name);
            for (int i = 0; i < whiteSpaces; i++)
            {
                sb.Append(" ");
            }
            whiteSpaces = 11 - item.DisplayPrice.ToString().Length;
            sb.Append(item.DisplayPrice.ToString());
            for (int i = 0; i < whiteSpaces; i++)
            {
                sb.Append(" ");
            }
            whiteSpaces = 11 - item.Subtotal.ToString().Length;
            sb.Append(item.Subtotal.ToString());
            for (int i = 0; i < whiteSpaces; i++)
            {
                sb.Append(" ");
            }
            return sb.ToString();
        }
    }
}
