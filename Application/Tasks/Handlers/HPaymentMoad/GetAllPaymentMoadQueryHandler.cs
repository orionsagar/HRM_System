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
    public class GetAllPaymentMoadQuery : IRequest<List<PaymentMoad>>
    {
        public int OrgId { get; set; }
    }
    public class GetAllPaymentMoadQueryHandler : IRequestHandler<GetAllPaymentMoadQuery, List<PaymentMoad>>
    {
        private readonly IUnitOfWork _work;

        public GetAllPaymentMoadQueryHandler(IUnitOfWork work)
        {
            _work = work;
        }

        public async Task<List<PaymentMoad>> Handle(GetAllPaymentMoadQuery request, CancellationToken cancellationToken)
        {
            var result = await _work.PaymentMoad.GetAll(request.OrgId);
            return result.ToList();
        }
    }
}
