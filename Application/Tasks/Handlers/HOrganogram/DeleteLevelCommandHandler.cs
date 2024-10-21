using Application.Tasks.Commands.CLevel;
using MediatR;
using Persistence.DAL;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Handlers.HOrganogram
{
    public class DeleteLevelCommandHandler : IRequestHandler<DeleteLevelCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteLevelCommandHandler(IUnitOfWork unitOfWork = null)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(DeleteLevelCommand request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork._levelRepository.Delete(request.Id);
            return result;
        }
    }
}
