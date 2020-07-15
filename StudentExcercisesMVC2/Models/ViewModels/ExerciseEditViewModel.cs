using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentExcercisesMVC2.Models.ViewModels
{
    public class ExerciseEditViewModel
    {


        public int ExerciseId { get; set; }

        [Display(Name = "Exercise")]
        [Required(ErrorMessage = "Exercise Name is required!")]
        [MinLength(2, ErrorMessage = "Exercise Name should be at least 2 characters long")]
        public string Name { get; set; }

        [Display(Name = "Language")]
        [Required(ErrorMessage = "Language is required!")]
        [MinLength(2, ErrorMessage = "Language should be at least 2 characters long")]
        public string Language { get; set; }


    }
}
