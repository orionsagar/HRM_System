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
    public class GetByTaxMasterIdQuery : IRequest<TaxMaster>
    {
        public int TaxMasterId { get; set; }
    }
    public class GetByTaxMasterIdQueryHandler : IRequestHandler<GetByTaxMasterIdQuery, TaxMaster>
    {
        private readonly IUnitOfWork _work;

        public GetByTaxMasterIdQueryHandler(IUnitOfWork work)
        {
            _work = work;
        }

        public async Task<TaxMaster> Handle(GetByTaxMasterIdQuery request, CancellationToken cancellationToken)
        {
            var data = await _work.Taxs.GetById(request.TaxMasterId);
            return data;
        }
    }
}
