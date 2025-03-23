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
    [Table("Products")]
    public class Product : BaseEntity<string>
    {
        public string? SubCategoryId { get; set; }
    }
}
