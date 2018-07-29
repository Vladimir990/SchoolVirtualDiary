using NLog;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using virtual_diary.Filters;
using virtual_diary.Models;
using virtual_diary.Services;

namespace virtual_diary.Controllers
{
    [RoutePrefix("diary/classes")]
    public class ClassController : ApiController
    {
        private IClassService classService;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public ClassController(IClassService classService)
        {
            this.classService = classService;
        }


        // GET: diary/Classes
        [AllowAnonymous]
        [Route("")]
        public IEnumerable<Class> GetClasses()
        {
            return classService.GetClasses();
        }

        // GET: diary/Classes/5
        [Authorize(Roles = "admins, teachers")]
        [Route("{id}")]
        public IHttpActionResult GetClass(int id)
        {
            Class classs = classService.GetClass(id);
            if (classs == null)
            {
                logger.Warn("This search does not exist");
                return NotFound();
            }
            return Ok(classs);
        }


        // GET: diary/Classes/name/name
        [Authorize(Roles = "admins, teachers")]
        [Route("name/{name}")]
        public IHttpActionResult GetClassByName(enumClass name)
        {
            Class classs = classService.GetClassByName(name);
            if (classs == null)
            {
                logger.Warn("This search does not exist");
                return NotFound();
            }

            return Ok(classs);
        }


        // POST: diary/Classes
        [Authorize(Roles ="admins")]
        [ValidateModel]
        [Route("", Name = "PostClass")]
        public IHttpActionResult PostClass(Class classs)
        {

            try
            {
                Class IsInDb = classService.GetClassByName(classs.Name);
                if (IsInDb.Name == classs.Name)
                {

                    return BadRequest("This class already exist");
                }
            }
            catch (NullReferenceException)
            {
                classService.PostClass(classs);
            }
            

                return Ok(classs);
        }


        // PUT: diary/Classes/5
        [Authorize(Roles ="admins")]
        [ValidateModel]
        [Route("{id:int}")]
        public IHttpActionResult PutClass(int id, Class classs)
        {
            try
            {
                classService.PutClass(id, classs);
                return Ok(classs);
            }
            catch (NullReferenceException)
            {
                logger.Warn("This search does not exist");
                return BadRequest("Class with this ID does not exist");
            }
        }


        // DELETE: diary/Classes/5
        [Authorize(Roles = "admins")]
        [ValidateModel]
        [Route("{id:int}")]
        public IHttpActionResult DeleteClass(int id)
        {
            try
            {
                Class classs = classService.GetClass(id);
          
                classService.DeleteClass(id);
                return Ok(classs);
            }
            catch (ArgumentNullException)
            {
                logger.Warn("This search does not exist");
                return BadRequest("Class with this ID does not exist");
            }
        }


        // GET: diary/classes/students/1
        [Authorize(Roles ="admins, teachers, parents")]
        [ValidateModel]
        [Route("students/{classId}")]
        public IHttpActionResult GetStudentsByClass(int classId)
        {
            Class classFromDb = classService.GetClass(classId);
            if (classFromDb == null)
            {
                logger.Warn("This search does not exist");
                return NotFound();
            }

            return Ok(classService.GetStudentsByClass(classId));
        }
    }
}
