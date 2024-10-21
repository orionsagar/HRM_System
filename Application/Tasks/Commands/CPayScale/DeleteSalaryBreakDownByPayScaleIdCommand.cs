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
    public class DeleteSalaryBreakDownByPayScaleIdCommand : IRequest<int>
    {
        public int PayScaleId { get; set; }
    }
    public class DeleteSalaryBreakDownByPayScaleIdCommandHandler : IRequestHandler<DeleteSalaryBreakDownByPayScaleIdCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteSalaryBreakDownByPayScaleIdCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(DeleteSalaryBreakDownByPayScaleIdCommand request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.PayScales.DeleteSalaryBreakDownByPayScaleId(request.PayScaleId);
            return result;
        }
    }
}
