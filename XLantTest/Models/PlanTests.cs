﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using XLantCore.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace XLantCore.Models.Tests
{
    [TestClass()]
    public class PlanTests
    {
        [TestMethod()]
        public void CreatePlanFromJsonString()
        {
            //arrange
            string jsonResponse = "{\"id\":55475389,\"href\":\"https://api.intelliflo.com/v2/clients/30944834/plans/55475389\",\"currency\":\"GBP\",\"discriminator\":\"LoanCreditPlan\",\"planType\":{\"name\":\"Bridging Loan\",\"portfolioCategory\":\"Loans\"},\"policyNumber\":\"123456798\",\"productName\":\"LoansRUs\",\"productProvider\":{\"id\":2139,\"href\":\"https://api.intelliflo.com/v2/productproviders/2139\",\"name\":\"1st Port Asset Management\"},\"sellingAdviser\":{\"id\":91653,\"href\":\"https://api.intelliflo.com/v2/advisers/91653\"},\"owners\":[{\"id\":30945926,\"href\":\"https://api.intelliflo.com/v2/clients/30945926\"},{\"id\":30944834,\"href\":\"https://api.intelliflo.com/v2/clients/30944834\"}],\"isVisibleToClient\":false,\"currentStatus\":\"Draft\",\"isPreExisting\":false,\"reference\":\"IOB55475389\",\"planTypes_href\":\"https://api.intelliflo.com/v2/plantypes\",\"valuations_href\":\"https://api.intelliflo.com/v2/clients/30944834/plans/55475389/valuations\",\"contributions_href\":\"https://api.intelliflo.com/v2/clients/30944834/plans/55475389/contributions\",\"topups_href\":\"https://api.intelliflo.com/v2/clients/30944834/plans/55475389/topups\",\"planHoldings_href\":\"https://api.intelliflo.com/v2/clients/30944834/plans/55475389/holdings\",\"lifecycle\":{\"id\":46582,\"name\":\"New Business - Mortgages\",\"href\":\"https://api.intelliflo.com/v2/lifecycles/46582\"},\"isTopup\":false,\"isAdviceOffPanel\":false,\"otherReferences\":{\"portalReference\":\"\"},\"clientCategory\":\"Retail\",\"available_plan_purposes_href\":\"https://api.intelliflo.com/v2/planpurposes?planType=Bridging%20Loan\",\"plan_purposes_href\":\"https://api.intelliflo.com/v2/clients/30944834/plans/55475389/purposes\",\"withdrawals_href\":\"https://api.intelliflo.com/v2/clients/30944834/plans/55475389/withdrawals\",\"banding\":{\"id\":106604,\"href\":\"https://api.intelliflo.com/v2/advisers/91653/bandingtemplates/106604\"},\"forwardIncomeTo\":{\"id\":91653,\"href\":\"https://api.intelliflo.com/v2/advisers/91653\",\"useBanding\":false},\"adviceStatus\":{\"value\":\"UnderAdvice\"}}";
            JObject obj = JObject.Parse(jsonResponse);

            //act
            MLFSPlan plan = new MLFSPlan(obj);

            //assert
            Assert.AreEqual("LoansRUs", plan.ProductName);
        }

        [TestMethod()]
        public void CreateListOfPlansFromJsonString()
        {
            //arrange
            string jsonResponse = "{\"href\":\"https://api.intelliflo.com/v2/clients/30944834/plans\",\"first_href\":\"https://api.intelliflo.com/v2/clients/30944834/plans?top=100&skip=0\",\"items\":[{\"id\":55475389,\"href\":\"https://api.intelliflo.com/v2/clients/30944834/plans/55475389\",\"currency\":\"GBP\",\"discriminator\":\"LoanCreditPlan\",\"planType\":{\"name\":\"Bridging Loan\",\"portfolioCategory\":\"Loans\"},\"policyNumber\":\"123456798\",\"productName\":\"LoansRUs\",\"productProvider\":{\"id\":2139,\"href\":\"https://api.intelliflo.com/v2/productproviders/2139\",\"name\":\"1st Port Asset Management\"},\"sellingAdviser\":{\"id\":91653,\"href\":\"https://api.intelliflo.com/v2/advisers/91653\"},\"owners\":[{\"id\":30945926,\"href\":\"https://api.intelliflo.com/v2/clients/30945926\"},{\"id\":30944834,\"href\":\"https://api.intelliflo.com/v2/clients/30944834\"}],\"isVisibleToClient\":false,\"currentStatus\":\"Draft\",\"isPreExisting\":false,\"reference\":\"IOB55475389\",\"planTypes_href\":\"https://api.intelliflo.com/v2/plantypes\",\"valuations_href\":\"https://api.intelliflo.com/v2/clients/30944834/plans/55475389/valuations\",\"contributions_href\":\"https://api.intelliflo.com/v2/clients/30944834/plans/55475389/contributions\",\"topups_href\":\"https://api.intelliflo.com/v2/clients/30944834/plans/55475389/topups\",\"planHoldings_href\":\"https://api.intelliflo.com/v2/clients/30944834/plans/55475389/holdings\",\"lifecycle\":{\"id\":46582,\"name\":\"New Business - Mortgages\",\"href\":\"https://api.intelliflo.com/v2/lifecycles/46582\"},\"isTopup\":false,\"isAdviceOffPanel\":false,\"otherReferences\":{\"portalReference\":\"\"},\"clientCategory\":\"Retail\",\"available_plan_purposes_href\":\"https://api.intelliflo.com/v2/planpurposes?planType=Bridging%20Loan\",\"plan_purposes_href\":\"https://api.intelliflo.com/v2/clients/30944834/plans/55475389/purposes\",\"withdrawals_href\":\"https://api.intelliflo.com/v2/clients/30944834/plans/55475389/withdrawals\",\"banding\":{\"id\":106604,\"href\":\"https://api.intelliflo.com/v2/advisers/91653/bandingtemplates/106604\"},\"forwardIncomeTo\":{\"id\":91653,\"href\":\"https://api.intelliflo.com/v2/advisers/91653\",\"useBanding\":false},\"adviceStatus\":{\"value\":\"UnderAdvice\"}},{\"id\":55475456,\"href\":\"https://api.intelliflo.com/v2/clients/30944834/plans/55475456\",\"currency\":\"GBP\",\"discriminator\":\"PensionContributionDrawdownPlan\",\"planType\":{\"name\":\"Family SIPP\",\"portfolioCategory\":\"Pensions\"},\"productName\":\"SIPPtastic\",\"productProvider\":{\"id\":1200,\"href\":\"https://api.intelliflo.com/v2/productproviders/1200\",\"name\":\"Abacus Financial Services Ltd\"},\"sellingAdviser\":{\"id\":91653,\"href\":\"https://api.intelliflo.com/v2/advisers/91653\"},\"owners\":[{\"id\":30944834,\"href\":\"https://api.intelliflo.com/v2/clients/30944834\"}],\"isVisibleToClient\":false,\"currentStatus\":\"Draft\",\"isPreExisting\":false,\"reference\":\"IOB55475456\",\"planTypes_href\":\"https://api.intelliflo.com/v2/plantypes\",\"valuations_href\":\"https://api.intelliflo.com/v2/clients/30944834/plans/55475456/valuations\",\"contributions_href\":\"https://api.intelliflo.com/v2/clients/30944834/plans/55475456/contributions\",\"topups_href\":\"https://api.intelliflo.com/v2/clients/30944834/plans/55475456/topups\",\"planHoldings_href\":\"https://api.intelliflo.com/v2/clients/30944834/plans/55475456/holdings\",\"lifecycle\":{\"id\":46583,\"name\":\"New Business - Pension\",\"href\":\"https://api.intelliflo.com/v2/lifecycles/46583\"},\"isTopup\":false,\"isAdviceOffPanel\":false,\"otherReferences\":{},\"clientCategory\":\"Retail\",\"available_plan_purposes_href\":\"https://api.intelliflo.com/v2/planpurposes?planType=Family%20SIPP\",\"plan_purposes_href\":\"https://api.intelliflo.com/v2/clients/30944834/plans/55475456/purposes\",\"withdrawals_href\":\"https://api.intelliflo.com/v2/clients/30944834/plans/55475456/withdrawals\",\"banding\":{\"id\":106604,\"href\":\"https://api.intelliflo.com/v2/advisers/91653/bandingtemplates/106604\"},\"forwardIncomeTo\":{\"id\":91653,\"href\":\"https://api.intelliflo.com/v2/advisers/91653\",\"useBanding\":false},\"adviceStatus\":{\"value\":\"UnderAdvice\"}}],\"count\":2}";
            JArray jarray = Tools.ExtractItemsArrayFromJsonString(jsonResponse);

            //act
            List<MLFSPlan> plans = MLFSPlan.CreateList(jarray);

            //assert
            Assert.AreEqual(2, plans.Count);
        }
    }
}