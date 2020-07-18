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
        public async void GetAccessTokenFromIO()
        {
            //arrange

            //act
            IOConnection.APIToken token = await IOConnection.GetToken();

            //assert
            Assert.IsNotNull(token);
            Assert.IsNotNull(token.AccessToken);
        }
    }
}