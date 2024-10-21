using Application.Tasks.Queries.QLevel;
using Application.Tasks.Queries.QOrganogram;
using Application.Tasks.Queries.QUserInfo;
using AutoMapper;
using Domains.Models;
using MediatR;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Handlers.HOrganogram
{
    public class GetAllOrgmQueryHandler : IRequestHandler<GetAllOrgmQuery, List<OrgHierarchy>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllOrgmQueryHandler(IUnitOfWork unitOfWork = null, IMapper mapper = null)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<OrgHierarchy>> Handle(GetAllOrgmQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork._organogramRepository.GetAllOrganogram(request.OrgID);
            return result.ToList();
        }
    }
}
