using Vector.User.API.Models;

namespace Vector.User.API.Services.IServices
{
    public interface IEmailService
    {
        public void SendEmail(EmailDTO request);


    }
}


