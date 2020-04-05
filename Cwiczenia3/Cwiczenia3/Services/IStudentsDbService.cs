using Cwiczenia3.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cwiczenia3.Services
{
    interface IStudentsDbService
    {
        IActionResult EnrollStudent(EnrollStudentRequest request);
        IActionResult PromoteStudent(PromoteStudentRequest request);
    }
}
