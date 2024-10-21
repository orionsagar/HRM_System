using Application.Tasks.Commands.CUserRole;
using AutoMapper;
using Domains.Models;
using MediatR;
using Persistence.DAL;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Handlers.HUserRole
{
    public class CreateUserRoleCommandHandler : IRequestHandler<CreateUserRoleCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        public IMapper _mapper { get; }

        public CreateUserRoleCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        public async Task<int> Handle(CreateUserRoleCommand request, CancellationToken cancellationToken)
        {
            var comp = _mapper.Map<UserRole>(request.UserRoleViewModel);
            var result = await _unitOfWork.UserRoles.Add(comp);
            return result;
        }
    }
}
