using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace virtual_diary.Models
{
    public class SubjectModel
    {
        public SubjectModel ()
        {
            TeachersSubjects = new List<TeacherSubject>();
        }

        public int Id { get; set; }
        
        [JsonConverter(typeof(StringEnumConverter))]
        public enumSubject Name { get; set; }

        public double Fond { get; set; }

        public virtual ICollection<TeacherSubject> TeachersSubjects { get; set; }

     
    }
}