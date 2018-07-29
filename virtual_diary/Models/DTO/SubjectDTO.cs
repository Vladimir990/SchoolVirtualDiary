using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace virtual_diary.Models.DTO
{
    public class SubjectDTO
    {
        public SubjectDTO()
        {

        }


        [Required]
        [JsonConverter(typeof(StringEnumConverter))]
        public enumSubject Name { get; set; }
        [Required]
        public double Fond { get; set; }

    }
}