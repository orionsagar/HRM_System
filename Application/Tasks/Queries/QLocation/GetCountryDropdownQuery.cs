using Domains.Models;
using MediatR;
using Persistence.DAL;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Queries.QLocation
{
    public class GetCountryDropdownQuery : IRequest<List<SelectListItemModel>>
    {
        
    }

    public class GetCountryDropdownHandler : IRequestHandler<GetCountryDropdownQuery, List<SelectListItemModel>>
    {
        private readonly IUnitOfWork _unitOfWork;


        public GetCountryDropdownHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<SelectListItemModel>> Handle(GetCountryDropdownQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.Locations.CountryDropdown();
            return result.ToList();
        }
    }
}
