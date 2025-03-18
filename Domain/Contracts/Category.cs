using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Base;

namespace Domain.Contracts
{
    [Table("Category")]
    public class Category : BaseEntity<string>
    {
        public string Slug { get; set; }

    }
}
