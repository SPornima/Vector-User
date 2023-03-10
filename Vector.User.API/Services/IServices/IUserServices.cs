using Vector.User.API.Models;
using Vector.User.Domain.Models;
using Vector.User.Infrastructure;

namespace Vector.User.API.Services.IServices
{
    public interface IUserServices
    {
        public bool Registration(UserModel userModel);
        public UserModel Authenticate(string email, string password);

        public UserOtpNotificationModel GetUserEmail(string email);
        public UserOtpNotificationModel OtpVerification(string email, string otp);

        public UserModel ForgetPassword(string email, string password, string otp);

        public UserModel ResetPassword(ResetPasswordRequest resetPasswordRequest);

        //public string BuildJWTToken();

        

        
    }
}
