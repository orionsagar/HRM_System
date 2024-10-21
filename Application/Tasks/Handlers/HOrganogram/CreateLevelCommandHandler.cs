using Application.Tasks.Commands.CLevel;
using AutoMapper;
using Domains.Models;
using MediatR;
using Persistence.DAL;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Handlers.HOrganogram
{
    public class CreateLevelCommandHandler : IRequestHandler<CreateLevelCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CreateLevelCommandHandler(IMapper mapper = null, IUnitOfWork unitOfWork = null)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(CreateLevelCommand request, CancellationToken cancellationToken)
        {
            var comp = _mapper.Map<Level>(request.levelViewModels);
            var result = await _unitOfWork._levelRepository.Add(comp);
            return result;
        }
    }
}
