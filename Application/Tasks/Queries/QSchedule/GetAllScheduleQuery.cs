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
    public class GetAllScheduleQuery : IRequest<List<Schedule>>
    {
        public int OrgId { get; set; }
    }
    public class GetAllScheduleQueryHandler : IRequestHandler<GetAllScheduleQuery, List<Schedule>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllScheduleQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Schedule>> Handle(GetAllScheduleQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.Schedules.GetAll(request.OrgId);
            return result.ToList();
        }
    }
}
