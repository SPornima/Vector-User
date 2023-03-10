using Vector.User.Domain.Models;
using Vector.User.Infrastructure;

namespace Vector.User.API.Services.IServices
{
    public interface ICustomerManagementService
    {
        public  IEnumerable<DataContext> GetAllCustomer();
        public CustomerManagementModel GetCustomer(int id);
        public List<CustomerManagementModel> GetActiveCustomer();
        public bool AddCustomerInformation(CustomerManagementModel customerManagementModel);
        public CustomerManagementModel UpdateCustomer(CustomerManagementModel customerManagementModel);
        public CustomerManagementModel DeleteCustomer(int Id);
        public CustomerManagementModel SoftDeletedCustomer(int Id);
        
        
    }
}
