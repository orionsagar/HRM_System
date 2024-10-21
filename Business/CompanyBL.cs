using Domains.ViewModels;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public interface ICompanyBL
    {
        Task<List<CompanyViewModel>> GetCompanyByComId(int ComId);
    }
    public class CompanyBL: ICompanyBL
    {
        private readonly IUnitOfWork _work;
        public CompanyBL(IUnitOfWork work)
        {
            _work = work;
        }

        public async Task<List<CompanyViewModel>> GetCompanyByComId(int ComId)
        {
            var cominfo = await _work.Companys.GetCompanyInfo(ComId);
            return cominfo;
        }
    }
}
