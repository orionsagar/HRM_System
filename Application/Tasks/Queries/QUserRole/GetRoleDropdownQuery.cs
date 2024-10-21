using Domains.Models;
using MediatR;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Queries.QUserRole
{
    public class GetRoleDropdownQuery : IRequest<List<SelectListItemModel>>
    {
        public int CompID { get; set; }
        public string RoleType { get; set; }
    }

    public class GetRoleDropdownQueryHandler : IRequestHandler<GetRoleDropdownQuery, List<SelectListItemModel>>
    {
        private readonly IUnitOfWork _unitOfWork;


        public GetRoleDropdownQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<SelectListItemModel>> Handle(GetRoleDropdownQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.UserRoles.Dropdown(request.CompID,request.RoleType);
            return result.ToList();
        }
    }
}
