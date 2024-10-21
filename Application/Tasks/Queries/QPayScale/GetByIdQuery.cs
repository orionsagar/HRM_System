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
    public class GetByIdQuery : IRequest<PayScale>
    {
        public int PayScaleId { get; set; }
    }
    public class GetByIdQueryHandler : IRequestHandler<GetByIdQuery, PayScale>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PayScale> Handle(GetByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.PayScales.GetById(request.PayScaleId);
            return result;
        }
    }
}
