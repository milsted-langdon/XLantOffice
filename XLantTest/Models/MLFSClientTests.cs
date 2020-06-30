using Microsoft.VisualStudio.TestTools.UnitTesting;
using XLantCore.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace XLantCore.Models.Tests
{
    [TestClass()]
    public class MLFSClientTests
    {
        [TestMethod()]
        public void CreateListTest()
        {
            //arrange
            string jsonString = "[{\"id\":30945926,\"href\":\"https://api.intelliflo.com/v2/clients/30945926\"},{\"id\":30944834,\"href\":\"https://api.intelliflo.com/v2/clients/30944834\"}]";            
            JArray _array = JArray.Parse(jsonString);
            List<MLFSClient> clients = new List<MLFSClient>();

            //act
            clients = MLFSClient.CreateList(_array);

            //assert
            Assert.AreEqual(2, clients.Count);
        }

        [TestMethod()]
        public void BuildMLFSClientFromJsonString()
        {
            //arrange
            string jsonRespoonse = "{\"id\":30944834,\"href\":\"https://api.intelliflo.com/v2/clients/30944834\",\"name\":\"John Smith\",\"createdAt\":\"2020-06-24T11:29:34Z\",\"category\":\"Retail\",\"externalReference\":\"30929016-30944834\",\"secondaryReference\":\"\",\"originalAdviser\":{\"id\":91653,\"name\":\"A rowe.sy\",\"href\":\"https://api.intelliflo.com/v2/advisers/91653\"},\"currentAdviser\":{\"id\":91653,\"name\":\"A rowe.sy\",\"href\":\"https://api.intelliflo.com/v2/advisers/91653\"},\"type\":\"Client\",\"partyType\":\"Person\",\"person\":{\"title\":\"Mr\",\"firstName\":\"John\",\"middleName\":\"Arthur\",\"lastName\":\"Smith\",\"dateOfBirth\":\"1978-01-01\",\"gender\":\"Male\",\"niNumber\":\"\",\"isDeceased\":false},\"addresses_href\":\"https://api.intelliflo.com/v2/clients/30944834/addresses\",\"contactdetails_href\":\"https://api.intelliflo.com/v2/clients/30944834/contactdetails\",\"plans_href\":\"https://api.intelliflo.com/v2/clients/30944834/plans\",\"relationships_href\":\"https://api.intelliflo.com/v2/clients/30944834/relationships\",\"servicecases_href\":\"https://api.intelliflo.com/v2/clients/30944834/servicecases\",\"dependants_href\":\"https://api.intelliflo.com/v2/clients/30944834/dependants\",\"tags\":[]}";

            //act
            MLFSClient client = new MLFSClient(jsonRespoonse);

            //assert
            Assert.AreEqual("John Smith", client.Name);
        }

        [TestMethod()]
        public void BuildAClientWithJustAnId()
        {
            //arrange
            string jsonRespoonse = "30944834";

            //act
            MLFSClient client = new MLFSClient(jsonRespoonse, true);

            //assert
            Assert.AreEqual(jsonRespoonse, client.PrimaryID);
        }
    }
}