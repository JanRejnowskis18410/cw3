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
        private static String ConString = "Data Source=db-mssql;Initial Catalog=s18410;Integrated Security=True";
        private IStudentsDbService service = new Sql_ServerDbService(ConString);

        [HttpPost]
        public IActionResult EnrollStudent(EnrollStudentRequest request)
        {
            return service.EnrollStudent(request);
        }

        [HttpPost("promotions")]
        public IActionResult PromoteStudent(PromoteStudentRequest request)
        {
            return service.PromoteStudent(request);
        }
    }
}