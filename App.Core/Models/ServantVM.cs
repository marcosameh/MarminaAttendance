﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Models
{
    public class ServantVM
    {
        public int Id { get; set; }

        public string ClassName { get; set; }

        public string Name { get; set; }

        public string Photo { private get; set; }
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

        public string Phone { get; set; }

        public string Address { get; set; }

        public string FatherOfConfession { get; set; }

        public string Email { get; set; }


    }
}
