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
    public class GetShortLeaveReportQuery : IRequest<List<ShortLeaveAssignEmpListVM>>
    {
        public EmpLeaveFilter EmpLeaveFilter { get; set; }
    }
    public class GetShortLeaveReportQueryHandler : IRequestHandler<GetShortLeaveReportQuery, List<ShortLeaveAssignEmpListVM>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetShortLeaveReportQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<ShortLeaveAssignEmpListVM>> Handle(GetShortLeaveReportQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.ShortLeaveAssign.ShortLeaveReport(request.EmpLeaveFilter);
            return result.ToList();
        }
    }
}
