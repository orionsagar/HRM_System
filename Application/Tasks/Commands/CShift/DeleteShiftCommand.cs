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
    public class DeleteShiftCommand : IRequest<int>
    {
        public int ShiftId { get; set; }
    }
    public class DeleteShiftCommandHandler : IRequestHandler<DeleteShiftCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteShiftCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(DeleteShiftCommand request, CancellationToken cancellationToken)
        {
            var data = await _unitOfWork.Shifts.Delete(request.ShiftId);
            return data;
        }
    }
}
