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
    public class CreateBankListCommand : IRequest<int>
    {
        public BankLists BankLists { get; set; }
    }
    public class CreateBankListCommandHandler : IRequestHandler<CreateBankListCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateBankListCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        //public async Task<int> Handle(CreateBankListCommand request, CancellationToken cancellationToken)
        //{
        //    var data = await _unitOfWork.BankLists.Add(request.BankLists);
        //    return data;
        //}

        public async Task<int> Handle(CreateBankListCommand request, CancellationToken cancellationToken)
        {
            if (_unitOfWork == null)
            {
                throw new ArgumentNullException(nameof(_unitOfWork), "_unitOfWork is not initialized.");
            }

            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "request is null.");
            }

            if (request.BankLists == null)
            {
                throw new ArgumentNullException(nameof(request.BankLists), "BankLists property of request is null.");
            }

            var data = await _unitOfWork.BankLists.Add(request.BankLists);
            return data;
        }

    }
}
