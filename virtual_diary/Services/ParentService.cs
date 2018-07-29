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
    public class ParentService: IParentService
    {
        private IUnitOfWork db;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public ParentService (IUnitOfWork db)
        {
            this.db = db;
        }

        public IEnumerable<Parent> GetParents()
        {
            return db.ParentRepository.Get();
        }

        public Parent GetParentById(string id)
        {
            return db.ParentRepository.GetByID(id);
        }

        public Parent GetParentByUserName (string username)
        {
            return db.ParentRepository.Get().FirstOrDefault(x => x.UserName == username);
        }

        public async Task<IdentityResult> RegisterParent(ParentDTO newParent)
        {
            Parent user = new Parent
            {

                FirstName = newParent.FirstName,
                LastName = newParent.LastName,
                UserName = newParent.UserName,
                Email = newParent.Email,
                PhoneNumber = newParent.PhoneNumber,

                
            };

            logger.Info("New Parent with name {0} {1} is created", user.FirstName, user.LastName);
            return await db.AuthRepository.RegisterParent(user, newParent.Password);
            
   
        }

        public Parent PutParent (string username, Parent parent)
        {
            Parent updateParent = db.ParentRepository.Get().FirstOrDefault(x => x.UserName == username);
            updateParent.FirstName = parent.FirstName;
            updateParent.LastName = parent.LastName;
            updateParent.UserName = parent.UserName;
            updateParent.Email = parent.Email;
            updateParent.PhoneNumber = parent.PhoneNumber;
            

            db.ParentRepository.Update(updateParent);
            db.Save();
            logger.Warn("Parent with id {0} and name {1} {2} is updated", parent.Id, parent.FirstName, parent.LastName);
            return updateParent;
        }


        public bool DeleteParent(string username)
        {
            Parent deleteParent = db.ParentRepository.Get().FirstOrDefault(x => x.UserName == username);
            db.ParentRepository.Delete(deleteParent);
            db.Save();
            logger.Warn("Parent with id {0} and name {1} {2} is deleted", deleteParent.Id, deleteParent.FirstName, deleteParent.LastName);
            return true;
        }

        public ParentStudentDTO GetStudentsFromParent(string parentUsername)
        {
            Parent parent = db.ParentRepository.Get().FirstOrDefault(x => x.UserName == parentUsername);
            ParentStudentDTO ps = new ParentStudentDTO();

            ps.ParentFirstName = parent.FirstName;
            ps.ParentLastName = parent.LastName;

            
            foreach (Student st in parent.Children)
            {
                StudentFNLNDTO student = new StudentFNLNDTO();

                student.FirstName = st.FirstName;
                student.LastName = st.LastName;
                student.Class = st.Class.Name.ToString();

                ps.Children.Add(student);
            }


            return ps;
        }


    }
}