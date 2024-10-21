using Domains.Models;
using MediatR;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Queries.QLunchSetup
{
    public class GetByIdQuery : IRequest<SnacksAllowance>
    {
        public int SnacksId { get; set; }
    }
    public class GetByIdQueryHandler : IRequestHandler<GetByIdQuery, SnacksAllowance>
    {
        private readonly IUnitOfWork _UnitOfWork;

        public GetByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _UnitOfWork = unitOfWork;
        }

        public async Task<SnacksAllowance> Handle(GetByIdQuery request, CancellationToken cancellationToken)
        {
            var data = await _UnitOfWork.LunchSetup.GetById(request.SnacksId);
            return data;
        }
    }
}
