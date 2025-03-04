using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class Student
{
    public int StudentId { get; set; }

    public int? UserId { get; set; }

    public string? StudentCode { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public DateOnly? EnrollmentDate { get; set; }

    public virtual ICollection<BorrowHistory> BorrowHistories { get; set; } = new List<BorrowHistory>();

    public virtual ICollection<BorrowRequest> BorrowRequests { get; set; } = new List<BorrowRequest>();

    public virtual ICollection<Commitment> Commitments { get; set; } = new List<Commitment>();

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual User? User { get; set; }
}
