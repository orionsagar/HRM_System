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
    public class DeleteReportCommand : IRequest<int>
    {
        public int ReportId { get; set; }
    }

    public class DeleteReportCommandHandler : IRequestHandler<DeleteReportCommand, int>
    {
        private readonly IUnitOfWork _work;

        public DeleteReportCommandHandler(IUnitOfWork work)
        {
            _work = work;
        }

        public async Task<int> Handle(DeleteReportCommand request, CancellationToken cancellationToken)
        {
            return await _work.Report.Delete(request.ReportId);
        }
    }
}
