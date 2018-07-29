using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace virtual_diary.Models.DTO
{
    public class StudentNLDTO
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public virtual ICollection<SubjectWithTeacherAndGradesDTO> SWTAG { get; set; }

        public StudentNLDTO ()
        {
            SWTAG = new List<SubjectWithTeacherAndGradesDTO>();
        }
    }
}