using MediatR;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Queries.QPayScale
{
    public class GetLebelIsGovtOrNotQuery : IRequest<bool>
    {
        public int PayScaleTypeId { get; set; }
    }
    public class GetLebelIsGovtOrNotQueryHandler : IRequestHandler<GetLebelIsGovtOrNotQuery, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetLebelIsGovtOrNotQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public Task<bool> Handle(GetLebelIsGovtOrNotQuery request, CancellationToken cancellationToken)
        {
            var lebel = _unitOfWork.PayScales.GetLebelIsGovtOrNot(request.PayScaleTypeId);
            return lebel;
        }
    }
}
