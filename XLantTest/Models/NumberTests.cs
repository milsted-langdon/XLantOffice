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
    public class NumberTests
    {
        [TestMethod()]
        public void CreateListFromJsonString()
        {
            //arrange
            string s = "{\"href\":\"https://api.intelliflo.com/v2/clients/30944834/contactdetails\",\"first_href\":\"https://api.intelliflo.com/v2/clients/30944834/contactdetails?top=100&skip=0\",\"items\":[{\"id\":30075066,\"href\":\"https://api.intelliflo.com/v2/clients/30944834/contactdetails/30075066\",\"type\":\"Telephone\",\"value\":\"0117 9452500\",\"isDefault\":true},{\"id\":30075067,\"href\":\"https://api.intelliflo.com/v2/clients/30944834/contactdetails/30075067\",\"type\":\"Mobile\",\"value\":\"07971 123456\",\"isDefault\":true}],\"count\":2}";
            JArray array = Tools.ExtractItemsArrayFromJsonString(s);

            //act
            List<Number> numbers = Number.CreateList(array);

            //assert
            Assert.AreEqual(2, numbers.Count);
        }

        [TestMethod()]
        public void CreateNumberFromJsonString()
        {
            //arrange
            string s = "{\"id\":30075066,\"href\":\"https://api.intelliflo.com/v2/clients/30944834/contactdetails/30075066\",\"type\":\"Telephone\",\"value\":\"0117 9452500\",\"isDefault\":true}";
            JObject obj = JObject.Parse(s);

            //act
            Number n = new Number(obj);

            //assert
            Assert.AreEqual("Telephone", n.Description);
        }
    }
}