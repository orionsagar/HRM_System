using AutoMapper;
using Domains.Models;
using Domains.ViewModels;
using MediatR;
using Persistence.DAL;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Handlers.HReaveRule
{
    public class UpsertLeaveRuleCommand : IRequest<int>
    {
        public LeaveRuleVM LeaveRuleVM { get; set; }
    }

    public class UpsertLeaveRuleCommandHandler : IRequestHandler<UpsertLeaveRuleCommand, int>
    {
        private readonly IUnitOfWork _work;
        private readonly IMapper _mapper;

        public UpsertLeaveRuleCommandHandler(IUnitOfWork work, IMapper mapper)
        {
            _work = work;
            _mapper = mapper;
        }

        public async Task<int> Handle(UpsertLeaveRuleCommand request, CancellationToken cancellationToken)
        {
            var leaveRule = _mapper.Map<LeaveRule>(request.LeaveRuleVM);

            if (request.LeaveRuleVM.LeaveRuleId > 0)
            {
                var result = await _work.LeaveRule.Update(leaveRule);
                return result;
            }
            else
            {
                var data = await _work.LeaveRule.Add(leaveRule);
                return data;
            }
        }
    }
}



