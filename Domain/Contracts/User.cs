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
public class User : BaseEntity<string>
{
    public string? PhoneNumber { get; set; } = default!;

    public string? Email { get; set; } = default!;

    public string? Role { get; set; } = Enum.GetName(typeof(Role), Domain.Enums.Role.None);

    public string? PasswordHash { get; set; } = default!;

    public string? RefreshToken { get; set; }

    public DateTime RefreshTokenExpiry { get; set; }

}
