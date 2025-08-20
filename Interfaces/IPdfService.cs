using DinkToPdf;

namespace HelpDeskSystem.Interfaces
{
    public interface IPdfService
    {
        byte[] ConvertPDF(string htmlcontent,Orientation? orientation=Orientation.Landscape,PaperKind? paperKind=PaperKind.A4);
    }
}
