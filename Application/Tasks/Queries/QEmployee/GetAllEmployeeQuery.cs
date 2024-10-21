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
    public class GetAllEmployeeQuery : IRequest<List<EmployeeVM>>
    {
        public int CompId { get; set; }
        public int OrgId { get; set; }
    }

    public class GetAllEmployeeHandler : IRequestHandler<GetAllEmployeeQuery, List<EmployeeVM>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllEmployeeHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<EmployeeVM>> Handle(GetAllEmployeeQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.Employees.GetAllByCompId(request.CompId, request.OrgId);
            return result.ToList();
        }
    }
}
