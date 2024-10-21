using Application.Tasks.Commands.CUserRole;
using MediatR;
using Persistence.DAL;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Handlers.HUserRole
{
    public class DeleteUserRoleCommandHandler : IRequestHandler<DeleteUserRoleCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteUserRoleCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(DeleteUserRoleCommand request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.UserRoles.Delete(request.Id);
            return result;
        }
    }
}
