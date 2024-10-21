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
    public class CreateFiscalYearCommand : IRequest<int>
    {
        public FiscalYear FiscalYear { get; set; }
    }
    public class CreateFiscalYearCommandHandler : IRequestHandler<CreateFiscalYearCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateFiscalYearCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<int> Handle(CreateFiscalYearCommand request, CancellationToken cancellationToken)
        {
            var data = await _unitOfWork.FiscalYear.Add(request.FiscalYear);
            return data;
        }
    }
}
