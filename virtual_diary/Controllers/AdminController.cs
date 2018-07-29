using NLog;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
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
    [RoutePrefix("diary/admins")]
    public class AdminController : ApiController
    {
        private IAdminService adminService;
        private static Logger logger = LogManager.GetCurrentClassLogger();


        public AdminController(IAdminService adminService)
        {
            this.adminService = adminService;
        }

        // GET: diary/admins/logs
        [Authorize(Roles ="admins")]
        [Route("logs")]
        public List<string> GetLogs()
        {
            int counter = 0;
            string line;



           StreamReader file = new StreamReader(@"C:\Users\SuVla\Desktop\New folder\Digitalni dnevnik osnovne skole Projekat\virtual_diary\virtual_diary\logs\app-log.txt");
            List<string> txt = new List<string>();
            while ((line = file.ReadLine()) != null)

            {
                txt.Add(line);
                counter++;
            }

            file.Close();
            return txt;
           
        }


        // GET: diary/admins
        [Authorize(Roles = "admins")]
        [Route("")]
        public IEnumerable<Admin> GetAdmins()
        {
            return adminService.GetAdmins();
        }


        // GET: diary/admins/users
        [Authorize(Roles = "admins")]
        [Route("users")]
        public IEnumerable<UserModel> GetUsers()
        {
            return adminService.GetUsers();
        }


        // GET: diary/admins/5
        [Authorize(Roles = "admins")]
        [ValidateModel]
        [Route("{id}")]
        public IHttpActionResult GetAdminById(string id)
        {
            Admin admin = adminService.GetAdminById(id);
            if (admin == null)
            {
                logger.Warn("This search does not exist");
                return NotFound();
            }

            return Ok(admin);
        }


        // GET: diary/admins/username/sima
        [Authorize(Roles = "admins")]
        [ValidateModel]
        [Route("username/{username}")]
        public IHttpActionResult GetAdminByUsername(string username)
        {
            Admin admin = adminService.GetAdminByUserName(username);
           
            if (admin == null)
            {
                logger.Warn("This search does not exist");
                return NotFound();
            }
            return Ok(admin);
        }


        // GET: diary/admins/users/5
        [Authorize(Roles = "admins")]
        [ValidateModel]
        [Route("users/{id}")]
        public IHttpActionResult GetUserById(string id)
        {
            UserModel user = adminService.GetUserById(id);
            if (user == null)
            {
                logger.Warn("This search does not exist");
                return NotFound();
            }
            return Ok(user);
        }


        // POST: diary/admins/register-admin
        //[Authorize(Roles ="admins")]
        [ValidateModel]
        [Route("register-admin", Name = "PostAdmin")]
        public async Task<IHttpActionResult> RegisterAdmin(AdminDTO newAdmin)
        {
            try
            {
                var result = await adminService.RegisterAdmin(newAdmin);
                return Created("PostAdmin", newAdmin);
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
                logger.Warn("Tryed to create admin with username that alreday exist");
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



        // PUT: diary/admins/sima
        [Authorize(Roles = "admins")]
        [ValidateModel]
        [Route("{username}")]
        public IHttpActionResult PutAdmin ([FromUri]string username, Admin admin)
        {
           
            try
            {
                adminService.PutAdmin(username, admin);
                return Ok(admin);
            }
            catch(NullReferenceException)
            {
                logger.Warn("This search does not exist");
                return BadRequest("This username does not exist");
            }
        }


        // DELETE: diary/admins/sima
        [Authorize(Roles = "admins")]
        [ValidateModel]
        [Route("{username}")]
        public IHttpActionResult DeleteAdmin (string username)
        {
            try
            {
                var result = adminService.GetAdminByUserName(username);
                
                adminService.DeleteAdmin(username);
                return Ok(result);
            }
            catch (ArgumentNullException)
            {
                logger.Warn("This search does not exist");
                return BadRequest("Admin with this username does not exist");
            }
            catch (Exception e)
            {
                logger.Error(e);
                return BadRequest(e.Message);
            }
            
        }
    }
}
