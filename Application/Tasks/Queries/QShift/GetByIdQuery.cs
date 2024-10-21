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
    public class GetByIdQuery : IRequest<Shift>
    {
        public int ShiftId { get; set; }
    }
    public class GetByIdQueryHandler : IRequestHandler<GetByIdQuery, Shift>
    {
        private readonly IUnitOfWork _UnitOfWork;

        public GetByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _UnitOfWork = unitOfWork;
        }

        public async Task<Shift> Handle(GetByIdQuery request, CancellationToken cancellationToken)
        {
            var data = await _UnitOfWork.Shifts.GetById(request.ShiftId);
            return data;
        }
    }
}
