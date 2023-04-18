using Facturacion_Tostatronic.Models.Clients;
using Facturacion_Tostatronic.Models.Products;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Facturacion_Tostatronic.Models;
using System.Windows;
using System.Globalization;
using System.Diagnostics;
using Facturacion_Tostatronic.Models.EF_Models.EFClientF;

namespace Facturacion_Tostatronic.Services
{
    public class PDFService
    {
        public static MemoryStream CreatePDF(string folio, string fecha, List<Product> productos, EFClient cliente, decimal impuesto, string tipo)
        {
            // Create a Document object
            string tv;
            tv = tipo;

            Document document = new Document(PageSize.LETTER, 30, 30, 30, 30);

            //MemoryStream
            MemoryStream PDFData = new MemoryStream();
            string fullPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            fullPath = System.IO.Path.Combine(fullPath, "Tostatronic");
            if(!File.Exists(fullPath))
            {
                try
                {
                    Directory.CreateDirectory(fullPath);
                }
                catch 
                {
                    MessageBox.Show("Error al crear directorio destino", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }
            }
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(fullPath + $"\\{folio}.pdf", FileMode.Create));

            // First, create our fonts
            var titleFont = FontFactory.GetFont("Arial", 14, Font.BOLD);
            var titleFontNegro = FontFactory.GetFont("Arial", 14, Font.BOLD, BaseColor.WHITE);
            var boldTableFont = FontFactory.GetFont("Arial", 10, Font.BOLD);
            var bodyFont = FontFactory.GetFont("Arial", 10, Font.NORMAL);
            Rectangle pageSize = writer.PageSize;

            // Open the Document for writing
            document.Open();
            //Add elements to the document here

            PdfPTable sp = new PdfPTable(1);
            sp.HorizontalAlignment = 2;
            sp.WidthPercentage = 200;
            sp.SetWidths(new float[] { 20 });  // then set the column's __relative__ widths
            sp.DefaultCell.Border = Rectangle.NO_BORDER;
            sp.SpacingAfter = 20;
            PdfPCell space = new PdfPCell(new Phrase(" ", titleFontNegro));
            space.HorizontalAlignment = 1;
            space.Border = Rectangle.NO_BORDER;
            space.BackgroundColor = BaseColor.WHITE;
            sp.AddCell(space);
            document.Add(sp);

            // Create the header table 

            iTextSharp.text.Image logo;
            //Agregando la imagen
            if (File.Exists(Directory.GetCurrentDirectory() + "\\PDFResourse\\ticket.png"))
                logo = iTextSharp.text.Image.GetInstance(Directory.GetCurrentDirectory() + "\\PDFResourse\\ticket.png");
            else
            {
                MessageBox.Show("Error al obtener el logo del PDF", "Error",MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }

            logo.SetAbsolutePosition(0, 0);
            logo.SetAbsolutePosition(80, document.PageSize.Height - 100);
            logo.ScaleToFit(logo.Width / 2, 200);
            document.Add(logo);

            PdfPTable title = new PdfPTable(2);
            title.HorizontalAlignment = 2;
            title.WidthPercentage = 40;
            title.SetWidths(new float[] { 20, 20 });  // then set the column's __relative__ widths
            title.DefaultCell.Border = Rectangle.NO_BORDER;
            title.SpacingAfter = 20;
            PdfPCell tileName = new PdfPCell(new Phrase(tv, titleFontNegro));
            tileName.HorizontalAlignment = 1;
            tileName.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER; ; ;
            tileName.BackgroundColor = BaseColor.BLACK;
            title.AddCell(tileName);
            tileName = new PdfPCell(new Phrase("Fecha", titleFontNegro));
            tileName.HorizontalAlignment = 1;
            tileName.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER; ;
            tileName.BackgroundColor = BaseColor.BLACK;
            title.AddCell(tileName);
            //Adicion del numero de venta y/o cotizacion
            tileName = new PdfPCell(new Phrase(folio, bodyFont));
            tileName.HorizontalAlignment = 1;
            tileName.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER; ;
            title.AddCell(tileName);
            tileName = new PdfPCell(new Phrase(fecha, bodyFont));
            tileName.HorizontalAlignment = 1;
            tileName.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER; ;
            title.AddCell(tileName);
            document.Add(title);
            //Añadimos a quien va dirigida la nota e la informacion de la empresa
            PdfPTable clienteYEmpresa = new PdfPTable(2);
            clienteYEmpresa.HorizontalAlignment = 0;
            clienteYEmpresa.WidthPercentage = 100;
            clienteYEmpresa.SetWidths(new float[] { 50, 50 });  // then set the column's __relative__ widths
            clienteYEmpresa.DefaultCell.Border = Rectangle.NO_BORDER;
            clienteYEmpresa.SpacingAfter = 10;

            string clientData = cliente.CompleteName + "\n" +
                   "Dirección: " + cliente.domicilio + "\n" +
                   "CP: " + cliente.codigoPostal + "\n" +
                   "R.F.C.: " + cliente.rfc + "\n" +
                   "Correo: " + cliente.correoElectronico+ "\n"+
                   "Tel: " + cliente.celular;
            PdfPCell client = new PdfPCell(new Phrase("Cliente", titleFontNegro));
            client.HorizontalAlignment = 1;
            client.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER; ; ;
            client.BackgroundColor = BaseColor.BLACK;
            clienteYEmpresa.AddCell(client);

            PdfPCell empresa = new PdfPCell(new Phrase("Datos empresa", titleFontNegro));
            empresa.HorizontalAlignment = 1;
            empresa.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER; ;
            empresa.BackgroundColor = BaseColor.BLACK;
            clienteYEmpresa.AddCell(empresa);

            client = new PdfPCell(new Phrase(clientData, bodyFont));
            client.HorizontalAlignment = 1;
            client.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER; ; ;
            client.BackgroundColor = BaseColor.WHITE;
            clienteYEmpresa.AddCell(client);

            string empresaData = "Tienda: Tostatronic\n" +
                               "R.F.C.: AATJ9502061EA" + "\n" +
                               "Direccion: Mineros 2550, Col. La Paz" +
                               $"{Environment.NewLine}CP: 44860" +
                               $"{Environment.NewLine}Guadalajara, Jalisco" + "\n" +
                               "Telefono: 3314575853"+ "\n";
            empresa = new PdfPCell(new Phrase(empresaData, bodyFont));
            empresa.HorizontalAlignment = 1;
            empresa.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER; ;
            empresa.BackgroundColor = BaseColor.WHITE;
            clienteYEmpresa.AddCell(empresa);
            document.Add(clienteYEmpresa);

            //Create body table
            PdfPTable itemTable = new PdfPTable(5);
            itemTable.HorizontalAlignment = 0;
            itemTable.WidthPercentage = 100;
            itemTable.SetWidths(new float[] { 40, 80, 20, 40, 40 });  // then set the column's __relative__ widths
            itemTable.SpacingAfter = 40;
            itemTable.DefaultCell.Border = Rectangle.BOX;
            PdfPCell cell1 = new PdfPCell(new Phrase("Codigo", boldTableFont));
            cell1.HorizontalAlignment = 1;
            itemTable.AddCell(cell1);
            PdfPCell cell2 = new PdfPCell(new Phrase("Nombre", boldTableFont));
            cell2.HorizontalAlignment = 1;
            itemTable.AddCell(cell2);
            PdfPCell cell3 = new PdfPCell(new Phrase("Cantidad", boldTableFont));
            cell3.HorizontalAlignment = 1;
            itemTable.AddCell(cell3);
            PdfPCell cell4 = new PdfPCell(new Phrase("Precio Unitario", boldTableFont));
            cell4.HorizontalAlignment = 1;
            itemTable.AddCell(cell4);
            PdfPCell cell5 = new PdfPCell(new Phrase("Sub Total", boldTableFont));
            cell5.HorizontalAlignment = 1;
            itemTable.AddCell(cell5);

            decimal total = 0;
            int cont = 1;
            foreach (Product producto in productos)
            {
                PdfPCell numberCell = new PdfPCell(new Phrase(producto.idProduct, bodyFont));
                numberCell.HorizontalAlignment = 0;
                numberCell.PaddingLeft = 10f;
                numberCell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                if (cont % 2 == 0)
                    numberCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                itemTable.AddCell(numberCell);

                PdfPCell descCell = new PdfPCell(new Phrase(producto.name, bodyFont));
                descCell.HorizontalAlignment = 0;
                descCell.PaddingLeft = 10f;
                descCell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                if (cont % 2 == 0)
                    descCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                itemTable.AddCell(descCell);

                PdfPCell qtyCell = new PdfPCell(new Phrase(producto.quantity, bodyFont));
                qtyCell.HorizontalAlignment = 1;
                qtyCell.PaddingLeft = 10f;
                qtyCell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                if (cont % 2 == 0)
                    qtyCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                itemTable.AddCell(qtyCell);

                decimal auxValue;
                Decimal.TryParse(producto.priceAtMoment, out auxValue);
                PdfPCell puCell = new PdfPCell(new Phrase(auxValue.ToString("C",CultureInfo.CurrentCulture), bodyFont));
                puCell.HorizontalAlignment = 1;
                puCell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                if (cont % 2 == 0)
                    puCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                itemTable.AddCell(puCell);

                Decimal.TryParse(producto.SubTotal, out auxValue);
                PdfPCell amtCell = new PdfPCell(new Phrase(auxValue.ToString("$0.00"), bodyFont));
                amtCell.HorizontalAlignment = 1;
                amtCell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                if (cont % 2 == 0)
                    amtCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                itemTable.AddCell(amtCell);
                total += auxValue;
                cont++;

            }
            // Table footer
            PdfPCell totalAmtCell1 = new PdfPCell(new Phrase(""));
            totalAmtCell1.Border = Rectangle.TOP_BORDER;
            itemTable.AddCell(totalAmtCell1);
            PdfPCell totalAmtCell2 = new PdfPCell(new Phrase(""));
            totalAmtCell2.Border = Rectangle.TOP_BORDER; //Rectangle.NO_BORDER; //Rectangle.TOP_BORDER;
            itemTable.AddCell(totalAmtCell2);
            PdfPCell totalAmtCell3 = new PdfPCell(new Phrase(""));
            totalAmtCell3.Border = Rectangle.TOP_BORDER; //Rectangle.NO_BORDER; //Rectangle.TOP_BORDER;
            itemTable.AddCell(totalAmtCell3);
            PdfPCell totalAmtStrCell = new PdfPCell(new Phrase("Sub Total", boldTableFont));
            totalAmtStrCell.Border = Rectangle.LEFT_BORDER | Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER;   //Rectangle.NO_BORDER; //Rectangle.TOP_BORDER;
            totalAmtStrCell.HorizontalAlignment = 1;
            itemTable.AddCell(totalAmtStrCell);
            PdfPCell totalAmtCell = new PdfPCell(new Phrase(total.ToString("$0.00"), boldTableFont));
            totalAmtStrCell.Border = Rectangle.LEFT_BORDER | Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER;
            totalAmtCell.HorizontalAlignment = 1;
            itemTable.AddCell(totalAmtCell);
            PdfPCell espace = new PdfPCell(new Phrase(""));
            espace.Border = Rectangle.NO_BORDER;
            itemTable.AddCell(espace);
            espace = new PdfPCell(new Phrase(""));
            espace.Border = Rectangle.NO_BORDER;
            itemTable.AddCell(espace);
            espace = new PdfPCell(new Phrase(""));
            espace.Border = Rectangle.NO_BORDER;
            itemTable.AddCell(espace);
            PdfPCell iva = new PdfPCell(new Phrase("IVA", boldTableFont));
            iva.Border = Rectangle.LEFT_BORDER | Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER;   //Rectangle.NO_BORDER; //Rectangle.TOP_BORDER;
            iva.HorizontalAlignment = 1;
            itemTable.AddCell(iva);
            PdfPCell ivaV = new PdfPCell(new Phrase((total * (impuesto - 1)).ToString("$0.00"), boldTableFont));
            ivaV.Border = Rectangle.LEFT_BORDER | Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER;
            ivaV.HorizontalAlignment = 1;
            itemTable.AddCell(ivaV);
            espace = new PdfPCell(new Phrase(""));
            espace.Border = Rectangle.NO_BORDER;
            itemTable.AddCell(espace);
            espace = new PdfPCell(new Phrase(""));
            espace.Border = Rectangle.NO_BORDER;
            itemTable.AddCell(espace);
            espace = new PdfPCell(new Phrase(""));
            espace.Border = Rectangle.NO_BORDER;
            itemTable.AddCell(espace);
            PdfPCell totalV = new PdfPCell(new Phrase("Total", boldTableFont));
            totalV.Border = Rectangle.LEFT_BORDER | Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER;   //Rectangle.NO_BORDER; //Rectangle.TOP_BORDER;
            totalV.HorizontalAlignment = 1;
            itemTable.AddCell(totalV);
            PdfPCell totalVV = new PdfPCell(new Phrase((total * impuesto).ToString("$0.00"), boldTableFont));
            totalVV.Border = Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
            totalVV.HorizontalAlignment = 1;
            itemTable.AddCell(totalVV);

            PdfPCell cell = new PdfPCell(new Phrase("***Garantia d 1 mes despues de efectuada la venta***", bodyFont));
            cell.Colspan = 4;
            cell.HorizontalAlignment = 1;
            itemTable.AddCell(cell);
            document.Add(itemTable);



            //Approved by
            PdfContentByte cb = new PdfContentByte(writer);
            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, true);

            cb = new PdfContentByte(writer);
            bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, true);
            cb = writer.DirectContent;
            cb.BeginText();
            cb.SetFontAndSize(bf, 10);
            cb.SetTextMatrix(pageSize.GetLeft(70), 80);
            cb.ShowText("Tejedores #680, Col. La paz, Guadalajara, Jalisco");
            cb.EndText();
            document.Close();
            return PDFData;
        }
        public static void Ver(string ruta)
        {
            try
            {
                //Process.Start(ruta);
                Process proceso = Process.Start(ruta);
                proceso.WaitForInputIdle();
            }
            catch (Exception)
            { }
        }
    }
}
