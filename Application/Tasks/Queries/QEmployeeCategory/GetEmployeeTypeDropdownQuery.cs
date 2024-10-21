using Domains.Models;
using MediatR;
using Persistence.DAL;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Queries.QEmployeeCategory
{
    public class GetEmployeeCategoryDropdownQuery : IRequest<List<SelectListItemModel>>
    {
        public int CompID { get; set; }
        public int OrgId { get; set; }
        public int ClientId { get; set; }
    }

    public class GetEmployeeCategoryDropdownHandler : IRequestHandler<GetEmployeeCategoryDropdownQuery, List<SelectListItemModel>>
    {
        private readonly IUnitOfWork _unitOfWork;


        public GetEmployeeCategoryDropdownHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<SelectListItemModel>> Handle(GetEmployeeCategoryDropdownQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.EmployeeCategorys.Dropdown(request.CompID, request.ClientId,request.OrgId);
            return result.ToList();
        }
    }
}
