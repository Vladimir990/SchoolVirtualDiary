using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace virtual_diary.Models.DTO
{
    public class TeacherSubjectDTO
    {
        public string TeacherFirstName { get; set; }

        public string TeacherLastName { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public enumSubject SubjectName { get; set; }
    }
}