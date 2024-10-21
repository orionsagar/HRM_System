using AutoMapper;
using Domains.ViewModels;
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
    public class GetAllLeaveRuleQuery : IRequest<List<LeaveRuleVM>>
    {
        //public DataTableParamVM Param { get; set; }
        public int OrgId { get; set; }
    }
    public class GetAllLeaveRuleQueryHandler : IRequestHandler<GetAllLeaveRuleQuery, List<LeaveRuleVM>>
    {
        private readonly IUnitOfWork _work;
        private readonly IMapper _mapper;

        public GetAllLeaveRuleQueryHandler(IUnitOfWork work, IMapper mapper)
        {
            _work = work;
            _mapper = mapper;
        }

        public async Task<List<LeaveRuleVM>> Handle(GetAllLeaveRuleQuery request, CancellationToken cancellationToken)
        {
            var data = await _work.LeaveRule.GetAll(request.OrgId);
            var result = _mapper.Map<List<LeaveRuleVM>>(data);
            return result;
        }
    }
}
