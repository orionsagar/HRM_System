using Domains.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Tasks.Queries.QOrganisation
{
    public class GetOrganisationDataByIdQuery : IRequest<OrganisationVM>
    {
        public int OrgId { get; set; }
    }
}
