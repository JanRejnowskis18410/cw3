﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cwiczenia3.Models
{
    public class Student
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IndexNumber { get; set; }
        public DateTime BirthDate { get; set; }
        public string StudiesName { get; set; }
        public int Semester { get; set; }
        public int IdEnrollment { get; set; }
    }
}
