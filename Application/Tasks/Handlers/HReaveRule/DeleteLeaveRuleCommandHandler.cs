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
    public class DeleteLeaveRuleCommand : IRequest<int>
    {
        public int LeaveRuleId { get; set; }
    }
    public class DeleteLeaveRuleCommandHandler : IRequestHandler<DeleteLeaveRuleCommand, int>
    {
        private readonly IUnitOfWork _work;

        public DeleteLeaveRuleCommandHandler(IUnitOfWork work)
        {
            _work = work;
        }

        public async Task<int> Handle(DeleteLeaveRuleCommand request, CancellationToken cancellationToken)
        {
            var data = await _work.LeaveRule.Delete(request.LeaveRuleId);
            return data;
        }
    }
}
