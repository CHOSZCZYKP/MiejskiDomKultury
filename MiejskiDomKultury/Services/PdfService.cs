using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace MiejskiDomKultury.Services
{
    class PdfService
    {

        public void GenerateSimplePdf()
        {
            PdfDocument document = new PdfDocument();
            document.Info.Title = "Przykładowy PDF";

            // Dodajemy stronę
            PdfPage page = document.AddPage();

            // Tworzymy obiekt do rysowania
            XGraphics gfx = XGraphics.FromPdfPage(page);

            // Ustawiamy czcionkę
            XFont font = new XFont("Verdana", 20);

            // Rysujemy tekst
            gfx.DrawString("Witaj, świecie PDF!", font, XBrushes.Black,
                new XRect(0, 0, page.Width, page.Height),
                XStringFormats.Center);

            // Zapisujemy do pliku
            string filename = "przykladowy.pdf";
            document.Save(filename);
        }
    }
}
