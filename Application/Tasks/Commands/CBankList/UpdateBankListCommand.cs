using Domains.Models;
using MediatR;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Commands.CBankList
{
    public class UpdateBankListCommand : IRequest<int>
    {
        public BankLists BankLists { get; set; }
    }
    public class UpdateBankListsCommandHandler : IRequestHandler<UpdateBankListCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateBankListsCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(UpdateBankListCommand request, CancellationToken cancellationToken)
        {
            var data = await _unitOfWork.BankLists.Update(request.BankLists);
            return data;
        }
    }
}
