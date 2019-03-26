using OneCoreTest.DataAccess.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OneCoreTest.Web.Models
{
    public class ApplicationUserModel
    {
        public string Id { get; set; }

        [Display(Name = "Correo electrónico")]
        [EmailAddress(ErrorMessage = "Formato de correo inválido")]
        [Required(ErrorMessage = "Campo requerido")]
        public string Email { get; set; }

        [Display(Name = "Usuario")]
        [MinLength(7, ErrorMessage = "La longitud mínima para Usuario son 7 caracteres")]
        [Required(ErrorMessage = "Campo requerido")]
        public string Username { get; set; }

        [Display(Name = "Contraseña")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Campo requerido")]
        [MinLength(10, ErrorMessage = "La longitud mínima para la Contraseña es 10 caracteres")]
        [RegularExpression("^.*(?=.{10,})(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$%^&+=]).*$", ErrorMessage = "La contraseña debe contener una mayúscula, minúscula, símbolo y número")]
        public string Password { get; set; }

        [Display(Name = "Confirmar contraseña")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Campo requerido")]
        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Estatus")]
        public bool Status { get; set; }

        [Display(Name = "Género")]
        public Genre Genre { get; set; }
    }
}