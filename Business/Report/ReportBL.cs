using Application.Tasks.Queries.QCompany;
using Domains.ViewModels;
using MediatR;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


namespace Business.Report
{
    public interface IReportBL
    {
        Task<List<IDCardVM>> IDCardDetails(int SectId, int DeptId, int DesigId, string CardNo, int CompId);
    }
    public class ReportBL : IReportBL
    {
        private readonly IUnitOfWork _work;

        public ReportBL(IUnitOfWork work)
        {
            _work = work;
        }

        public async Task<List<IDCardVM>> IDCardDetails(int SectId, int DeptId, int DesigId, string CardNo, int CompId)
        {
            var cardlist = await _work.ManPowerReport.SP_Rpt_IDCard(SectId, DeptId, DesigId, CardNo, CompId);
            return cardlist;
        }        
    }
}
