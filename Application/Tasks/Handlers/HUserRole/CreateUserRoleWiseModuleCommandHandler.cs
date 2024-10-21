using Application.Tasks.Commands.CUserRole;
using AutoMapper;
using MediatR;
using Persistence.DAL;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Handlers.HUserRole
{
    public class CreateUserRoleWiseModuleCommandHandler : IRequestHandler<CreateUserRoleWiseModuleCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateUserRoleWiseModuleCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateUserRoleWiseModuleCommand request, CancellationToken cancellationToken)
        {
            //var comp = _mapper.Map<UserRolewiseModule>(request.UserRolewiseModule);
            var result = await _unitOfWork.UserRoles.AddUserWiseModule(request.RoleId, request.UserRolewiseModule);
            return result;
        }
    }
}
