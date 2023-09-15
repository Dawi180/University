using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Biblioteka.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; } 
        public string? Imie { get; set; } 
        public string? Nazwisko { get; set; } 
        public string? PESEL { get; set; } 
        public DateTime? DataUrodzenia { get; set; }
        public ICollection<Book>? Books { get; set; }
    }
}
