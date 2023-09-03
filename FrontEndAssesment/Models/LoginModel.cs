using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace FrontEndAssesment.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "The user name field is required.")]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
