using Domains.Models;
using MediatR;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Queries.QShift
{
    public class ShiftDropdownByScheduleIdQuery : IRequest<List<SelectListItemModel>>
    {
        public int CompId { get; set; }
        public int ScheduleId { get; set; }
    }
    public class ShiftDropdownByScheduleIdQueryHandler : IRequestHandler<ShiftDropdownByScheduleIdQuery, List<SelectListItemModel>>
    {
        private readonly IUnitOfWork _unitOfWork;


        public ShiftDropdownByScheduleIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<SelectListItemModel>> Handle(ShiftDropdownByScheduleIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.Shifts.ShiftDropdownByScheduleId(request.CompId,request.ScheduleId);
            return result.ToList();
        }
    }
}
