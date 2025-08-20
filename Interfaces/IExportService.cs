using DinkToPdf;
using HelpDeskSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HelpDeskSystem.Interfaces
{
    public interface IExportService
    {
        Task<byte[]> ExportToPDF(string URL, string reportTitle, PaperKind? paperKind = PaperKind.A4, Orientation? or = Orientation.Portrait);
        Task<byte[]> ExportPageToPDF<T>(ReportGenerationViewModel<T> model);
        FileStreamResult ExportToExcel(IEnumerable<object> data, string fileName);
        FileStreamResult ExportToExcel<T>(List<T> listData, string fileName);
    }
}
