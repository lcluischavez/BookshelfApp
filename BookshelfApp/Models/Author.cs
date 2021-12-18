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
        public int Id { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";
        public string Penname { get; set; } //(optional)
        [Display(Name = "Preferred Genre")]
        public string PreferredGenre {get; set;} //(optional)
        public List<Book> Books { get; set; }
        [Required]
        public string ApplicationUserId { get; set; }
        [Display(Name = "Created By")]
        public ApplicationUser ApplicationUser { get; set; }
    }
}
