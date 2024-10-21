using Domains.Models;
using MediatR;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Queries.QLeaveType
{
    public class GetAllLeaveTypeByIdQuery : IRequest<LeaveType>
    {
        public int LeaveTypeId { get; set; }
    }
    public class GetAllLeaveTypeByIdQueryHandler : IRequestHandler<GetAllLeaveTypeByIdQuery, LeaveType>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllLeaveTypeByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<LeaveType> Handle(GetAllLeaveTypeByIdQuery request, CancellationToken cancellationToken)
        {
            var data = _unitOfWork.LeaveTypes.GetById(request.LeaveTypeId);
            return data;
        }
    }
}
