using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookshelfApp.Models
{
    public class Author
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";
        public string Penname { get; set; } //(optional)
        public string PreferredGenre {get; set;} //(optional)
        public List<Book> Books { get; set; }
        [Required]
        public string ApplicationUserId { get; set; }
        [Display(Name = "Created By")]
        public ApplicationUser ApplicationUser { get; set; }
    }
}
