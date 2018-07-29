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
    
    public class TeacherSubjectService: ITeacherSubjectService
    {
        private IUnitOfWork db;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public TeacherSubjectService (IUnitOfWork db)
        {
            this.db = db;
        }

        public IEnumerable<TeacherSubject> GetAll ()
        {
            return db.TeacherSubjectRepository.Get();
        }

        public TeacherSubjectDTO PostTeacherSubject(string teacherUsername, int subjectId)
        {
            Teacher teacherIn = db.TeacherRepository.Get().FirstOrDefault(x => x.UserName == teacherUsername);
            SubjectModel subjectIn = db.SubjectRepository.GetByID(subjectId);
            


            TeacherSubject newTS = new TeacherSubject()
            {
                
                Teacher = teacherIn,
                TeacherId = teacherIn.Id,
                Subject = subjectIn,
                SubjectId = subjectIn.Id

            };

            TeacherSubjectDTO newDTo = new TeacherSubjectDTO()
            {
                TeacherFirstName = teacherIn.FirstName,
                TeacherLastName = teacherIn.LastName,
                SubjectName = subjectIn.Name
            };


            db.TeacherSubjectRepository.Insert(newTS);
            
            db.Save();
            logger.Info("New teacher-subject with id {0} is created", newTS.Id);
            return newDTo;

        }


        public TeacherSubject DeleteTS (int id)
        {
            TeacherSubject ts = db.TeacherSubjectRepository.GetByID(id);
            db.TeacherSubjectRepository.Delete(ts);
            db.Save();
            logger.Warn("Teacher-subject with id {0} is deleted", ts.Id);
            return ts;

        }

        public TeacherSubject GetTeacherSubject (int tsId)
        {
            return db.TeacherSubjectRepository.GetByID(tsId);
        }

        public IEnumerable<TeacherSubject> GetTSByTeacher(string teacherUsername)
        {
            Teacher teacher = db.TeacherRepository.Get().FirstOrDefault(x => x.UserName == teacherUsername);
            TeacherSubject findByTeacher = db.TeacherSubjectRepository.Get().FirstOrDefault(x => x.Teacher.UserName == teacherUsername);
            List<TeacherSubject> TSList = new List<TeacherSubject>();
            foreach (var t in findByTeacher.Teacher.TeachersSubjects)
            {
                TSList.Add(t);
            }

            return TSList;
        }

        public IEnumerable<TeacherSubject> GetTSBySubject(int id)
        {
            SubjectModel subject = db.SubjectRepository.GetByID(id);
            TeacherSubject findBySubject = db.TeacherSubjectRepository.Get().FirstOrDefault(x => x.Subject.Id == id);
            List<TeacherSubject> TSList = new List<TeacherSubject>();
            foreach (var s in findBySubject.Subject.TeachersSubjects)
            {
                TSList.Add(s);
            }

            return TSList;
        }


    }
}