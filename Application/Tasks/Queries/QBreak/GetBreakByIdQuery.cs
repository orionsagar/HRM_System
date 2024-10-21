using Domains.Models;
using MediatR;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Queries.QBreak
{
    public class GetBreakByIdQuery : IRequest<Break>
    {
        public int BreakId { get; set; }
    }
    public class GetBreakByIdQueryHandler : IRequestHandler<GetBreakByIdQuery, Break>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetBreakByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<Break> Handle(GetBreakByIdQuery request, CancellationToken cancellationToken)
        {
            var data = _unitOfWork.Breaks.GetById(request.BreakId);
            return data;
        }
    }
}
