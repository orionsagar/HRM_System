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
    public class UpsertPaymentTypeCommand : IRequest<int>
    {
        public PaymentType PaymentType { get; set; }
    }
    public class UpsertPaymentTypeCommandHandler : IRequestHandler<UpsertPaymentTypeCommand, int>
    {
        private readonly IUnitOfWork _work;

        public UpsertPaymentTypeCommandHandler(IUnitOfWork work)
        {
            _work = work;
        }

        public async Task<int> Handle(UpsertPaymentTypeCommand request, CancellationToken cancellationToken)
        {
            if (request.PaymentType.PaymentTypeId > 0)
            {
                var result = await _work.PaymentType.Update(request.PaymentType);

                return result;
            }
            else
            {
                var data = await _work.PaymentType.Add(request.PaymentType);
                return data;
            }

        }
    }
}
