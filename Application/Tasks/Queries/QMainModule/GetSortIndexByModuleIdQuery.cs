using MediatR;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Queries.QMainModule
{
    public class GetSortIndexByModuleIdQuery : IRequest<int>
    {
        public int ModuleId { get; set; }
    }
    public class GetSortIndexByModuleIdQueryHandler : IRequestHandler<GetSortIndexByModuleIdQuery,int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetSortIndexByModuleIdQueryHandler(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        public async Task<int> Handle(GetSortIndexByModuleIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.UserAccess.GetSortIndexByModuleId(request.ModuleId);
            return result;
        }
    }
}
