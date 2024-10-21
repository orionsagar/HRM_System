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

namespace Application.Tasks.Queries.QEmployeeStatus
{
    public class GetEmpByStatusQuery : IRequest<List<EmployeeVM>>
    {
        public EmpStatusFilter EmpStatusFilter { get; set; }
    }
    public class GetEmpByStatusQueryHandler : IRequestHandler<GetEmpByStatusQuery, List<EmployeeVM>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetEmpByStatusQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<EmployeeVM>> Handle(GetEmpByStatusQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.EmployeeStatus.EmpStatusList(request.EmpStatusFilter);
            return result;
        }
    }
}
