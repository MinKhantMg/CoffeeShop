using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Base;
using Domain.Enums;

namespace Domain.Contracts;

[Table("Users")]
public class User : GuidBaseEntity
{
    public string? PhoneNumber { get; set; } = default!;

    public string? Email { get; set; } = default!;

    public string? Role { get; set; } = Enum.GetName(typeof(Role), Domain.Enums.Role.None);

    public string? Password { get; set; } = default!;

    public string? PasswordHash { get; set; } = default!;

}
