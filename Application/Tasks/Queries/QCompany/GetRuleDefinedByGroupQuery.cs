using Domains.Models;
using MediatR;
using Persistence.DAL;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;

namespace Application.Tasks.Queries.QCompany
{
    public class GetRuleDefindByGroupQuery : IRequest<List<RuleDefined>>
    {
        public string RuleGroup { get; set; }
    }

    public class GetRuleDefindByGroupQueryHandler : IRequestHandler<GetRuleDefindByGroupQuery, List<RuleDefined>>
    {
        private readonly IUnitOfWork _unitOfWork;


        public GetRuleDefindByGroupQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }

        public async Task<List<RuleDefined>> Handle(GetRuleDefindByGroupQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.Companys.GetRulesDefindByGroupAsync(request.RuleGroup);
            return result.ToList();
        }
    }
}
