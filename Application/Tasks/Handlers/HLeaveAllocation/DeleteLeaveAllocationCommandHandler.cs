using MediatR;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Handlers.HLeaveAllocation
{
    public class DeleteLeaveAllocationCommand : IRequest<int>
    {
        public int LeaveAllocationId { get; set; }
    }
    public class DeletePaymentGroupCommandHandler : IRequestHandler<DeleteLeaveAllocationCommand, int>
    {
        private readonly IUnitOfWork _work;

        public DeletePaymentGroupCommandHandler(IUnitOfWork work)
        {
            _work = work;
        }

        public async Task<int> Handle(DeleteLeaveAllocationCommand request, CancellationToken cancellationToken)
        {
            var data = await _work.LeaveAllocation.Delete(request.LeaveAllocationId);
            return data;
        }
    }
}
