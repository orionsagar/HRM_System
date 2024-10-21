using Domains.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Tasks.Commands.COrganisation
{
    public class OrganisationCommand : IRequest<int>
    {
        public OrganisationVM OrganisationVM { get; set; }
    }
}
