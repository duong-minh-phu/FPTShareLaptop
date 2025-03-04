using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class User
{
    public int UserId { get; set; }

    public string? FullName { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public int? RoleId { get; set; }

    public string? ProfilePicture { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

    public virtual Role? Role { get; set; }

    public virtual Shop? Shop { get; set; }

    public virtual Sponsor? Sponsor { get; set; }

    public virtual Student? Student { get; set; }
}
