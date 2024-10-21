using MediatR;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Queries.QSubModule
{
    public class GetSubModuleByUaToolIdQuery : IRequest<int>
    {
        public int UAToolid { get; set; }
    }
    public class GetSubModuleByUaToolIdQueryHandler : IRequestHandler<GetSubModuleByUaToolIdQuery, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetSubModuleByUaToolIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<int> Handle(GetSubModuleByUaToolIdQuery request, CancellationToken cancellationToken)
        {
            var submoduleid = _unitOfWork.SubModule.GetSubModuleByUaToolId(request.UAToolid);
            return submoduleid;
        }
    }
}
