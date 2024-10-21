using Domains.ViewModels;
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
    public class GetSubModuleByModuleIdQuery : IRequest<List<SubModuleVM>>
    {
        public int ModuleId { get; set; }
    }
    public class GetSubModuleByModuleIdQueryHandler : IRequestHandler<GetSubModuleByModuleIdQuery, List<SubModuleVM>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetSubModuleByModuleIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<SubModuleVM>> Handle(GetSubModuleByModuleIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.SubModule.GetSubModuleByModuleId(request.ModuleId);
            return result.ToList();
        }
    }
}
