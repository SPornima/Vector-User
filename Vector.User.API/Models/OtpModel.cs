using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vector.User.API.Models
{
    public class OtpModel
    {
        [Required]
        public string Otp { get; set; }
        public string Email { get; set; }
    }
}
