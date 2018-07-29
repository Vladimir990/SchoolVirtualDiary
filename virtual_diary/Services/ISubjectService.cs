using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using virtual_diary.Models;
using virtual_diary.Models.DTO;

namespace virtual_diary.Services
{
    public interface ISubjectService
    {
        IEnumerable<SubjectDTO>GetSubjects();
        SubjectModel GetSubject(int id);
        SubjectModel GetSubjectByName(enumSubject name);
        SubjectModel PostSubject(SubjectModel subject);
        SubjectModel PutSubject(int id, SubjectModel subject);
        bool DeleteSubject(enumSubject name);
    }
}
