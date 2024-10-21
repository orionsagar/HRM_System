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
    public class GetAllLeaveTypeQuery : IRequest<List<LeaveType>>
    {
        public int OrgId { get; set; }
    }
    public class GetAllLeaveTypeQueryHandler : IRequestHandler<GetAllLeaveTypeQuery, List<LeaveType>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllLeaveTypeQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<LeaveType>> Handle(GetAllLeaveTypeQuery request, CancellationToken cancellationToken)
        {
            var data = await _unitOfWork.LeaveTypes.GetAll(request.OrgId);
            return data.ToList();
        }
    }
}
