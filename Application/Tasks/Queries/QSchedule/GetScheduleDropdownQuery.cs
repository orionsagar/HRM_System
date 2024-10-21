using Domains.Models;
using MediatR;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Queries.QSchedule
{
    public class GetScheduleDropdownQuery : IRequest<List<SelectListItemModel>>
    {
        public int OrgId { get; set; }
    }
    public class GetScheduleDropdownQueryHandler : IRequestHandler<GetScheduleDropdownQuery, List<SelectListItemModel>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetScheduleDropdownQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<SelectListItemModel>> Handle(GetScheduleDropdownQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.Schedules.Dropdown(request.OrgId);
            return result.ToList();
        }
    }
}
