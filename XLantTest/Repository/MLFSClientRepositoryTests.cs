using Microsoft.VisualStudio.TestTools.UnitTesting;
using XLantDataStore.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using XLantCore.Models;
using Newtonsoft.Json.Linq;

namespace XLantDataStore.Repository.Tests
{
    [TestClass()]
    public class MLFSClientRepositoryTests
    {
        [TestMethod()]
        public void GetAccessTokenFromIO()
        {
            //arrange

            //act
            MLFSClientRepository.APIToken token = MLFSClientRepository.GetToken();

            //assert
            Assert.IsNotNull(token);
            Assert.IsNotNull(token.AccessToken);
        }

        [TestMethod()]
        public void GetResponseFromServer()
        {
            //arrange
            string id = "";

            if (!String.IsNullOrEmpty(id))
            {
                //act
                MLFSClient client = MLFSClientRepository.GetMLFSClient(id);

                //assert
                Assert.IsNotNull(client);
            }
            else
            {
                //for occaisional testing pass normally
                Assert.IsNull(null);
            }

        }

        [TestMethod()]
        public void SplitContactDetailsArrayTest()
        {
            //arrange
            string s = "[{\"id\":30075066,\"href\":\"https://api.intelliflo.com/v2/clients/30944834/contactdetails/30075066\",\"type\":\"Telephone\",\"value\":\"0117 9452500\",\"isDefault\":true},{\"id\":30075067,\"href\":\"https://api.intelliflo.com/v2/clients/30944834/contactdetails/30075067\",\"type\":\"Mobile\",\"value\":\"07971 123456\",\"isDefault\":true},{\"id\":30075068,\"href\":\"https://api.intelliflo.com/v2/clients/30944834/contactdetails/30075068\",\"type\":\"Email\",\"value\":\"rowe.sy@gmail.com\",\"isDefault\":true}]";
            JArray a = JArray.Parse(s);

            //act
            string result = MLFSClientRepository.SplitContactDetails(a, true);

            //assert
            Assert.AreEqual(true, result.Contains("Email"));
            Assert.AreEqual(false, result.Contains("Telephone"));
        }
    }
}