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
    [RoutePrefix("diary/parents")]
    public class ParentController : ApiController
    {
        private IParentService parentService;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public ParentController (IParentService parentService)
        {
            this.parentService = parentService;
        }


        // GET: diary/parents
        [Authorize(Roles ="admins")]
        [Route("")]
        public IEnumerable<Parent> GetParents()
        {
            return parentService.GetParents();
        }


        // GET: diary/parents/5
        [Authorize(Roles ="admins, parents, teachers")]
        [ValidateModel]
        [Route("{id}")]
        public IHttpActionResult GetParent (string id)
        {
            Parent parent = parentService.GetParentById(id);
            string userId = ((ClaimsPrincipal)RequestContext.Principal).FindFirst(x => x.Type == "UserId").Value;
            if (parent == null)
            {
                logger.Warn("This search does not exist");
                return NotFound();
            }
            

            if (User.IsInRole("parents"))
            {
                if (userId == parent.Id)
                {
                    return Ok(parentService.GetParentById(id));
                }
                else
                {
                    return BadRequest("You are not authorized to see this content");
                }
            }

            return Ok(parentService.GetParentById(id));
        }


        // GET: diary/parents/username/sima
        [Authorize(Roles ="admins, teachers, parents")]
        [ValidateModel]
        [Route("username/{username}")]
        public IHttpActionResult GetParentByUsername(string username)
        {
            Parent parent = parentService.GetParentByUserName(username);
            string userId = ((ClaimsPrincipal)RequestContext.Principal).FindFirst(x => x.Type == "UserId").Value;
            if (parent == null)
            {
                logger.Warn("This search does not exist");
                return NotFound();
            }


            if (User.IsInRole("parents"))
            {
                if (userId == parent.Id)
                {
                    return Ok(parentService.GetParentByUserName(username));
                }
                else
                {
                    return BadRequest("You are not authorized to see this content");
                }
            }

            return Ok(parentService.GetParentById(username));
        }


        // POST: diary/parents/register-parent
        [Authorize(Roles ="admins")]
        [ValidateModel]
        [Route("register-parent", Name ="PostParent")]
        public async Task<IHttpActionResult> RegisterParent(ParentDTO newParent)
        {
         

            try
            {
                var result = await parentService.RegisterParent(newParent);

                return Created("PostParent", newParent);
            }
            catch (DbEntityValidationException ex)
            {
                logger.Error(ex);
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
                logger.Warn("Tryed to create parent with username that already exist");
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

        // PUT: diary/parents/sima
        [Authorize(Roles ="admins")]
        [ValidateModel]
        [Route("{username}")]
        public IHttpActionResult PutParent (string username, Parent parent)
        {
            try
            {
                parentService.PutParent(username, parent);
                return Ok(parent);
            }
            catch (NullReferenceException)
            {
                logger.Warn("Search not found");
                return BadRequest("This username does not exist");
            }
        }



        // DELETE: diary/parents/sima
        [Authorize(Roles = "admins")]
        [ValidateModel]
        [Route("{username}")]
        public IHttpActionResult DeleteParent(string username)
        {
           

            try
            {
                var result = parentService.GetParentByUserName(username);

                parentService.DeleteParent(username);
                return Ok(result);
            }
            catch (ArgumentNullException)
            {
                logger.Warn("This search does not exist");
                return BadRequest("Parent with this username does not exist");
            }
            catch (Exception e)
            {
                logger.Error(e, "Exception thrown");
                return BadRequest(e.Message);
            }
        }

        // GET: diary/parents/students/sima
        [Authorize(Roles ="admins, teachers, parents")]
        [ValidateModel]
        [Route("students/{parentUsername}")]
        public IHttpActionResult GetStudentsFromParent(string parentUsername)
        {
            Parent parent = parentService.GetParentByUserName(parentUsername);
            string userId = ((ClaimsPrincipal)RequestContext.Principal).FindFirst(x => x.Type == "UserId").Value;

            if (parent == null)
            {
                logger.Warn("This search does not exist");
                return NotFound();
            }

            if (User.IsInRole("parents"))
            {
                if (userId == parent.Id)
                {
                    return Ok(parentService.GetParentByUserName(parentUsername));
                }
                else
                {
                    return BadRequest("You are not authorized to see this content");
                }
            }


            return Ok(parentService.GetStudentsFromParent(parentUsername));
        }



    }
}
