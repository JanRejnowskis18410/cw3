using Cwiczenia3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cwiczenia3.DAL
{
    
    public class MockDbService : IDbService
    {
        private static IEnumerable<Student> _students;

        static MockDbService()
        {
            _students = new List<Student>
            {
                new Student{IndexNumber="1", FirstName="Jan", LastName="Kowalski"},
                new Student{IndexNumber="3", FirstName="Anna", LastName="Malewski"},
                new Student{IndexNumber="4", FirstName="Andrzej", LastName="Andrzejewicz"},
            };
        }

        public IEnumerable<Student> GetStudents()
        {
            return _students;
        }
    }
}
