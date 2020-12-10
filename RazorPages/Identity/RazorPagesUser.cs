using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace RazorPages.Identity
{
    // Add profile data for application users by adding properties to the RazorPagesUser class
    public class RazorPagesUser : IdentityUser
    {
        public String Name { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime LoginDate { get; set; }
        public String Status { get; set; }

    }
}
