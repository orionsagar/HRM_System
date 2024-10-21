using Domains.Models;
using MediatR;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Handlers.HPaymentMoad
{
    public class GetByPaymentMoadIdQuery : IRequest<PaymentMoad>
    {
        public int PaymentMoadId { get; set; }
    }
    public class GetByPaymentMoadIdQueryHandler : IRequestHandler<GetByPaymentMoadIdQuery, PaymentMoad>
    {
        private readonly IUnitOfWork _work;

        public GetByPaymentMoadIdQueryHandler(IUnitOfWork work)
        {
            _work = work;
        }

        public async Task<PaymentMoad> Handle(GetByPaymentMoadIdQuery request, CancellationToken cancellationToken)
        {
            var data = await _work.PaymentMoad.GetById(request.PaymentMoadId);
            return data;
        }
    }
}
