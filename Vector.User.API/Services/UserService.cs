using BCryptNet = BCrypt.Net.BCrypt;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Vector.User.API.Services.IServices;
using Vector.User.API.Models;
using Vector.User.Domain.Models;
using Vector.User.Infrastructure;
using VECTOR.CRUDLibrary.Interface;
using VECTOR.CRUDLibrary.Repositories;
using System.Globalization;

namespace Vector.User.API.Services
{

    public class UserService : IUserServices
    {
        private DataContext dataContext;

        private readonly IEmailService emailSender;

        private IConfiguration configuration;

        private IBaseRepository<UserModel> userRepository;

        private IBaseRepository<UserOtpNotificationModel> userOtpRepository;

        public UserService(DataContext context,
                            IEmailService emailsender,
                            IConfiguration configuration)

        {
            dataContext = context;
            emailSender = emailsender;
            this.configuration = configuration;
            userRepository = new BaseRepository<UserModel>(dataContext);
            userOtpRepository = new BaseRepository<UserOtpNotificationModel>(dataContext);

        }

        public bool Registration(UserModel userModel)
        {
            try
            {
                var obj = userRepository.GetByCondition(x => x.Email == userModel.Email).FirstOrDefault(); //try


                if (!dataContext.User.Any(x => x.Email == userModel.Email))
                {

                    userModel.PasswordHash = BCryptNet.HashPassword(userModel.PasswordHash);

                    userRepository.Create(userModel);
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }


        ///////////////////////  TOKEN  //////////////////////////

        //public string BuildJWTToken()
        //{
        //    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtToken:SecretKey"]));
        //    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        //    var issuer = configuration["JwtToken:Issuer"];
        //    var audience = configuration["JwtToken:Audience"];
        //    var jwtValidity = DateTime.Now.AddMinutes(Convert.ToDouble(configuration["JwtToken:TokenExpiry"]));

        //    var token = new JwtSecurityToken(issuer,
        //      audience,
        //      expires: jwtValidity,
        //      signingCredentials: creds);

        //    return new JwtSecurityTokenHandler().WriteToken(token);
        //}


        ////////////////////////  LOGIN  /////////////////////////
        public UserModel Authenticate(string email, string password)
        {
            var user = userRepository.GetByCondition(x => x.Email == email).FirstOrDefault();

            if (user == null || !BCryptNet.Verify(password, user.PasswordHash))
            {
                return null;
            }
            else
            {
                return user;
            }

        }

        /////////////////////////////// GET_USER_EMAIL /////////////////////////////////

        public UserOtpNotificationModel GetUserEmail(string email)
        {
            UserOtpNotificationModel notificationModel = new UserOtpNotificationModel();
            var verificationemail = userRepository.GetByCondition(x => x.Email == email).FirstOrDefault();

            if (verificationemail != null)
            {

                Random randomNo = new Random();
                string otpNumber = randomNo.Next(100000, 999999).ToString();

                EmailDTO emailDTO = new EmailDTO();
                emailDTO.To = email;
                emailDTO.Body = otpNumber;

                var RecievingEmail = userOtpRepository.GetByCondition(x => x.ReceiverEmail == email).FirstOrDefault();

                if (RecievingEmail == null)
                {

                    notificationModel.ReceiverEmail = email;
                    notificationModel.Data = otpNumber;
                    notificationModel.CreatedDate = DateTime.Now;
                    notificationModel.Status = "Pending";

                    userOtpRepository.Create(notificationModel);

                   

                    emailSender.SendEmail(emailDTO);
                    return notificationModel;

                }
                else
                {

                  

                    RecievingEmail.Data = otpNumber;
                    RecievingEmail.CreatedDate = DateTime.Now;
                    RecievingEmail.Status = "Pending";

                    userOtpRepository.Update(RecievingEmail.Id, RecievingEmail);

                    emailSender.SendEmail(emailDTO);
                    return RecievingEmail;

                }

            }
            else
            {
                return null;
            }
        }

        /////////////////////////////// OTP_VERIFICATION /////////////////////////////////

        public UserOtpNotificationModel OtpVerification(string email, string otp)
        {
            var OtpDb = userOtpRepository.GetByCondition(x => x.ReceiverEmail == email).FirstOrDefault();

            if (OtpDb != null)
            {
                if (otp == OtpDb.Data && OtpDb.Status == "Pending")
                {
                    
                    OtpDb.Data = otp;
                    OtpDb.UpdatedDate = DateTime.Now;
                    OtpDb.Status = "Completed";

                    userOtpRepository.Update(OtpDb.Id, OtpDb);


                    return OtpDb;
                }
                else
                {

                    return null;
                }
            }
            else
            {
                return OtpDb;
            }

        }

        /////////////////////////////// FORGOT_PASSWORD /////////////////////////////////
        public UserModel ForgetPassword(string email, string password, string confirmPassword)
        {

            var user = userRepository.GetByCondition(x => x.Email == email).FirstOrDefault();
            

            if (password == confirmPassword)
            {

                if (user != null && !BCryptNet.Verify(password, user.PasswordHash))
                {

                    password = BCryptNet.HashPassword(password);
                    user.PasswordHash = password;

                    userRepository.Update(user.Id, user);


                    return user;
                }
                else
                {
                    return null;
                }


            }
            else
            {
                return null;
            }
        }

        /////////////////////////////// RESET_PASSWORD /////////////////////////////////

        public UserModel ResetPassword(ResetPasswordRequest resetPasswordRequest)
        {
            var user = userRepository.GetByCondition(x => x.Email == resetPasswordRequest.Email).FirstOrDefault();

            if (user != null && BCryptNet.Verify(resetPasswordRequest.OldPassword, user.PasswordHash))
            {

                if (resetPasswordRequest.OldPassword != resetPasswordRequest.NewPassword && resetPasswordRequest.NewPassword == resetPasswordRequest.ConfirmPassword)
                {
                    resetPasswordRequest.NewPassword = BCryptNet.HashPassword(resetPasswordRequest.NewPassword);
                    user.PasswordHash = resetPasswordRequest.NewPassword;

                    userRepository.Update(user.Id, user);


                    return user;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        

        //public IEnumerable<DataContext> GetAllUser()
        //{
        //    var user = userRepository.GetAll();
        //    return (IEnumerable<DataContext>)user;
        //}


    }
}



