using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using virtual_diary.Models;

namespace virtual_diary.Repositories
{
    public class AuthRepository : IAuthRepository, IDisposable
    {
        

        private UserManager<UserModel> _userManager;

        public AuthRepository(DbContext context)
        {
            
            _userManager = new UserManager<UserModel>(new UserStore<UserModel>(context));
        }


        public async Task<IdentityResult> RegisterParent(Parent parent, string password)
        {
            var result = await _userManager.CreateAsync(parent, password);
            _userManager.AddToRole(parent.Id, "parents");
            return result;
        }

        public async Task<IdentityResult> RegisterTeacher(Teacher teacher, string password)
        {
            var result = await _userManager.CreateAsync(teacher, password);
            _userManager.AddToRole(teacher.Id, "teachers");
            return result;
        }

        public async Task<IdentityResult> RegisterStudent(Student student, string password)
        {
            var resault = await _userManager.CreateAsync(student, password);
            _userManager.AddToRole(student.Id, "students");
            return resault;
        }

        public async Task<IdentityResult> RegisterAdmin(Admin admin, string password)
        {
            var result = await _userManager.CreateAsync(admin, password);
            _userManager.AddToRole(admin.Id, "admins");
            return result;
        }

        public async Task<UserModel> FindUser(string userName, string password)
        {
            UserModel user = await _userManager.FindAsync(userName, password);
            return user;
        }


        public async Task<IList<string>> FindRoles(string userId)
        {
            return await _userManager.GetRolesAsync(userId);
        }

        public void Dispose()
        {
            if (_userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }
        }

    }
}