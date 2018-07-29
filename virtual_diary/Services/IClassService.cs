using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using virtual_diary.Models;
using virtual_diary.Models.DTO;

namespace virtual_diary.Services
{
    public interface IClassService
    {
        IEnumerable<Class> GetClasses();
        Class GetClass(int id);
        Class GetClassByName(enumClass name);
        Class PostClass(Class subject);
        Class PutClass(int id, Class classs);
        bool DeleteClass(int id);
        ClassDTO GetStudentsByClass(int classId);
    }
}
