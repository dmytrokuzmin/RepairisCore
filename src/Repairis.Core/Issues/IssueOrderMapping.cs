using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Repairis.Orders;

namespace Repairis.Issues
{
    public class IssueOrderMapping
    {
        // All keys are configured via Fluent API

        public long OrderId { get; set; }
        public virtual Order Order { get; set; }


        public int IssueId { get; set; }
        public virtual Issue Issue { get; set; }
    }
}
