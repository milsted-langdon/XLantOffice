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
        public void CreateSummaryListTest()
        {
            //arrange
            string jsonString = "[{\"id\":30945926,\"href\":\"https://api.intelliflo.com/v2/clients/30945926\"},{\"id\":30944834,\"href\":\"https://api.intelliflo.com/v2/clients/30944834\"}]";
            JArray _array = JArray.Parse(jsonString);
            List<MLFSClient> clients = new List<MLFSClient>();

            //act
            clients = MLFSClient.CreateSummaryList(_array);

            //assert
            Assert.AreEqual(2, clients.Count);
        }

        [TestMethod()]
        public void BuildMLFSClientFromJsonString()
        {
            //arrange
            string jsonResponse = "{\"id\":30944834,\"href\":\"https://api.intelliflo.com/v2/clients/30944834\",\"name\":\"John Smith\",\"createdAt\":\"2020-06-24T11:29:34Z\",\"category\":\"Retail\",\"externalReference\":\"30929016-30944834\",\"secondaryReference\":\"\",\"originalAdviser\":{\"id\":91653,\"name\":\"A rowe.sy\",\"href\":\"https://api.intelliflo.com/v2/advisers/91653\"},\"currentAdviser\":{\"id\":91653,\"name\":\"A rowe.sy\",\"href\":\"https://api.intelliflo.com/v2/advisers/91653\"},\"type\":\"Client\",\"partyType\":\"Person\",\"person\":{\"title\":\"Mr\",\"firstName\":\"John\",\"middleName\":\"Arthur\",\"lastName\":\"Smith\",\"dateOfBirth\":\"1978-01-01\",\"gender\":\"Male\",\"niNumber\":\"\",\"isDeceased\":false},\"addresses_href\":\"https://api.intelliflo.com/v2/clients/30944834/addresses\",\"contactdetails_href\":\"https://api.intelliflo.com/v2/clients/30944834/contactdetails\",\"plans_href\":\"https://api.intelliflo.com/v2/clients/30944834/plans\",\"relationships_href\":\"https://api.intelliflo.com/v2/clients/30944834/relationships\",\"servicecases_href\":\"https://api.intelliflo.com/v2/clients/30944834/servicecases\",\"dependants_href\":\"https://api.intelliflo.com/v2/clients/30944834/dependants\",\"tags\":[]}";
            JObject obj = JObject.Parse(jsonResponse);

            //act
            MLFSClient client = new MLFSClient(obj);

            //assert
            Assert.AreEqual("John Smith", client.Name);
        }

        [TestMethod()]
        public void BuildAClientWithJustAnId()
        {
            //arrange
            string jsonRespoonse = "30944834";

            //act
            MLFSClient client = new MLFSClient(jsonRespoonse);

            //assert
            Assert.AreEqual(jsonRespoonse, client.PrimaryID);
        }

        [TestMethod()]
        public void CreateListTest()
        {
            string jsonString = "[{\"id\":25575177,\"href\":\"https://api.intelliflo.com/v2/clients/25575177\",\"name\":\"Marion Beck\",\"createdAt\":\"2019-02-27T15:14:46Z\",\"category\":\"Retail\",\"externalReference\":\"7690372-25575177\",\"secondaryReference\":\"BE424\",\"originalAdviser\":{\"id\":27066,\"name\":\"Garry Dornan\",\"href\":\"https://api.intelliflo.com/v2/advisers/27066\"},\"currentAdviser\":{\"id\":27066,\"name\":\"Garry Dornan\",\"href\":\"https://api.intelliflo.com/v2/advisers/27066\"},\"type\":\"Client\",\"partyType\":\"Person\",\"person\":{\"title\":\"Mrs\",\"firstName\":\"Marion\",\"middleName\":\"Elaine\",\"lastName\":\"Beck\",\"dateOfBirth\":\"1964-04-15\",\"gender\":\"Female\",\"niNumber\":\"NE103320B\",\"isDeceased\":false},\"addresses_href\":\"https://api.intelliflo.com/v2/clients/25575177/addresses\",\"contactdetails_href\":\"https://api.intelliflo.com/v2/clients/25575177/contactdetails\",\"plans_href\":\"https://api.intelliflo.com/v2/clients/25575177/plans\",\"relationships_href\":\"https://api.intelliflo.com/v2/clients/25575177/relationships\",\"servicecases_href\":\"https://api.intelliflo.com/v2/clients/25575177/servicecases\",\"dependants_href\":\"https://api.intelliflo.com/v2/clients/25575177/dependants\",\"isHeadOfFamilyGroup\":false,\"servicingAdministrator\":{\"id\":52724,\"href\":\"https://api.intelliflo.com/v2/users/52724\"},\"tags\":[]},{\"id\":25584235,\"href\":\"https://api.intelliflo.com/v2/clients/25584235\",\"name\":\"Graham Allen\",\"createdAt\":\"2019-03-04T11:47:34Z\",\"category\":\"Retail\",\"externalReference\":\"7690381-25584235\",\"secondaryReference\":\"AL279\",\"originalAdviser\":{\"id\":27075,\"name\":\"Dana Mackie\",\"href\":\"https://api.intelliflo.com/v2/advisers/27075\"},\"currentAdviser\":{\"id\":27075,\"name\":\"Dana Mackie\",\"href\":\"https://api.intelliflo.com/v2/advisers/27075\"},\"type\":\"Client\",\"partyType\":\"Person\",\"person\":{\"salutation\":\"\",\"title\":\"Mr\",\"firstName\":\"Graham\",\"middleName\":\"John\",\"lastName\":\"Allen\",\"dateOfBirth\":\"1967-03-24\",\"gender\":\"Male\",\"maidenName\":\"\",\"niNumber\":\"NM975566D\",\"isDeceased\":false,\"territorialProfile\":{\"ukResident\":false},\"healthProfile\":{\"isSmoker\":\"No\"},\"isPowerOfAttorneyGranted\":false,\"nationalClientIdentifier\":\"\"},\"addresses_href\":\"https://api.intelliflo.com/v2/clients/25584235/addresses\",\"contactdetails_href\":\"https://api.intelliflo.com/v2/clients/25584235/contactdetails\",\"plans_href\":\"https://api.intelliflo.com/v2/clients/25584235/plans\",\"relationships_href\":\"https://api.intelliflo.com/v2/clients/25584235/relationships\",\"servicecases_href\":\"https://api.intelliflo.com/v2/clients/25584235/servicecases\",\"dependants_href\":\"https://api.intelliflo.com/v2/clients/25584235/dependants\",\"isHeadOfFamilyGroup\":false,\"servicingAdministrator\":{\"id\":52733,\"href\":\"https://api.intelliflo.com/v2/users/52733\"},\"tags\":[]}]";
            JArray _array = JArray.Parse(jsonString);
            List<MLFSClient> clients = new List<MLFSClient>();

            //act
            clients = MLFSClient.CreateList(_array);

            //assert
            Assert.AreEqual(2, clients.Count);
        }
    }
}