using Microsoft.AspNet.Identity;
using NLog;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using virtual_diary.Models;
using virtual_diary.Models.DTO;
using virtual_diary.Repositories;

namespace virtual_diary.Services
{
    public class AdminService: IAdminService
    {
        private IUnitOfWork db;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public AdminService (IUnitOfWork db)
        {
            this.db = db;
        }


        public IEnumerable<Admin> GetAdmins()
        {
           return db.AdminRepository.Get();
        }

        public IEnumerable<UserModel> GetUsers()
        {
            return db.UsersRepository.Get();
        }

        public Admin GetAdminById (string id)
        {
            return db.AdminRepository.GetByID(id);
        }

        public UserModel GetUserById (string id)
        {
            return db.UsersRepository.GetByID(id);
        }

        public Admin GetAdminByUserName (string username)
        {
            return db.AdminRepository.Get().FirstOrDefault(x => x.UserName == username);
        }

       

        public async Task<IdentityResult> RegisterAdmin(AdminDTO newAdmin)
        {
            
            Admin user = new Admin
            {
                FirstName = newAdmin.FirstName,
                LastName = newAdmin.LastName,
                UserName = newAdmin.UserName,
                Email = newAdmin.Email,

            };
            logger.Info("New Admin with name {0} {1} is created", user.FirstName, user.LastName);
            return await db.AuthRepository.RegisterAdmin(user, newAdmin.Password);

        }


        public Admin PutAdmin (string username, Admin admin)
        {
            Admin updateAdmin = db.AdminRepository.Get().FirstOrDefault(x => x.UserName == username);
            updateAdmin.FirstName = admin.FirstName;
            updateAdmin.LastName = admin.LastName;
            updateAdmin.Email = admin.Email;
            updateAdmin.UserName = admin.UserName;

            db.AdminRepository.Update(updateAdmin);
            db.Save();
            logger.Warn("Admin with id {0} and name {1} {2} is updated", admin.Id, admin.FirstName, admin.LastName);
            return updateAdmin;
        }

       
        public bool DeleteAdmin (string username)
        {
            Admin deleteAdmin = db.AdminRepository.Get().FirstOrDefault(x => x.UserName == username);
            db.AdminRepository.Delete(deleteAdmin);
            db.Save();
            logger.Warn("Admin with id {0} and name {1} {2} is deleted", deleteAdmin.Id, deleteAdmin.FirstName, deleteAdmin.LastName);
            return true;
        }

       
    }
}