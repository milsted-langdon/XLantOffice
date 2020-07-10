using Microsoft.VisualStudio.TestTools.UnitTesting;
using XLantCore.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Data;

namespace XLantCore.Models.Tests
{
    [TestClass()]
    public class MLFSAdvisorTests
    {
        [TestMethod()]
        public void MLFSAdvisorTest()
        {
            //arrange
            DataTable table = new DataTable();
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("Name", typeof(string));
            table.Columns.Add("Organisation", typeof(string));

            DataRow row = table.NewRow();
            row["Id"] = 1;
            row["Name"] = "Jeff Bloggs";
            row["Organisation"] = "FPP";

            //act
            MLFSAdvisor adv = new MLFSAdvisor(row);

            //assert
            Assert.AreEqual("Jeff Bloggs", adv.Name, "The name does not match");
        }
    }
}