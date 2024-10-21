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
    public class BankListDropdownQuery : IRequest<List<SelectListItemModel>>
    {
        public int OrgId { get; set; }
        public int ClientId { get; set; }
    }
    public class BankListDropdownQueryHandler : IRequestHandler<BankListDropdownQuery, List<SelectListItemModel>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public BankListDropdownQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<SelectListItemModel>> Handle(BankListDropdownQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.BankLists.Dropdown(request.OrgId,request.ClientId);
            return result.ToList();
        }
    }
}
