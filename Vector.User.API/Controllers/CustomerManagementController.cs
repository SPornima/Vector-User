using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using Vector.User.API.Models;
using Vector.User.API.Models.CustomerManagementRequest;
using Vector.User.API.Services;
using Vector.User.API.Services.IServices;
using Vector.User.Domain.Models;
using Vector.User.Infrastructure;
using VECTOR.CRUDLibrary.Interface;
using VECTOR.CRUDLibrary.Repositories;

namespace Vector.User.API.Controllers
{
    [EnableCors]
    [Route("[controller]")]
    [ApiController]

    public class CustomerManagementController : Controller
    {
        private readonly ICustomerManagementService customerManagementService;
        private IBaseRepository<UserModel> customerRepository;

        public CustomerManagementController(ICustomerManagementService customerManagementService, DataContext dataContext)
        {
            this.customerManagementService = customerManagementService;
            customerRepository = new BaseRepository<UserModel>(dataContext);
        }


        /////////////////////// GET ALL Active And InActive CUSTOMER ///////////////////////

        /// <summary>
        /// We can get all the active customer form GetAllCustomer
        /// </summary>
        /// <returns></returns>

        [HttpGet("GetAllCustomer")]
        public IActionResult getAllCustomer()
        {
            var customer = customerRepository.GetAll();
            MessageModel msg = new MessageModel();

            msg.Status = true;
            msg.Message = "Fetched All Customer";
            msg.Data = customer;
            return Ok(msg);
        }

        /////////////////////// GET ANY CUSTOMER BY ID ///////////////////////

        /// <summary>
        /// We can fetch each customer by its ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [HttpGet("GetCustomerByID")]
        public IActionResult getCustomer(int id)
        {
            var customer = customerManagementService.GetCustomer(id);
            MessageModel msg = new MessageModel();

            msg.Status = true;
            msg.Message = "Fetched Customer BY ID";
            msg.Data = customer;
            return Ok(msg);
        }

        /////////////////////// GET ALL ACTIVE CUSTOMER ///////////////////////

        /// <summary>
        /// We Get All The Active Customer who All have IsDeleted Status As False
        /// </summary>
        /// <returns></returns>

        [HttpGet("GetAllActiveCustomer")]
        public IActionResult getActiveCustomer()
        {
            var customer = customerManagementService.GetActiveCustomer();
            MessageModel msg = new MessageModel();

            msg.Status = true;
            msg.Message = "Fetched only Active customer";
            msg.Data = customer;
            return Ok(msg);
        }

        /////////////////////// ADD NEW CUSTOMER ///////////////////////

        /// <summary>
        /// Add New Customer
        /// </summary>
        /// <param name="customerManagementRequest"></param>
        /// <returns></returns>

        [HttpPost("AddCustmer")]
        public IActionResult AddCustomerInformation(CustomerManagementRequest customerManagementRequest)
        {
            CustomerManagementModel customerManagementModel = new CustomerManagementModel();
            customerManagementModel.FirstName = customerManagementRequest.FirstName;
            customerManagementModel.MiddleName = customerManagementRequest.MiddleName;
            customerManagementModel.LastName = customerManagementRequest.LastName;
            customerManagementModel.EmailID = customerManagementRequest.EmailID;
            customerManagementModel.Gender = customerManagementRequest.Gender;
            customerManagementModel.City = customerManagementRequest.City;
            customerManagementModel.Address = customerManagementRequest.Address;
            customerManagementModel.DateOfBirth = customerManagementRequest.DateOfBirth;
            customerManagementModel.PrimaryPhoneNo = customerManagementRequest.PrimaryPhoneNo;
            customerManagementModel.SecondaryPhoneNo = customerManagementRequest.SecondaryPhoneNo;
            customerManagementModel.PinCode = customerManagementRequest.PinCode;
            customerManagementModel.State = customerManagementRequest.State;
            customerManagementModel.Occupation = customerManagementRequest.Occupation;
            customerManagementModel.Bio = customerManagementRequest.Bio;

            var result = customerManagementService.AddCustomerInformation(customerManagementModel);
            MessageModel msg = new MessageModel();
            if (result == true )
            {
                Regex emailRegex = new Regex("([\\w\\.\\-]+)@([\\w\\-]+)((\\.(\\w){2,})+)$");
                bool isValidEmailID = emailRegex.IsMatch(customerManagementModel.EmailID);
                if (!isValidEmailID)
                {
                    msg.Status = false;
                    msg.Message = "Invalid Email Id";
                    return Ok(msg);
                }

                Regex mobileRegex = new Regex("[6789]\\d{9}$");
                bool isValidPrimaryPhone = mobileRegex.IsMatch(customerManagementModel.PrimaryPhoneNo);
                bool isValidSecondaryPhone = mobileRegex.IsMatch(customerManagementModel.SecondaryPhoneNo);
                if (!isValidPrimaryPhone)
                {
                    msg.Status = false;
                    msg.Message = "Invalid Primary Phone Number";
                    return Ok(msg);
                }

                if (!isValidSecondaryPhone)
                {
                    msg.Status = false;
                    msg.Message = "Invalid Secondary Phone Number";
                    return Ok(msg);
                }

                Regex pincodeRegex = new Regex("([4]{1}[0-9]{5}|[1-9]{1}[0-9]{3}[0-9]{3})$");
                bool isValidPinCode = pincodeRegex.IsMatch(customerManagementModel.PinCode);
                if (!isValidPinCode)
                {
                    msg.Status = false;
                    msg.Message = "Invalid PinCode";
                    return Ok(msg);
                }

                Regex regex = new Regex("\\d{2}\\/\\d{2}\\/\\d{4}$");
                bool isValid = regex.IsMatch(customerManagementModel.DateOfBirth);
                if (!isValid)
                {
                    msg.Status = false;
                    msg.Message = "Invalid Date of birth";
                    return Ok(msg);

                }

                customerManagementService.AddCustomerInformation(customerManagementModel);
                msg.Status = true;
                msg.Message = "Customer Added Successfully";
                return Ok(msg);

            }
            else
            {
                msg.Status = false;
                msg.Message = "User already Exists";
                return Ok(msg);

            }
        }


        /////////////////////// UPDATE CUSTOMER DATA ///////////////////////

        /// <summary>
        /// Customer can update his/her data with UpdateCustomer
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>

        [HttpPost("UpdateCustomer")]
        public IActionResult editCustomer(CustomerManagementModel customerManagementModel)
        {
            
            MessageModel msg = new MessageModel();
            if (customerManagementModel != null)
            {
                Regex emailRegex = new Regex("([\\w\\.\\-]+)@([\\w\\-]+)((\\.(\\w){2,})+)$");
                bool isValidEmailID = emailRegex.IsMatch(customerManagementModel.EmailID);
                if (!isValidEmailID)
                {
                    msg.Status = false;
                    msg.Message = "Invalid Email Id";
                    return Ok(msg);
                }

                Regex mobileRegex = new Regex("[6789]\\d{9}$");
                bool isValidPrimaryPhone = mobileRegex.IsMatch(customerManagementModel.PrimaryPhoneNo);
                bool isValidSecondaryPhone = mobileRegex.IsMatch(customerManagementModel.SecondaryPhoneNo);
                if (!isValidPrimaryPhone)
                {
                    msg.Status = false;
                    msg.Message = "Invalid Primary Phone Number";
                    return Ok(msg);
                }

                if (!isValidSecondaryPhone)
                {
                    msg.Status = false;
                    msg.Message = "Invalid Secondary Phone Number";
                    return Ok(msg);
                }

                Regex pincodeRegex = new Regex("([4]{1}[0-9]{5}|[1-9]{1}[0-9]{3}[0-9]{3})$");
                bool isValidPinCode = pincodeRegex.IsMatch(customerManagementModel.PinCode);
                if (!isValidPinCode)
                {
                    msg.Status = false;
                    msg.Message = "Invalid PinCode";
                    return Ok(msg);
                }

                Regex regex = new Regex("\\d{2}\\/\\d{2}\\/\\d{4}$");
                bool isValid = regex.IsMatch(customerManagementModel.DateOfBirth);
                if (!isValid)
                {
                    msg.Status = false;
                    msg.Message = "Invalid Date of birth";
                    return Ok(msg);

                }

                var customer = customerManagementService.UpdateCustomer(customerManagementModel);
                msg.Status = true;
                msg.Message = "Customer Data Updated Successfull";
                return Ok(msg);

            }
            else
            {
                msg.Status = false;
                msg.Message = "Data Update Fail";
                return Ok(msg);

            }
        }

        /////////////////////// DELETE CUSTOMER ///////////////////////

        /// <summary>
        /// We can delete customer with help of DeleteCustomer
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>

        [HttpDelete("DeleteCustomer")]
        public IActionResult deleteCustomer(int Id)
        {
            var customer = customerManagementService.DeleteCustomer(Id);
            MessageModel msg = new MessageModel();

            if (customer == null)
            {
                msg.Status = false;
                msg.Message = "User not found";
                return Ok(msg);
            }
            else
            {
                msg.Status = true;
                msg.Message = "User deleted";
                return Ok(msg);
            }
        }

        /////////////////////// SOFT DELETE CUSTOMER ///////////////////////

        /// <summary>
        /// We can set any Customer as INACTIVE with softdelete
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>

        [HttpDelete("SoftDelete")]
        public IActionResult softDeletedCustomer(int Id)
        {
            var customer = customerManagementService.SoftDeletedCustomer(Id);
            MessageModel msg = new MessageModel();

            if (customer == null)
            {
                msg.Status = false;
                msg.Message = "User not found";
                return Ok(msg);
            }
            else
            {
                msg.Status = true;
                msg.Message = "User is Now InActive";
                return Ok(msg);
            }
        }

    }

}
