using Domains.Models;
using MediatR;
using Persistence.DAL;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Queries.QLocation
{
    public class GetThanaDropdownQuery : IRequest<List<SelectListItemModel>>
    {
        public int DistrictId { get; set; }
    }

    public class GetThanaDropdownHandler : IRequestHandler<GetThanaDropdownQuery, List<SelectListItemModel>>
    {
        private readonly IUnitOfWork _unitOfWork;


        public GetThanaDropdownHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<SelectListItemModel>> Handle(GetThanaDropdownQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.Locations.ThanaDropdown(request.DistrictId);
            return result.ToList();
        }
    }
}
