using MediatR;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Commands.CMenu
{
    public class DeleteMenuCommand : IRequest<int>
    {
        public int Id { get; set; }
    }
    public class DeleteMenuCommandHandler : IRequestHandler<DeleteMenuCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteMenuCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(DeleteMenuCommand request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.ModuleMenu.Delete(request.Id);
            return result;
        }
    }
}
