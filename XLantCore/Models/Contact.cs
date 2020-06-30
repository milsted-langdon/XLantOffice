using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XLantCore.Models
{
    public partial class Contact: Person
    {
        public ContactType Type { get; set; }
        public bool DoNotMail { get; set; }
        public string Position { get; set; }
        public OccupationType Occupation { get; set; }

    }
}
