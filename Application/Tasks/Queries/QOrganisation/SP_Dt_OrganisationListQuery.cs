using Application.Tasks.Handlers.HCompany;
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

namespace Application.Tasks.Queries.QOrganisation
{
    public class SP_Dt_OrganisationListQuery : IRequest<List<OrganisationVM>>
    {
        public int DisplayLength { get; set; }
        public int Start { get; set; }
        public int SortCol { get; set; }
        public int ClientId { get; set; }
        public int OrgId { get; set; }
        public string SortDir { get; set; }
        public string Search { get; set; }
        public string RoleType { get; set; }
    }
    public class SP_Dt_OrganisationListQueryHandler : IRequestHandler<SP_Dt_OrganisationListQuery, List<OrganisationVM>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public SP_Dt_OrganisationListQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<List<OrganisationVM>> Handle(SP_Dt_OrganisationListQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.OrgRepo.SP_Dt_OrganisationList(request.DisplayLength, request.Start, request.SortCol, request.SortDir, request.Search,request.ClientId, request.OrgId,request.RoleType);
            return result.ToList();
        }
    }
}
