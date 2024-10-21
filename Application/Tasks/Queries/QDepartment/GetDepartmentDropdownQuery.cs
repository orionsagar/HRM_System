using Domains.Models;
using MediatR;
using Persistence.DAL;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Queries.QEmployee
{
    public class GetDepartmentDropdownQuery : IRequest<List<SelectListItemModel>>
    {
        public int CompID { get; set; }
        public int OrgId { get; set; }
    }

    public class GetDepartmentDropdownHandler : IRequestHandler<GetDepartmentDropdownQuery, List<SelectListItemModel>>
    {
        private readonly IUnitOfWork _unitOfWork;


        public GetDepartmentDropdownHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<SelectListItemModel>> Handle(GetDepartmentDropdownQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.Departments.Dropdown(request.CompID, request.OrgId);
            return result.ToList();
        }
    }
}
