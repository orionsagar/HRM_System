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
    public class GetHolidayByIdQuery : IRequest<Holiday>
    {
        public int HolidayId { get; set; }
    }
    public class GetHolidayByIdQueryHandler : IRequestHandler<GetHolidayByIdQuery, Holiday>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetHolidayByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Holiday> Handle(GetHolidayByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.Holidays.GetById(request.HolidayId);
            return result;
        }
    }
}
