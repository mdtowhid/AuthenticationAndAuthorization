using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AuthAsp.Models.ViewModels
{
    public class CreateEmployeeViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public Dept Department { get; set; }
        public IFormFile PhotoPath { get; set; }
    }
}
