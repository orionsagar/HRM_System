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
    public class GetEmpShiftByEmpIdQuery : IRequest<List<EmployeeShift>>
    {
        public int EmpId { get; set; }
    }
    public class GetEmpByShiftByEmpIdQueryHandler : IRequestHandler<GetEmpShiftByEmpIdQuery, List<EmployeeShift>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetEmpByShiftByEmpIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<EmployeeShift>> Handle(GetEmpShiftByEmpIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.EmployeeShifts.GetByEmpId(request.EmpId);
            return result;
        }
    }
}
