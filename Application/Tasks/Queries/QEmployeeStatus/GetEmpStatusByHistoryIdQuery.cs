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
    public class GetEmpStatusByHistoryIdQuery : IRequest<EmployeeHistory>
    {
        public int HistoryId { get; set; }
    }
    public class GetEmpByStatusByEmpIdQueryHandler : IRequestHandler<GetEmpStatusByHistoryIdQuery, EmployeeHistory>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetEmpByStatusByEmpIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EmployeeHistory> Handle(GetEmpStatusByHistoryIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.Employees.GetHistoryStatusById(request.HistoryId);
            return result;
        }
    }
}
