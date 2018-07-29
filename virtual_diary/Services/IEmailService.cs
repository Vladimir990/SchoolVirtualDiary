using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using virtual_diary.Models;

namespace virtual_diary.Services
{
    public interface IEmailService
    {
        void SendMail(StudentTeacherSubject sts, int value);
    }
}
