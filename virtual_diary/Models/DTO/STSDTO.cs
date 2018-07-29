using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace virtual_diary.Models.DTO
{
    public class STSDTO
    {
        public STSDTO ()
        {
            Grades = new List<int>();
        }

        public string Student { get; set; }

        public string Teacher { get; set; }

        public string Subject { get; set; }

        public List<int> Grades { get; set; }
    }
}