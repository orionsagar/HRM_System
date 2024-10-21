using Domains.ViewModels;
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
    public class GetAllLeaveAllowcationQuery : IRequest<List<LeaveAllocationVM>>
    {
        public DataTableParamVM Param { get; set; }
    }
    public class GetAllPaymentGroupQueryHandler : IRequestHandler<GetAllLeaveAllowcationQuery, List<LeaveAllocationVM>>
    {
        private readonly IUnitOfWork _work;

        public GetAllPaymentGroupQueryHandler(IUnitOfWork work)
        {
            _work = work;
        }

        public async Task<List<LeaveAllocationVM>> Handle(GetAllLeaveAllowcationQuery request, CancellationToken cancellationToken)
        {
            var data = await _work.LeaveAllocation.DtList(request.Param);
            return data;
        }
    }
}
