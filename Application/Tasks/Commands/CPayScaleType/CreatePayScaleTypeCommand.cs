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
    public class CreatePayScaleTypeCommand : IRequest<int>
    {
        public PayScaleType PayScaleType { get; set; }
    }
    public class CreatePayScaleTypeCommandHandler : IRequestHandler<CreatePayScaleTypeCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreatePayScaleTypeCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(CreatePayScaleTypeCommand request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.PayScales.AddPayScaleType(request.PayScaleType);
            return result;
        }
    }
}
