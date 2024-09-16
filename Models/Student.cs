using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentRegSys.Models
{
    public class Student
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [MaxLength(255)]
        [Display(Name = "Full Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Date of Birth")]

        public DateTime? DateofBirth { get; set; }

        [Required]
        public Gender Gender { get; set; }

        [MaxLength(255)]
        public string Address { get; set; }

        [MaxLength(255)]
        [Phone]
        [LessThan16YearRequirePhoneNo]
        public string Phone { get; set; }

        public string Photo { get; set; }

        [ForeignKey("Major")]
        [Display(Name = "Major")]
        public byte MajorID { get; set; }
        public Major Major { get; set; }

        // Static Variables
        public static readonly string Male = "Male";
        public static readonly string Female = "Female";
    }
}