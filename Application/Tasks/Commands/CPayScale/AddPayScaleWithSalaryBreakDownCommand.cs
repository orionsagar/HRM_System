using Domains.Models;
using Domains.ViewModels;
using MediatR;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Commands.CPayScale
{
    public class AddPayScaleWithSalaryBreakDownCommand : IRequest<int>
    {
        public PayScale PayScale { get; set; }
    }
    public class AddPayScaleWithSalaryBreakDownCommandHandler : IRequestHandler<AddPayScaleWithSalaryBreakDownCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddPayScaleWithSalaryBreakDownCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(AddPayScaleWithSalaryBreakDownCommand request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.PayScales.AddPayScaleWithSalaryBreakDown(request.PayScale);
            return result;
        }
    }
}
