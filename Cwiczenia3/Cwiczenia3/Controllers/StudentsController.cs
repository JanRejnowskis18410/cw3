using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Cwiczenia3.DAL;
using Cwiczenia3.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cwiczenia3.Controllers
{
    [Route("api/students")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IDbService _dbService;
        private String ConnString = "Data Source=db-mssql;Initial Catalog=s18410;Integrated Security=true";
        
        public StudentsController(IDbService dbService)
        {
            _dbService = dbService;
        }
        
        //[HttpGet]
        //public string GetStudents(string orderBy)
        //{
        //    return $"Jan, Anna, Katarzyna sortowanie={orderBy}";
        //}

        [HttpGet]
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
                    
                    result.Add(st);
                }
            }
            return Ok(result);
        }

        [HttpGet("{id}")]
        public IActionResult GetStudent(int id)
        {
            if(id == 1)
            {
                return Ok("Kowalski");
            } else if (id == 2)
            {
                return Ok("Malewski");
            }

            return NotFound("Nie znaleziono studenta");
        }

        [HttpPost]
        public IActionResult CreateStudent(Student student)
        {
            //... add to database
            //... generating index number
            student.IndexNumber = $"s{new Random().Next(1, 20000)}";
            return Ok(student);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateStudents()
        {
            return Ok("Aktualizacja dokończona");
        }

        [HttpDelete("{id}")]
        public IActionResult deleteStudents()
        {
            return Ok("Usuwanie ukończone");
        }
    }
}