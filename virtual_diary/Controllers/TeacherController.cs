using NLog;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using virtual_diary.Filters;
using virtual_diary.Models;
using virtual_diary.Models.DTO;
using virtual_diary.Services;

namespace virtual_diary.Controllers
{
    [RoutePrefix("diary/teachers")]
    public class TeacherController : ApiController
    {
        private ITeacherService teacherService;
        private ISubjectService subjectService;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public TeacherController (ITeacherService teacherService, ISubjectService subjectService)
        {
            this.teacherService = teacherService;
            this.subjectService = subjectService;
            
        }

        // GET: diary/teachers
        [Authorize(Roles ="admins, teachers")]
        [Route("")]
        public IEnumerable<Teacher> GetTeachers()
        {
            return teacherService.GetTeachers();
        }

        // GET: diary/teachers/5
        [Authorize(Roles ="admins, teachers")]
        [ValidateModel]
        [Route("{id}")]
        public IHttpActionResult GetTeacher(string id)
        {
            Teacher teacher = teacherService.GetTeacherById(id);
            if (teacher == null)
            {
                logger.Warn("This search does not exist");
                return NotFound();
            }
            return Ok(teacher);
        }

        // GET: diary/teachers/username/sima
        [Authorize(Roles ="admins, teachers")]
        [ValidateModel]
        [Route("username/{username}")]
        public IHttpActionResult GetTeacherByUsername(string username)
        {
            Teacher teacher = teacherService.GetTeacherByUserName(username);
            if (teacher == null)
            {
                logger.Warn("This search does not exist");
                return NotFound();
            }
            return Ok(teacher);
        }

        // POST: diary/teachers/register-teacher
        [Authorize(Roles ="admins")]
        [ValidateModel]
        [Route("register-teacher", Name ="Post Teacher")]
        public async Task<IHttpActionResult> RegisterTeacher(TeacherDTO newTeacher)
        {
            try
            {
                var result = await teacherService.RegisterTeacher(newTeacher);
                return Created("Post Teacher", newTeacher);
            }
            catch (DbEntityValidationException ex)
            {

                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
            catch (InvalidOperationException)
            {
                logger.Error("Username already exist");
                return BadRequest("This UserName already exist");
            }
            catch (Exception e)
            {
                logger.Error(e, "Exception thrown");
                return BadRequest(e.Message);
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        // PUT: diary/teachers/sima
        [Authorize(Roles ="admins")]
        [ValidateModel]
        [Route("{username}")]
        public IHttpActionResult PutTeacher(string username, Teacher teacher)
        {
            try
            {
                teacherService.PutTeacher(username, teacher);
                return Ok(teacher);
            }
            catch (NullReferenceException)
            {
                return BadRequest("This username does not exist");
            }
        }


        // DELETE: diary/teachers/sima
        [Authorize(Roles = "admins")]
        [ValidateModel]
        [Route("{username}")]
        public IHttpActionResult DeleteTeacher(string username)
        {
            try
            {
                var result = teacherService.GetTeacherByUserName(username);

                teacherService.DeleteTeacher(username);
                return Ok(result);
            }
            catch (ArgumentNullException)
            {
                return BadRequest("Teacher with this username does not exist");
            }
            catch (Exception e)
            {
                logger.Error(e, "Exception is thrown");
                return BadRequest("An error occurred ");
            }
        }

    }
}

