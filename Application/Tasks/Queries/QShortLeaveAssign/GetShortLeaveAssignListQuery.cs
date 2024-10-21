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
    public class GetShortLeaveAssignListQuery : IRequest<List<ShortLeaveAssignEmpListVM>>
    {
        public ShortLeaveAssignFilter ShortLeaveAssignFilter { get; set; }
    }
    public class GetShortLeaveAssignListQueryHandler : IRequestHandler<GetShortLeaveAssignListQuery, List<ShortLeaveAssignEmpListVM>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetShortLeaveAssignListQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<ShortLeaveAssignEmpListVM>> Handle(GetShortLeaveAssignListQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.ShortLeaveAssign.ShortLeaveAssignList(request.ShortLeaveAssignFilter);
            return result.ToList();
        }
    }
}
