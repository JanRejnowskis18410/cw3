using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cwiczenia3.DTOs
{
    public class PromoteStudentRequest
    {
        [Required(ErrorMessage = "Nazwa studiów nie może być pusta!")]
        public string Studies { get; set; }
        [Required]
        [Range(1,10, ErrorMessage = "Numer semestru powinien zawierać się między {1} a {2}")]
        public int Semester { get; set; }
    }
}
