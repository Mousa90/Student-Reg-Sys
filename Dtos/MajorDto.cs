using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StudentRegSys.Dtos
{
    public class MajorDto
    {
        public byte ID { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }
    }
}