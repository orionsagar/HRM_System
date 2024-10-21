using AutoMapper;
using Domains.Models;
using MediatR;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Commands.CUserRole
{
    public class CreateUserRoleWiseSubModuleCommand : IRequest<int>
    {
        public int RoleId { get; set; }
        public List<UserRolewiseSubModule> UserRolewiseSubModules { get; set; }
    }

    public class CreateUserRoleWiseSubModuleCommandHandler : IRequestHandler<CreateUserRoleWiseSubModuleCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateUserRoleWiseSubModuleCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateUserRoleWiseSubModuleCommand request, CancellationToken cancellationToken)
        {
            //var comp = _mapper.Map<UserRolewiseModule>(request.UserRolewiseModule);
            var result = await _unitOfWork.UserRoles.AddUserWiseSubModule(request.RoleId, request.UserRolewiseSubModules);
            return result;
        }
    }
}
