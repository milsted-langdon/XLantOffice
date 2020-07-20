using System;
using System.Collections.Generic;
using System.Text;

namespace XLantCore.Models
{
    public partial class Staff
    {
        public string Fullname
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }
    }
}
