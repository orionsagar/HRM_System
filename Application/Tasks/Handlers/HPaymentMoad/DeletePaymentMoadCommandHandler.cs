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
    public class DeletePaymentMoadCommand : IRequest<int>
    {
        public int PaymentMoadId { get; set; }
    }
    public class DeletePaymentMoadCommandHandler : IRequestHandler<DeletePaymentMoadCommand, int>
    {
        private readonly IUnitOfWork _work;

        public DeletePaymentMoadCommandHandler(IUnitOfWork work)
        {
            _work = work;
        }

        public async Task<int> Handle(DeletePaymentMoadCommand request, CancellationToken cancellationToken)
        {
            var data = await _work.PaymentMoad.Delete(request.PaymentMoadId);
            return data;
        }
    }
}
