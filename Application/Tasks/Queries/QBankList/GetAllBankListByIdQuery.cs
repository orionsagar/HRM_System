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
    public class GetAllBankListByIdQuery : IRequest<BankLists>
    {
        public int BankId { get; set; }
    }
    public class GetAllBankListByIdQueryHandler : IRequestHandler<GetAllBankListByIdQuery, BankLists>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllBankListByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<BankLists> Handle(GetAllBankListByIdQuery request, CancellationToken cancellationToken)
        {
            var data = _unitOfWork.BankLists.GetById(request.BankId);
            return data;
        }
    }
}
