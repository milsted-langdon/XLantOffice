using Microsoft.VisualStudio.TestTools.UnitTesting;
using XLantCore.Models;
using System;
using System.Collections.Generic;
using System.Text;


namespace XLantCore.Models.Tests
{
    [TestClass()]
    public class PersonTests
    {
        [TestMethod()]
        public void ParseTitleTest()
        {
            //arrange
            string s = "Mr";
            string u = "dfgdsgfdg";

            Title t = Person.ParseTitle(s);
            Title t2 = Person.ParseTitle(u);

            Assert.AreEqual(Title.Mr, t, "Mr not assigned");
            Assert.AreEqual(Title.Unknown, t2, "Unknown not assigned where no match");
        }
    }
}