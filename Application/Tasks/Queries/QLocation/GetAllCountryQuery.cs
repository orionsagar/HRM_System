using Domains.Models;
using MediatR;
using Persistence.DAL;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Queries.QLocation
{
    public class GetAllCountryQuery : IRequest<List<Country>>
    {
        
    }

    public class GetAllCountryHandler : IRequestHandler<GetAllCountryQuery, List<Country>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllCountryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Country>> Handle(GetAllCountryQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.Locations.GetCountries();
            return result.ToList();
        }
    }
}
