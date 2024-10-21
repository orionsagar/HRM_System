using Domains.Models;
using MediatR;
using Persistence.DAL;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Queries.QEmployeeCategory
{
    public class GetAllEmployeeCategoryQuery : IRequest<List<EmployeeCategory>>
    {
        public int CompId { get; set; }
    }

    public class GetAllEmployeeCategoryHandler : IRequestHandler<GetAllEmployeeCategoryQuery, List<EmployeeCategory>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllEmployeeCategoryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<EmployeeCategory>> Handle(GetAllEmployeeCategoryQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.EmployeeCategorys.GetAllByCompId(request.CompId);
            return result.ToList();
        }
    }
}
