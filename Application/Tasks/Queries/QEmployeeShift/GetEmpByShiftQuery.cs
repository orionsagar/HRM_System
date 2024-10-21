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

namespace Application.Tasks.Queries.QEmployeeShift
{
    public class GetEmpByShiftQuery : IRequest<List<EmpShiftListVM>>
    {
        public EmpShiftFilter EmpShiftFilter { get; set; }
    }
    public class GetEmpByShiftQueryHandler : IRequestHandler<GetEmpByShiftQuery, List<EmpShiftListVM>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetEmpByShiftQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<EmpShiftListVM>> Handle(GetEmpByShiftQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.EmployeeShifts.EmpShiftList(request.EmpShiftFilter);
            return result.ToList();
        }
    }
}
