using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace XLantCore.Models
{
    public partial class EmailAddress
    {
        /// <summary>
        /// Creates a list of email addresses from IO jarray data
        /// </summary>
        /// <param name="jarray">the array of "items" representing the email address</param>
        /// <returns>the completed list</returns>
        public static List<EmailAddress> CreateList(JArray jarray)
        {
            List<EmailAddress> emails = new List<EmailAddress>();
            if (jarray == null)
            {
                return null;
            }
            else
            {
                foreach (JObject obj in jarray)
                {
                    EmailAddress a = new EmailAddress(obj);
                    emails.Add(a);
                }
                return emails; 
            }
        }
    }
}
