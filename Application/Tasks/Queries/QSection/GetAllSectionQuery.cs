using Domains.Models;
using MediatR;
using Persistence.DAL;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Queries.QSection
{
    public class GetAllSectionQuery : IRequest<List<Section>>
    {
        public int CompId { get; set; }
        public int OrgId { get; set; }
    }

    public class GetAllSectionHandler : IRequestHandler<GetAllSectionQuery, List<Section>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllSectionHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Section>> Handle(GetAllSectionQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.Sections.GetAllByCompId(request.CompId,request.OrgId);
            return result.ToList();
        }
    }
}
