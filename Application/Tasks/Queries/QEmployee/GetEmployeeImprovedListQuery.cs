using Domains.ViewModels;
using MediatR;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Queries.QEmployee
{
    public class GetEmployeeImprovedListQuery : IRequest<List<EmployeeVM>>
    {       
        public EmployeeFilterVM EmployeeFilterVM { get; set; }
    }
    public class GetEmployeeImprovedListQueryHandler : IRequestHandler<GetEmployeeImprovedListQuery, List<EmployeeVM>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetEmployeeImprovedListQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<List<EmployeeVM>> Handle(GetEmployeeImprovedListQuery request, CancellationToken cancellationToken)
        {
            var result = _unitOfWork.Employees.GetEmployeeImproveList(request.EmployeeFilterVM);
            return result;
        }
    }

    public class GetEmployeeNotINBOnusSetListQuery : IRequest<List<EmployeeVM>>
    {
        public EmployeeFilterVM EmployeeFilterVM { get; set; }
    }
    public class GetEmployeeNotINBOnusSetListQueryyHandler : IRequestHandler<GetEmployeeNotINBOnusSetListQuery, List<EmployeeVM>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetEmployeeNotINBOnusSetListQueryyHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<List<EmployeeVM>> Handle(GetEmployeeNotINBOnusSetListQuery request, CancellationToken cancellationToken)
        {
            var result = _unitOfWork.Employees.GetEmployeeNotInBonusSet(request.EmployeeFilterVM);
            return result;
        }
    }
}
