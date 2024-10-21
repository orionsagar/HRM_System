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
    public class DeleteBankListCommand : IRequest<int>
    {
        public int BankId { get; set; }
    }
    public class DeleteBankListCommandHandler : IRequestHandler<DeleteBankListCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteBankListCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<int> Handle(DeleteBankListCommand request, CancellationToken cancellationToken)
        {
            var data = _unitOfWork.BankLists.Delete(request.BankId);
            return data;
        }
    }
}
