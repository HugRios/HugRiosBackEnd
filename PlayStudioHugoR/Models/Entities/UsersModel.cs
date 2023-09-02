using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlayStudioHugoR.Models.Entities
{
    public class UsersModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string? full_name { get; set; }
        [Required(ErrorMessage = "The Email field is required.")]
        [EmailAddress(ErrorMessage = "The Email field is not a valid e-mail address.")]
        public string? email { get; set; }
        [Required(ErrorMessage = "The password field is required.")]
        public string? password { get; set; }
    }
}
