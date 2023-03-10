using Vector.User.API.Models.CustomerManagementRequest;
using Vector.User.API.Services.IServices;
using Vector.User.Domain.Models;
using Vector.User.Infrastructure;
using VECTOR.CRUDLibrary.Interface;
using VECTOR.CRUDLibrary.Repositories;

namespace Vector.User.API.Services
{
    public class CustomerManagementService : ICustomerManagementService
    {
        private IBaseRepository<CustomerManagementModel> customerRepository;
        private readonly DataContext dataContext;

        public CustomerManagementService(DataContext dataContext)

        {
            this.dataContext = dataContext;
            customerRepository = new BaseRepository<CustomerManagementModel>(dataContext);

        }

        public CustomerManagementService()
        {
            this.customerRepository = customerRepository;
        }

        /////////////////////// GET ALL ACTIVE CUSTOMER ///////////////////////
        public IEnumerable<DataContext> GetAllCustomer()
        {
            var customer = customerRepository.GetAll();
            return (IEnumerable<DataContext>)customer;

        }

        /////////////////////// GET ANY CUSTOMER BY ID ///////////////////////
        public CustomerManagementModel GetCustomer(int Id)
        {
            var customer = customerRepository.GetByCondition(x => x.Id == Id).FirstOrDefault();
            return (customer);
        }

        /////////////////////// GET ALL ACTIVE CUSTOMER ///////////////////////
        public List<CustomerManagementModel> GetActiveCustomer()
        {

            // Storing the Id of the last product in the Product Management DB
            var dbLastId = customerRepository.GetAll().ToList().LastOrDefault().Id;

            var customer = customerRepository.GetByCondition(x => x.IsDeleted == false).ToList();

            return customer;
        }


        /////////////////////// ADD NEW CUSTOMER ///////////////////////

        public bool AddCustomerInformation(CustomerManagementModel customerManagementModel)
        {
            //customerRepository.Create(customerManagementModel);
            //return customerManagementModel;

            try
            {
                var customer = customerRepository.GetByCondition(x => x.EmailID == customerManagementModel.EmailID).FirstOrDefault(); //try


                if (!dataContext.CustomerManagement.Any(x => x.EmailID == customerManagementModel.EmailID))
                {

                    //userModel.PasswordHash = BCryptNet.HashPassword(userModel.PasswordHash);
                   
                    customerRepository.Create(customerManagementModel);
                    //return customer;
                }
                else
                {
                    return false;
                }
                //return customer;
            }
           catch (Exception e)
           {
                return false;
           }

            return true;
        }


        /////////////////////// UPDATE CUSTOMER DATA ///////////////////////

        public CustomerManagementModel UpdateCustomer(CustomerManagementModel customerManagementModel)
        {
            var customer = customerRepository.GetByCondition(x => x.Id == customerManagementModel.Id).FirstOrDefault();
            if (customer == null)
            {
                return null;
            }
            else
            {

                customer.FirstName = customerManagementModel.FirstName;
                customer.MiddleName = customerManagementModel.MiddleName;
                customer.LastName = customerManagementModel.LastName;
                customer.Gender = customerManagementModel.Gender;
                customer.EmailID = customerManagementModel.EmailID;
                customer.PrimaryPhoneNo = customerManagementModel.PrimaryPhoneNo;
                customer.SecondaryPhoneNo= customerManagementModel.SecondaryPhoneNo;
                customer.Address = customerManagementModel.Address;
                customer.City = customerManagementModel.City;
                customer.PinCode = customerManagementModel.PinCode;
                customer.State = customerManagementModel.State;
                customer.DateOfBirth= customerManagementModel.DateOfBirth;
                customer.Occupation= customerManagementModel.Occupation;
                customer.Bio = customerManagementModel.Bio;
                customer.IsDeleted = customerManagementModel.IsDeleted;

                                
                customerRepository.Update(customer.Id, customer);
                return customer;
            }
        }

        /////////////////////// DELETE CUSTOMER ///////////////////////
        
        public CustomerManagementModel DeleteCustomer(int Id)
        {
            CustomerManagementModel customerManagementModel = customerRepository.GetByCondition(x => x.Id == Id).FirstOrDefault();
            if (customerManagementModel == null)
            {
                return null;
            }
            else
            {
                customerRepository.Delete(customerManagementModel);
                return customerManagementModel;
            }

        }

        /////////////////////// SOFT DELETE CUSTOMER ///////////////////////
       
        public CustomerManagementModel SoftDeletedCustomer(int Id)
        {
            CustomerManagementModel customerManagementModel = customerRepository.GetByCondition(x => x.Id == Id).FirstOrDefault();
            if (customerManagementModel == null)
            {
                return null;
            }
            else
            {
                customerRepository.SoftDelete(customerManagementModel);
                return customerManagementModel;
            }

        }
    }
}
