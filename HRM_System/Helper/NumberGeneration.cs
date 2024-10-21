using Domains.ViewModels;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace UKHRM.Helper
{
    public class NumberGeneration
    {
        private IApplicationReadDbConnection _readDb;
        private IApplicationDbContext _db;

        public NumberGeneration(IApplicationReadDbConnection readDb, IApplicationDbContext db)
        {
            _readDb = readDb;
            _db = db;
        }
        protected IDbConnection CreateConnection()
        {
            return new SqlConnection(AppConfig.DefaultConnection);
        }
        public async Task<string> GenarateBillNo(int OrgId, DateTime date, bool IsYear = true, bool IsMonth = true)
        {
            var query = "";
            var query1 = "";
            var billcode = "";
            using (var con = CreateConnection())
            {
                con.Open();
                if (IsYear && IsMonth)
                {
                    query = $@"select isnull(MAX(convert(int,substring(BillingNo,15,len(BillingNo)-13))),0) from BillingMaster where OrgId = {OrgId} AND YEAR(BillingDate) =  '{date.Year}' AND MONTH(BillingDate) =  '{date.Month}'";
                }
                else if (IsYear)
                {
                    query = $@"select isnull(MAX(convert(int,substring(BillingNo,15,len(BillingNo)-13))),0) from BillingMaster where OrgId = {OrgId} AND YEAR(BillingDate) =  '{date.Year}'";
                }
                else if (IsMonth)
                {
                    query = $@"select isnull(MAX(convert(int,substring(BillingNo,15,len(BillingNo)-13))),0) from BillingMaster where OrgId = {OrgId} AND MONTH(BillingDate) =  '{date.Month}'";
                }
                else
                {
                    query = $@"select isnull(MAX(convert(int,substring(BillingNo,15,len(BillingNo)-13))),0) from BillingMaster where OrgId = {OrgId}";
                }
                var ocode = "";
                var ccode = "";
                query1 = $@"select OrgCode from Organisations Where (OrgId = {OrgId} OR {OrgId} = 0)";
                var orgcode = await _readDb.QueryAsync<OrganisationVM>(query1);
                ocode = orgcode.Count == 1 ? orgcode.FirstOrDefault().OrgCode.ToString() : "Org-Code";

                if (OrgId > 0)
                {
                    query1 = $@"select ComCode from Companys where CompID in (select CompId from Organisations where OrgId = {OrgId})";
                    var comcode = await _readDb.QueryFirstOrDefaultAsync<CompanyViewModel>(query1);
                    ccode = comcode != null ? comcode.ComCode.ToString() : "Org-Code";
                }

                var InvNo = "0000000";
                var code = "";
                var prefix = "INV-" + ccode + ocode + "-";
                var year = "";
                if (IsMonth)
                {
                    year = date.ToString("yyMM");
                }
                else
                {
                    year = date.ToString("yy");
                }
                var invno = await _readDb.QueryFirstOrDefaultAsync<string>(query, "");
                code = invno;
                var invoiceno = SLNO(code, true);
                var nextcustid = prefix + year + invoiceno;                
                return nextcustid;
            }                      
        }
        public async Task<string> GenaratePRNo(int OrgId, DateTime date, bool IsYear = true, bool IsMonth = true)
        {
            var query = "";
            var query1 = "";
            var billcode = "";
            using (var con = CreateConnection())
            {
                con.Open();
                if (IsYear && IsMonth)
                {
                    query = $@"select isnull(MAX(convert(int,substring(PRNo,15,len(PRNo)-13))),0) from PaymentReceiptMaster where OrgId = {OrgId} AND YEAR(PRDate) =  '{date.Year}' AND MONTH(PRDate) =  '{date.Month}'";
                }
                else if (IsYear)
                {
                    query = $@"select isnull(MAX(convert(int,substring(PRNo,15,len(PRNo)-13))),0) from PaymentReceiptMaster where OrgId = {OrgId} AND YEAR(PRDate) =  '{date.Year}'";
                }
                else if (IsMonth)
                {
                    query = $@"select isnull(MAX(convert(int,substring(PRNo,15,len(PRNo)-13))),0) from PaymentReceiptMaster where OrgId = {OrgId} AND MONTH(PRDate) =  '{date.Month}'";
                }
                else
                {
                    query = $@"select isnull(MAX(convert(int,substring(PRNo,15,len(PRNo)-13))),0) from PaymentReceiptMaster where OrgId = {OrgId}";
                }
                var ocode = "";
                var ccode = "";
                query1 = $@"select OrgCode from Organisations Where (OrgId = {OrgId} OR {OrgId} = 0)";
                var orgcode = await _readDb.QueryAsync<OrganisationVM>(query1);
                ocode = orgcode.Count == 1 ? orgcode.FirstOrDefault().OrgCode.ToString() : "Org-Code";

                if (OrgId > 0)
                {
                    query1 = $@"select ComCode from Companys where CompID in (select CompId from Organisations where OrgId = {OrgId})";
                    var comcode = await _readDb.QueryFirstOrDefaultAsync<CompanyViewModel>(query1);
                    ccode = comcode != null ? comcode.ComCode.ToString() : "Org-Code";
                }

                var InvNo = "0000000";
                var code = "";
                var prefix = "INV-" + ccode + ocode + "-";
                var year = "";
                if (IsMonth)
                {
                    year = date.ToString("yyMM");
                }
                else
                {
                    year = date.ToString("yy");
                }
                var invno = await _readDb.QueryFirstOrDefaultAsync<string>(query, "");
                code = invno;
                var invoiceno = SLNO(code, true);
                var nextcustid = prefix + year + invoiceno;                
                return nextcustid;
            }                      
        }

        public string SLNO(string sl, bool ismonth)
        {
            int no = int.Parse(sl);
            int final = no + 1;
            string length = final.ToString();
            var result = "";
            if (length.Length == 1)
            {
                result = "000" + final.ToString();
            }
            else if (length.Length == 2)
            {
                result = "00" + final.ToString();
            }
            else if (length.Length == 3)
            {
                result = "0" + final.ToString();
            }
            else
            {
                result = final.ToString();
            }
            return result.ToString();
        }

    }
}
