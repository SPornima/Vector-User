using VECTOR.CRUDLibrary.Entities;

namespace Vector.User.Domain.Models
{
    public class UserModel : BaseEntity
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }
        public string PasswordHash { get; set; }

        public bool TwoFA { get; set; }
    }
}
