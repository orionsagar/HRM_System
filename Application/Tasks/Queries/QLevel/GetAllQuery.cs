using Domains.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Tasks.Queries.QLevel
{
    public class GetAllQuery : IRequest<List<Level>>
    {
        public int OrgID { get; set; }
    }
}
