using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cwiczenia3.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cwiczenia3.Controllers
{
    [Route("api/enrollments")]
    [ApiController]
    public class EnrollmentsController : ControllerBase
    {
        [HttpPost]
        public IActionResult CreateStudent(Student student)
        {
            //... add to database
            //... generating index number
            //student.IndexNumber = $"s{new Random().Next(1, 20000)}";
            //return Ok(student);
            //if (String.IsNullOrEmpty(student.IndexNumber) || )
            if (String.IsNullOrEmpty(student.IndexNumber))
            {
                return NotFound("Field IndexNumber not found!");
            } else if (String.IsNullOrEmpty(student.LastName))
            {
                return NotFound("Field LastName not found!");
            } else if (String.IsNullOrEmpty(student.StudiesName))
            {
                return NotFound("Field StudiesName not found!");
            }
            else
                return Ok(student.IndexNumber);
        }
    }
}