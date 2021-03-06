﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentExcercisesMVC2.Models
{
    public class Instructor
    {
        public int Id { get; set; }


        [Display(Name = "First Name")]
        public string FirstName { get; set; }


        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Slack Handle")]
        public string SlackHandle { get; set; }

        [Display(Name = "Specialty")]
        public string Specialty { get; set; }

        [Display(Name = "Cohort")]
        public int CohortId { get; set; }
    }
}
