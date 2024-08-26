using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace LibraryMVC.Models
{
    public class Book
    {
        internal readonly object BookAuthor;

        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; } = "";

        [MaxLength(100)]
        public string Author { get; set; } = "";

        [MaxLength(100)]
        public string Genre { get; set; } = "";

        [Precision(16, 2)]
        public decimal Price { get; set; } 


        public string Description { get; set; } = "";

        [MaxLength(100)]
        public string ImageFileName { get; set; } = "";


        public DateTime CreatedAt { get; set; }
        public object BookName { get; internal set; }

        public Book() { }

    }
}
