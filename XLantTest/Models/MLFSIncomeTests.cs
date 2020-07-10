﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using XLantCore.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace XLantCore.Models.Tests
{
    [TestClass()]
    public class MLFSIncomeTests
    {
        [TestMethod()]
        public void MLFSIncomeCreateFromDataRowTest()
        {
            //arrange
            DataTable table = new DataTable();
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("IOReference", typeof(string));
            table.Columns.Add("ReportingPeriodId", typeof(int));
            table.Columns.Add("Organisation", typeof(string));
            table.Columns.Add("RelevantDate", typeof(DateTime));
            table.Columns.Add("AdvisorId", typeof(int));
            table.Columns.Add("ProviderName", typeof(string));
            table.Columns.Add("ClientName", typeof(string));
            table.Columns.Add("ClientId", typeof(string));
            table.Columns.Add("JointClientName", typeof(string));
            table.Columns.Add("JointClientId", typeof(string));
            table.Columns.Add("Campaign", typeof(string));
            table.Columns.Add("CampaignSource", typeof(string));
            table.Columns.Add("Amount", typeof(decimal));
            table.Columns.Add("FeeStatus", typeof(string));
            table.Columns.Add("PlanType", typeof(string));
            table.Columns.Add("PlanNumber", typeof(string));
            table.Columns.Add("IsTopup", typeof(bool));
            table.Columns.Add("IncomeType", typeof(string));

            DataRow row = table.NewRow();
            row["Id"] = 1;
            row["IOReference"] = "IOF123456";
            row["ReportingPeriodId"] = 1;
            row["Organisation"] = "MLFS";
            row["RelevantDate"] = DateTime.Now;
            row["AdvisorId"] = 4;
            row["ProviderName"] = "Elevate";
            row["ClientName"] = "Jeff Bloggs";
            row["ClientId"] = 5;
            row["JointClientName"] = "Jane Bloggs";
            row["JointClientId"] = 6;
            row["Campaign"] = "Teachers";
            row["CampaignSource"] = "ML";
            row["Amount"] = 1100;
            row["FeeStatus"] = "Paid";
            row["PlanType"] = "Pension";
            row["PlanNumber"] = "123456";
            row["IsTopUp"] = true;
            row["IncomeType"] = "InitialFee";
            table.Rows.Add(row);

            //act
            MLFSIncome income = new MLFSIncome(row, false);

            //assert
            Assert.AreEqual("Elevate", income.ProviderName, "Provider doesn't match");
            Assert.AreEqual(1100, income.Amount, "Gross amount does not match");
        }

        [TestMethod()]
        public void MLFSIncomeCreateFromIODataRowTest()
        {
            //arrange
            DataTable table = new DataTable();
            table.Columns.Add("IORef", typeof(string));
            table.Columns.Add("GroupOne", typeof(string));
            table.Columns.Add("Submitted", typeof(DateTime));
            table.Columns.Add("CRMContactId", typeof(int));
            table.Columns.Add("Provider", typeof(string));
            table.Columns.Add("ClientName", typeof(string));
            table.Columns.Add("ClientId", typeof(string));
            table.Columns.Add("JointClientName", typeof(string));
            table.Columns.Add("JointClientId", typeof(string));
            table.Columns.Add("CampaignType", typeof(string));
            table.Columns.Add("CampaignSource", typeof(string));
            table.Columns.Add("GrossFCI Excl.VAT", typeof(decimal));
            table.Columns.Add("FeeStatus", typeof(string));
            table.Columns.Add("PlanType", typeof(string));
            table.Columns.Add("PlanNumber", typeof(string));
            table.Columns.Add("IsTopup", typeof(bool));
            table.Columns.Add("IncomeType", typeof(string));

            DataRow row = table.NewRow();
            row["IORef"] = "IOF123456";
            row["GroupOne"] = "MLFS";
            row["Submitted"] = DateTime.Now;
            row["CRMContactId"] = 4;
            row["Provider"] = "Elevate";
            row["ClientName"] = "Jeff Bloggs";
            row["ClientId"] = 5;
            row["JointClientName"] = "Jane Bloggs";
            row["JointClientId"] = 6;
            row["Campaigntype"] = "Teachers";
            row["CampaignSource"] = "ML";
            row["GrossFCI Excl.VAT"] = 1100;
            row["FeeStatus"] = "Paid";
            row["PlanType"] = "Pension";
            row["PlanNumber"] = "123456";
            row["IsTopup"] = true;
            row["IncomeType"] = "InitialFee";
            table.Rows.Add(row);

            //act
            MLFSIncome income = new MLFSIncome(row, true);

            //assert
            Assert.AreEqual("Elevate", income.ProviderName, "Provider doesn't match");
            Assert.AreEqual(1100, income.Amount, "Amount does not match");
        }
    }
}