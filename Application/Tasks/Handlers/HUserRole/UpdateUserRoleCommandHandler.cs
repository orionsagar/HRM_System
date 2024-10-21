using Application.Tasks.Commands.CUserRole;
using AutoMapper;
using Domains.Models;
using MediatR;
using Persistence.DAL;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Handlers.HUserRole
{
    public class UpdateUserRoleCommandHandler : IRequestHandler<UpdateUserRoleCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateUserRoleCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<int> Handle(UpdateUserRoleCommand request, CancellationToken cancellationToken)
        {
            var comp = _mapper.Map<UserRole>(request.UserRoleViewModel);
            var result = await _unitOfWork.UserRoles.Update(comp);
            return result;
        }
    }
}
