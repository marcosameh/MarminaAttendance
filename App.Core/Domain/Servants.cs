using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace App.Core.Entities
{
    public partial class Servants
    {
        [NotMapped]
        public IFormFile PhotoFile { get; set; }     
        public string PhotoPath
        {
            get
            {
                if (string.IsNullOrEmpty(Photo))
                {
                    return "/photos/الخدام/default.jpg";
                }
                return "/photos/الخدام/" + Photo;
            }

        }

    }
}
