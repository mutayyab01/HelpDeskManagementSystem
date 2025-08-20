using DinkToPdf;
using DinkToPdf.Contracts;
using HelpDeskSystem.Interfaces;

namespace HelpDeskSystem.Services
{
    public class PdfService : IPdfService
    {
        public readonly IConverter _converter;
        public PdfService(IConverter converter)
        {
            _converter = converter;
        }
        public byte[] ConvertPDF(string htmlcontent, Orientation? orientation = Orientation.Landscape, PaperKind? paperKind = PaperKind.A4)
        {
            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings =
                {
                    ColorMode=ColorMode.Color,
                    Orientation=orientation ?? Orientation.Landscape,
                    PageOffset=0,
                    PaperSize=paperKind??PaperKind.A4,
                    Margins=new MarginSettings()
                    {
                        Top=1.0,
                        Bottom=1.0,
                        Left=0.85,
                        Right=0.85,
                        Unit=Unit.Centimeters
                    }

                },
                Objects =
                {
                    new ObjectSettings
                    {
                        HtmlContent=htmlcontent,
                        PagesCount=true,
                        WebSettings={DefaultEncoding="utf-8"},
                        FooterSettings =
                        {
                            FontSize=7,
                            FontName="Bahnschrift",
                            Right="page [Page] of [toPage]",
                            Line=true,
                            Spacing=0.3
                        }
                    }
                }
            };
            byte[] pdf = _converter.Convert(doc);
            return pdf;

        }
    }
}
