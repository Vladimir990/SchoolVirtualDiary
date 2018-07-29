using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using virtual_diary.Models;

namespace virtual_diary.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<UserModel> UsersRepository { get; }
        IGenericRepository<Admin> AdminRepository { get; }
        IGenericRepository<Parent> ParentRepository { get; }
        IGenericRepository<Teacher> TeacherRepository { get; }
        IGenericRepository<Student> StudentRepository { get; }
        IGenericRepository<SubjectModel> SubjectRepository { get; }
        IGenericRepository<Class> ClassRepository { get; }
        IAuthRepository AuthRepository { get; }
        IGenericRepository<TeacherSubject> TeacherSubjectRepository { get; }
        IGenericRepository<StudentTeacherSubject> STSRepository { get; }
        IGenericRepository<Grades>GradesRepository { get; }

        void Save();
    }
}
