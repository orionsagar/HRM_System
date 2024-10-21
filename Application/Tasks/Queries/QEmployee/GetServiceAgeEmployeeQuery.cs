using Domains.Models;
using Domains.ViewModels;
using MediatR;
using Persistence.DAL;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Queries.QEmployee
{
    public class GetServiceAgeEmployeeQuery : IRequest<List<EmployeeVM>>
    {
        public EmployeeFilterVM EmployeeFilterVM { get; set; }
    }

    public class GetServiceAgeEmployeeQueryHandler : IRequestHandler<GetServiceAgeEmployeeQuery, List<EmployeeVM>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetServiceAgeEmployeeQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<EmployeeVM>> Handle(GetServiceAgeEmployeeQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.Employees.EmployeeSearchByServiceAge(request.EmployeeFilterVM);
            return result.ToList();
        }
    }
}
