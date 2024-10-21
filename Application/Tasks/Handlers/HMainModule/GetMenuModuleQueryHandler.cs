using Application.Tasks.Queries.QMainModule;
using AutoMapper;
using Domains.Models;
using MediatR;
using Persistence.DAL;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Handlers.HMainModule
{
    public class GetMenuModuleQueryHandler : IRequestHandler<GetMenuModuleQuery, List<MainModule>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetMenuModuleQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<MainModule>> Handle(GetMenuModuleQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.UserAccess.GetMenuModule(request.RoleID);
            return _mapper.Map<List<MainModule>>(result);
        }
    }
}
