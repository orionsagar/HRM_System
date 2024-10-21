using Domains.Models;
using MediatR;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Handlers.HReaveRule
{
    public class GetByLeaveRuleIdQuery : IRequest<LeaveRule>
    {
        public int LeaveRuleId { get; set; }
    }
    public class GetByLeaveRuleIdQueryHandler : IRequestHandler<GetByLeaveRuleIdQuery, LeaveRule>
    {
        private readonly IUnitOfWork _work;

        public GetByLeaveRuleIdQueryHandler(IUnitOfWork work)
        {
            _work = work;
        }

        public async Task<LeaveRule> Handle(GetByLeaveRuleIdQuery request, CancellationToken cancellationToken)
        {
            var data = await _work.LeaveRule.GetById(request.LeaveRuleId);
            return data;
        }
    }
}
