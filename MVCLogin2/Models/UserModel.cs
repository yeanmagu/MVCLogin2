using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVCLogin2.Models
{
    public class UserModel
    {
        [Required]
        [EmailAddress]
        [StringLength(150)]
        [Display(Name="Correo")]
        public string Email { get; set; }

        [Required]
        [StringLength(20,MinimumLength=6)]

        [DataType(DataType.Password)]
        [Display(Name="Contraseña")]
        public string Password { get; set; }

    }
}