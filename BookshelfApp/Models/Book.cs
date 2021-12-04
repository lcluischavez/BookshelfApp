using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BookshelfApp.Models
{
    public class Book
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int ISBN { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Genre { get; set; }

        [Required(ErrorMessage = "Enter the issued date.")]
        [DataType(DataType.Date)]
        public DateTime PublishDate { get; set; }

        //@Html.TextBoxFor(model => model.IssueDate)
        //@Html.ValidationMessageFor(model => model.IssueDate)
        //https://stackoverflow.com/questions/18288675/display-datetime-value-in-dd-mm-yyyy-format-in-asp-net-mvc
       
        [Required]
        public string Author { get; set; } //one author
        [Required]
        public string ApplicationUserId { get; set; }
        [Display(Name = "Owner")]
        public ApplicationUser ApplicationUser { get; set; }
    }
}
