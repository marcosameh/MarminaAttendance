using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Tenant.Entities
{
    public partial class Tenant
    {
        [NotMapped]
        public string LogoPath { get { return "/photos/tenant/"+Logo; } }
    }
}
