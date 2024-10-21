using Domains.Models;
using MediatR;
using Persistence.DAL;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Queries.QEmployee
{
    public class GetAllEmployeeTyeQuery : IRequest<List<EmployeeType>>
    {
        public int CompId { get; set; }
        public int OrgId { get; set; }
    }

    public class GetAllEmployeeTyeHandler : IRequestHandler<GetAllEmployeeTyeQuery, List<EmployeeType>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllEmployeeTyeHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<EmployeeType>> Handle(GetAllEmployeeTyeQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.EmployeeTypes.GetAllByCompId(request.CompId, request.OrgId);
            return result.ToList();
        }
    }
}
