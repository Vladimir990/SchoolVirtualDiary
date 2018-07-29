using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace virtual_diary.Models
{
    public class Class
    {
        public Class()
        {
            
            Students = new List<Student>();
        }

        public int Id { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public enumClass Name { get; set; }

        public virtual ICollection<Student> Students { get; set; }

       
    }
}