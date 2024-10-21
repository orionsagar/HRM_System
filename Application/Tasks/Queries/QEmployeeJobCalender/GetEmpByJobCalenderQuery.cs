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
    public class GetEmpByJobCalenderQuery : IRequest<List<EmpJobCalendarListVM>>
    {
        public EmpJobCalendarFilter EmpJobCalenderFilter { get; set; }
    }
    public class GetEmpByJobCalenderQueryHandler : IRequestHandler<GetEmpByJobCalenderQuery, List<EmpJobCalendarListVM>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetEmpByJobCalenderQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<EmpJobCalendarListVM>> Handle(GetEmpByJobCalenderQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.EmployeeJobCalendars.EmpJobCalenderList(request.EmpJobCalenderFilter);
            return result.ToList();
        }
    }
}
