using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CompraVenta.ViewModels
{
    public class EditAccountViewModel
    {
        [Required]
        [DataType(DataType.Text)]
        public string UserName { get; set; }

        public string Name { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [DataType(DataType.Text)]
        public string Details { get; set; }

        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar contraseña")]
        [Compare("NewPassword",
                ErrorMessage = "La contraseña y la confirmación no coinciden.")]
        public string Confirmation { get; set; }

        public string Role { get; set; }
        
        public IFormFile ProfileImage { get; set; }

        public string ProfileImagePath { get; set; }

        public string OldImagePath { get; set; }
    }
}
