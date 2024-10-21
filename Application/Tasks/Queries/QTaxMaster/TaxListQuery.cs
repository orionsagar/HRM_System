using Domains.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Tasks.Queries.QTaxMaster
{
    public class TaxListQuery : IRequest<List<TaxMaster>>
    {
        public int OrgId { get; set; }
    }
}


