using Application.Tasks.Queries.QLevel;
using AutoMapper;
using Domains.ViewModels;
using MediatR;
using Persistence.DAL;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Handlers.HOrganogram
{
    public class GetByIdQueryHandler : IRequestHandler<GetByIdQuery, LevelViewModels>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetByIdQueryHandler(IUnitOfWork unitOfWork = null, IMapper mapper = null)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<LevelViewModels> Handle(GetByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork._levelRepository.GetById(request.ID);
            return _mapper.Map<LevelViewModels>(result);
        }
    }
}
