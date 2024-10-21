using Application.Tasks.Queries.QPaymentType;
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
    public class GetAllPaymentTypeQueryHandler : IRequestHandler<GetAllPaymentTypeQuery, List<PaymentType>>
    {
        private readonly IUnitOfWork _work;

        public GetAllPaymentTypeQueryHandler(IUnitOfWork work)
        {
            _work = work;
        }

        public async Task<List<PaymentType>> Handle(GetAllPaymentTypeQuery request, CancellationToken cancellationToken)
        {
            var paymenttypes = await _work.PaymentType.GetAll(request.OrgId);
            return paymenttypes.ToList();
        }
    }
}
