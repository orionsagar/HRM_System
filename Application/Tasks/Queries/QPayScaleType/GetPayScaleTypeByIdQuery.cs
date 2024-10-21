using Domains.Models;
using MediatR;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Queries.QPayScaleType
{
    public class GetPayScaleTypeByIdQuery : IRequest<PayScaleType>
    {
        public int PayScaleTypeId { get; set; }
    }
    public class GetPayScaleTypeByIdQueryHandler : IRequestHandler<GetPayScaleTypeByIdQuery, PayScaleType>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetPayScaleTypeByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PayScaleType> Handle(GetPayScaleTypeByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.PayScales.GetPayScaleTypeById(request.PayScaleTypeId);
            return result;
        }
    }

}
