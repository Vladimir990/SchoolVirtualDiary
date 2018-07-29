using Microsoft.AspNet.Identity;
using NLog;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using virtual_diary.Infrastructure;
using virtual_diary.Models;
using virtual_diary.Models.DTO;
using virtual_diary.Repositories;

namespace virtual_diary.Services
{
    public class StudentService: IStudentService
    {
        private IUnitOfWork db;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public StudentService (IUnitOfWork db)
        {
            this.db = db;
        }

        public IEnumerable<Student> GetStudents()
        {
            return db.StudentRepository.Get();
        }

        public Student GetStudentById(string id)
        {
            return db.StudentRepository.GetByID(id);
        }

        public Student GetStudentByUserName(string username)
        {
            return db.StudentRepository.Get().FirstOrDefault(x => x.UserName == username);
        }

        public async Task<IdentityResult> RegisterStudent(StudentDTO newStudent)
        {
            Student user = new Student
            {

                FirstName = newStudent.FirstName,
                LastName = newStudent.LastName,
                UserName = newStudent.UserName,
                DateOfBirth = newStudent.DateOfBirth,

            };
            logger.Info("New Student with name {0} {1} is created", user.FirstName, user.LastName);
            return await db.AuthRepository.RegisterStudent(user, newStudent.Password);

            
        }

        public Student PutStudent(string username, Student student)
        {
            Student updateStudent = db.StudentRepository.Get().FirstOrDefault(x => x.UserName == username);
            updateStudent.FirstName = student.FirstName;
            updateStudent.LastName = student.LastName;
            updateStudent.UserName = student.UserName;


            db.StudentRepository.Update(updateStudent);
            db.Save();
            logger.Warn("Student with id {0} and name {1} {2} is updated", student.Id, student.FirstName, student.LastName);
            return updateStudent;
        }


        public bool DeleteStudent(string username)
        {
            Student deleteStudent = db.StudentRepository.Get().FirstOrDefault(x => x.UserName == username);
            db.StudentRepository.Delete(deleteStudent);
            db.Save();
            logger.Warn("Student with id {0} and name {1} {2} is deleted", deleteStudent.Id, deleteStudent.FirstName, deleteStudent.LastName);
            return true;
        }

        public Student PutParent (string studentUser, string parentUser)
        {
            Student student = db.StudentRepository.Get().FirstOrDefault(x => x.UserName == studentUser);
            Parent parent = db.ParentRepository.Get().FirstOrDefault(x => x.UserName == parentUser);
            student.Parent = parent;
            db.StudentRepository.Update(student);
            db.Save();
            logger.Info("Parednt with id {0} and name {1} {2} is added to student with id {3} and name {4} {5}", parent.Id, parent.FirstName, parent.LastName, student.Id, student.FirstName, student.LastName);
            return student;
        }

        public Student PutStudentInClass (string studentUsername, int classId)
        {
            Student student = db.StudentRepository.Get().FirstOrDefault(x => x.UserName == studentUsername);
            Class classs = db.ClassRepository.GetByID(classId);
            student.Class = classs;
            db.StudentRepository.Update(student);
            db.Save();
            logger.Info("Student with id {0} and name {1} {2} is added to class {3}", student.Id, student.FirstName, student.LastName, classs.Name.ToString());
            return student;
        }

        public StudentNLDTO GetNameAndGrades(string studentUsername)
        {
            Student studentFromDb = db.StudentRepository.Get().FirstOrDefault(x => x.UserName == studentUsername);
            StudentNLDTO student = new StudentNLDTO();
            

            student.Id = studentFromDb.Id;

            student.FirstName = studentFromDb.FirstName;

            student.LastName = studentFromDb.LastName;
  
            
            foreach (StudentTeacherSubject sts in studentFromDb.STS)
            {
                SubjectWithTeacherAndGradesDTO swt = new SubjectWithTeacherAndGradesDTO(); 

                
            
                    swt.TeacherFirstName = sts.TeacherSubject.Teacher.FirstName;
                    swt.TeacherLastName = sts.TeacherSubject.Teacher.LastName;
                    swt.SubjectName = sts.TeacherSubject.Subject.Name.ToString();
                    
                    
                    foreach (Grades gr in sts.Grades)
                    {
                        swt.Grades.Add(gr.Value);

                    }


                student.SWTAG.Add(swt);

            }
            

            return student;


        }
    }
}