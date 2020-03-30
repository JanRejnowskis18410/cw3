using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
                    st.BirthDate = DateTime.Parse(dr["BirthDate"].ToString());
                    st.StudiesName = dr["Name"].ToString();
                    st.Semester = Int32.Parse(dr["Semester"].ToString());
                    
                    result.Add(st);
                }
            }
            return Ok(result);
        }

        [HttpGet("{id}")]
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