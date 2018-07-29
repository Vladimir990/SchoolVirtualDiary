using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.OAuth;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using virtual_diary.Filters;
using virtual_diary.Infrastructure;
using virtual_diary.Models;
using virtual_diary.Repositories;
using virtual_diary.Services;

namespace virtual_diary.Controllers
{
    [RoutePrefix("diary/sts")]
    public class STSController : ApiController
    {
        ISTSService stsService;
        IStudentService studentService;
        ITeacherSubjectService teacherSubjectService;
        IEmailService emailService;
        IUnitOfWork db;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        



        public STSController (ISTSService stsService, IStudentService studentService, ITeacherSubjectService teacherSubjectService, IEmailService emailService ,IUnitOfWork db)
        {
            this.stsService = stsService;
            this.studentService = studentService;
            this.teacherSubjectService = teacherSubjectService;
            this.emailService = emailService;
            this.db = db;
            
        }

        // GET: diary/sts
        [Authorize(Roles ="admins")]
        [Route("")]
        public IEnumerable<StudentTeacherSubject> GetAllSTS()
        {
            return stsService.GetAllSTS();
        }


        // GET: diary/sts/1
        [Authorize(Roles ="admins")]
        [Route("{id}")]
        public IHttpActionResult GetSTSByID(int id)
        {
            StudentTeacherSubject sts = stsService.GetSTSByID(id);
            if (sts == null)
            {
                logger.Warn("This search does not exist");
                return NotFound();
            }
            return Ok(sts);
        }


        // POST: diary/sts/pera/1
        [Authorize(Roles ="admins")]
        [ValidateModel]
        [Route("{studentUsername}/{teacherSubjectId}", Name ="PostSTS")]
        public IHttpActionResult PostSTS(string studentUsername, int teacherSubjectId)
        {
            Student student = studentService.GetStudentByUserName(studentUsername);
            if (student == null)
            {
                logger.Warn("This search does not exist");
                return NotFound();
            }
            TeacherSubject ts = teacherSubjectService.GetTeacherSubject(teacherSubjectId);
            if (ts == null)
            {
                logger.Warn("This search does not exist");
                return NotFound();
            }

            return Created("PostSTS", stsService.PostSTS(studentUsername, teacherSubjectId));            
        }


        // PUT: diary/sts/1
        [Authorize(Roles ="admins")]
        [ValidateModel]
        [Route("{id}")]
        public IHttpActionResult PutSemesterToSTS(int id, StudentTeacherSubject sts)
        {
            StudentTeacherSubject stsUpdate = stsService.GetSTSByID(id);
            if (stsUpdate == null)
            {
                logger.Warn("This search does not exist");
                return NotFound();
            }

            return Ok(stsService.PutSemesterToSTS(id, sts));
        }

        // PUT: diary/sts/1/grades
        [Authorize(Roles ="admins, teachers")]
        [ValidateModel]
        [Route("{stsId}/grades")]
        public IHttpActionResult PutGradesToSTS(int stsId, [FromBody] Grades grade)
        {


            try
            {
                StudentTeacherSubject stsUpdate = stsService.GetSTSByID(stsId);

                string userId = ((ClaimsPrincipal)RequestContext.Principal).FindFirst(x => x.Type == "UserId").Value;

                int gradeId = grade.Value;
                emailService.SendMail(stsUpdate, grade.Value);

                if (User.IsInRole("teachers"))
                {
                    if (userId == stsUpdate.TeacherSubject.TeacherId)
                    {
                        return Ok(stsService.PutGrades(stsId, gradeId));
                    }
                    else
                    {
                        logger.Warn("Unauthorized user");
                        return BadRequest("You are not authorised to give a grade to this student");
                    }
                }

               

                return Ok(stsService.PutGrades(stsId, gradeId));
            }
            catch (NullReferenceException)
            {
                logger.Warn("STS with this id does not exist");
                return NotFound();
            }
            catch (Exception e)
            {
                logger.Error(e, "Exception thrown");
                return BadRequest(e.Message);
            }

        }

        // DELETE diary/sts/1
        [Authorize(Roles ="admins")]
        [ValidateModel]
        [Route("{id}")]
        public IHttpActionResult DeleteSTS(int id)
        {
            StudentTeacherSubject sts = stsService.GetSTSByID(id);
            if (sts == null)
            {
                logger.Warn("This search does not exist");
                return NotFound();
            }

            stsService.DeleteSTS(id);
            return Ok(sts);
        }

        // GET: diary/sts/student/sima
        [Authorize(Roles ="admins, teachers,parents, students")]
        [ValidateModel]
        [Route("student/{studentUsername}")]
        public IHttpActionResult GetSTSByStudent(string studentUsername)
        {

            try
            {
                Student student = studentService.GetStudentByUserName(studentUsername);

                string userId = ((ClaimsPrincipal)RequestContext.Principal).FindFirst(x => x.Type == "UserId").Value;

                if (User.IsInRole("parents"))
                {
                    if (userId == student.Parent.Id)
                    {
                        return Ok(studentService.GetNameAndGrades(studentUsername));
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
                        return Ok(studentService.GetNameAndGrades(studentUsername));
                    }
                    else
                    {
                        return Ok(stsService.GetStudentInfo(studentUsername));
                    }
                }

                return Ok(studentService.GetNameAndGrades(studentUsername));
            }
            catch (NullReferenceException)
            {
                logger.Warn("Student with this username does not exist");
                return NotFound();
            }
            catch (Exception e)
            {
                logger.Error(e, "Exception thrown");
                return BadRequest(e.Message);
            }



            
        }

        // GET: diary/sts/grades/1
        [Authorize(Roles ="admins, teachers, parents, students")]
        [ValidateModel]
        [Route("grades/{stsId}")]
        public IHttpActionResult GetGrades(int stsId)
        {

            try
            {
                StudentTeacherSubject sts = stsService.GetSTSByID(stsId);

                string userId = ((ClaimsPrincipal)RequestContext.Principal).FindFirst(x => x.Type == "UserId").Value;

                if (User.IsInRole("parents"))
                {
                    if (userId == sts.Student.Parent.Id)
                    {
                        return Ok(stsService.GetGrades(stsId));
                    }
                    else
                    {
                        logger.Warn("Unauthorized user");
                        return BadRequest("You are not authorised to see this contetnt");
                    }
                }

                if (User.IsInRole("students"))
                {
                    if (userId == sts.StudentId)
                    {
                        return Ok(stsService.GetGrades(stsId));
                    }
                    else
                    {
                        logger.Warn("Unauthorized user");
                        return BadRequest("You are not authorised to see this contetnt");
                    }
                }

                if (User.IsInRole("teachers"))
                {
                    if (userId == sts.TeacherSubject.TeacherId)
                    {
                        return Ok(stsService.GetGrades(stsId));
                    }
                    else
                    {
                        logger.Warn("Unauthorized user");
                        return BadRequest("You are not authorised to see this contetnt");
                    }
                }

                return Ok(stsService.GetGrades(stsId));
            }
            catch (NullReferenceException)
            {
                logger.Warn("STS with this id does not exist");
                return NotFound();
            }
            catch (Exception e)
            {
                logger.Error(e, "Exception thrown");
                return BadRequest(e.Message);
            }

        }
    }
}
