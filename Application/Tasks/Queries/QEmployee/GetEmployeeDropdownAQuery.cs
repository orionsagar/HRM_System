using Domains.Models;
using MediatR;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Queries.QEmployee
{
    public class GetEmployeeDropdownAQuery : IRequest<List<SelectListItemModel>>
    {
        public int ClientID { get; set; }
        public int OrgID { get; set; }
    }

    public class GetEmployeeDropdownAQueryHandler : IRequestHandler<GetEmployeeDropdownAQuery, List<SelectListItemModel>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetEmployeeDropdownAQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<SelectListItemModel>> Handle(GetEmployeeDropdownAQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.Employees.Dropdown(request.OrgID, request.ClientID);
            return result.ToList();
        }
    }
}
