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

namespace Application.Tasks.Queries.QEmployeeJobCalender
{
    public class GetEmpByJobCalenderByEmpIdQuery : IRequest<List<EmployeeJobCalendar>>
    {
        public int EmpId { get; set; }
    }
    public class GetEmpByJobCalenderByEmpIdQueryHandler : IRequestHandler<GetEmpByJobCalenderByEmpIdQuery, List<EmployeeJobCalendar>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetEmpByJobCalenderByEmpIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<EmployeeJobCalendar>> Handle(GetEmpByJobCalenderByEmpIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.EmployeeJobCalendars.GetByEmpId(request.EmpId);
            return result;
        }
    }
}
