using Domains.Models;
using MediatR;
using Persistence.DAL;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Queries.QDesignation
{
    public class GetAllDesignationQuery : IRequest<List<Designation>>
    {
        public int CompId { get; set; }
    }

    public class GetAllDesignationHandler : IRequestHandler<GetAllDesignationQuery, List<Designation>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllDesignationHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Designation>> Handle(GetAllDesignationQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.Designations.GetAllByCompId(request.CompId);
            return result.ToList();
        }
    }
}
