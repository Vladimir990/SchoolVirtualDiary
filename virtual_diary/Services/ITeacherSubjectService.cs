using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using virtual_diary.Models;
using virtual_diary.Models.DTO;
using virtual_diary.Repositories;

namespace virtual_diary.Services
{
    public interface ITeacherSubjectService
    {
        IEnumerable<TeacherSubject> GetAll();
        TeacherSubjectDTO PostTeacherSubject(string teacherUsername, int subjectId);
        TeacherSubject GetTeacherSubject(int tsId);
        IEnumerable<TeacherSubject> GetTSByTeacher(string teacherUsername);
        IEnumerable<TeacherSubject> GetTSBySubject(int id);
        TeacherSubject DeleteTS(int id);
        
    }
}
