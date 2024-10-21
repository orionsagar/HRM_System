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
    public class DeletePaymentGroupCommand : IRequest<int>
    {
        public int PayroupId { get; set; }
    }
    public class DeletePaymentGroupCommandHandler : IRequestHandler<DeletePaymentGroupCommand, int>
    {
        private readonly IUnitOfWork _work;

        public DeletePaymentGroupCommandHandler(IUnitOfWork work)
        {
            _work = work;
        }

        public async Task<int> Handle(DeletePaymentGroupCommand request, CancellationToken cancellationToken)
        {
            var data = await _work.PaymentGroup.Delete(request.PayroupId);
            return data;
        }
    }
}
