using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Parsing;

namespace Repairis.Web.Helpers
{
    public class PdfResult : ActionResult
    {
        public string FileName { get; set; }
        public PdfDocument PdfDoc { get; }
        public PdfLoadedDocument PdfLoadedDoc { get; }
        public HttpResponse Response { get; }
        public HttpReadType ReadType { get; set; }

        public PdfResult(PdfDocument pdfDocument, string filename, HttpResponse response, HttpReadType type)
        {
            PdfDoc = pdfDocument;
            PdfLoadedDoc = null;
            FileName = filename;
            Response = response;
            ReadType = type;
        }

        public PdfResult(PdfLoadedDocument pdfLoadedDocument, string filename, HttpResponse response, HttpReadType type)
        {
            PdfDoc = null;
            PdfLoadedDoc = pdfLoadedDocument;
            FileName = filename;
            Response = response;
            ReadType = type;
        }

        public void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                return;
            PdfLoadedDoc?.Save(FileName);
            if (PdfDoc != null)
            {
                PdfDoc.Save(FileName);
                PdfDoc.Close(true);
            }
        }
    }
}
