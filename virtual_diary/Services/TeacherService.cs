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
    public class TeacherService:ITeacherService
    {
        private IUnitOfWork db;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public TeacherService (IUnitOfWork db)
        {
            this.db = db;
        }

        public IEnumerable<Teacher> GetTeachers()
        {
            return db.TeacherRepository.Get();
        }

        public Teacher GetTeacherById(string id)
        {
            return db.TeacherRepository.GetByID(id);
        }

        public Teacher GetTeacherByUserName (string username)
        {
            return db.TeacherRepository.Get().FirstOrDefault(x => x.UserName == username);
        }

        public async Task<IdentityResult> RegisterTeacher(TeacherDTO newTeacher)
        {
            Teacher user = new Teacher
            {

                FirstName = newTeacher.FirstName,
                LastName = newTeacher.LastName,
                UserName = newTeacher.UserName,
                PhoneNumber = newTeacher.PhoneNumber,

            };

           
                logger.Info("New Teacher with name {0} {1} created", user.FirstName, user.LastName);
                return await db.AuthRepository.RegisterTeacher(user, newTeacher.Password);
                
          
        }

        public Teacher PutTeacher(string username, Teacher teacher)
        {
            Teacher updateTeacher = db.TeacherRepository.Get().FirstOrDefault(x => x.UserName == username);
            updateTeacher.FirstName = teacher.FirstName;
            updateTeacher.LastName = teacher.LastName;
            updateTeacher.UserName = teacher.UserName;
            updateTeacher.Email = teacher.Email;
            updateTeacher.PhoneNumber = teacher.PhoneNumber;


            db.TeacherRepository.Update(updateTeacher);
            db.Save();
            logger.Warn("Teacher with id {0} and name {1} {2} is updated", teacher.Id, teacher.FirstName, teacher.LastName);
            return updateTeacher;
        }



        public bool DeleteTeacher(string username)
        {
            Teacher deleteTeacher = db.TeacherRepository.Get().FirstOrDefault(x => x.UserName == username);
            db.TeacherRepository.Delete(deleteTeacher);
            db.Save();
            logger.Warn("Teacher with id {0} and name {1} {2} is deleted", deleteTeacher.Id, deleteTeacher.FirstName, deleteTeacher.LastName);
            return true;
        }

    }
}