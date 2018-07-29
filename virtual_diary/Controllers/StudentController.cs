using NLog;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using virtual_diary.Filters;
using virtual_diary.Models;
using virtual_diary.Models.DTO;
using virtual_diary.Services;

namespace virtual_diary.Controllers
{
    [RoutePrefix("diary/students")]
    public class StudentController : ApiController
    {
        private IStudentService studentService;
        private IParentService parentService;
        private IClassService classService;
        private ISTSService stsService;
        private static Logger logger = LogManager.GetCurrentClassLogger();


        public StudentController(IStudentService studentService, IParentService parentService, IClassService classService, ISTSService stsService)
        {
            this.studentService = studentService;
            this.parentService = parentService;
            this.classService = classService;
            this.stsService = stsService;
        }

        // GET: diary/students
        [Authorize(Roles ="admins, teachers")]
        [Route("")]
        public IEnumerable<Student> GetStudents()
        {
            return studentService.GetStudents();
        }


        // GET: diary/students/5
        [Authorize(Roles ="admins, teachers, parents, students")]
        [ValidateModel]
        [Route("{id}")]
        public IHttpActionResult GetStudent(string id)
        {
            Student student = studentService.GetStudentById(id);
            string userId = ((ClaimsPrincipal)RequestContext.Principal).FindFirst(x => x.Type == "UserId").Value;
            string studentUsername = student.UserName;

            if (student == null)
            {
                logger.Warn("Search does not exist");
                return NotFound();
            }
            

            if (User.IsInRole("parents"))
            {
                if (userId == student.Parent.Id)
                {
                    return Ok(studentService.GetStudentById(id));
                }
                else
                {
                    return Ok(stsService.GetStudentInfo(studentUsername));
                }
            }

            if (User.IsInRole("students"))
            {
                if (userId == student.Id)
                {
                    return Ok(studentService.GetStudentById(id));
                }
                else
                {
                    return Ok(stsService.GetStudentInfo(studentUsername));
                }
            }

            return Ok(studentService.GetStudentById(id));
        }

        // GET: diary/students/username/sima
        [Authorize(Roles ="admins, teachers, parents, students")]
        [ValidateModel]
        [Route("username/{username}")]
        public IHttpActionResult GetStudentByUsername(string username)
        {
            Student student = studentService.GetStudentByUserName(username);
            string userId = ((ClaimsPrincipal)RequestContext.Principal).FindFirst(x => x.Type == "UserId").Value;
            

            if (student == null)
            {
                logger.Warn("Search does not exist");
                return NotFound();
            }


            if (User.IsInRole("parents"))
            {
                if (userId == student.Parent.Id)
                {
                    return Ok(studentService.GetStudentByUserName(username));
                }
                else
                {
                    return Ok(stsService.GetStudentInfo(username));
                }
            }

            if (User.IsInRole("students"))
            {
                if (userId == student.Id)
                {
                    return Ok(studentService.GetStudentByUserName(username));
                }
                else
                {
                    return Ok(stsService.GetStudentInfo(username));
                }
            }

            return Ok(studentService.GetStudentByUserName(username));
        }

        // POST: diary/students/register-student
        [Authorize(Roles ="admins")]
        [ValidateModel]
        [Route("register-student", Name = "PostStudent")]
        public async Task<IHttpActionResult> RegisterStudent(StudentDTO newStudent)
        {

            try
            {
                var result = await studentService.RegisterStudent(newStudent);

                return Created("PostStudent", newStudent);
            }
            catch (DbEntityValidationException ex)
            {
                logger.Error(ex, "Exception is thrown");
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
                return BadRequest("This UserName already exist");
            }
            catch (Exception e)
            {
                logger.Error(e, "Exception is thrown");
                return BadRequest(e.Message);
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        // PUT: diary/students/sima
        [Authorize(Roles ="admins")]
        [ValidateModel]
        [Route("{username}")]
        public IHttpActionResult PutStudent(string username, Student student)
        {
            try
            {
                studentService.PutStudent(username, student);
                return Ok(student);
            }
            catch (NullReferenceException)
            {
                return BadRequest("This username does not exist");
            }
        }



        // DELETE: diary/students/sima
        [Authorize(Roles = "admins")]
        [ValidateModel]
        [Route("{username}")]
        public IHttpActionResult DeleteStudent(string username)
        {

            try
            {
                var result = studentService.GetStudentByUserName(username);

                studentService.DeleteStudent(username);
                return Ok(result);
            }
            catch (ArgumentNullException)
            {
                return BadRequest("Student with this username does not exist");
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception is thrown");
                return BadRequest("An error occurred ");
            }
        }

        // PUT: diary/students/pavle/parent/jovan
        [Authorize(Roles ="admins")]
        [ValidateModel]
        [Route("{studentUser}/parent/{parentUser}", Name = "AddParent")]
        public IHttpActionResult PutParent (string studentUser, string parentUser)
        {
            Student student = studentService.GetStudentByUserName(studentUser);
            if (student == null)
            {
                logger.Warn("Search does not exist ");
                return NotFound();
            }

            Parent parent = parentService.GetParentByUserName(parentUser);
            if (parent == null)
            {
                logger.Warn("Search does not exist ");
                return NotFound();
            }
           

            studentService.PutParent(studentUser, parentUser);
            return CreatedAtRoute("AddParent", new { studentUser = student.UserName, parentUser = parent.UserName }, student);
        }

        // PUT: diary/students/sima/class/1
        [Authorize(Roles = "admins")]
        [ValidateModel]
        [Route("{studentUsername}/class/{classId:int}", Name ="AddClass")]
        public IHttpActionResult PutStudentInClass([FromUri] string studentUsername, [FromUri] int classId)
        {
            Student student = studentService.GetStudentByUserName(studentUsername);
            if (student == null)
            {
                logger.Warn("Search does not exist ");
                return NotFound();
            }

            Class classs = classService.GetClass(classId);
            if (classs == null)
            {
                logger.Warn("Search does not exist ");
                return NotFound();
            }


            studentService.PutStudentInClass(studentUsername, classId);
            return CreatedAtRoute("AddClass", new { studentUsername = student.UserName, classId = classs.Id }, student);
        }

        // GET: diary/students/nameandgrades/sima
        [Authorize(Roles ="admins, teachers, parents, students")]
        [ValidateModel]
        [Route("nameandgrades/{studentUsername}")]
        public IHttpActionResult GetNameANdGrades(string studentUsername)
        {
            Student student = studentService.GetStudentByUserName(studentUsername);
            string userId = ((ClaimsPrincipal)RequestContext.Principal).FindFirst(x => x.Type == "UserId").Value;
            


            if (student == null)
            {
                logger.Warn("Search does not exist");
                return NotFound();
            }


            if (User.IsInRole("parents"))
            {
                if (userId == student.Parent.Id)
                {
                    return Ok(studentService.GetStudentByUserName(studentUsername));
                }
                else
                {
                    return Ok(stsService.GetStudentInfo(studentUsername));
                }
            }

            if (User.IsInRole("students"))
            {
                if (userId == student.Id)
                {
                    return Ok(studentService.GetStudentByUserName(studentUsername));
                }
                else
                {
                    return Ok(stsService.GetStudentInfo(studentUsername));
                }
            }
            
            if (User.IsInRole("teachers"))
            {
                
                foreach (var stu in student.STS)
                {

                    string te = stu.TeacherSubject.TeacherId;
                    if (userId == te)
                    {
                        return Ok(studentService.GetStudentByUserName(studentUsername));
                    }
                    else
                    {
                        return Ok(stsService.GetStudentInfo(studentUsername));
                    }
                }
               
            }

            
            return Ok(studentService.GetNameAndGrades(studentUsername));
        }

    }
}
