using System;
using System.Collections.Generic;
using System.Text;

namespace XLantCore.Models
{
    public partial class Staff
    {
        /// <summary>
        /// Read Only - The first and last names concatenated.
        /// </summary>
        public string Fullname
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }
    }
}
