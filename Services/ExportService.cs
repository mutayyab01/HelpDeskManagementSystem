using ClosedXML.Excel;
using ClosedXML.Extensions;
using DinkToPdf;
using ElmahCore;
using HelpDeskSystem.Interfaces;
using HelpDeskSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace HelpDeskSystem.Services
{
    public class ExportService : IExportService
    {
        private readonly IPdfService _pdfService;
        public ExportService(IPdfService pdfService)
        {

            _pdfService = pdfService;
        }
        public async Task<byte[]> ExportPageToPDF<T>(ReportGenerationViewModel<T> model)
        {
            byte[] PDF = null;
            try
            {
                if (model.Method == "POST" || string.IsNullOrEmpty(model.Method))
                {
                    var client = new RestClient(model.URL);
                    var request = new RestRequest
                    {
                        Method = Method.Post
                    };
                    request.AddBody(model);
                    var response = await client.PostAsync(request);
                    PDF = _pdfService.ConvertPDF(response.Content, paperKind: model.PaperKind, orientation: model.PageOrientation);


                }
                else
                {
                    var client = new RestClient(model.URL);
                    var request = new RestRequest
                    {
                        Method = Method.Get
                    };
                    request.AddBody(model);
                    var response = await client.GetAsync(request);
                    PDF = _pdfService.ConvertPDF(response.Content, paperKind: model.PaperKind, orientation: model.PageOrientation);

                }
                return PDF;
            }
            catch (Exception ex)
            {
                ElmahExtensions.RaiseError(ex);
                return null;
            }

        }

        public FileStreamResult ExportToExcel(IEnumerable<object> data, string fileName)
        {
            var wb = new XLWorkbook();
            var worksheetName = fileName;
            if (worksheetName.Length > 23)
            {
                worksheetName = worksheetName.Substring(0, 23) + "...";
            }
            var ws = wb.AddWorksheet(worksheetName);
            fileName = fileName + ".xlsx";
            ws.Cell(1, 1).InsertData(data);
            ws.Columns().AdjustToContents();
            var xlTable = ws.Tables.FirstOrDefault();
            if (xlTable != null)
            {
                xlTable.ShowAutoFilter = true;
            }
            // ✅ Instead of wb.Deliver()
            var stream = new MemoryStream();
            wb.SaveAs(stream);
            stream.Position = 0;

            return new FileStreamResult(stream,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                FileDownloadName = fileName
            };
        }

        public FileStreamResult ExportToExcel<T>(List<T> listData, string fileName)
        {
            var wb = new XLWorkbook();
            var worksheetName = fileName;
            if (worksheetName.Length > 23)
            {
                worksheetName = worksheetName.Substring(0, 23) + "...";
            }
            var ws = wb.AddWorksheet(worksheetName);
            fileName = fileName + ".xlsx";
            ws.Cell(1, 1).InsertData(listData);
            ws.Columns().AdjustToContents();
            var xlTable = ws.Tables.FirstOrDefault();
            if (xlTable != null)
            {
                xlTable.ShowAutoFilter = true;
            }
            // ✅ Instead of wb.Deliver()
            var stream = new MemoryStream();
            wb.SaveAs(stream);
            stream.Position = 0;

            return new FileStreamResult(stream,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                FileDownloadName = fileName
            };
        }

        public async Task<byte[]> ExportToPDF(string URL, string reportTitle, PaperKind? paperKind = PaperKind.A4, Orientation? orientation = Orientation.Portrait)
        {
            try
            {
                PaperKind pk = PaperKind.A4;
                var httpclient = new HttpClient();
                httpclient.BaseAddress = new Uri(URL);
                var client = new RestClient();
                var request = new RestRequest
                {
                    Method = Method.Get
                };
                var response = await client.ExecuteAsync(request);
                var htmlInput = response.Content!.Replace(@"\n", "").Replace(@"\r", "");
                byte[] pdf = _pdfService.ConvertPDF(htmlcontent: htmlInput, orientation, paperKind);
                return pdf;
            }
            catch (Exception ex)
            {
                ElmahExtensions.RaiseError(ex);
                return null;
            }
        }
    }
}
