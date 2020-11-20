using System;
using System.Globalization;
using System.IO;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;

namespace Frapper.Services.PdfService
{
    public class PdfGenerator : IPdfGenerator
    {
        private readonly IConverter _converter;
        public PdfGenerator(IConverter converter)
        {
            _converter = converter;
        }
        public FileResult DownloadInvoicepdf()
        {
            var basePath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Templates")).Root + "invoice.html";
            string strMailBody;
            using (StreamReader objectStreamReader = new StreamReader(basePath))
            {
                strMailBody = objectStreamReader.ReadToEnd();
                objectStreamReader.Close();
            }
            strMailBody = strMailBody.Replace("###CompanyName", "Frapper");
            strMailBody = strMailBody.Replace("###Date", DateTime.Now.ToString());


            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Landscape,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 10 },
                DocumentTitle = "Invoice"
            };
            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = strMailBody
            };
            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };
            var file = _converter.Convert(pdf);
            FileResult fileResult = new FileContentResult(file, "application/pdf");
            fileResult.FileDownloadName = "Invoice_" + DateTime.UtcNow.ToString(CultureInfo.InvariantCulture) + ".pdf";
            return fileResult;
        }
    }
}