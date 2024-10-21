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
    public class DeletePaymentTypeCommand : IRequest<int>
    {
        public int PaymentTypeId { get; set; }
    }
    public class DeletePaymentTypeCommandHandler : IRequestHandler<DeletePaymentTypeCommand, int>
    {
        private readonly IUnitOfWork _work;

        public DeletePaymentTypeCommandHandler(IUnitOfWork work)
        {
            _work = work;
        }

        public async Task<int> Handle(DeletePaymentTypeCommand request, CancellationToken cancellationToken)
        {
            var data = await _work.PaymentType.Delete(request.PaymentTypeId);
            return data;
        }
    }
}
