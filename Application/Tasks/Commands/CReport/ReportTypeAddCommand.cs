using Domains.Models;
using MediatR;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Commands.CReport
{
    public class ReportTypeAddCommand : IRequest<int>
    {
        public ReportTypes ReportTypes { get; set; }
    }

    public class ReportTypeAddCommandHandler : IRequestHandler<ReportTypeAddCommand, int>
    {
        private readonly IUnitOfWork _work;

        public ReportTypeAddCommandHandler(IUnitOfWork work)
        {
            _work = work;
        }

        public async Task<int> Handle(ReportTypeAddCommand request, CancellationToken cancellationToken)
        {
            var result = await _work.Report.ReportTypeAdd(request.ReportTypes);
            return result;
        }
    }
}
