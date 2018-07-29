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
    public class STSService: ISTSService
    {
        private IUnitOfWork db;
        IEmailService emailService;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public STSService (IUnitOfWork db, IEmailService emailService)
        {
            this.db = db;
            this.emailService = emailService;
        }

        public IEnumerable<StudentTeacherSubject> GetAllSTS ()
        {
            return db.STSRepository.Get();
        }

        public StudentTeacherSubject GetSTSByID (int id)
        {
            return db.STSRepository.GetByID(id);
        }

        public STSDTO PostSTS(string studentUsername, int teacherSubjectId)
        {
            Student student = db.StudentRepository.Get().FirstOrDefault(x => x.UserName == studentUsername);
            TeacherSubject ts = db.TeacherSubjectRepository.GetByID(teacherSubjectId);

            StudentTeacherSubject newSTS = new StudentTeacherSubject()
            {
                StudentId = student.Id,
                Student = student,
                TeacherSubjectId = ts.Id,
                TeacherSubject = ts,

            };

            STSDTO newDTo = new STSDTO()
            {
                Student = string.Format("{0} {1}", student.FirstName, student.LastName),
                Teacher = string.Format("{0} {1}", ts.Teacher.FirstName, ts.Teacher.LastName),
                Subject = ts.Subject.Name.ToString()
               
            };

            db.STSRepository.Insert(newSTS);
            db.Save();
            logger.Info("New Student-Teacher-Subejct with id {0} is created", newSTS.Id);
            return newDTo;
        }

        public StudentTeacherSubject PutSemesterToSTS(int id, StudentTeacherSubject sts)
        {
            StudentTeacherSubject stsUpdate = db.STSRepository.GetByID(id);

            stsUpdate.Semester = sts.Semester;

            db.STSRepository.Update(stsUpdate);
            db.Save();
            logger.Warn("STS with id {0} is updated", stsUpdate.Id);
            return stsUpdate;

        }

        public bool DeleteSTS(int id)
        {
            StudentTeacherSubject sts = db.STSRepository.GetByID(id);

            db.STSRepository.Delete(sts);
            db.Save();
            logger.Warn("STS with id {0} is deleted", sts.Id);
            return true;
        }

        public STSDTO PutGrades(int id, int gradeId)
        {
            
            StudentTeacherSubject stsUpdate = db.STSRepository.GetByID(id);
            STSDTO sts = new STSDTO();

            sts.Student = string.Format("{0} {1}", stsUpdate.Student.FirstName, stsUpdate.Student.LastName);
            sts.Teacher = string.Format("{0} {1}", stsUpdate.TeacherSubject.Teacher.FirstName, stsUpdate.TeacherSubject.Teacher.LastName);
            sts.Subject = stsUpdate.TeacherSubject.Subject.Name.ToString();
            
            Grades grade = new Grades()
            {
                Value = gradeId
                
            };
            stsUpdate.Grades.Add(grade);
            sts.Grades.Add(grade.Value);

            db.STSRepository.Update(stsUpdate);
            db.Save();
            return sts;
        }

        public IEnumerable<StudentTeacherSubject> GetSTSByStudent(string studentUsername)
        {
            Student student = db.StudentRepository.Get().FirstOrDefault(x => x.UserName == studentUsername);

            List<StudentTeacherSubject> stsList = new List<StudentTeacherSubject>();
            foreach (var s in student.STS)
            {
                
                stsList.Add(s);
            }

            return stsList;
        }

        public ICollection<int> GetGrades (int stsId)
        {
            StudentTeacherSubject sts = db.STSRepository.GetByID(stsId);


            List<int> grades = new List<int>();
            foreach (var grade in sts.Grades)
            {
                grades.Add(grade.Value);
            }

            return grades;
        }

        public StudentFNLNDTO GetStudentInfo(string studentUsername)
        {
            Student student = db.StudentRepository.Get().FirstOrDefault(x => x.UserName == studentUsername);
            StudentFNLNDTO st = new StudentFNLNDTO();
            st.FirstName = student.FirstName;
            st.LastName = student.LastName;
            st.Class = student.Class.Name.ToString();

            return st;
        }

    }
}