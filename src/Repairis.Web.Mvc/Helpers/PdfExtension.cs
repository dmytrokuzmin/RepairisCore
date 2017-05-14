using Microsoft.AspNetCore.Http;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Parsing;

namespace Repairis.Web.Helpers
{
    public static class PdfExtension
    {
        public static PdfResult ExportAsActionResult(this PdfDocument pdfDoc, string filename, HttpResponse response,
            HttpReadType type)
        {
            return new PdfResult(pdfDoc, "/wwwroot/" + filename, response, type);
        }

        public static PdfResult ExportAsActionResult(this PdfLoadedDocument pdfdoc, string filename,
            HttpResponse response, HttpReadType type)
        {
            return new PdfResult(pdfdoc, "/wwwroot/" + filename, response, type);
        }
    }
}
