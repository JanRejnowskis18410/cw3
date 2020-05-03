using Cwiczenia3.DTOs;
using Cwiczenia3.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cwiczenia3.Services
{
    public interface IStudentsDbService
    {
        Enrollment EnrollStudent(EnrollStudentRequest request);
        Enrollment PromoteStudent(PromoteStudentRequest request);
        Student GetStudent(string index);
        Student GetLoggingStudent(StudentLoginRequest request);
    }
}
