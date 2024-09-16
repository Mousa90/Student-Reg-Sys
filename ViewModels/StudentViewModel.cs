using StudentRegSys.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentRegSys.ViewModels
{
    public class StudentViewModel
    {
        public Student Student { get; set; }
        public IEnumerable<Major> Majors { get; set; }

        // public Variables
        public bool IsAdd = false;

    }
}