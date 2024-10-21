using Application.Tasks.Queries.QLevel;
using Application.Tasks.Queries.QOrganogram;
using AutoMapper;
using Domains.ViewModels;
using MediatR;
using Persistence.DAL;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Handlers.HOrganogram
{
    public class GetByIdOrgmQueryHandler : IRequestHandler<GetByIdOrgmQuery, OrgHierarchyViewModel>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetByIdOrgmQueryHandler(IUnitOfWork unitOfWork = null, IMapper mapper = null)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<OrgHierarchyViewModel> Handle(GetByIdOrgmQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork._organogramRepository.GetById(request.ID);
            return _mapper.Map<OrgHierarchyViewModel>(result);
        }
    }
}
