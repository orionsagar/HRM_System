using MediatR;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Commands.CPayScale
{
    public class DeletePayScaleCommand : IRequest<int>
    {
        public int PayScaleId { get; set; }
    }
    public class DeletePayScaleCommandHandler : IRequestHandler<DeletePayScaleCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeletePayScaleCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(DeletePayScaleCommand request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.PayScales.Delete(request.PayScaleId);
            return result;
        }
    }
}
