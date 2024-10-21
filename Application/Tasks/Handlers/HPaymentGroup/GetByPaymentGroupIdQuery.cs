using Domains.Models;
using MediatR;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Handlers.HPaymentGroup
{
    public class GetByPaymentGroupIdQuery : IRequest<PaymentGroup>
    {
        public int PayGroupId { get; set; }
    }
    public class GetByPaymentGroupIdQueryHandler : IRequestHandler<GetByPaymentGroupIdQuery, PaymentGroup>
    {
        private readonly IUnitOfWork _work;

        public GetByPaymentGroupIdQueryHandler(IUnitOfWork work)
        {
            _work = work;
        }

        public async Task<PaymentGroup> Handle(GetByPaymentGroupIdQuery request, CancellationToken cancellationToken)
        {
            var data = await _work.PaymentGroup.GetById(request.PayGroupId);
            return data;
        }
    }
}
