using Domains.ViewModels;
using MediatR;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Queries._LeaveReport
{
    public class SP_LeaveBalanceQuery : IRequest<List<LeaveDetailsVM>>
    {
        public int FiscalYearId { get; set; }
        public int SectId { get; set; }
        public int DesigId { get; set; }
        public int DeptId { get; set; }
        public int EmpId { get; set; }
    }
    public class SP_LeaveBalanceQueryHandler : IRequestHandler<SP_LeaveBalanceQuery, List<LeaveDetailsVM>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public SP_LeaveBalanceQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<LeaveDetailsVM>> Handle(SP_LeaveBalanceQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.LeaveTransaction.SP_LeaveBalance(request.FiscalYearId, request.SectId, request.DesigId, request.DeptId, request.EmpId);
            return result;
        }
    }
}
