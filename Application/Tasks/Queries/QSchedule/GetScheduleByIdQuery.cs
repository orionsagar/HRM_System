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
    public class GetScheduleByIdQuery : IRequest<Schedule>
    {
        public int ScheduleId { get; set; }
    }
    public class GetScheduleByIdQueryHandler : IRequestHandler<GetScheduleByIdQuery, Schedule>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetScheduleByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Schedule> Handle(GetScheduleByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.Schedules.GetById(request.ScheduleId);
            return result;
        }
    }
}
