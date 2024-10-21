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
    public class UpdateShiftCommand : IRequest<int>
    {
        public Shift Shift { get; set; }
    }
    public class UpdateShiftCommandHandler : IRequestHandler<UpdateShiftCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateShiftCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(UpdateShiftCommand request, CancellationToken cancellationToken)
        {
            var data = await _unitOfWork.Shifts.Update(request.Shift);
            return data;
        }
    }
}
