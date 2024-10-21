using Domains.Models;
using MediatR;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Queries.QBreak
{
    public class GetAllBreakQuery : IRequest<List<Break>>
    {
    }
    public class GetAllBreakQueryHandler : IRequestHandler<GetAllBreakQuery, List<Break>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllBreakQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Break>> Handle(GetAllBreakQuery request, CancellationToken cancellationToken)
        {
            var data = await _unitOfWork.Breaks.GetAll();
            return data.ToList();
        }
    }
}
