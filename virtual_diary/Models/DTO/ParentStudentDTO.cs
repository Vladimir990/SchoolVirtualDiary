using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace virtual_diary.Models.DTO
{
    public class ParentStudentDTO
    {
        public string ParentFirstName { get; set; }

        public string ParentLastName { get; set; }

        public virtual ICollection<StudentFNLNDTO> Children { get; set; }

        public ParentStudentDTO ()
        {
            Children = new List<StudentFNLNDTO>();
        }
    }
}