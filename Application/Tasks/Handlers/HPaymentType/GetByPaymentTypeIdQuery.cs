using Domains.Models;
using MediatR;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Handlers.HPaymentType
{
    public class GetByPaymentTypeIdQuery : IRequest<PaymentType>
    {
        public int PaymentTypeId { get; set; }
    }
    public class GetByPaymentTypeIdQueryHandler : IRequestHandler<GetByPaymentTypeIdQuery, PaymentType>
    {
        private readonly IUnitOfWork _work;

        public GetByPaymentTypeIdQueryHandler(IUnitOfWork work)
        {
            _work = work;
        }

        public async Task<PaymentType> Handle(GetByPaymentTypeIdQuery request, CancellationToken cancellationToken)
        {
            var data = await _work.PaymentType.GetById(request.PaymentTypeId);
            return data;
        }
    }
}
