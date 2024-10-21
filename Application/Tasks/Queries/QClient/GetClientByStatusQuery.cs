using Domains.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Tasks.Queries.QClient
{
    public class GetClientByStatusQuery : IRequest<List<ClientViewModel>>
    {
        public string Status { get; set; }
    }
}
