using Application.Tasks.Commands.CLevel;
using Application.Tasks.Commands.COrganogram;
using MediatR;
using Persistence.DAL;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Handlers.HOrganogram
{
    public class DeleteOrganogramCommandHandler : IRequestHandler<DeleteOrganogramCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteOrganogramCommandHandler(IUnitOfWork unitOfWork = null)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(DeleteOrganogramCommand request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork._organogramRepository.Delete(request.Id);
            return result;
        }
    }
}
