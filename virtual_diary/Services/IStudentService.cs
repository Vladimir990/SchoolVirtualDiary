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
    public interface IStudentService
    {
        IEnumerable<Student> GetStudents();
        Student GetStudentById(string id);
        Student GetStudentByUserName(string username);
        Task<IdentityResult> RegisterStudent(StudentDTO sudent);
        Student PutStudent(string username, Student student);
        bool DeleteStudent(string username);
        Student PutParent(string studentUser, string parentUser);
        Student PutStudentInClass(string studentUsername, int classId);
        StudentNLDTO GetNameAndGrades(string studentUsername);
    }
}
