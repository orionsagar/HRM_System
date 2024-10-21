using MediatR;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Queries.QMenu
{
    public class GetMenuSortIndexQuery : IRequest<int>
    {
        public int ModuleId { get; set; }
        public int SubModuleId { get; set; }
    }
    public class GetMenuSortIndexQueryHandler : IRequestHandler<GetMenuSortIndexQuery, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetMenuSortIndexQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<int> Handle(GetMenuSortIndexQuery request, CancellationToken cancellationToken)
        {
            var index = _unitOfWork.ModuleMenu.GetMenuSortIndex(request.ModuleId, request.SubModuleId);
            return index;
        }
    }
}
