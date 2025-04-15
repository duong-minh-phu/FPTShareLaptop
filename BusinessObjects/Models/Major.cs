using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class Major
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Status { get; set; }

    public virtual ICollection<BorrowRequest> BorrowRequests { get; set; } = new List<BorrowRequest>();
}
