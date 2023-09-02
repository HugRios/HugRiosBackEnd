using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FrontEndAssesment.Models
{
    public class UsersModel
    {
        [Required(ErrorMessage = "The user name field is required.")]
        public string? full_name { get; set; }
        [Required(ErrorMessage = "The Email field is required.")]
        [EmailAddress(ErrorMessage = "The Email field is not a valid e-mail address.")]
        public string? email { get; set; }
        [Required(ErrorMessage = "The password field is required.")]
        public string? password { get; set; }
    }
}
