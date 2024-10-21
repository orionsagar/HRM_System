using Application.Tasks.Queries.QOrganisation;
using AutoMapper;
using Domains.ViewModels;
using MediatR;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Handlers.HOrganisation
{
    public class GetOrganisationDataByIdQueryHandler : IRequestHandler<GetOrganisationDataByIdQuery, OrganisationVM>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetOrganisationDataByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<OrganisationVM> Handle(GetOrganisationDataByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.OrgRepo.GetById(request.OrgId);
            return _mapper.Map<OrganisationVM>(result);
        }
    }
}
