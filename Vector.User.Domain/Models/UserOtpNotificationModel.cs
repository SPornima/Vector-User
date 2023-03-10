using VECTOR.CRUDLibrary.Entities;

namespace Vector.User.Domain.Models
{
    public class UserOtpNotificationModel : BaseEntity
    {
        
        public string ReceiverEmail { get; set; }
        public string Data { get; set; }
        public string Status { get; set; }
    }
    
}
