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

namespace Application.Tasks.Queries.QShortLeaveAssign
{
    public class GetEmployeeListForShortLeaveAssignQuery : IRequest<List<ShortLeaveAssignEmpListVM>>
    {
        public ShortLeaveAssignFilter ShortLeaveAssignFilter { get; set; }
    }

    public class GetEmployeeShortLeaveAssignListQueryHandler : IRequestHandler<GetEmployeeListForShortLeaveAssignQuery, List<ShortLeaveAssignEmpListVM>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetEmployeeShortLeaveAssignListQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<ShortLeaveAssignEmpListVM>> Handle(GetEmployeeListForShortLeaveAssignQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.ShortLeaveAssign.EmpList(request.ShortLeaveAssignFilter);
            return result.ToList();
        }
    }
}
