using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using virtual_diary.Models;
using virtual_diary.Models.DTO;

namespace virtual_diary.Services
{
    public interface ITeacherService
    {
        IEnumerable<Teacher> GetTeachers();
        Teacher GetTeacherById(string id);
        Teacher GetTeacherByUserName(string username);
        Task<IdentityResult> RegisterTeacher(TeacherDTO teacher);
        Teacher PutTeacher(string username, Teacher teacher);
        bool DeleteTeacher(string username);
    }
}
