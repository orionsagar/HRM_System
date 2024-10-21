//using AutoMapper;
//using Domains.Models;
//using Domains.ViewModels;
//using MediatR;
//using Persistence.DAL;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;


//namespace Application.Tasks.Handlers.HLeaveAllocation
//{
//    public class UpsertLeaveAllocationCommand : IRequest<int>
//    {
//        public LeaveAllocationVM LeaveAllocationVM { get; set; }
//    }
//    public class UpsertLeaveAllocationCommandHandler : IRequestHandler<UpsertLeaveAllocationCommand, int>
//    {
//        private readonly IUnitOfWork _work;

//        public UpsertLeaveAllocationCommandHandler(IUnitOfWork work)
//        {
//            _work = work;
//        }

//        public async Task<int> Handle(UpsertLeaveAllocationCommand request, CancellationToken cancellationToken)
//        {
//            if (request.leaveAllocations.LeaveAllocationId > 0)
//            {
//                var result = await _work.LeaveAllocation.Update(request.leaveAllocations);

//                return result;
//            }
//            else
//            {
//                var data = await _work.LeaveAllocation.Add(request.leaveAllocations);
//                return data;
//            }

//        }
//    }
//}


using AutoMapper;
using Domains.Models;
using Domains.ViewModels;
using MediatR;
using Persistence.DAL;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Handlers.HLeaveAllocation
{
    public class UpsertLeaveAllocationCommand : IRequest<int>
    {
        public LeaveAllocationVM LeaveAllocationVM { get; set; }
    }

    public class UpsertPaymentGroupCommandHandler : IRequestHandler<UpsertLeaveAllocationCommand, int>
    {
        private readonly IUnitOfWork _work;
        private readonly IMapper _mapper;

        public UpsertPaymentGroupCommandHandler(IUnitOfWork work, IMapper mapper)
        {
            _work = work;
            _mapper = mapper;
        }

        public async Task<int> Handle(UpsertLeaveAllocationCommand request, CancellationToken cancellationToken)
        {
            var LeaveAllocation = _mapper.Map<LeaveAllocation>(request.LeaveAllocationVM);

            if (request.LeaveAllocationVM.LeaveAllocationId > 0)
            {
                var result = await _work.LeaveAllocation.Update(LeaveAllocation);
                return result;
            }
            else
            {
                var data = await _work.LeaveAllocation.Add(LeaveAllocation);
                return data;
            }
        }
    }
}



