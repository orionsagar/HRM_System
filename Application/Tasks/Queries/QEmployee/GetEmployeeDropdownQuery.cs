using Domains.Models;
using MediatR;
using Persistence.DAL;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Queries.QEmployee
{
    public class GetEmployeeDropdownQuery : IRequest<List<SelectListItemModel>>
    {
        public int CompID { get; set; }
        public int OrgId { get; set; }
        public int ClientId { get; set; }
    }

    public class GetEmployeeDropdownHandler : IRequestHandler<GetEmployeeDropdownQuery, List<SelectListItemModel>>
    {
        private readonly IUnitOfWork _unitOfWork;


        public GetEmployeeDropdownHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<SelectListItemModel>> Handle(GetEmployeeDropdownQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.Employees.Dropdown(request.OrgId,request.ClientId);
            return result.ToList();
        }
    }
}
