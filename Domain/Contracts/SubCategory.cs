using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Base;

namespace Domain.Contracts
{
    [Table("SubCategory")]
    public class SubCategory: BaseEntity<string>
    {
        public string? CategoryId { get; set; }
    }
}
