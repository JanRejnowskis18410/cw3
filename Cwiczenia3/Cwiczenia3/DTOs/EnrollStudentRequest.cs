using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cwiczenia3.DTOs
{
    public class EnrollStudentRequest
    {
        [Required(ErrorMessage = "Musisz podać identyfikator")]
        [RegularExpression("^s[0-9]+$", ErrorMessage = "Błędny format identyfikatora")]
        public string IndexNumber { get; set; }

        [Required(ErrorMessage = "Musisz podać imię")]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Musisz podać nazwisko")]
        [MaxLength(100)]
        public string LastName { get; set; }

        [Range(typeof(DateTime), "1/1/1900", "31/12/9999", ErrorMessage = "Data urodzenia musi być pomiędzy {1} a {2}")]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "Musisz podać nazwę studiów")]
        [MaxLength(100)]
        public string Studies { get; set; }
    }
}
