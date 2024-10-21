using Domains.Models;
using MediatR;
using Persistence.DAL;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Queries.QUser
{
    public class GetUserDropdownQuery : IRequest<List<SelectListItemModel>>
    {
        public int CompID { get; set; }
        public int OrgId { get; set; }
    }

    public class GetUserDropdownHandler : IRequestHandler<GetUserDropdownQuery, List<SelectListItemModel>>
    {
        private readonly IUnitOfWork _unitOfWork;


        public GetUserDropdownHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<SelectListItemModel>> Handle(GetUserDropdownQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.UserInfos.Dropdown(request.CompID, request.OrgId);
            return result.ToList();
        }
    }
}
