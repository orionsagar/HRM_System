using Domains.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Tasks.Queries.QOrganogram
{
    public class GetAllOrgmQuery : IRequest<List<OrgHierarchy>>
    {
        public int OrgID { get; set; }
    }
}
