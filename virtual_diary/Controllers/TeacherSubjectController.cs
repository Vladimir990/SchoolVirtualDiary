using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using virtual_diary.Filters;
using virtual_diary.Models;
using virtual_diary.Services;

namespace virtual_diary.Controllers
{
    [RoutePrefix("diary/teacher-subject")]
    public class TeacherSubjectController : ApiController
    {

        ITeacherSubjectService teacherSubjectService;
        ITeacherService teacherService;
        ISubjectService subjectService;
        private static Logger logger = LogManager.GetCurrentClassLogger();


        public TeacherSubjectController(ITeacherSubjectService teacherSubjectService, ITeacherService teacherService, ISubjectService subjectService)
        {
            this.teacherSubjectService = teacherSubjectService;
            this.teacherService = teacherService;
            this.subjectService = subjectService;
        }

        // GET: diary/teacher-subject
        [Authorize(Roles ="admins, teachers")]
        [Route("")]
        public IEnumerable<TeacherSubject> GetAll()
        {
            return teacherSubjectService.GetAll();
        }

        // POST: diary/teacher-subject
        [Authorize(Roles ="admins")]
        [ValidateModel]
        [Route("{teacherUsername}/{subjectId}", Name = "PostTeacherSubjet")]
        public IHttpActionResult PostTeacherSubject(string teacherUsername, int subjectId)
        {

        

            Teacher teacherIn = teacherService.GetTeacherByUserName(teacherUsername);
            if (teacherIn == null)
            {
                logger.Warn("This search does not exist");
                return NotFound();
                
            }

            SubjectModel subjectIn = subjectService.GetSubject(subjectId);
            if (subjectIn == null)
            {
                logger.Warn("This search does not exist");
                return NotFound();
            }


            TeacherSubject ts = teacherIn.TeachersSubjects.FirstOrDefault();
            

            if (ts.SubjectId == subjectIn.Id)
            {
                logger.Warn("This combination already exist");
                return BadRequest("This combination already exist");
            }


            return Created("PostTeacherSubject", teacherSubjectService.PostTeacherSubject(teacherUsername, subjectId));
        }

        // DELETE: diary/teacher-subject/1
        [Authorize(Roles ="admins")]
        [ValidateModel]
        [Route("{id}")]
        public IHttpActionResult DeleteTS (int id)
        {
            TeacherSubject ts = teacherSubjectService.GetTeacherSubject(id);
            if(ts == null)
            {
                logger.Warn("This search does not exist");
                return NotFound();
            }

            return Ok(teacherSubjectService.DeleteTS(id));
        }

        // GET: diary/teacher-subject/1
        [Authorize(Roles ="admins, teachers")]
        [Route("{id:int}")]
        public IHttpActionResult GetTSbyId (int id)
        {
            TeacherSubject ts = teacherSubjectService.GetTeacherSubject(id);
            if (ts == null)
            {
                logger.Warn("This search does not exist");
                return NotFound();
            }

            return Ok(ts);
        }

        // GET: diary/teacher-subject/sima
        [Authorize(Roles = "admins, teachers")]
        [Route("{teacherUsername}")]
        public IEnumerable<TeacherSubject> GetTSByTeacher(string teacherUsername)
        {
            Teacher teacher = teacherService.GetTeacherByUserName(teacherUsername);
            if (teacher == null)
            {
                logger.Warn("This search does not exist");
                return null;
            }

            return teacherSubjectService.GetTSByTeacher(teacherUsername);
        }

        // GET: diary/teacher-subject/subject/1
        [Authorize(Roles = "admins, teachers")]
        [Route("subject/{id:int}")]
        public IEnumerable<TeacherSubject> GetTSBySubject(int id)
        {
            SubjectModel subject = subjectService.GetSubject(id);
            if (subject == null)
            {
                logger.Warn("This search does not exist");
                return null;
            }

            return teacherSubjectService.GetTSBySubject(id);
        }
    }
}
