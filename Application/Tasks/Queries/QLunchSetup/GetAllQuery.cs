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
    public class GetAllQuery : IRequest<List<SnacksAllowance>>
    {
        public int ComId { get; set; }
        public int OrgId { get; set; }
    }

    public class GetAllQueryHandler : IRequestHandler<GetAllQuery, List<SnacksAllowance>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<SnacksAllowance>> Handle(GetAllQuery request, CancellationToken cancellationToken)
        {
            var data = await _unitOfWork.LunchSetup.GetAll(request.ComId,request.OrgId);
            return data.ToList();
        }
    }
}
