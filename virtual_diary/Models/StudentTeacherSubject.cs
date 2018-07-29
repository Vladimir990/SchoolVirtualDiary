using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace virtual_diary.Models
{
    public class StudentTeacherSubject
    {
        
        public int Id { get; set; }

        
        public string StudentId { get; set; }

        
        public virtual Student Student { get; set; }

        
        public int TeacherSubjectId { get; set; }

        
        public virtual TeacherSubject TeacherSubject { get; set; }

        public virtual ICollection<Grades> Grades { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public enumSemester Semester { get; set; }

        public StudentTeacherSubject()
        {
            Grades = new List<Grades>();
        }
    }
}