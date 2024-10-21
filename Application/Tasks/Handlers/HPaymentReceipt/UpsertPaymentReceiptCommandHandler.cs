using Application.Tasks.Commands.CPaymentReceipt;
using AutoMapper;
using Domains.Models;
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
    public class UpsertPaymentReceiptCommandHandler : IRequestHandler<UpsertPaymentReceiptCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpsertPaymentReceiptCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<int> Handle(UpsertPaymentReceiptCommand request, CancellationToken cancellationToken)
        {
            var payment = _mapper.Map<PaymentReceiptMaster>(request.ReceiptMasterVM);
            var details = _mapper.Map<List<PaymentReceiptDetails>>(request.ReceiptMasterVM.ReceiptDetailsVMs);
            payment.Status = "Due";
            if (payment.BillAmount == payment.ReceiptAmount && payment.ReceiptAmount > 0)
            {
                payment.Status = "Paid";
            }
            
            payment.PaymentReceiptDetails = details;
            var result = await _unitOfWork.PaymentReceiptRepo.Add(payment);
            return result;
        }
    }
}
