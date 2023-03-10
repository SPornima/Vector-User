using System.ComponentModel.DataAnnotations;

namespace Vector.User.API.Models

{
    public class UserEmailModel
    {
        [Required]

        public string Email { get; set; }
    }
}
