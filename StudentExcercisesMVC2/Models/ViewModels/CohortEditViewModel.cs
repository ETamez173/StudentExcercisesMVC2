using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentExcercisesMVC2.Models.ViewModels
{
    public class CohortEditViewModel
    {
        [Display(Name = "Cohort")]
        public int CohortId { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "Cohort is required!")]
        [MinLength(2, ErrorMessage = "Cohort should be at least 2 characters long")]

        public string Name { get; set; }

     

        public List<SelectListItem> CohortOptions { get; set; }

    }
}
