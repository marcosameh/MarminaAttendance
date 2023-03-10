using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Models
{
    public class ServantWeeksDTO
    {
        public ServantWeeksDTO()
        {
            IsWeekSelected = new Dictionary<int, bool>();
        }
        public int ServantId { get; set; }

        public Dictionary<int, bool> IsWeekSelected { get; set; } 
    }
}
