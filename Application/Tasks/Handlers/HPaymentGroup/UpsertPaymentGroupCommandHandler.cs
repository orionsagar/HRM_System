using MediatR;
using Persistence.DAL;
using System.Threading;
using System.Threading.Tasks;
using Domains.Models;

namespace Application.Tasks.Handlers.HPaymentGroup
{
    public class UpsertPaymentGroupCommand : IRequest<int>
    {
        public PaymentGroup PaymentGroup { get; set; }
    }

    public class UpsertPaymentGroupCommandHandler : IRequestHandler<UpsertPaymentGroupCommand, int>
    {
        private readonly IUnitOfWork _work;

        public UpsertPaymentGroupCommandHandler(IUnitOfWork work)
        {
            _work = work;
        }

        public async Task<int> Handle(UpsertPaymentGroupCommand request, CancellationToken cancellationToken)
        {
            var paymentGroup = request.PaymentGroup;

            if (paymentGroup.PayGroupId > 0)
            {
                var result = await _work.PaymentGroup.Update(paymentGroup);
                return result;
            }
            else
            {
                var data = await _work.PaymentGroup.Add(paymentGroup);
                return data;
            }
        }
    }
}
