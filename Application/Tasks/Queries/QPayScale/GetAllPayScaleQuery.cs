using Domains.Models;
using MediatR;
using Persistence.DAL;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Queries.QPayScale
{
    public class GetAllPayScaleQuery : IRequest<List<PayScale>>
    {
        public int CompId { get; set; }
    }

    public class GetAllPayScaleHandler : IRequestHandler<GetAllPayScaleQuery, List<PayScale>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllPayScaleHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<PayScale>> Handle(GetAllPayScaleQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.PayScales.GetAllByCompId(request.CompId);
            return result.ToList();
        }
    }
}
