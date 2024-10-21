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

namespace Application.Tasks.Queries.QEmployee
{
    public class GetEmpHistoryInfoByEmpIdQuery : IRequest<EmployeeVM>
    {
        public int EmpId { get; set; }
    }
    public class GetEmpHistoryInfoByEmpIdQueryHandler : IRequestHandler<GetEmpHistoryInfoByEmpIdQuery, EmployeeVM>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetEmpHistoryInfoByEmpIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EmployeeVM> Handle(GetEmpHistoryInfoByEmpIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.Employees.GetEmpHistoryInfoByEmpId(request.EmpId);
            return result;
        }
    }
}
