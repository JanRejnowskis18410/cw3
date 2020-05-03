using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cwiczenia3.DTOs
{
    public class StudentLoginRequest
    {
        [Required(ErrorMessage = "Musisz podać identyfikator")]
        [RegularExpression("^s[0-9]+$", ErrorMessage = "Błędny format identyfikatora")]
        public string IndexNumber { get; set; }
        [Required(ErrorMessage = "Musisz podać hasło")]
        public string Password { get; set; }
    }
}
