using MediatR;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Commands.CLeaveType
{
    public class DeleteLeaveTypeCommand : IRequest<int>
    {
        public int LeaveTypeId { get; set; }
    }
    public class DeleteLeaveTypeCommandHandler : IRequestHandler<DeleteLeaveTypeCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteLeaveTypeCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<int> Handle(DeleteLeaveTypeCommand request, CancellationToken cancellationToken)
        {
            var data = _unitOfWork.LeaveTypes.Delete(request.LeaveTypeId);
            return data;
        }
    }
}
