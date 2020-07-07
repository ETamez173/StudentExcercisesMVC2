using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentExcercisesMVC2.Models.ViewModels
{
    public class StudentEditViewModel
    {
        public int StudentId { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "First Name is required!")]
        [MinLength(2, ErrorMessage = "First Name should be at least 2 characters long")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Last Name is required!")]
        [MinLength(3, ErrorMessage = "Last Name should be at least 3 characters long")]
        public string LastName { get; set; }

        [Display(Name = "Cohort")]
        [Required]
        public int CohortId { get; set; }

        [Display(Name = "Slack Handle")]
        [Required(ErrorMessage = "Slack Handle is required!")]
        [MinLength(2, ErrorMessage = "Slack Handle should be at least 2 characters long")]
        public string SlackHandle { get; set; }

        public List<SelectListItem> CohortOptions { get; set; }
    }
}
