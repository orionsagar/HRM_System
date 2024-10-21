using Domains.Models;
using MediatR;
using Persistence.DAL;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Queries.QEmployee
{
    public class GetAllDepartmentQuery : IRequest<List<Department>>
    {
        public int CompId { get; set; }
    }

    public class GetAllDepartmentHandler : IRequestHandler<GetAllDepartmentQuery, List<Department>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllDepartmentHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Department>> Handle(GetAllDepartmentQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.Departments.GetAllByCompId(request.CompId);
            return result.ToList();
        }
    }
}
