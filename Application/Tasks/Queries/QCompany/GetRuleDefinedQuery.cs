using Domains.Models;
using MediatR;
using Persistence.DAL;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;

namespace Application.Tasks.Queries.QCompany
{
    public class GetRuleDefindQuery : IRequest<List<RuleDefined>>
    {
    }

    public class GetRuleDefindQueryHandler : IRequestHandler<GetRuleDefindQuery, List<RuleDefined>>
    {
        private readonly IUnitOfWork _unitOfWork;


        public GetRuleDefindQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }

        public async Task<List<RuleDefined>> Handle(GetRuleDefindQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.Companys.GetRulesDefindAsync();
            return result.ToList();
        }
    }
}
