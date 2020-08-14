using System;
using System.Collections.Generic;
using System.Text;

namespace XLantCore.Models
{
    public partial class Person
    {
        public static Title ParseTitle(string s)
        {
            Title title = Title.Unknown;
            s = s.Replace(" ", string.Empty);
            try
            {
                title = (Title)Enum.Parse(typeof(Title), s, true);
            }
            catch (Exception)
            {
                title = Title.Unknown;
            }
            return title;
        }
    }
}
