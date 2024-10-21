using Domains.Models;
using MediatR;
using Persistence.DAL;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Queries.QLocation
{
    public class GetAllThanaQuery : IRequest<List<Thana>>
    {
        public int DistrictId { get; set; }
    }

    public class GetAllThanaHandler : IRequestHandler<GetAllThanaQuery, List<Thana>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllThanaHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Thana>> Handle(GetAllThanaQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.Locations.GetThana(request.DistrictId);
            return result.ToList();
        }
    }
}
