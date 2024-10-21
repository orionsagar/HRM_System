using Domains.Models;
using MediatR;
using Persistence.DAL;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Queries.QLocation
{
    public class GetAllDistrictQuery : IRequest<List<District>>
    {
        public int CountryId { get; set; }
    }

    public class GetAllDistrictHandler : IRequestHandler<GetAllDistrictQuery, List<District>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllDistrictHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<District>> Handle(GetAllDistrictQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.Locations.GetDistrict(request.CountryId);
            return result.ToList();
        }
    }
}
