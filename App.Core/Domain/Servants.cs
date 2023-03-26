using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Entities
{
    public partial class Servants
    {
        [NotMapped]
        public IFormFile PhotoFile { get; set; }    
    }
}
