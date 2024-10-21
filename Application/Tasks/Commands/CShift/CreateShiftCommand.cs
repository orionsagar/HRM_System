using Domains.Models;
using MediatR;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Commands.CShift
{
    public class CreateShiftCommand : IRequest<int>
    {
        public Shift Shift { get; set; }
    }
    public class CreateShiftCommandHandler : IRequestHandler<CreateShiftCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateShiftCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(CreateShiftCommand request, CancellationToken cancellationToken)
        {
            var data = await _unitOfWork.Shifts.Add(request.Shift);
            return data;
        }
    }
}
