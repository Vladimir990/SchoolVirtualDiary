using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace virtual_diary.Models.DTO
{
    public class ClassDTO
    {
        public string ClassName { get; set; }

        public virtual ICollection<StudentFNLNDTO> Students { get; set; }

        public ClassDTO ()
        {
            Students = new List<StudentFNLNDTO>();
        }
    }
}