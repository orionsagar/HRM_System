using Domains.ViewModels;
using MediatR;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Queries.QEmployeeReport
{
    public class CardWiseEmployeeDetailsReportQuery : IRequest<List<EmployeeReportVM>>
    {
        public string CardNo { get; set; }
        public int CompId { get; set; }
    }

    public class CardWiseEmployeeDetailsReportQueryHandler : IRequestHandler<CardWiseEmployeeDetailsReportQuery, List<EmployeeReportVM>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CardWiseEmployeeDetailsReportQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<EmployeeReportVM>> Handle(CardWiseEmployeeDetailsReportQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.Employees.CardWiseEmployeeDetailsReport(request.CardNo,request.CompId);
            return result;
        }
    }
}
