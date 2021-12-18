using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace BookshelfApp.Models.ViewModels
{
    public class BookFormViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        public int ISBN { get; set; }
        [Required(ErrorMessage = "Enter the issued date.")]
        [DataType(DataType.Date)]
        public DateTime PublishDate { get; set; }
        public List<SelectListItem> GenreOptions { get; set; }

        public int GenreId { get; set; }

        public Genre Genre { get; set; }

        public string ApplicationUserId { get; set; }
    }
}
