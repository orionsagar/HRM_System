using Domains.Models;
using MediatR;
using Persistence.DAL;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Queries.QDesignation
{
    public class GetDesignationDropdownQuery : IRequest<List<SelectListItemModel>>
    {
        public int CompID { get; set; }
        public int OrgId { get; set; }
    }

    public class GetDesignationDropdownHandler : IRequestHandler<GetDesignationDropdownQuery, List<SelectListItemModel>>
    {
        private readonly IUnitOfWork _unitOfWork;


        public GetDesignationDropdownHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<SelectListItemModel>> Handle(GetDesignationDropdownQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.Designations.Dropdown(request.CompID, request.OrgId);
            return result.ToList();
        }
    }
}
