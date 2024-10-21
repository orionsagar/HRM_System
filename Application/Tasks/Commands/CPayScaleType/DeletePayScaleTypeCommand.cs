using MediatR;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Commands.CPayScaleType
{
    public class DeletePayScaleTypeCommand : IRequest<int>
    {
        public int PayScaleTypeId { get; set; }
    }
    public class DeletePayScaleTypeCommandHandler : IRequestHandler<DeletePayScaleTypeCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeletePayScaleTypeCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(DeletePayScaleTypeCommand request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.PayScales.DeletePayScaleType(request.PayScaleTypeId);
            return result;
        }
    }
}
