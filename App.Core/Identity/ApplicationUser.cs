using App.Core.Entities;
using Microsoft.AspNetCore.Identity;
using System;

namespace MarminaAttendance.Identity
{
    public class ApplicationUser : IdentityUser
    {

        [PersonalData]
        public string Photo { get; set; }
        public int? ServantId { get; set; }
        [PersonalData]
        public Servants? Servant { get; set; }

    }
}