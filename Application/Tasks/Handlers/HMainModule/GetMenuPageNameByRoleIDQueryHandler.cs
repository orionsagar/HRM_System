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
    class GetMenuPageNameByRoleIDQueryHandler : IRequestHandler<GetMenuPageNameByRoleIDQuery,  List<UserAccessTools>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetMenuPageNameByRoleIDQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<UserAccessTools>> Handle(GetMenuPageNameByRoleIDQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.UserAccess.GetMenuPageName(request.RoleID, request.ModuleID);
            return _mapper.Map<List<UserAccessTools>>(result);
        }
    }
}
