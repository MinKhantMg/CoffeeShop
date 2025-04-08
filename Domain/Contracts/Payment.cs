using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Base;
using Domain.Enums;

namespace Domain.Contracts
{
    [Table("Payments")]
    public class Payment : BaseEntity<string>
    {
        public string PaymentType { get; set; } = Enum.GetName(typeof(PaymentType), Enums.PaymentType.Cash)!;

        public decimal Amount { get; set; }

        public string OrderId { get; set; }
    }
}
