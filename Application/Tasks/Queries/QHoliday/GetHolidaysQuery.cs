using Domains.Models;
using MediatR;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Queries.QHoliday
{
    public class GetHolidaysQuery : IRequest<List<Holiday>>
    {
        public int JobCalendarId { get; set; }
        public int CompId { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
    }
    public class GetHolidaysQueryHandler : IRequestHandler<GetHolidaysQuery, List<Holiday>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetHolidaysQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Holiday>> Handle(GetHolidaysQuery request, CancellationToken cancellationToken)
        {
            var data = await _unitOfWork.Holidays.GetHolidays(request.JobCalendarId, request.CompId, request.Year, request.Month);
            return data;
        }
    }
}
