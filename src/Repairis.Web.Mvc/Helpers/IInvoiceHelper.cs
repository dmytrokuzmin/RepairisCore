using Abp.Dependency;
using Repairis.Orders.Dto;
using Syncfusion.Pdf;

namespace Repairis.Web.Helpers
{
    public interface IInvoiceHelper : ISingletonDependency
    {
        PdfDocument GenerateDeviceReceipt(OrderFullEntityDto order);
        PdfDocument GenerateFinalInvoice(OrderFullEntityDto order);
    }
}
