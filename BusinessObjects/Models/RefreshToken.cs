using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class RefreshToken
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string Token { get; set; }

    public DateTime ExpiredAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public string Status { get; set; }

    public virtual User User { get; set; }
}
