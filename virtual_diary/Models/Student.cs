using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace virtual_diary.Models
{
    public class Student:UserModel
    {
        public Student ()
        {
            STS = new List<StudentTeacherSubject>();
        }

        public DateTime DateOfBirth { get; set; }

        public virtual Class Class { get; set; }

        public virtual ICollection<StudentTeacherSubject> STS { get; set; }

        public virtual Parent Parent { get; set; }
        

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<UserModel> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}