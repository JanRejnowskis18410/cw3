using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Cwiczenia3.DTOs;
using Cwiczenia3.Models;
using Cwiczenia3.Services;
using Microsoft.AspNetCore.Mvc;

namespace Cwiczenia3.Controllers
{
    [Route("api/enrollments")]
    [ApiController]
    public class EnrollmentsController : ControllerBase
    {
        private IStudentsDbService service;

        public EnrollmentsController(IStudentsDbService service)
        {
            this.service = service;
        }

        [HttpPost]
        public IActionResult EnrollStudent(EnrollStudentRequest request)
        {
            try
            {
                Enrollment result = service.EnrollStudent(request);
                return Created(nameof(result), result);
            } catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("promotions")]
        public IActionResult PromoteStudent(PromoteStudentRequest request)
        {
            try
            {
                Enrollment result = service.PromoteStudent(request);
                return Created(nameof(result), result);
            }
            catch (Exception e)
            {
                return NotFound(e);
            }
        }
    }
}