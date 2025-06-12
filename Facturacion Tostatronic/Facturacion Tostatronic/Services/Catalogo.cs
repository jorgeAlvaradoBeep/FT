using iTextSharp.text.pdf.qrcode;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;
using Facturacion_Tostatronic.Models.EF_Models.EFProduct;
using System.IO;

namespace Facturacion_Tostatronic.Services
{
    public static class Catalogo
    {
        public static void GenerateCatalog(List<DatosProductos> products, string outputPath, IProgress<int> progress = null)
        {
            string baseImagePath = "C:\\Users\\jorge\\OneDrive\\Documents\\MEGAsync\\Imagenes";
            const float margin = 40f;
            // Altura disponible menos márgenes y espacio para el footer (15 pts)
            float printableHeight = PageSize.LETTER.Height - 2 * margin - 15f;
            // Calculamos la altura fija de cada celda: la dividimos entre 2 filas
            float cellHeight = printableHeight / 2f;

            using (var fs = new FileStream(outputPath, FileMode.Create, FileAccess.Write, FileShare.None))
            using (var document = new Document(PageSize.LETTER, margin, margin, margin, margin))
            {
                PdfWriter writer = PdfWriter.GetInstance(document, fs);
                document.Open();

                // Fuente base
                BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                // Fuente para el footer
                Font footerFont = new Font(bf, 10);

                // Tabla 2 columnas, 100% ancho, bordes en todas las celdas
                PdfPTable table = new PdfPTable(2)
                {
                    WidthPercentage = 100,
                    SpacingBefore = 0,
                    SpacingAfter = 0
                };
                table.SetWidths(new float[] { 1f, 1f });
                table.DefaultCell.Border = Rectangle.BOX;

                int count = 0;
                foreach (var prod in products)
                {
                    // Celda fija
                    PdfPCell cell = new PdfPCell
                    {
                        Border = Rectangle.BOX,
                        Padding = 5f,
                        FixedHeight = cellHeight
                    };

                    // 1. Imagen
                    string imgPath = Path.Combine(baseImagePath, prod.Producto.imagen);
                    if (File.Exists(imgPath))
                    {
                        try
                        {
                            var img = iTextSharp.text.Image.GetInstance(imgPath);
                            img.ScaleToFit((PageSize.LETTER.Width - 2 * margin) / 2 - 10, cellHeight * 0.4f);
                            img.Alignment = Element.ALIGN_CENTER;
                            cell.AddElement(img);
                        }
                        catch { /* ignorar */ }
                    }

                    // 2. Nombre con ajuste de tamaño
                    string name = prod.Producto.nombre;
                    float nameFontSize = 12f;
                    if (name.Length > 40) nameFontSize = 10f;
                    if (name.Length > 80) nameFontSize = 8f;
                    var nameFont = new Font(bf, nameFontSize, Font.BOLD);

                    Paragraph p = new Paragraph();
                    p.Add(new Chunk($"Nombre: ", nameFont));
                    p.Add(new Chunk(name + "\n", nameFont));
                    p.Add(new Chunk($"Código: {prod.Codigo}\n", nameFont));

                    // Precios
                    var priceFont = new Font(bf, 11);
                    p.Add(new Chunk($"Precio Mayorista: {prod.Producto.precioDistribuidor:C}\n", priceFont));
                    p.Add(new Chunk($"Precio Distribuidor: {prod.Producto.precioMinimo:C}\n", priceFont));
                    cell.AddElement(p);

                    // 3. Código QR centrado
                    // 3. Código QR: 200×200
                    string link = string.IsNullOrEmpty(prod.Link)
                        ? "https://www.tostatronic.com"
                        : prod.Link;

                    var qr = new BarcodeQRCode(link, 1, 1, null);
                    var qrImg = qr.GetImage();
                    qrImg.ScaleAbsolute(100f, 100f);
                    qrImg.Alignment = Element.ALIGN_CENTER;
                    cell.AddElement(qrImg);

                    // 4. “Botón” clicable debajo del QR
                    // — Creamos una fuente blanca sobre fondo azul —
                    Font btnFont = new Font(bf, 8, Font.BOLD, BaseColor.WHITE);

                    // — Creamos un Anchor, que en iTextSharp es un texto que lleva un Reference (URL) —
                    Anchor button = new Anchor("Abrir enlace", btnFont)
                    {
                        Reference = link
                    };

                    // — Para que parezca un botón, le damos borde, fondo y padding —
                    PdfPCell btnWrapper = new PdfPCell(new Phrase(button))
                    {
                        Border = Rectangle.BOX,
                        BackgroundColor = new BaseColor(0, 121, 193), // azul corporativo
                        PaddingTop = 6f,
                        PaddingBottom = 6f,
                        PaddingLeft = 10f,
                        PaddingRight = 10f,
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        VerticalAlignment = Element.ALIGN_MIDDLE,
                        // Sin altura fija para que se ajuste al contenido
                        MinimumHeight = 0
                    };
                    // Lo centramos usando una mini-tabla de 1 columna
                    PdfPTable btnTable = new PdfPTable(1)
                    {
                        WidthPercentage = 50f,        // ocupa la mitad del ancho de la celda padre
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        SpacingBefore = 5f            // separador del QR
                    };
                    btnTable.AddCell(btnWrapper);
                    cell.AddElement(btnTable);

                    table.AddCell(cell);
                    count++;
                    // *** Reportamos el progreso ***
                    progress?.Report(count);

                    // Cada 4 productos, añadimos la tabla, el footer y página nueva
                    if (count % 4 == 0)
                    {
                        document.Add(table);

                        // Footer centrado al fondo
                        ColumnText.ShowTextAligned(
                            writer.DirectContent,
                            Element.ALIGN_CENTER,
                            new Phrase("www.tostatronic.com", footerFont),
                            document.PageSize.Width / 2,
                            margin / 2,
                            0
                        );

                        document.NewPage();
                        table.DeleteBodyRows();
                    }
                }

                // Si hay sobrantes (<4), rellenamos hasta completar 4 celdas
                if (count % 4 != 0)
                {
                    int missing = 4 - (count % 4);
                    for (int i = 0; i < missing; i++)
                    {
                        table.AddCell(new PdfPCell { Border = Rectangle.BOX, FixedHeight = cellHeight });
                    }
                    document.Add(table);
                    ColumnText.ShowTextAligned(
                        writer.DirectContent,
                        Element.ALIGN_CENTER,
                        new Phrase("www.tostatronic.com", footerFont),
                        document.PageSize.Width / 2,
                        margin / 2,
                        0
                    );
                }

                document.Close();
            }
        }
    }
}
