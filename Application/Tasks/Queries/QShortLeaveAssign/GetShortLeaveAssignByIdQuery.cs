using Domains.Models;
using Domains.ViewModels;
using MediatR;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Queries.QShortLeaveAssign
{
    public class GetShortLeaveAssignByIdQuery : IRequest<ShortLeaveAssign>
    {
        public int ShortLeaveAssignId { get; set; }
    }
    public class GetShortLeaveAssignByIdQueryHandler : IRequestHandler<GetShortLeaveAssignByIdQuery, ShortLeaveAssign>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetShortLeaveAssignByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ShortLeaveAssign> Handle(GetShortLeaveAssignByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.ShortLeaveAssign.GetById(request.ShortLeaveAssignId);
            return result;
        }
    }
}
