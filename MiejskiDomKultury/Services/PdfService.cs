using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using MiejskiDomKultury.Model;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;
using ZXing.Windows.Compatibility;

namespace MiejskiDomKultury.Services
{
    public class PdfService
    {
        public Attachment GenerateTickets(List<SeansBilet> bilety)
        {
            using (var document = new PdfDocument())
            {
                foreach (var bilet in bilety)
                {
                    var page = document.AddPage();
                    using (var gfx = XGraphics.FromPdfPage(page))
                    {
                       
                        var fontTitle = new XFont("Arial", 20, XFontStyleEx.Bold);
                        gfx.DrawString("Bilet do Kina Fidelio", fontTitle, XBrushes.Black,
                            new XRect(0, 40, page.Width, page.Height), XStringFormats.TopCenter);

                        
                        var fontRegular = new XFont("Arial", 14);
                        double yPosition = 100;

                        DrawTicketField(gfx, fontRegular, $"Film: {bilet.Seans.Film.Tytul}", ref yPosition);
                        DrawTicketField(gfx, fontRegular, $"Data seansu: {bilet.Seans.DataStart:dd.MM.yyyy HH:mm}", ref yPosition);
                        DrawTicketField(gfx, fontRegular, $"Miejsce: {bilet.SeatNumber}", ref yPosition);
                        DrawTicketField(gfx, fontRegular, $"Cena: {bilet.Cena} PLN", ref yPosition);
                        DrawTicketField(gfx, fontRegular, $"Data zakupu: {bilet.Date:dd.MM.yyyy HH:mm}", ref yPosition);
                        DrawTicketField(gfx, fontRegular, $"Numer biletu: {bilet.Id}", ref yPosition);

                        yPosition += 40;
                        DrawBarcode(gfx, bilet.Id.ToString(), ref yPosition);
                        yPosition += 20;
                        DrawQrCode(gfx, bilet.Id.ToString(), ref yPosition);
                    }
                }
                
                string fileName = $"Bilety_{DateTime.Now:yyyyMMddHHmmss}.pdf";

                document.Save(fileName);
                return new Attachment(fileName);
            }
        }

        private void DrawBarcode(XGraphics gfx, string code, ref double yPosition)
        {
            var writer = new BarcodeWriter
            {
                Format = BarcodeFormat.CODE_128,
                Options = new EncodingOptions
                {
                    Height = 60,
                    Width = 300,
                    Margin = 2
                }
            };

            using (var bitmap = writer.Write(code))
            using (var stream = new MemoryStream())
            {
                bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                var image = XImage.FromStream(stream);
                gfx.DrawImage(image, 50, yPosition, 300, 60);
            }

            yPosition += 70;
        }

        private void DrawQrCode(XGraphics gfx, string code, ref double yPosition)
        {
            var writer = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new QrCodeEncodingOptions
                {
                    Width = 150,
                    Height = 150,
                    Margin = 1,
                    ErrorCorrection = ZXing.QrCode.Internal.ErrorCorrectionLevel.M
                }
            };

            using (var bitmap = writer.Write(code))
            using (var stream = new MemoryStream())
            {
                bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                var image = XImage.FromStream(stream);
                gfx.DrawImage(image, 50, yPosition, 150, 150);
            }

            yPosition += 160;
        }

        private void DrawTicketField(XGraphics gfx, XFont font, string text, ref double yPosition)
        {
            gfx.DrawString(text, font, XBrushes.Black,
                new XRect(50, yPosition, 500, 20), XStringFormats.TopLeft);
            yPosition += 30;
        }
    }
}