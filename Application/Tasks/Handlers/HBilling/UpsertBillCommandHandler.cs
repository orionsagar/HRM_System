using Application.Tasks.Commands.CBilling;
using AutoMapper;
using Domains.Models;
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
    public class UpsertBillCommandHandler : IRequestHandler<UpsertBillCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpsertBillCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<int> Handle(UpsertBillCommand request, CancellationToken cancellationToken)
        {
            var billingdata = _mapper.Map<BillingMaster>(request.BillingMasterVM);
            var detailsdata = _mapper.Map<List<BillingDetails>>(request.BillingMasterVM.BillingDetailsVM);
            billingdata.Status = "Due";
            billingdata.BillingDetails = detailsdata;            
            var result = await _unitOfWork.BillingRepo.Add(billingdata);
            return result;
        }
    }
}
