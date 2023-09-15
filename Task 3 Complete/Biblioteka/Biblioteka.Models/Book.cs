using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Biblioteka.Models
{
    public class Book
    {
        [Key]
        public int BookId { get; set; }
        public string? Tytul { get; set; }
        public string? Autor { get; set; }
        public string? Gatunek { get; set; }
        [Range(1800, 2100, ErrorMessage = "Rok wydania musi być w zakresie od 1800 do 2100.")]
        public int RokWydania { get; set; }
        // Dodaj pole, które śledzi dostępność książki (np. true - dostępna, false - niedostępna)
        public bool IsAvailable { get; set; }

        // Dodaj pole, które określa status książki (np. Dostępna, Zarezerwowana, Wypożyczona)
        public string? Status { get; set; }

        // Dodaj pole, które przechowuje informacje o użytkowniku, który wypożyczył książkę
        public int? UserId { get; set; }
        public User? User { get; set; }
    }
}
