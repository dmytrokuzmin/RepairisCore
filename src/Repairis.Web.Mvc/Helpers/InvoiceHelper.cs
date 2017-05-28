using System;
using System.Globalization;
using System.IO;
using System.Linq;
using Abp;
using Abp.Domain.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Repairis.Configuration;
using Repairis.Orders.Dto;
using Repairis.SpareParts;
using Syncfusion.Drawing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Grid;

namespace Repairis.Web.Helpers
{
    public class InvoiceHelper : AbpServiceBase, IInvoiceHelper
    {
        private readonly IConfigurationRoot _appConfiguration;
        private readonly IRepository<SparePart> _sparePartRepository;

        public InvoiceHelper(IHostingEnvironment env, IRepository<SparePart> sparePartRepository)
        {
            _sparePartRepository = sparePartRepository;
            _appConfiguration = env.GetAppConfiguration();
            LocalizationSourceName = RepairisConsts.LocalizationSourceName;
        }


        public PdfDocument GenerateDeviceReceipt(OrderFullEntityDto order)
        {
            PdfDocument document = new PdfDocument
            {
                PageSettings =
                {
                    Orientation = PdfPageOrientation.Portrait,
                    Margins = {All = 50}
                }            
            };
            string path = $"{Directory.GetCurrentDirectory()}\\wwwroot\\fonts\\LiberationSerif-Regular.ttf";
            Stream fontStream = new FileStream(path, FileMode.Open, FileAccess.Read);
            var regularFont10 = new PdfTrueTypeFont(fontStream, 10, PdfFontStyle.Regular);
            var regularFont12 = new PdfTrueTypeFont(fontStream, 12, PdfFontStyle.Regular);
            var regularFont14 = new PdfTrueTypeFont(fontStream, 14, PdfFontStyle.Regular);

            string companyName = _appConfiguration["Company:Name"];
            string companyStreet = _appConfiguration["Company:Street"];
            string companyCity = _appConfiguration["Company:City"];
            string companyState = _appConfiguration["Company:State"];
            string companyContactNumber = _appConfiguration["Company:ContactNumber"];
            string companyContactEmail = _appConfiguration["Company:ContactEmail"];

            PdfPage page = document.Pages.Add();
            PdfGraphics g = page.Graphics;
            string companyText =
                $"{companyName}\n{companyStreet},\n{companyCity}\n{companyState}\n{L("Phone")}: {companyContactNumber}\nEmail: {companyContactEmail}";
            PdfTextElement element =
                new PdfTextElement(companyText)
                {
                    Font = regularFont10,
                    Brush = new PdfSolidBrush(new PdfColor(89, 89, 93))
                };
            var companyTextWidth = regularFont10.MeasureString(companyText).Width;
            PdfLayoutResult result = element.Draw(page, new RectangleF(page.Graphics.ClientSize.Width - companyTextWidth, 0, page.Graphics.ClientSize.Width / 2, 130));

            //var logoPath = $"{Directory.GetCurrentDirectory()}\\wwwroot\\images\\logo.png";
            //if (File.Exists(logoPath))
            //{
            //    Stream logoStream = File.OpenRead(logoPath);// new FileStream(logoPath, FileMode.Open, FileAccess.Read);
            //    PdfImage image = new PdfBitmap(logoStream);
            //    page.Graphics.DrawImage(image, new RectangleF(g.ClientSize.Width - 200, result.Bounds.Y, 190, 45));
            //}
            PdfFont subHeadingFont = regularFont14;
            g.DrawRectangle(new PdfSolidBrush(new PdfColor(126, 151, 173)), new RectangleF(0, result.Bounds.Bottom + 40, g.ClientSize.Width, 30));
            element = new PdfTextElement(L("DeviceReceipt")+ " №" + order.Id, subHeadingFont) {Brush = PdfBrushes.White};
            result = element.Draw(page, new PointF(10, result.Bounds.Bottom + 48));
            string currentDate = order.CreationTime;
            SizeF textSize = subHeadingFont.MeasureString(currentDate);
            g.DrawString(currentDate, subHeadingFont, element.Brush, new PointF(g.ClientSize.Width - textSize.Width - 10, result.Bounds.Y));

            element = new PdfTextElement(L("Customer"), regularFont10)
            {
                Brush = new PdfSolidBrush(new PdfColor(126, 155, 203))
            };
            result = element.Draw(page, new PointF(10, result.Bounds.Bottom + 25));
            g.DrawLine(new PdfPen(new PdfColor(126, 151, 173), 0.70f), new PointF(0, result.Bounds.Bottom + 3), new PointF(g.ClientSize.Width, result.Bounds.Bottom + 3));

            var customer = order.Customer;
            element = new PdfTextElement(
                $"{customer.Surname} {customer.Name} {customer.FatherName}\n" +
                $"{L("Address")}: {customer.Address}\n" +
                $"{L("PrimaryPhoneNumber")}: {customer.PhoneNumber} {L("SecondaryPhoneNumber")}: {customer.SecondaryPhoneNumber}\n" +
                $"{L("Email")}: {customer.EmailAddress}", regularFont12)
            {
                Brush = new PdfSolidBrush(new PdfColor(89, 89, 93))
            };
            result = element.Draw(page, new RectangleF(10, result.Bounds.Bottom + 5, g.ClientSize.Width / 2, 100));

            element = new PdfTextElement(L("Device"), regularFont10)
            {
                Brush = new PdfSolidBrush(new PdfColor(126, 155, 203))
            };
            result = element.Draw(page, new PointF(10, result.Bounds.Bottom + 25));
            g.DrawLine(new PdfPen(new PdfColor(126, 151, 173), 0.70f), new PointF(0, result.Bounds.Bottom + 3), new PointF(g.ClientSize.Width, result.Bounds.Bottom + 3));

            var device = order.Device;
            element = new PdfTextElement(
                $"{L("DeviceName")}: {device.DeviceCategoryName} {device.BrandName} {device.DeviceModelName}\n" +
                $"{L("SerialNumber")}: {device.SerialNumber}\n" +
                $"{L("IssueDescription")}: {order.IssueDescription}\n" +
                $"{L("AdditionalEquipment")}: {order.AdditionalEquipment}\n", regularFont12)
            {
                Brush = new PdfSolidBrush(new PdfColor(89, 89, 93))
            };
            result = element.Draw(page, new RectangleF(10, result.Bounds.Bottom + 5, g.ClientSize.Width / 2, 100));

            element = new PdfTextElement(
                $"{L("CustomerSignature")}: ______________________" , regularFont10)
            {
                Brush = new PdfSolidBrush(new PdfColor(89, 89, 93))
            };
            element.Draw(page, new RectangleF(0, result.Bounds.Bottom + 50, g.ClientSize.Width / 2, 100));


            var employeeSignatureString = $"{L("EmployeeSignature")}: ______________________";
            var employeeSignatureStringWidth = regularFont10.MeasureString(employeeSignatureString).Width;
            element = new PdfTextElement(employeeSignatureString, regularFont10)
            {
                Brush = new PdfSolidBrush(new PdfColor(89, 89, 93))
            };
            element.Draw(page, new RectangleF(page.Graphics.ClientSize.Width - employeeSignatureStringWidth, result.Bounds.Bottom + 50, g.ClientSize.Width / 2, 100));
            
            return document;
        }

        public PdfDocument GenerateFinalInvoice(OrderFullEntityDto order)
        {
            PdfDocument document = new PdfDocument
            {
                PageSettings =
                {
                    Orientation = PdfPageOrientation.Portrait,
                    Margins = {All = 50}
                }
            };
            string path = $"{Directory.GetCurrentDirectory()}\\wwwroot\\fonts\\LiberationSerif-Regular.ttf";
            Stream fontStream = new FileStream(path, FileMode.Open, FileAccess.Read);
            var regularFont10 = new PdfTrueTypeFont(fontStream, 10, PdfFontStyle.Regular);
            var regularFont12 = new PdfTrueTypeFont(fontStream, 12, PdfFontStyle.Regular);
            var regularFont14 = new PdfTrueTypeFont(fontStream, 14, PdfFontStyle.Regular);

            string companyName = _appConfiguration["Company:Name"];
            string companyStreet = _appConfiguration["Company:Street"];
            string companyCity = _appConfiguration["Company:City"];
            string companyState = _appConfiguration["Company:State"];
            string companyContactNumber = _appConfiguration["Company:ContactNumber"];
            string companyContactEmail = _appConfiguration["Company:ContactEmail"];

            PdfPage page = document.Pages.Add();
            PdfGraphics g = page.Graphics;
            string companyText =
                $"{companyName}\n{companyStreet},\n{companyCity}\n{companyState}\n{L("Phone")}: {companyContactNumber}\nEmail: {companyContactEmail}";
            PdfTextElement element =
                new PdfTextElement(companyText)
                {
                    Font = regularFont10,
                    Brush = new PdfSolidBrush(new PdfColor(89, 89, 93))
                };
            var companyTextWidth = regularFont10.MeasureString(companyText).Width;
            PdfLayoutResult result = element.Draw(page,
                new RectangleF(page.Graphics.ClientSize.Width - companyTextWidth, 0, page.Graphics.ClientSize.Width / 2,
                    130));

            //var logoPath = $"{Directory.GetCurrentDirectory()}\\wwwroot\\images\\logo.png";
            //if (File.Exists(logoPath))
            //{
            //    Stream logoStream = File.OpenRead(logoPath);// new FileStream(logoPath, FileMode.Open, FileAccess.Read);
            //    PdfImage image = new PdfBitmap(logoStream);
            //    page.Graphics.DrawImage(image, new RectangleF(g.ClientSize.Width - 200, result.Bounds.Y, 190, 45));
            //}
            PdfFont subHeadingFont = regularFont14;
            g.DrawRectangle(new PdfSolidBrush(new PdfColor(126, 151, 173)),
                new RectangleF(0, result.Bounds.Bottom + 40, g.ClientSize.Width, 30));
            element = new PdfTextElement(L("FinalInvoice") + " №" + order.Id, subHeadingFont) {Brush = PdfBrushes.White};
            result = element.Draw(page, new PointF(10, result.Bounds.Bottom + 48));
            string currentDate = (order.DevicePickupDate ?? DateTime.Now).ToString(CultureInfo.CurrentUICulture); ;
            SizeF textSize = subHeadingFont.MeasureString(currentDate);
            g.DrawString(currentDate, subHeadingFont, element.Brush,
                new PointF(g.ClientSize.Width - textSize.Width - 10, result.Bounds.Y));

            element = new PdfTextElement(L("Customer"), regularFont10)
            {
                Brush = new PdfSolidBrush(new PdfColor(126, 155, 203))
            };
            result = element.Draw(page, new PointF(10, result.Bounds.Bottom + 25));
            g.DrawLine(new PdfPen(new PdfColor(126, 151, 173), 0.70f), new PointF(0, result.Bounds.Bottom + 3),
                new PointF(g.ClientSize.Width, result.Bounds.Bottom + 3));

            var customer = order.Customer;
            element = new PdfTextElement(
                $"{customer.Surname} {customer.Name} {customer.FatherName}\n" +
                $"{L("Address")}: {customer.Address}\n" +
                $"{L("PrimaryPhoneNumber")}: {customer.PhoneNumber} {L("SecondaryPhoneNumber")}: {customer.SecondaryPhoneNumber}\n" +
                $"{L("Email")}: {customer.EmailAddress}", regularFont12)
            {
                Brush = new PdfSolidBrush(new PdfColor(89, 89, 93))
            };
            result = element.Draw(page, new RectangleF(10, result.Bounds.Bottom + 5, g.ClientSize.Width / 2, 100));

            element = new PdfTextElement(L("Device"), regularFont10)
            {
                Brush = new PdfSolidBrush(new PdfColor(126, 155, 203))
            };
            result = element.Draw(page, new PointF(10, result.Bounds.Bottom + 25));
            g.DrawLine(new PdfPen(new PdfColor(126, 151, 173), 0.70f), new PointF(0, result.Bounds.Bottom + 3), new PointF(g.ClientSize.Width, result.Bounds.Bottom + 3));

            var device = order.Device;
            element = new PdfTextElement(
                $"{L("DeviceName")}: {device.DeviceCategoryName} {device.BrandName} {device.DeviceModelName}\n" +
                $"{L("SerialNumber")}: {device.SerialNumber}\n" +
                $"{L("IssueDescription")}: {order.IssueDescription}\n" +
                $"{L("AdditionalEquipment")}: {order.AdditionalEquipment}\n", regularFont12)
            {
                Brush = new PdfSolidBrush(new PdfColor(89, 89, 93))
            };
            result = element.Draw(page, new RectangleF(10, result.Bounds.Bottom + 5, g.ClientSize.Width / 2, 100));

            element = new PdfTextElement(L("RepairInformation"), regularFont10)
            {
                Brush = new PdfSolidBrush(new PdfColor(126, 155, 203))
            };
            result = element.Draw(page, new PointF(10, result.Bounds.Bottom + 25));
            g.DrawLine(new PdfPen(new PdfColor(126, 151, 173), 0.70f), new PointF(0, result.Bounds.Bottom + 3), new PointF(g.ClientSize.Width, result.Bounds.Bottom + 3));
            element = new PdfTextElement(
                $"{L("WorkDoneDescription")}: {order.WorkDoneDescripton}\n" +
                $"{L("RepairPrice")}: {order.RepairPrice} {L("UAH")}\n", regularFont12)
            {
                Brush = new PdfSolidBrush(new PdfColor(89, 89, 93))
            };
            result = element.Draw(page, new RectangleF(10, result.Bounds.Bottom + 5, g.ClientSize.Width / 2, 100));

            if (order.SparePartsUsed.Any())
            {
                PdfGrid pdfGrid = new PdfGrid();
                pdfGrid.Columns.Add(4);
                pdfGrid.Headers.Add(1);
                PdfGridCellStyle cellStyle = new PdfGridCellStyle {Borders = {All = PdfPens.White}};

                PdfGridRow pdfGridHeader = pdfGrid.Headers[0];
                pdfGridHeader.Cells[0].Value = L("SparePartName");
                pdfGridHeader.Cells[1].Value = L("Quantity");
                pdfGridHeader.Cells[2].Value = L("PricePerItem");
                pdfGridHeader.Cells[3].Value = L("Subtotal");

                PdfGridCellStyle headerStyle =
                    new PdfGridCellStyle
                    {
                        Borders = {All = new PdfPen(new PdfColor(126, 151, 173))},
                        BackgroundBrush = new PdfSolidBrush(new PdfColor(126, 151, 173)),
                        TextBrush = PdfBrushes.White,
                        Font = regularFont14
                    };

                for (int i = 0; i < pdfGridHeader.Cells.Count; i++)
                {
                    if (i == 0)
                        pdfGridHeader.Cells[i].StringFormat =
                            new PdfStringFormat(PdfTextAlignment.Left, PdfVerticalAlignment.Middle);
                    else
                        pdfGridHeader.Cells[i].StringFormat =
                            new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
                }

                pdfGridHeader.ApplyStyle(headerStyle);
                cellStyle.Borders.Bottom = new PdfPen(new PdfColor(217, 217, 217), 0.70f);
                cellStyle.Font = regularFont12;
                cellStyle.TextBrush = new PdfSolidBrush(new PdfColor(131, 130, 136));

                foreach (var sparePartMapping in order.SparePartsUsed)
                {
                    var sparePart = _sparePartRepository
                        .GetAllIncluding(x => x.Brand).First(x => x.Id == sparePartMapping.SparePartId);

                    //Add rows.
                    PdfGridRow pdfGridRow = pdfGrid.Rows.Add();

                    pdfGridRow.Cells[0].Value = $"{sparePart.Brand.BrandName} {sparePart.SparePartName}";

                    pdfGridRow.Cells[1].Value = sparePartMapping.Quantity;

                    pdfGridRow.Cells[2].Value = sparePartMapping.PricePerItem;

                    pdfGridRow.Cells[3].Value = sparePartMapping.Quantity * sparePartMapping.PricePerItem;
                }

                foreach (PdfGridRow row in pdfGrid.Rows)
                {
                    row.ApplyStyle(cellStyle);
                    for (int i = 0; i < row.Cells.Count; i++)
                    {
                        PdfGridCell cell = row.Cells[i];
                        cell.StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle);
                        if (i > 0)
                        {
                            decimal val;
                            decimal.TryParse(cell.Value.ToString(), out val);
                            cell.Value = val.ToString("G");
                        }
                    }
                }

                PdfGridLayoutFormat layoutFormat = new PdfGridLayoutFormat {Layout = PdfLayoutType.Paginate};

                result = pdfGrid.Draw(page,
                    new RectangleF(new PointF(0, result.Bounds.Bottom + 20),
                        new SizeF(g.ClientSize.Width, g.ClientSize.Height - 100)), layoutFormat);
                float pos = 0.0f;
                for (int i = 0; i < pdfGrid.Columns.Count - 1; i++)
                    pos += pdfGrid.Columns[i].Width;
            }



            decimal repairPrice = order.RepairPrice ?? 0;
            string repairPriceString = $"{L("RepairPrice")} : {repairPrice:G} {L("UAH")}";
            decimal sparePartsTotal = order.SparePartsUsed.Sum(x => x.Quantity * x.PricePerItem);
            string sparePartsTotalString = $"{L("SparePartsTotal")} : {sparePartsTotal:G} {L("UAH")}";
            string grandTotalString = $"{L("GrandTotal")} : {sparePartsTotal + repairPrice} {L("UAH")}";
            var repairPriceWidth = regularFont14.MeasureString(repairPriceString).Width;
            var sparePartsTotalWidth = regularFont14.MeasureString(sparePartsTotalString).Width;
            var grandTotalWidth = regularFont14.MeasureString(grandTotalString).Width;

            element = new PdfTextElement(repairPriceString, regularFont10)
            {
                Brush = new PdfSolidBrush(new PdfColor(126, 155, 203)),
                StringFormat = new PdfStringFormat(PdfTextAlignment.Right)
            };
            result = element.Draw(page, new RectangleF(page.Graphics.ClientSize.Width - repairPriceWidth, result.Bounds.Bottom + 20, repairPriceWidth, 20));

            element = new PdfTextElement(sparePartsTotalString, regularFont10)
            {
                Brush = new PdfSolidBrush(new PdfColor(126, 155, 203)),
                StringFormat = new PdfStringFormat(PdfTextAlignment.Right)
            };
            result = element.Draw(page, new RectangleF(page.Graphics.ClientSize.Width - sparePartsTotalWidth, result.Bounds.Bottom, sparePartsTotalWidth, 20));

            element = new PdfTextElement(grandTotalString, regularFont14)
            {
                Brush = new PdfSolidBrush(new PdfColor(126, 155, 203)),
                StringFormat = new PdfStringFormat(PdfTextAlignment.Right)
            };
            result = element.Draw(page, new RectangleF(page.Graphics.ClientSize.Width - grandTotalWidth, result.Bounds.Bottom, grandTotalWidth, 20));


            element = new PdfTextElement(L("WarrantyInformation"), regularFont10)
            {
                Brush = new PdfSolidBrush(new PdfColor(126, 155, 203))
            };
            result = element.Draw(page, new PointF(10, result.Bounds.Bottom + 25));
            g.DrawLine(new PdfPen(new PdfColor(126, 151, 173), 0.70f), new PointF(0, result.Bounds.Bottom + 3), new PointF(g.ClientSize.Width, result.Bounds.Bottom + 3));
            element = new PdfTextElement(
                $"{L("WarrantyExpirationDate")}: {order.WarrantyExpirationDate}", regularFont12)
            {
                Brush = new PdfSolidBrush(new PdfColor(89, 89, 93))
            };
            result = element.Draw(page, new RectangleF(10, result.Bounds.Bottom + 5, g.ClientSize.Width / 2, 100));




            element = new PdfTextElement(
                $"{L("CustomerSignature")}: ______________________", regularFont10)
            {
                Brush = new PdfSolidBrush(new PdfColor(89, 89, 93))
            };
            element.Draw(page, new RectangleF(0, result.Bounds.Bottom + 40, g.ClientSize.Width / 2, 100));


            var employeeSignatureString = $"{L("EmployeeSignature")}: ______________________";
            var employeeSignatureStringWidth = regularFont10.MeasureString(employeeSignatureString).Width;
            element = new PdfTextElement(employeeSignatureString, regularFont10)
            {
                Brush = new PdfSolidBrush(new PdfColor(89, 89, 93))
            };
            element.Draw(page, new RectangleF(page.Graphics.ClientSize.Width - employeeSignatureStringWidth, result.Bounds.Bottom + 50, g.ClientSize.Width / 2, 100));

            return document;
        }
    }
}
