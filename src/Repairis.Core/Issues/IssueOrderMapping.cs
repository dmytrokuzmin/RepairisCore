using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Repairis.Orders;

namespace Repairis.Issues
{
    public class IssueOrderMapping : Entity<string>
    {
        // All keys are configured via Fluent API
        [NotMapped]
        public override string Id
        {
            get { return OrderId + "-" + IssueId; }
        }

        public long OrderId { get; set; }
        public virtual Order Order { get; set; }


        public int IssueId { get; set; }
        public virtual Issue Issue { get; set; }
    }
}
