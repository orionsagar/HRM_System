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
    public class UpsertPaymentMoadCommand : IRequest<int>
    {
        public PaymentMoad PaymentMoad { get; set; }
    }
    public class UpsertPaymentMoadCommandHandler : IRequestHandler<UpsertPaymentMoadCommand, int>
    {
        private readonly IUnitOfWork _work;

        public UpsertPaymentMoadCommandHandler(IUnitOfWork work)
        {
            _work = work;
        }

        public async Task<int> Handle(UpsertPaymentMoadCommand request, CancellationToken cancellationToken)
        {
            if (request.PaymentMoad.PaymentMoadId > 0)
            {
                var result = await _work.PaymentMoad.Update(request.PaymentMoad);
                return result;
            }
            else
            {
                var data = await _work.PaymentMoad.Add(request.PaymentMoad);
                return data;
            }
            
        }
    }
}
