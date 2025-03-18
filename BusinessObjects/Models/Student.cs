using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class Student
{
    public int StudentId { get; set; }

    public int? UserId { get; set; }

    public string? StudentCode { get; set; }

    public string? IdentityCard { get; set; }

    public DateTime? EnrollmentDate { get; set; }

    public string? Status { get; set; }

    public virtual User? User { get; set; }
}
