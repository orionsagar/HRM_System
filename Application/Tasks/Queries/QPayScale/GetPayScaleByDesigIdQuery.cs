using Domains.Models;
using Domains.ViewModels;
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
    public class GetPayScaleByDesigIdQuery : IRequest<PayScale>
    {
        public int DesigId { get; set; }
    }
    public class GetPayScaleByDesigIdQueryHandler : IRequestHandler<GetPayScaleByDesigIdQuery, PayScale>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetPayScaleByDesigIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PayScale> Handle(GetPayScaleByDesigIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.PayScales.GetByDesigId(request.DesigId);
            return result;
        }
    }
}
