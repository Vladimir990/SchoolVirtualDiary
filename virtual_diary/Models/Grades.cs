using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace virtual_diary.Models
{
    public class Grades
    {
        public Grades ()
        {
            
        }

        public int Id { get; set; }

        [Range(1,5,ErrorMessage ="Grade must be in range 1 to 5")]
        public int Value { get; set; }

        public virtual StudentTeacherSubject STS { get; set; }
    }
}