﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.IService
{
    public interface IEmailService
    {
        Task SendUserResetPassword(string fullName, string userEmail, string newPassword);
        Task SendUnifiedAppointmentEmailToSponsor(string sponsorName, string sponsorEmail);
        Task SendUnifiedAppointmentEmailToStudent(string studentName, string studentEmail);
    }
}
