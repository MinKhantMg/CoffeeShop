using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Base;

namespace Domain.Contracts
{
    [Table("Tables")]
    public class Table : BaseEntity<string>
    {

    }
}
