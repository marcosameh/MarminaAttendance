using Microsoft.AspNetCore.Identity;
using System;

namespace MarminaAttendance.Identity
{
    public class ApplicationUser : IdentityUser
    {
      
        [PersonalData]
        public string Photo { get; set; }
        
    }
}