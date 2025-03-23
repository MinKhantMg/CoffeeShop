using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Base;

namespace Domain.Contracts
{
    [Table("Categories")]
    public class Category : BaseEntity<string>
    {
 
    }
}
