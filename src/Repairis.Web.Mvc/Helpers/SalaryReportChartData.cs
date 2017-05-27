using System;
using System.Globalization;

namespace Repairis.Web.Helpers
{
    public class SalaryReportChartData
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

        public decimal SalaryValue { get; set; }
    }
}
