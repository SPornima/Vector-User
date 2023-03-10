namespace Vector.User.API.Models.CustomerManagementRequest
{
    public class CustomerManagementRequest
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; } 
        public string EmailID { get; set; }
        public string PrimaryPhoneNo { get; set; }
        public string SecondaryPhoneNo { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PinCode { get; set; }
        public string State { get; set; }
        public string DateOfBirth { get; set; }
        public string Occupation { get; set; }
        public string Bio { get; set; }
    }
}
