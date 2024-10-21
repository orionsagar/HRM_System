using Domains.Models;
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
    public class UpdateLeaveTypeCommand : IRequest<int>
    {
        public LeaveType LeaveType { get; set; }
    }
    public class UpdateLeaveTypeCommandHandler : IRequestHandler<UpdateLeaveTypeCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateLeaveTypeCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(UpdateLeaveTypeCommand request, CancellationToken cancellationToken)
        {
            var data = await _unitOfWork.LeaveTypes.Update(request.LeaveType);
            return data;
        }
    }
}
