using Domains.Models;
using MediatR;
using Persistence.DAL;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;

namespace Application.Tasks.Queries.QCompany
{
    public class GetCompanyRuleByGroupNameQuery : IRequest<CompanyRule>
    {
        public int CompId { get; set; }
        public string RuleGroup { get; set; }
    }

    public class GetCompanyRuleByGroupNameQueryHandler : IRequestHandler<GetCompanyRuleByGroupNameQuery, CompanyRule>
    {
        private readonly IUnitOfWork _unitOfWork;


        public GetCompanyRuleByGroupNameQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }

        public async Task<CompanyRule> Handle(GetCompanyRuleByGroupNameQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.Companys.GetCompanyRuleByGroupNameAsync(request.RuleGroup, request.CompId);
            return result;
        }
    }
}
