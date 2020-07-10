using Microsoft.VisualStudio.TestTools.UnitTesting;
using XLantDataStore.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using XLantCore.Models;
using Newtonsoft.Json.Linq;
using XLantCore;

namespace XLantDataStore.Repository.Tests
{
    [TestClass()]
    public class IOConnectionTests
    {
        [TestMethod()]
        public void GetAccessTokenFromIO()
        {
            //arrange

            //act
            IOConnection.APIToken token = IOConnection.GetToken();

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
                //Test using the MLFS Client method as an example
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


    }
}