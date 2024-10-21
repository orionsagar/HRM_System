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
    public class GetAllQuery : IRequest<List<Shift>>
    {
        public int OrgId { get; set; }
    }
    public class GetAllQueryHandler : IRequestHandler<GetAllQuery, List<Shift>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Shift>> Handle(GetAllQuery request, CancellationToken cancellationToken)
        {
            var data = await _unitOfWork.Shifts.GetAll(request.OrgId);
            return data.ToList();
        }
    }
}
