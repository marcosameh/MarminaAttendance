using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Models
{
    public class ServedVM
    {
        public int Id { get; set; }

        public string ClassName { get; set; }

        public string ResponsibleServant { get; set; }

        public string Name { get; set; }

        public string Photo {private get ; set; }
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


        public string Phone { get; set; }

        public string Address { get; set; }

        public string FatherOfConfession { get; set; }

        public string Birthday { get; set; }

    }
}
