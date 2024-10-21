using Application.Tasks.Queries.QMainModule;
using AutoMapper;
using Domains.Models;
using Domains.ViewModels;
using MediatR;
using Persistence.DAL;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Handlers.HMainModule
{
    public class GetRoleWiseModuleAccessQueryHandler : IRequestHandler<GetRoleWiseModuleAccessQuery, List<RoleWiseUserAccessToolsVM>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetRoleWiseModuleAccessQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<RoleWiseUserAccessToolsVM>> Handle(GetRoleWiseModuleAccessQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.UserAccess.GetRoleWiseModuleAccess(request.RoleId, request.ModuleID,request.SubModuleID);
            return (List<RoleWiseUserAccessToolsVM>)result;
        }
    }
}
