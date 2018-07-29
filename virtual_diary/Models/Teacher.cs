using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace virtual_diary.Models
{
    public class Teacher:UserModel
    {
        public Teacher()
        {
            TeachersSubjects = new List<TeacherSubject>();
        }

        
        public override string PhoneNumber { get; set; }
        public virtual ICollection<TeacherSubject> TeachersSubjects { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<UserModel> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}