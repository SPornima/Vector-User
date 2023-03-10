namespace Vector.User.API.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Vector.User.API.Models;
using Vector.User.API.Services.IServices;
using Vector.User.Domain.Models;


[EnableCors]
[ApiController]
[Route("User")]


public class UserController : ControllerBase
{
    private IUserServices userService;


    public UserController(IUserServices userService)
    {
        this.userService = userService;
    }


    /////////////////////////REGISTRATION////////////////////////////

    /// <summary>
    /// User Can Register for them using his/her Information
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>

    [AllowAnonymous]
    [HttpPost("Register")]
    public IActionResult Register(UserRequest model)
    {
        UserModel registrationModel = new UserModel();
        registrationModel.FirstName = model.FirstName;
        registrationModel.LastName = model.LastName;
        registrationModel.Email = model.Email;
        registrationModel.PasswordHash = model.Password;
        registrationModel.TwoFA = model.TwoFA;

        var result = userService.Registration(registrationModel);
        MessageModel msg = new MessageModel();
        if (result == true)
        {
            msg.Status = true;
            msg.Message = "Registration Successfull";
            return Ok(msg);
        }
        else
        {
            msg.Status = false;
            msg.Message = "User already Exists";
            return Ok(msg);

        }
    }

    /*------------------------ LOGIN -------------------------------*/

    /// <summary>
    /// Here only registered user can login there account with Email and Password
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>


    [AllowAnonymous]
    [HttpPost("Login")]
    public IActionResult Authenticate(AuthenticateRequest model)

    {

        var response = userService.Authenticate(model.Email, model.Password);


       // string tokenString = string.Empty;


        MessageModel msg = new MessageModel();

        if (response == null)
        {

            msg.Status = false;
            msg.Message = "Invalid Login Credentials";
            msg.Data = null;
            return Ok(msg);
        }
        else
        {
            LoginResponse loginResponse = new LoginResponse();
            if (response.TwoFA == true)
            {
                loginResponse.TwoFA = response.TwoFA;
                //tokenString = userService.BuildJWTToken();
                msg.Status = true;
                msg.Message = "Login Successfull";
                loginResponse.Id = response.Id;
                //loginResponse.Token = tokenString;
                msg.Data = loginResponse;
                return Ok(msg);
            }
            else
            {
                loginResponse.TwoFA = response.TwoFA;
                //tokenString = userService.BuildJWTToken();
                msg.Status = true;
                msg.Message = "Login Successfull";
                loginResponse.Id = response.Id;
                //loginResponse.Token = tokenString;
                msg.Data = loginResponse;
                return Ok(msg);
            }

        }

    }

    /////////////////////////// GENERATE-OTP FORGOT PASSWORD /////////////////////////////////

    /// <summary>
    /// Send OTP on the registered Email only
    /// </summary>
    /// <param name="userEmailModel"></param>
    /// <returns></returns>


    [HttpPost("ForgotPasswordOtpGeneration")]

    public IActionResult GetUserEmail(UserEmailModel userEmailModel)
    {
        MessageModel msg = new MessageModel();
        var response = userService.GetUserEmail(userEmailModel.Email);
        if (response == null)
        {

            msg.Status = false;
            msg.Message = "Email not registered";
            msg.Data = userEmailModel.Email;

            return Ok(msg);
        }
        else
        {

            msg.Status = true;
            msg.Message = "OTP Generated";
            msg.Data = userEmailModel.Email;

            return Ok(msg);
        }

    }

    ////////////////////////////////OTP_VERIFICATION/////////////////////////////

    /// <summary>
    /// Verify the OTP which is on the Email 
    /// </summary>
    /// <param name="otpModel"></param>
    /// <returns></returns>
    /// 

    [HttpPost("ForgotPasswordOtpVerification")]
    public IActionResult OtpVerify(OtpModel otpModel)
    {

        var response = userService.OtpVerification(otpModel.Email, otpModel.Otp);
        MessageModel msg = new MessageModel();
        if (response != null)
        {

            msg.Status = true;
            msg.Message = "OTP is Verified";
            return Ok(msg);
        }
        else
        {

            msg.Status = false;
            msg.Message = "OTP or Email is invalid";
            return Ok(msg);
        }
    }

    ////////////////////////////// FORGOT_PASSWORD ////////////////////////

    /// <summary>
    /// If user does not recognize the Password then here they can change the Password on registered Email
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>


    [HttpPost("ForgetPassword")]
    public IActionResult forgetPassword(ForgetPasswordRequest model)

    {

        var response = userService.ForgetPassword(model.Email, model.Password, model.ConfirmPassword);
        MessageModel msg = new MessageModel();
        if (response == null)
        {
            msg.Status = false;
            msg.Message = "Please check your Password";
            msg.Data = null;
            return Ok(msg);
        }
        else
        {
            msg.Status = true;
            msg.Message = "Updated Successfull";
            msg.Data = null;
            return Ok(msg);
        }

    }

    /////////////////////// RESET_PASSWORD ///////////////////////////////

    /// <summary>
    /// If user wants to change the Password then here they can change the Password on the registered Email
    /// </summary>
    /// <param name="Model"></param>
    /// <returns></returns>
    /// 

    [HttpPost("ResetPassword")]
    //[Authorize]

    public IActionResult resetPassword(ResetPasswordRequest Model)
    {
        var response = userService.ResetPassword(Model);
        MessageModel msg = new MessageModel();
        if (response == null)
        {
            msg.Status = false;
            msg.Message = "Please check your Password";
            msg.Data = null;
            return Ok(msg);
        }
        else
        {
            msg.Status = true;
            msg.Message = "Updated Successfull";
            msg.Data = null;
            return Ok(msg);
        }

    }

}







