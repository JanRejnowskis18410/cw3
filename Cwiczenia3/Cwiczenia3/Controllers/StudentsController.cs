using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Cwiczenia3.DAL;
using Cwiczenia3.DTOs;
using Cwiczenia3.Models;
using Cwiczenia3.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Cwiczenia3.Controllers
{
    [Route("api/students")]
    [ApiController]
    public class StudentsController : ControllerBase
    {

        private IStudentsDbService service;
        private String ConnString = "Data Source=db-mssql;Initial Catalog=s18410;Integrated Security=true";
        private IConfiguration configuration;
        public StudentsController(IDbService dbService, IConfiguration configuration, IStudentsDbService service)
        {
            this.configuration = configuration;
            this.service = service;
        }

        [HttpGet]
        [Authorize(Roles = "student")]
        public IActionResult GetStudents()
        {
            var result = new List<Student>();

            using (SqlConnection con = new SqlConnection(ConnString))
            using (SqlCommand com = new SqlCommand())   
            {
                com.Connection = con;
                com.CommandText = "SELECT FirstName, LastName, BirthDate, Name, Semester FROM Student JOIN Enrollment ON Student.IdEnrollment = Enrollment.IdEnrollment JOIN Studies ON Enrollment.IdStudy = Studies.IdStudy";

                con.Open();
                SqlDataReader dr = com.ExecuteReader();
                while(dr.Read())
                {
                    var st = new Student();
                    st.FirstName = dr["FirstName"].ToString();
                    st.LastName = dr["LastName"].ToString();
                    st.BirthDate = DateTime.Parse(dr["BirthDate"].ToString());
                    st.StudiesName = dr["Name"].ToString();
                    st.Semester = Int32.Parse(dr["Semester"].ToString());
                    
                    result.Add(st);
                }
            }
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize]
        public IActionResult GetStudentEnrollment(int id)
        {
            var result = new Enrollment();
            using (SqlConnection con = new SqlConnection(ConnString))
            using (SqlCommand com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = "select Enrollment.IdEnrollment, Semester, IdStudy, StartDate from Enrollment join Student on Student.IdEnrollment = Enrollment.IdEnrollment WHERE IndexNumber=@id";

                /*
                SqlParameter par = new SqlParameter();
                par.Value = id;
                par.ParameterName = "id";
                com.Parameters.Add(par);
                */

                com.Parameters.AddWithValue("id", id);

                con.Open();
                SqlDataReader dr = com.ExecuteReader();
                while (dr.Read())
                {
                    result.IdEnrollment = Int32.Parse(dr["IdEnrollment"].ToString());
                    result.Semester = Int32.Parse(dr["Semester"].ToString());
                    result.IdStudy = Int32.Parse(dr["IdStudy"].ToString());
                    result.StartDate = DateTime.Parse(dr["StartDate"].ToString());
                }
            }
            return Ok(result);
        }

        [HttpPut("{id}")]
        [Authorize]
        public IActionResult UpdateStudents()
        {
            return Ok("Aktualizacja dokończona");
        }

        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult deleteStudents()
        {
            return Ok("Usuwanie ukończone");
        }

        [HttpPost("login")]
        public IActionResult Login(StudentLoginRequest request)
        {
            var student = service.GetLoggingStudent(request);
            if (student == null)
                return Unauthorized("Student o podanym indeksie i haśle nie istnieje w bazie danych!");
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, student.IndexNumber),
                new Claim(ClaimTypes.Name, student.FirstName),
                new Claim(ClaimTypes.Role, "student")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken
            (
                issuer: "Gakko",
                audience: "Students",
                claims: claims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: creds
            );
            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                refreshToken = Guid.NewGuid()
            });
        }
    }
}