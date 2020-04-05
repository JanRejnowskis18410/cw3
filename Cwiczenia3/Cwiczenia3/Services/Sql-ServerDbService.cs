using Cwiczenia3.DTOs;
using Cwiczenia3.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Cwiczenia3.Services
{
    public class Sql_ServerDbService : IStudentsDbService
    {
        public string ConString { get; set; }

        public Sql_ServerDbService(string ConString)
        {
            this.ConString = ConString;
        }

        public IActionResult EnrollStudent(EnrollStudentRequest request)
        {
            Enrollment result;
            ObjectResult msg;
            using (var con = new SqlConnection(ConString))
            using (var com = new SqlCommand())
            {
                com.Connection = con;
                con.Open();
                var tran = con.BeginTransaction();
                com.Transaction = tran;
                SqlDataReader dr;

                try
                {
                    com.CommandText = "SELECT IdStudy FROM Studies where Name=@name";
                    com.Parameters.AddWithValue("name", request.Studies);
                    dr = com.ExecuteReader();
                    if (!dr.Read())
                    {
                        dr.Close();
                        tran.Rollback();
                        msg = new ObjectResult("Studia o podanej nazwie nie istnieją w bazie danych!");
                        msg.StatusCode = 400;
                        return msg;
                    }
                    int idstudies = (int)dr["IdStudy"];
                    dr.Close();

                    com.CommandText = "SELECT IndexNumber FROM Student WHERE IndexNumber = @index";
                    com.Parameters.AddWithValue("index", request.IndexNumber);
                    dr = com.ExecuteReader();
                    if (dr.Read())
                    {
                        dr.Close();
                        tran.Rollback();
                        msg = new ObjectResult("Student o podanym indeksie już istnieje w bazie danych!");
                        msg.StatusCode = 400;
                        return msg;
                    }
                    dr.Close();

                    com.CommandText = "SELECT TOP 1 IdEnrollment FROM Enrollment WHERE IdStudy = @idstudies AND Semester = 1 ORDER BY StartDate DESC";
                    com.Parameters.AddWithValue("idstudies", idstudies);
                    dr = com.ExecuteReader();
                    int enrollmentid;
                    if (!dr.Read())
                    {
                        dr.Close();
                        com.CommandText = "INSERT INTO Enrollment VALUES ((SELECT MAX(IdEnrollment) + 1 FROM Enrollment), 1, @idstudies, CONVERT(date, GETDATE()))";
                        com.ExecuteNonQuery();
                        com.CommandText = "SELECT MAX(IdEnrollment) \"IdEnrollment\" FROM Enrollment";
                        dr = com.ExecuteReader();
                        dr.Read();
                        enrollmentid = (int)dr["IdEnrollment"];
                    }
                    else
                    {
                        enrollmentid = (int)dr["IdEnrollment"];
                    }
                    dr.Close();

                    com.CommandText = "INSERT INTO Student VALUES (@index, @fname, @lname, @birth, @enrollmentid)";
                    com.Parameters.AddWithValue("fname", request.FirstName);
                    com.Parameters.AddWithValue("lname", request.LastName);
                    com.Parameters.AddWithValue("birth", request.BirthDate);
                    com.Parameters.AddWithValue("enrollmentid", enrollmentid);
                    com.ExecuteNonQuery();

                    tran.Commit();
                }
                catch (SqlException exc)
                {
                    tran.Rollback();
                    msg = new ObjectResult("Nie udało się przeprowadzić operacji! Błąd w bazie danych:\n" + exc);
                    msg.StatusCode = 400;
                    return msg;
                };

                result = new Enrollment();
                com.CommandText = "SELECT Enrollment.IdEnrollment, Semester, IdStudy, StartDate FROM Enrollment JOIN Student ON Student.IdEnrollment = Enrollment.IdEnrollment WHERE IndexNumber = @index";
                dr = com.ExecuteReader();
                while (dr.Read())
                {
                    result.IdEnrollment = (int)dr["IdEnrollment"];
                    result.Semester = (int)dr["Semester"];
                    result.IdStudy = (int)dr["IdStudy"];
                    result.StartDate = DateTime.Parse(dr["StartDate"].ToString());
                }
            }
            msg = new ObjectResult(result);
            msg.StatusCode = 201;
            return msg;
        }

        public IActionResult PromoteStudent(PromoteStudentRequest request)
        {
            var Studies = request.Studies;
            var Semester = request.Semester;
            Enrollment result;
            ObjectResult msg;

            using (var con = new SqlConnection(ConString))
            using (var com = new SqlCommand())
            {
                com.Connection = con;
                con.Open();
                com.CommandText = "SELECT 'X' FROM Studies JOIN Enrollment ON Studies.IdStudy = Enrollment.IdStudy WHERE Name = @studies AND Semester = @semester;";
                com.Parameters.AddWithValue("studies", request.Studies);
                com.Parameters.AddWithValue("semester", request.Semester);
                var dr = com.ExecuteReader();
                if (!dr.Read())
                {
                    dr.Close();
                    msg = new ObjectResult("Studia o podanych parametrach nie istnieją w bazie danych!");
                    msg.StatusCode = 404;
                    return msg;
                }
                dr.Close();

                com.Parameters.Clear();
                com.CommandType = System.Data.CommandType.StoredProcedure;
                com.CommandText = "Promote";
                com.Parameters.Add(new SqlParameter("@Studies", request.Studies));
                com.Parameters.Add(new SqlParameter("@Semester", request.Semester));

                var returnParameter = com.Parameters.Add("@IdEnroll", System.Data.SqlDbType.Int);
                returnParameter.Direction = System.Data.ParameterDirection.ReturnValue;
                com.ExecuteNonQuery();

                result = new Enrollment();
                com.CommandType = System.Data.CommandType.Text;
                var returnValue = returnParameter.Value;
                com.CommandText = "SELECT * FROM Enrollment WHERE IdEnrollment = @enrollid";
                com.Parameters.AddWithValue("enrollid", returnValue);
                dr = com.ExecuteReader();
                while (dr.Read())
                {
                    result.IdEnrollment = (int)dr["IdEnrollment"];
                    result.Semester = (int)dr["Semester"];
                    result.IdStudy = (int)dr["IdStudy"];
                    result.StartDate = DateTime.Parse(dr["StartDate"].ToString());
                }
                dr.Close();
                msg = new ObjectResult(result);
                msg.StatusCode = 201;
                return msg;
            }
        }
    }
}
