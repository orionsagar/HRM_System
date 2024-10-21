using Application.Tasks.Queries.QTaxMaster;
using Domains.Models;
using MediatR;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Handlers.HTax
{
    public class TaxListQueryHandler : IRequestHandler<TaxListQuery, List<TaxMaster>>
    {
        private readonly IUnitOfWork _work;


        public TaxListQueryHandler(IUnitOfWork work)
        {
            _work = work;
        }

        public async Task<List<TaxMaster>> Handle(TaxListQuery request, CancellationToken cancellationToken)
        {
            var taxlist = await _work.Taxs.GetAll(request.OrgId);
            return taxlist.ToList();
        }
    }
}
