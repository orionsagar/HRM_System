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
    public class CreateLeaveTypeCommand : IRequest<int>
    {
        public LeaveType LeaveType { get; set; }
    }
    public class CreateLeaveTypeCommandHandler : IRequestHandler<CreateLeaveTypeCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateLeaveTypeCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(CreateLeaveTypeCommand request, CancellationToken cancellationToken)
        {
            var data = await _unitOfWork.LeaveTypes.Add(request.LeaveType);
            return data;
        }
    }
}
