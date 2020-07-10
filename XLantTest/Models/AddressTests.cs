using Microsoft.VisualStudio.TestTools.UnitTesting;
using XLantCore.Models.Extension;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace XLantCore.Models.Extension.Tests
{
    [TestClass()]
    public class AddressTests
    {
        [TestMethod()]
        public void CreateListFromJsonString()
        {
            //arrange
            string s = "{\"href\":\"https://api.intelliflo.com/v2/clients/30944834/addresses\",\"first_href\":\"https://api.intelliflo.com/v2/clients/30944834/addresses?top=100&skip=0\",\"items\":[{\"id\":28477609,\"href\":\"https://api.intelliflo.com/v2/clients/30944834/addresses/28477609\",\"residencyStatus\":\"OwnerOccupierMortgaged\",\"type\":\"Home\",\"residentFrom\":\"2010-01-01T00:00:00Z\",\"residentTo\":null,\"status\":\"Current\",\"isDefault\":true,\"address\":{\"line1\":\"One Redcliff Street\",\"line2\":\"\",\"line3\":\"\",\"line4\":\"\",\"locality\":\"Bristol\",\"postalCode\":\"BS1 6NP\",\"country\":{\"code\":\"GB\",\"name\":\"United Kingdom\"},\"county\":{\"code\":\"GB-BST\",\"name\":\"Bristol\"}},\"isRegisteredOnElectoralRoll\":null}],\"count\":1}";
            JArray jarray = Tools.ExtractItemsArrayFromJsonString(s);

            //act
            List<Address> addresses = Address.CreateList(jarray);

            //assert
            Assert.AreEqual(1, addresses.Count);
        }

        [TestMethod()]
        public void CreateAddressFromJsonString()
        {
            //arrange
            string s = "{\"id\":28477609,\"href\":\"https://api.intelliflo.com/v2/clients/30944834/addresses/28477609\",\"residencyStatus\":\"OwnerOccupierMortgaged\",\"type\":\"Home\",\"residentFrom\":\"2010-01-01T00:00:00Z\",\"residentTo\":null,\"status\":\"Current\",\"isDefault\":true,\"address\":{\"line1\":\"One Redcliff Street\",\"line2\":\"\",\"line3\":\"\",\"line4\":\"\",\"locality\":\"Bristol\",\"postalCode\":\"BS1 6NP\",\"country\":{\"code\":\"GB\",\"name\":\"United Kingdom\"},\"county\":{\"code\":\"GB-BST\",\"name\":\"Bristol\"}},\"isRegisteredOnElectoralRoll\":null}";
            JObject obj = JObject.Parse(s);

            //act
            Address a = new Address(obj);

            //assert
            Assert.AreEqual("Bristol", a.City);
        }
    }
}