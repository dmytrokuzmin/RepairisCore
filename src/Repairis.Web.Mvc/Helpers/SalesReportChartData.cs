using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public int RepairTotalPrice { get; set; }
        public decimal SparePartsTotalCost { get; set; }
        public decimal SparePartsTotalSupplierCost { get; set; }
    }
}
