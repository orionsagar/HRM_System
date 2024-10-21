using Application.Tasks.Queries.QClient;
using Domains.Models;
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
    public class OrganisationDropdownQuery : IRequest<List<SelectListItemModel>>
    {
        public int OrgId { get; set; }
        public int ClientId { get; set; }
    }

    public class OrganisationDropdownQueryHandler : IRequestHandler<OrganisationDropdownQuery, List<SelectListItemModel>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrganisationDropdownQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<SelectListItemModel>> Handle(OrganisationDropdownQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.OrgRepo.Dropdown();
            if (request.OrgId > 0)
            {
                result = await _unitOfWork.OrgRepo.Dropdown(request.OrgId);
            }
            if(request.ClientId > 0)
            {
                result = await _unitOfWork.OrgRepo.Dropdown(0,request.ClientId);
            }
            if(request.ClientId > 0 && request.OrgId > 0)
            {
                result = await _unitOfWork.OrgRepo.Dropdown(request.OrgId, request.ClientId);
            }
            return result.ToList();
        }
    }
}
