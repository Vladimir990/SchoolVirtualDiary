using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace virtual_diary.Models
{
    
    public class TeacherSubject
    {
        public TeacherSubject()
        {
            STS = new List<StudentTeacherSubject>();
        }

        
        
        public int Id { get; set; }

        
        public string TeacherId { get; set; }

        public virtual Teacher Teacher { get; set; }

       
        public int SubjectId { get; set; }
       
        public virtual SubjectModel Subject { get; set; }

        public virtual ICollection<StudentTeacherSubject> STS { get; set; }
    }
}