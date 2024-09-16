using StudentRegSys.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StudentRegSys.Dtos
{
    public class StudentDto
    {
        public int ID { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        public DateTime? DateofBirth { get; set; }

        [Required]
        public Gender Gender { get; set; }

        [MaxLength(255)]
        public string Address { get; set; }

        [MaxLength(255)]
        public string Phone { get; set; }

        public string Photo { get; set; }

        public byte MajorID { get; set; }
    }
}