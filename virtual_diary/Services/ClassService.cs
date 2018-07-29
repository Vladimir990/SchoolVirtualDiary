using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using virtual_diary.Models;
using virtual_diary.Models.DTO;
using virtual_diary.Repositories;

namespace virtual_diary.Services
{
    public class ClassService: IClassService
    {
        private IUnitOfWork db;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public ClassService (IUnitOfWork db)
        {
            this.db = db;
        }

        public IEnumerable<Class> GetClasses()
        {
            return db.ClassRepository.Get();
        }

        public Class GetClass(int id)
        {
            return db.ClassRepository.GetByID(id);
        }

        public Class GetClassByName(enumClass name)
        {
            return db.ClassRepository.Get().FirstOrDefault(x => x.Name == name);
        }

        public Class PostClass(Class classs)
        {
            Class newClass = new Class()
            {
                
                Name = classs.Name

            };

            db.ClassRepository.Insert(newClass);
            db.Save();
            logger.Info("New Class with name {0} is created", newClass.Name.ToString());
            return newClass;
        }

        public Class PutClass(int id, Class classs)
        {
            Class updateClass = db.ClassRepository.GetByID(id);

            updateClass.Name = classs.Name;


            db.ClassRepository.Update(updateClass);
            db.Save();
            logger.Warn("Class with id {0} is updated", updateClass.Id);
            return updateClass;
        }

        public bool DeleteClass(int id)
        {
            Class deleteClass = db.ClassRepository.GetByID(id);
            db.ClassRepository.Delete(deleteClass);
            db.Save();
            logger.Warn("Class with name {0} is deleted", deleteClass.Name.ToString());
            return true;
        }

        public ClassDTO GetStudentsByClass (int classId)
        {
            Class classFromDb = db.ClassRepository.GetByID(classId);
            ClassDTO classs = new ClassDTO();

            classs.ClassName = classFromDb.Name.ToString();

            foreach(Student s in classFromDb.Students)
            {
                StudentFNLNDTO student = new StudentFNLNDTO();

                student.FirstName = s.FirstName;
                student.LastName = s.LastName;

                classs.Students.Add(student);
            }

            return classs;
        }



    }
}