using System.ComponentModel.DataAnnotations;

namespace LibraryMVC.Models
{
    public class BookDto
    {
        [Required, MaxLength(100)]
        public string Name { get; set; } = "";

        [Required, MaxLength(100)]
        public string Author { get; set; } = "";

        [Required, MaxLength(100)]
        public string Genre { get; set; } = "";

        [Required]
        public decimal Price { get; set; }

        [Required]
        public string Description { get; set; } = "";

        public IFormFile ImageFileName { get; set; }

        public static implicit operator BookDto(BookDto v)
        {
            throw new NotImplementedException();
        }
    }
}


