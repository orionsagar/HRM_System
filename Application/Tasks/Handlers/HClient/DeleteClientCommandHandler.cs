using Application.Tasks.Commands.CClient;
using Application.Tasks.Commands.CCompany;
using MediatR;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Handlers.HClient
{
    public class DeleteClientCommandHandler : IRequestHandler<DeleteClientCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        public DeleteClientCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(DeleteClientCommand request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.ClientRepo.Delete(request.Id);
            return result;
        }
    }
}
