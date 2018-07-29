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
    public class SubjectService: ISubjectService
    {
        private IUnitOfWork db;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public SubjectService (IUnitOfWork db)
        {
            this.db = db;
        }

        public IEnumerable<SubjectDTO> GetSubjects()
        {
            IEnumerable<SubjectModel> subjects = db.SubjectRepository.Get();
            List<SubjectDTO> dtoList = new List<SubjectDTO>();
            
            foreach (var s in subjects)
            {
                SubjectDTO dto = new SubjectDTO();
                dto.Name = s.Name;
                dto.Fond = s.Fond;

                dtoList.Add(dto);
            }


            return dtoList;

            //return db.SubjectRepository.Get();
        }

        public SubjectModel GetSubject (int id)
        {
            return db.SubjectRepository.GetByID(id);
        }

        public SubjectModel GetSubjectByName (enumSubject name)
        {
            return db.SubjectRepository.Get().FirstOrDefault(x => x.Name == name);
        }

        public SubjectModel PostSubject (SubjectModel subject)
        {

            SubjectModel newSubject = new SubjectModel()
            {
                
                Name = subject.Name,
                Fond = subject.Fond
            };
         

            db.SubjectRepository.Insert(newSubject);
            db.Save();
            logger.Info("New Subject with name {0} is created", newSubject.Name.ToString());
            return newSubject;
        }

        public SubjectModel PutSubject (int id, SubjectModel subject)
        {
            SubjectModel updateSubject = db.SubjectRepository.GetByID(id);

            updateSubject.Name = subject.Name;
            updateSubject.Fond = subject.Fond;

            db.SubjectRepository.Update(updateSubject);
            db.Save();
            logger.Warn("Subject with name {0} is updated", updateSubject.Name.ToString());
            return updateSubject;
        }

        public bool DeleteSubject (enumSubject subjectName)
        {
            SubjectModel deleteSubject = db.SubjectRepository.Get().FirstOrDefault(x => x.Name == subjectName);
            db.SubjectRepository.Delete(deleteSubject);
            db.Save();
            logger.Warn("Subject with name {0} is deleted", deleteSubject.Name.ToString());
            return true;
        }

    }
}