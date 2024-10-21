using Domains.Models;
using MediatR;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Commands.CUserRole
{
    public class AddReportAccessCommand : IRequest<int>
    {
        public int RoleId { get; set; }
        public List<ReportAccess> ReportAccesses { get; set; }
    }

    public class AddReportAccessCommandHandler : IRequestHandler<AddReportAccessCommand, int>
    {
        private readonly IUnitOfWork _work;

        public AddReportAccessCommandHandler(IUnitOfWork work)
        {
            _work = work;
        }

        public async Task<int> Handle(AddReportAccessCommand request, CancellationToken cancellationToken)
        {
            return await _work.UserRoles.AddReportAccess(request.RoleId, request.ReportAccesses);
        }
    }
}
