using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using virtual_diary.Models;
using virtual_diary.Models.DTO;

namespace virtual_diary.Services
{
    public interface ISTSService
    {
        IEnumerable<StudentTeacherSubject> GetAllSTS();
        StudentTeacherSubject GetSTSByID(int id);
        STSDTO PostSTS(string studentUsername, int teacherSubjectId);
        StudentTeacherSubject PutSemesterToSTS(int id, StudentTeacherSubject sts);
        bool DeleteSTS(int id);
        STSDTO PutGrades(int stsId, int gradeId);
        IEnumerable<StudentTeacherSubject> GetSTSByStudent(string studentUsername);
        ICollection<int> GetGrades(int stsId);
        StudentFNLNDTO GetStudentInfo(string studentUsername);
    }
}
