using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using XLantCore.Models;


namespace XLantDataStore.Repository
{
    public class MLFSAdvisorRepository
    {
        public static Staff GetMLFSAdvisor(string id)
        {
            DataTable table = SQLConnection.ReturnTable("select * from MLFSAdvisor where id = @param1", id);
            if (table.Rows.Count > 0)
            {
                MLFSAdvisor staff = new MLFSAdvisor(table.Rows[0]);
                table = SQLConnection.ReturnTable("select * from MLFSBudget where advisorID = @param1", staff.PrimaryID);
                staff.Budget = MLFSBudget.CreateList(table);
                table = SQLConnection.ReturnTable("select * from MLFSCommissionRates where advisorID = @param1", staff.PrimaryID);
                staff.CommissionRates = MLFSCommissionRate.CreateList(table);
                return staff;
            }
            else
            {
                return null;
            }
        }
    }
}
