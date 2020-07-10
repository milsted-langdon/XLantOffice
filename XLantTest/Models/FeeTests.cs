using Microsoft.VisualStudio.TestTools.UnitTesting;
using XLantCore.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace XLantCore.Models.Tests
{
    [TestClass()]
    public class FeeTests
    {
        [TestMethod()]
        public void FeeTest()
        {
            //arrange
            string f = "{\"id\":6796456,\"href\":\"https://api.intelliflo.com/v2/clients/30944834/fees/6796456\",\"sequentialRef\":\"IOF06796456\",\"sellingAdviser\":{\"id\":91653,\"href\":\"https://crm.intelliflo.com/v2/advisers/91653\"},\"adviceCategory\":\"IndependentAdvice\",\"feeType\":{\"name\":\"PFP Premium Fee\",\"category\":\"OngoingFee\"},\"paymentType\":{\"name\":\"By Provider\",\"paidBy\":\"Provider\"},\"feeChargingType\":{\"name\":\"PercentOfFumAum\"},\"feePercentage\":0.50,\"net\":{\"currency\":\"GBP\",\"amount\":\"5000.0000\"},\"vat\":{\"currency\":\"GBP\",\"amount\":\"1000.0000\"},\"vatRate\":{\"rate\":0.0,\"isExempt\":true},\"status\":\"Draft\",\"statusDate\":\"2020-06-24T15:00:27Z\",\"recurring\":{\"frequency\":\"Monthly\",\"startsOn\":\"2020-07-31\",\"endsOn\":\"2030-06-30\"},\"isConsultancyFee\":false,\"banding\":{\"id\":106604,\"href\":\"https://api.intelliflo.com/v2/advisers/91653/bandingtemplates/106604\",\"percentage\":60.0},\"forwardIncomeTo\":{\"id\":91653,\"href\":\"https://crm.intelliflo.com/v2/advisers/91653\",\"useBanding\":false},\"clients\":[{\"id\":30944834,\"href\":\"https://crm.intelliflo.com/v2/clients/30944834\"}],\"plan_href\":\"https://portfolio.intelliflo.com/v2/clients/30944834/plans?filter=id in (55475456)\"}";
            JObject obj = JObject.Parse(f);

            //act
            Fee fee = new Fee(obj);

            //assert
            Assert.AreEqual(5000, fee.NetAmount);
        }
    }
}