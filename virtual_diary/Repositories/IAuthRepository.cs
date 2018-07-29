using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using virtual_diary.Models;

namespace virtual_diary.Repositories
{
    public interface IAuthRepository : IDisposable
    {
        Task<IdentityResult> RegisterParent(Parent parent, string password);
        Task<IdentityResult> RegisterTeacher(Teacher teacher, string password);
        Task<IdentityResult> RegisterStudent(Student student, string password);
        Task<IdentityResult> RegisterAdmin(Admin admin, string password);
        Task<UserModel> FindUser(string userName, string password);
        Task<IList<string>> FindRoles(string userId);
    }
}
