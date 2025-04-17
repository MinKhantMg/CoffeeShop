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
    [Table("Orders")]
    public class Order : BaseEntity<string>
    {
        public int TotalAmount { get; set; }

        public string OrderStatus { get; set; } = Enum.GetName(typeof(OrderStatus), Enums.OrderStatus.Pending)!;

        public string OrderType { get; set; }

        public string PaymentType { get; set; } = Enum.GetName(typeof(PaymentType), Enums.PaymentType.Cash)!;

    }
}
