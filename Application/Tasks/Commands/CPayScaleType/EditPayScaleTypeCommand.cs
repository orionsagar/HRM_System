using Domains.Models;
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
    public class EditPayScaleTypeCommand : IRequest<int>
    {
        public PayScaleType PayScaleType { get; set; }
    }
    public class EditPayScaleTypeCommandHandler : IRequestHandler<EditPayScaleTypeCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public EditPayScaleTypeCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(EditPayScaleTypeCommand request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.PayScales.UpdatePayScaleType(request.PayScaleType);
            return result;
        }
    }
}
