using Application.Tasks.Queries.QBill;
using AutoMapper;
using Domains.ViewModels;
using MediatR;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Handlers.HBilling
{
    public class GetBillByIdQueryHandler : IRequestHandler<GetBillByIdQuery, BillingMasterVM>
    {
        private readonly IUnitOfWork _work;
        private readonly IMapper _mapper;
        public GetBillByIdQueryHandler(IUnitOfWork work, IMapper mapper)
        {
            _work = work;
            _mapper = mapper;
        }
        public async Task<BillingMasterVM> Handle(GetBillByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _work.BillingRepo.GetById(request.BillingMasterId);
            var billdata = _mapper.Map<BillingMasterVM>(result);
            var details = await _work.BillingRepo.GetDetailsByBillingMasterId(request.BillingMasterId);
            billdata.BillingDetailsVM = details;
            return billdata;
        }
    }
}
