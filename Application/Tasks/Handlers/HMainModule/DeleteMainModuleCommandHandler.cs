using Application.Tasks.Commands.CMainModule;
using MediatR;
using Persistence.DAL;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Handlers.HMainModule
{
    public class DeleteMainModuleCommandHandler : IRequestHandler<DeleteMainModuleCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteMainModuleCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(DeleteMainModuleCommand request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.MainModules.Delete(request.Id);
            return result;
        }
    }
}
