using Domains.Models;
using MediatR;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Commands.HPaymentGroup
{
    public class CreatePaymentGroupCommand : IRequest<int>
    {
        public PaymentGroup PaymentGroup { get; set; }
    }
    public class CreatePaymentGroupCommandHandler : IRequestHandler<CreatePaymentGroupCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreatePaymentGroupCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        //public async Task<int> Handle(CreatePaymentGroupCommand request, CancellationToken cancellationToken)
        //{
        //    var data = await _unitOfWork.PaymentGroups.Add(request.PaymentGroups);
        //    return data;
        //}

        public async Task<int> Handle(CreatePaymentGroupCommand request, CancellationToken cancellationToken)
        {
            if (_unitOfWork == null)
            {
                throw new ArgumentNullException(nameof(_unitOfWork), "_unitOfWork is not initialized.");
            }

            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "request is null.");
            }

            if (request.PaymentGroup == null)
            {
                throw new ArgumentNullException(nameof(request.PaymentGroup), "PaymentGroups property of request is null.");
            }

            var data = await _unitOfWork.PaymentGroup.Add(request.PaymentGroup);
            return data;
        }

    }
}

