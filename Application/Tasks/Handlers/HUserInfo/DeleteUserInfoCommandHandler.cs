using Application.Tasks.Commands.CUserInfo;
using MediatR;
using Persistence.DAL;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Handlers.HUserInfo
{
    public class DeleteUserInfoCommandHandler : IRequestHandler<DeleteUserInfoCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        public DeleteUserInfoCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(DeleteUserInfoCommand request, CancellationToken cancellationToken)
        {

            var result = await _unitOfWork.UserInfos.Delete(request.Id);
            return result;
        }
    }
}
