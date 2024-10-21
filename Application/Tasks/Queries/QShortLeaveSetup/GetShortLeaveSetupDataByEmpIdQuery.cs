using Domains.Models;
using MediatR;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Queries.QShortLeaveSetup
{
    public class GetShortLeaveSetupDataByEmpIdQuery : IRequest<List<ShortLeaveSetup>>
    {
        public int EmpId { get; set; }
    }

    public class GetShortLeaveSetupDataByEmpIdQueryHandler : IRequestHandler<GetShortLeaveSetupDataByEmpIdQuery, List<ShortLeaveSetup>>
    {
        private readonly IUnitOfWork _work;

        public GetShortLeaveSetupDataByEmpIdQueryHandler(IUnitOfWork work)
        {
            _work = work;
        }

        public Task<List<ShortLeaveSetup>> Handle(GetShortLeaveSetupDataByEmpIdQuery request, CancellationToken cancellationToken)
        {
            var result = _work.ShortLeaveSetup.GetByEmpId(request.EmpId);
            return result;
        }
    }
}
