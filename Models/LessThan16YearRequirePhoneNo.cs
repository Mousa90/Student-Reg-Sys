using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StudentRegSys.Models
{
    public class LessThan16YearRequirePhoneNo : ValidationAttribute   
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
           var student =  (Student)validationContext.ObjectInstance;

            var age = DateTime.Now.Year - student.DateofBirth.Value.Year;

            return ((student.Phone == "" || student.Phone == null) && age < 16) 
                ? new ValidationResult("Phone No is Required for the students that thier age is less than 16!") 
                :  ValidationResult.Success;
        }
    }
}