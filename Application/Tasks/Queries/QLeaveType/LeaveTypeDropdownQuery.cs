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
    public class LeaveTypeDropdownQuery : IRequest<List<SelectListItemModel>>
    {
        public int CompId { get; set; }
        public int OrgId { get; set; }
    }
    public class LeaveTypeDropdownQueryHandler : IRequestHandler<LeaveTypeDropdownQuery, List<SelectListItemModel>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public LeaveTypeDropdownQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<SelectListItemModel>> Handle(LeaveTypeDropdownQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.LeaveTypes.Dropdown(request.CompId, request.OrgId);
            return result.ToList();
        }
    }
}
