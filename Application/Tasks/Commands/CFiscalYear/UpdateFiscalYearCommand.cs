using Domains.Models;
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
    public class UpdateFiscalYearCommand : IRequest<int>
    {
        public FiscalYear FiscalYear { get; set; }
    }
    public class UpdateFiscalYearCommandHandler : IRequestHandler<UpdateFiscalYearCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateFiscalYearCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<int> Handle(UpdateFiscalYearCommand request, CancellationToken cancellationToken)
        {
            var data = await _unitOfWork.FiscalYear.Update(request.FiscalYear);
            return data;
        }
    }
}
