using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using virtual_diary.Filters;
using virtual_diary.Models;
using virtual_diary.Models.DTO;
using virtual_diary.Services;

namespace virtual_diary.Controllers
{
    [RoutePrefix("diary/subjects")]
    public class SubjectController : ApiController
    {
        private ISubjectService subjectService;
        private ITeacherService teacherService;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public SubjectController(ISubjectService subjectService, ITeacherService teacherService) 
        {
            this.subjectService = subjectService;
            this.teacherService = teacherService;
            
        }

        // GET: diary/subjects
        [AllowAnonymous]
        [Route("")]
        public IEnumerable<SubjectDTO> GetSubjects()
        {
            return subjectService.GetSubjects();
        }

        // GET: diary/subjects/5
        [AllowAnonymous]
        [Route("{id}")]
        public IHttpActionResult GetSubject(int id)
        {
            SubjectModel subject = subjectService.GetSubject(id);
            if (subject == null)
            {
                logger.Warn("This search does not exist");
                return NotFound();
            }
            return Ok(subject);
        }

        // GET: diary/subjects/name/name
        [AllowAnonymous]
        [Route("name/{name}")]
        public IHttpActionResult GetSubjectByName (enumSubject name)
        {
            SubjectModel subject = subjectService.GetSubjectByName(name);
            if (subject == null)
            {
                logger.Warn("This search does not exist");
                return NotFound();
            }

            return Ok(subject);
        }

        // POST: diary/subjects
        [ValidateModel]
        [Authorize(Roles ="admins")]
        [Route("", Name = "PostSubject")]
        [ResponseType(typeof(SubjectModel))]
        public IHttpActionResult PostSubject(SubjectModel subject)
        {

            try
            {
                SubjectModel IsInDb = subjectService.GetSubjectByName(subject.Name);
                if (IsInDb.Name == subject.Name)
                {

                    return BadRequest("This subject already exist");
                }
            }
            catch (NullReferenceException)
            {
                subjectService.PostSubject(subject);
            }


            return Ok(subject);

        }

        // PUT: diary/subjects/name
        [Authorize(Roles ="admins")]
        [ValidateModel]
        [Route("{id:int}")]
        public IHttpActionResult PutSubject(int id,SubjectModel subject)
        {
            try
            {
                subjectService.PutSubject(id, subject);
                return Ok(subject);
            }
            catch (NullReferenceException)
            {
                logger.Warn("This search does not exist");
                return BadRequest("Subject with this ID does not exist");
            }
        }

        // DELETE: diary/subjects/name
        [Authorize(Roles = "admins")]
        [ValidateModel]
        [Route("{name}")]
        public IHttpActionResult DeleteSubject(enumSubject name)
        {
            try
            {
                SubjectModel subject = subjectService.GetSubjectByName(name);
                subjectService.DeleteSubject(name);
                return Ok(subject);
            }
            catch(ArgumentNullException)
            {
                return BadRequest("Subject with this name does not exist");
            }
            catch (Exception e)
            {
                logger.Error(e);
                return BadRequest(e.Message);
            }
        }

    }
}
