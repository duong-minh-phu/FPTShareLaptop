using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class Student
{
    public int StudentId { get; set; }

    public int UserId { get; set; }

    public string StudentCode { get; set; } = null!;

    public string IdentityCard { get; set; } = null!;

    public string EnrollmentDate { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
