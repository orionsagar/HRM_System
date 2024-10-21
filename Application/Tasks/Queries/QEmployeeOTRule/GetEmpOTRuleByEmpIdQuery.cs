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
    public class GetEmpOTRuleByEmpIdQuery : IRequest<List<EmployeeOTRule>>
    {
        public int EmpId { get; set; }
    }
    public class GetEmpByOTRuleByEmpIdQueryHandler : IRequestHandler<GetEmpOTRuleByEmpIdQuery, List<EmployeeOTRule>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetEmpByOTRuleByEmpIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<EmployeeOTRule>> Handle(GetEmpOTRuleByEmpIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.EmployeeOTRules.GetByEmpId(request.EmpId);
            return result;
        }
    }
}
