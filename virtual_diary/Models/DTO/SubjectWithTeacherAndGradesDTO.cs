using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace virtual_diary.Models.DTO
{
    public class SubjectWithTeacherAndGradesDTO
    {
     
        public string TeacherFirstName { get; set; }

        public string TeacherLastName { get; set; }

        public string SubjectName { get; set; }

        public ICollection<int> Grades { get; set; }

        public SubjectWithTeacherAndGradesDTO ()
        {
            Grades = new List<int>();
        }
    }
}