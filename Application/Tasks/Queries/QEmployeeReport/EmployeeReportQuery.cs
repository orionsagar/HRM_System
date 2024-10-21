using Domains.ViewModels;
using MediatR;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Queries.QEmployeeEmployee
{
    public class EmployeeReportQuery : IRequest<List<EmployeeReportVM>>
    {
        //int DisplayLength, int DisplayStart, int SortCol, string SortDir, string Search, int ComId
        public int SectId { get; set; }
        public int DeptId { get; set; }
        public int DesigId { get; set; }       
        public int ComId { get; set; }
    }
    public class EmployeeReportQueryHandler : IRequestHandler<EmployeeReportQuery, List<EmployeeReportVM>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public EmployeeReportQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<List<EmployeeReportVM>> Handle(EmployeeReportQuery request, CancellationToken cancellationToken)
        {
            var result = _unitOfWork.Employees.EmployeeReport(request.SectId, request.DeptId, request.DesigId, request.ComId);
            return result;
        }
    }
}
