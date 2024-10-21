using MediatR;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Commands.CFiscalYear
{
    public class DeleteFiscalYearCommand : IRequest<int>
    {
        public int FiscalYearId { get; set; }
    }
    public class DeleteFiscalYearCommandHandler : IRequestHandler<DeleteFiscalYearCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteFiscalYearCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(DeleteFiscalYearCommand request, CancellationToken cancellationToken)
        {
            var data = await _unitOfWork.FiscalYear.Delete(request.FiscalYearId);
            return data;
        }
    }
}
