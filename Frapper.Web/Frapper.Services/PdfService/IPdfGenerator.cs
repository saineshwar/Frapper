using Microsoft.AspNetCore.Mvc;

namespace Frapper.Services.PdfService
{
    public interface IPdfGenerator
    {
        FileResult DownloadInvoicepdf();
    }
}