using Domains.Models;
using MediatR;
using Persistence.DAL;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Queries.QEmployee
{
    public class GetEmployeeTypeDropdownQuery : IRequest<List<SelectListItemModel>>
    {
        public int CompID { get; set; }
        public int OrgId { get; set; }
    }

    public class GetEmployeeTypeDropdownHandler : IRequestHandler<GetEmployeeTypeDropdownQuery, List<SelectListItemModel>>
    {
        private readonly IUnitOfWork _unitOfWork;


        public GetEmployeeTypeDropdownHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<SelectListItemModel>> Handle(GetEmployeeTypeDropdownQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.EmployeeTypes.Dropdown(request.CompID, request.OrgId);
            return result.ToList();
        }
    }
}
