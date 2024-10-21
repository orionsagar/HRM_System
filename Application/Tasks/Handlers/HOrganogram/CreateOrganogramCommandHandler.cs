using Application.Tasks.Commands.CLevel;
using Application.Tasks.Commands.COrganogram;
using AutoMapper;
using Domains.Models;
using MediatR;
using Persistence.DAL;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Handlers.HOrganogram
{
    public class CreateOrganogramCommandHandler : IRequestHandler<CreateOrganogramCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CreateOrganogramCommandHandler(IMapper mapper = null, IUnitOfWork unitOfWork = null)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(CreateOrganogramCommand request, CancellationToken cancellationToken)
        {
            var comp = _mapper.Map<OrgHierarchy>(request.OrgHierarchy);
            var result = await _unitOfWork._organogramRepository.Add(comp);
            return result;
        }
    }
}
