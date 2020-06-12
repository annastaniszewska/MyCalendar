﻿using System.ComponentModel.DataAnnotations;

namespace MyCalendar.ViewModels
{
    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}