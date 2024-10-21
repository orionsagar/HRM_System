using Application.Tasks.Queries.QPaymentReceipt;
using Domains.ViewModels;
using MediatR;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Handlers.HPaymentReceipt
{
    internal class GetPaymentReceiptByBillingMasterIdQueryHandler : IRequestHandler<GetPaymentReceiptByBillingMasterIdQuery, List<PaymentReceiptMasterVM>>
    {
        private readonly IUnitOfWork _work;

        public GetPaymentReceiptByBillingMasterIdQueryHandler(IUnitOfWork work)
        {
            _work = work;
        }

        public Task<List<PaymentReceiptMasterVM>> Handle(GetPaymentReceiptByBillingMasterIdQuery request, CancellationToken cancellationToken)
        {
            var paymentdata = _work.PaymentReceiptRepo.GetPaymentReceiptByBillingMasterId(request.BillingMasterId);
            return paymentdata;
        }
    }
}
