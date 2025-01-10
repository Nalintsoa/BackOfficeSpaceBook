using Frontoffice.Models;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace Frontoffice.Helpers
{
    public static class PdfHelper
    {
        public static byte[] GenerateInvoice(string companyName, string customerName, string customerAddress, string invoiceNumber,
    DateTime invoiceDate, string productDescription, int quantity, double unitPrice, double taxRate, Booking booking)
        {
            using (var memoryStream = new MemoryStream())
            {
                PdfWriter writer = new PdfWriter(memoryStream);
                PdfDocument pdf = new PdfDocument(writer);
                Document document = new Document(pdf);

                // Ajouter une police pour le style "gras"
                PdfFont boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
                PdfFont regularFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);

                // Titre principal
                document.Add(new Paragraph(new Text($"FACTURE\n{companyName}")
                   .SetFont(boldFont)
                   .SetFontSize(14))
                   .SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT));
                document.Add(new Paragraph("anthonnychristian14@gmail.com\n+261 32 92 820 40")
                    .SetFont(regularFont)
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT));



                // Informations de facturation
                document.Add(new Paragraph("\nDestinataire :")
                    .SetFont(boldFont));
                document.Add(new Paragraph($"{customerName}\n{customerAddress}")
                    .SetFont(regularFont));

                // Détails de la facture
                document.Add(new Paragraph("\nDétails de la facture :")
                    .SetFont(boldFont));

                Table detailsTable = new Table(2);
                detailsTable.AddCell(new Cell().Add(new Paragraph("Date de la facture :").SetFont(boldFont)));
                detailsTable.AddCell(new Cell().Add(new Paragraph(invoiceDate.ToString("dd/MM/yyyy")).SetFont(regularFont)));
                detailsTable.AddCell(new Cell().Add(new Paragraph("Numéro de facture :").SetFont(boldFont)));
                detailsTable.AddCell(new Cell().Add(new Paragraph(invoiceNumber).SetFont(regularFont)));

                document.Add(detailsTable);

                document.Add(new Paragraph(new Text("\n")));

                // Ajouter un tableau des produits
                Table productTable = new Table(6);
                productTable.AddHeaderCell(new Cell().Add(new Paragraph("Description").SetFont(boldFont)));
                productTable.AddHeaderCell(new Cell().Add(new Paragraph("Début").SetFont(boldFont)));
                productTable.AddHeaderCell(new Cell().Add(new Paragraph("Fin").SetFont(boldFont)));
                productTable.AddHeaderCell(new Cell().Add(new Paragraph("Quantité").SetFont(boldFont)));
                productTable.AddHeaderCell(new Cell().Add(new Paragraph("Prix unitaire").SetFont(boldFont)));

                double amountHT = quantity * unitPrice;
                double taxAmount = amountHT * taxRate / 100;
                double totalAmount = amountHT + taxAmount;

                productTable.AddCell(new Cell().Add(new Paragraph(productDescription).SetFont(regularFont)));
                productTable.AddCell(new Cell().Add(new Paragraph(booking.BookingDate.ToString("dd/MM/yyyy")).SetFont(regularFont)));
                productTable.AddCell(new Cell().Add(new Paragraph(booking.BookingDate.ToString("dd/MM/yyyy")).SetFont(regularFont)));
                productTable.AddCell(new Cell().Add(new Paragraph(quantity.ToString()).SetFont(regularFont)));
                productTable.AddCell(new Cell().Add(new Paragraph($"{unitPrice.ToString("N1")} Ar").SetFont(regularFont)));

                document.Add(productTable);

                // Total général
                document.Add(new Paragraph("\nTotal :")
                    .SetFont(boldFont)
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT));
                document.Add(new Paragraph($"Montant HT : {amountHT.ToString("N1")} Ar")
                    .SetFont(regularFont)
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT));
                document.Add(new Paragraph($"TVA : {taxAmount.ToString("N1")} Ar")
                    .SetFont(regularFont)
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT));
                document.Add(new Paragraph($"Total TTC : {totalAmount.ToString("N1")} Ar")
                    .SetFont(boldFont)
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT));

                // Informations supplémentaires
                document.Add(new Paragraph("\nInfos supplémentaires :")
                    .SetFont(boldFont));
                document.Add(new Paragraph("Merci d'avoir choisi notre service. Pour toute question, contactez-nous.")
                    .SetFont(regularFont));

                document.Close();
                return memoryStream.ToArray();
            }
        }


    }
}
