using AutoMapper;
using Domains.Models;
using Domains.ViewModels;
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
    public class CreatePayScaleCommand : IRequest<int>
    {
        public PayScaleVM payScaleVM { get; set; }
    }
    public class CreatePayScaleCommandHandler : IRequestHandler<CreatePayScaleCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreatePayScaleCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreatePayScaleCommand request, CancellationToken cancellationToken)
        {            
            var result = _mapper.Map<PayScale>(request.payScaleVM);
            var data = await _unitOfWork.PayScales.Add(result);
            return data;
        }
    }
}
