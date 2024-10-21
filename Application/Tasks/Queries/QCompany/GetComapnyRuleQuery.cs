using Domains.Models;
using MediatR;
using Persistence.DAL;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;

namespace Application.Tasks.Queries.QCompany
{
    public class GetCompanyRuleQuery : IRequest<List<CompanyRule>>
    {
        public int CompId { get; set; }
    }

    public class GetCompanyRuleQueryHandler : IRequestHandler<GetCompanyRuleQuery, List<CompanyRule>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetCompanyRuleQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<CompanyRule>> Handle(GetCompanyRuleQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.Companys.GetCompanyRuleAsync(request.CompId);
            return result.ToList();
        }
    }
}
