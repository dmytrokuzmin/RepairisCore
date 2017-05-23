using System;
using System.Globalization;

namespace Repairis.Web.Helpers
{
    public class SalesReportChartData
    {
        public DateTime Date { get; set; }

        public string MonthName
        {
            get
            {
                return Date.ToString("MMM", CultureInfo.CurrentUICulture);
            }
        }

        public int Year { get; set; }

        public decimal TotalRevenue
        {
            get { return RepairTotalPrice + SparePartsTotalCost - SparePartsTotalSupplierCost; }
        }

        public decimal RepairTotalPrice { get; set; }
        public decimal SparePartsTotalCost { get; set; }
        public decimal SparePartsTotalSupplierCost { get; set; }
    }
}
