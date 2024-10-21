using Domains.Models;
using MediatR;
using Persistence.DAL;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Queries.QLocation
{
    public class GetDistrictDropdownQuery : IRequest<List<SelectListItemModel>>
    {
        public int CountryId { get; set; }
    }

    public class GetDistrictDropdownHandler : IRequestHandler<GetDistrictDropdownQuery, List<SelectListItemModel>>
    {
        private readonly IUnitOfWork _unitOfWork;


        public GetDistrictDropdownHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<SelectListItemModel>> Handle(GetDistrictDropdownQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.Locations.DistrictDropdown(request.CountryId);
            return result.ToList();
        }
    }
}
