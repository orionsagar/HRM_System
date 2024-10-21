using Domains.Models;
using MediatR;
using Persistence.DAL;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Queries.QEmployeeStatus
{
    public class GetEmployeeStatusDropdownQuery : IRequest<List<SelectListItemModel>>
    {
        public int CompID { get; set; }
    }

    public class GetEmployeeStatusDropdownHandler : IRequestHandler<GetEmployeeStatusDropdownQuery, List<SelectListItemModel>>
    {
        private readonly IUnitOfWork _unitOfWork;


        public GetEmployeeStatusDropdownHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<SelectListItemModel>> Handle(GetEmployeeStatusDropdownQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.EmployeeStatus.Dropdown(request.CompID);
            return result.ToList();
        }
    }
}
