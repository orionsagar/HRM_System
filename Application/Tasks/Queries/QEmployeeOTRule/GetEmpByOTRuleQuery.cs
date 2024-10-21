using Domains.Models;
using Domains.ViewModels;
using MediatR;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Queries.QEmployeeOTRule
{
    public class GetEmpByOTRuleQuery : IRequest<List<EmpOTListVM>>
    {
        public EmpOTRuleFilter EmpOTRuleFilter { get; set; }
    }
    public class GetEmpByOTRuleQueryHandler : IRequestHandler<GetEmpByOTRuleQuery, List<EmpOTListVM>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetEmpByOTRuleQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<EmpOTListVM>> Handle(GetEmpByOTRuleQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.EmployeeOTRules.EmpOTRuleList(request.EmpOTRuleFilter);
            return result.ToList();
        }
    }
}
