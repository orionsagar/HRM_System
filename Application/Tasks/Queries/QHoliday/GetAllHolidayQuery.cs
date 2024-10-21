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
    public class GetAllHolidayQuery : IRequest<List<Holiday>>
    {
    }
    public class GetAllHolidayQueryHandler : IRequestHandler<GetAllHolidayQuery, List<Holiday>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllHolidayQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Holiday>> Handle(GetAllHolidayQuery request, CancellationToken cancellationToken)
        {
            var data = await _unitOfWork.Holidays.GetAll();
            return data.ToList();
        }
    }
}
