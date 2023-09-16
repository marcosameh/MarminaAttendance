using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Entities
{
    public partial class Served
    {
        [NotMapped]
        public IFormFile PhotoFile { get; set; }     
        public string PhotoPath
        {
            get
            {
                if (string.IsNullOrEmpty(Photo))
                {
                    return "/photos/المخدومين/default.jpg";
                }
                return "/photos/المخدومين/" + Photo;
            }

        }

    }
}
