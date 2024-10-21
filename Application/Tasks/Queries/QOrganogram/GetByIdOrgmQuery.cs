using Domains.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Tasks.Queries.QOrganogram
{
    public class GetByIdOrgmQuery : IRequest<OrgHierarchyViewModel>
    {
        public int ID { get; set; }
    }
}
