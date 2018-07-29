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
    public interface IAdminService
    {
        IEnumerable<Admin> GetAdmins();
        IEnumerable<UserModel> GetUsers();
        Admin GetAdminById(string id);
        Admin GetAdminByUserName(string username);
        UserModel GetUserById(string id);
        Task<IdentityResult> RegisterAdmin(AdminDTO admin);
        Admin PutAdmin(string username, Admin admin);
        bool DeleteAdmin(string username);
        
        
    }
}
