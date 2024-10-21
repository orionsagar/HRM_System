using Domains.Models;
using MediatR;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Queries.QBankList
{
    public class GetAllBankListQuery : IRequest<List<BankLists>>
    {
        public int OrgId { get; set; }
    }
    public class GetAllBankListQueryHandler : IRequestHandler<GetAllBankListQuery, List<BankLists>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllBankListQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<BankLists>> Handle(GetAllBankListQuery request, CancellationToken cancellationToken)
        {
            var data = await _unitOfWork.BankLists.GetAllByOrgId(request.OrgId);
            return data.ToList();
        }
    }
}
