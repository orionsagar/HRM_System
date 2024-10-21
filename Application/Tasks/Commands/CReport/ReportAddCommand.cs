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
    public class ReportAddCommand : IRequest<int>
    {
        public Reports Reports { get; set; }
    }

    public class ReportAddCommandHandler : IRequestHandler<ReportAddCommand, int>
    {
        private readonly IUnitOfWork _work;

        public ReportAddCommandHandler(IUnitOfWork work)
        {
            _work = work;
        }

        public async Task<int> Handle(ReportAddCommand request, CancellationToken cancellationToken)
        {
            return await _work.Report.Add(request.Reports);
        }
    }
}
