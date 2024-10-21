//using Domains.Models;
//using MediatR;
//using Persistence.DAL;
//using System;
//using System.Collections.Generic;


using Domains.Models;
using MediatR;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Queries.HPaymentGroup
{
    public class GetAllPaymentGroupQuery : IRequest<List<PaymentGroup>>
    {
        public int OrgId { get; set; }
    }
    public class GetAllPaymentGroupQueryHandler : IRequestHandler<GetAllPaymentGroupQuery, List<PaymentGroup>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllPaymentGroupQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<PaymentGroup>> Handle(GetAllPaymentGroupQuery request, CancellationToken cancellationToken)
        {
            var data = await _unitOfWork.PaymentGroup.GetAll(request.OrgId);
            return data.ToList();
        }
    }
}
