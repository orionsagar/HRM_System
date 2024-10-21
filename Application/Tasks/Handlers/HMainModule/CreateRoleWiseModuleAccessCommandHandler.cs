using Application.Tasks.Commands.CMainModule;
using AutoMapper;
using MediatR;
using Persistence.DAL;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Handlers.HMainModule
{
    public class CreateRoleWiseModuleAccessCommandHandler : IRequestHandler<CreateRoleWiseModuleAccessCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateRoleWiseModuleAccessCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateRoleWiseModuleAccessCommand request, CancellationToken cancellationToken)
        {
            //var comp = _mapper.Map<UserAccessList>(request.UserAccessList);
            var result = await _unitOfWork.UserAccess.AddUserAccessTools(request.UserAccessList, request.RoleId, request.ModuleId,request.SubModuleId);
            return result;
        }
    }
}
