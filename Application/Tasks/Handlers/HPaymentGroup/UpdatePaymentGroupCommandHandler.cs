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
    public class UpdatePaymentGroupCommand : IRequest<int>
    {
        public PaymentGroup PaymentGroup { get; set; }
    }
    public class UpdatePaymentGroupsCommandHandler : IRequestHandler<UpdatePaymentGroupCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdatePaymentGroupsCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(UpdatePaymentGroupCommand request, CancellationToken cancellationToken)
        {
            var data = await _unitOfWork.PaymentGroup.Update(request.PaymentGroup);
            return data;
        }
    }
}
