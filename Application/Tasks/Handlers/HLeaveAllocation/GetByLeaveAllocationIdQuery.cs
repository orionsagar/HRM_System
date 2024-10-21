using Domains.Models;
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
    public class GetByLeaveAllocationIdQuery : IRequest<LeaveAllocation>
    {
        public int LeaveAllocationId { get; set; }
    }
    public class GetByLeaveAllocationIdQueryHandler : IRequestHandler<GetByLeaveAllocationIdQuery, LeaveAllocation>
    {
        private readonly IUnitOfWork _work;

        public GetByLeaveAllocationIdQueryHandler(IUnitOfWork work)
        {
            _work = work;
        }

        public async Task<LeaveAllocation> Handle(GetByLeaveAllocationIdQuery request, CancellationToken cancellationToken)
        {
            var data = await _work.LeaveAllocation.GetById(request.LeaveAllocationId);
            return data;
        }
    }
}
