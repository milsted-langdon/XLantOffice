using Microsoft.VisualStudio.TestTools.UnitTesting;
using XLantCore.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Linq;
using Newtonsoft.Json;

namespace XLantCore.Models.Tests
{
    [TestClass()]
    public class EmailAddressTests
    {
        [TestMethod()]
        public void CreateFromJsonString()
        {
            //arrange
            string s = "{\"id\":30075068,\"href\":\"https://api.intelliflo.com/v2/clients/30944834/contactdetails/30075068\",\"type\":\"Email\",\"value\":\"rowe.sy@gmail.com\",\"isDefault\":true}";
            JObject obj = JObject.Parse(s);

            //act
            EmailAddress email = new EmailAddress(obj);

            //assert
            Assert.AreEqual("rowe.sy@gmail.com", email.Address);
        }

        [TestMethod()]
        public void CreateListFromJsonString()
        {
            //arrange
            string s = "{\"href\":\"https://api.intelliflo.com/v2/clients/30944834/contactdetails\",\"first_href\":\"https://api.intelliflo.com/v2/clients/30944834/contactdetails?top=100&skip=0\",\"items\":[{\"id\":30075068,\"href\":\"https://api.intelliflo.com/v2/clients/30944834/contactdetails/30075068\",\"type\":\"Email2\",\"value\":\"syrowe@hotmail.com\",\"isDefault\":true},{\"id\":30075068,\"href\":\"https://api.intelliflo.com/v2/clients/30944834/contactdetails/30075068\",\"type\":\"Email\",\"value\":\"rowe.sy@gmail.com\",\"isDefault\":true}],\"count\":3}";
            JArray jarray = Tools.ExtractItemsArrayFromJsonString(s);

            //act
            List<EmailAddress> emails = EmailAddress.CreateList(jarray);

            //assert
            Assert.AreEqual(2, emails.Count);
        }
    }
}