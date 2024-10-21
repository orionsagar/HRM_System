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
    public class EditPayScaleCommand : IRequest<int>
    {
        public PayScaleVM payScaleVM { get; set; }
    }
    public class EditPayScaleCommandHandler : IRequestHandler<EditPayScaleCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EditPayScaleCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<int> Handle(EditPayScaleCommand request, CancellationToken cancellationToken)
        {
            var date = _mapper.Map<PayScale>(request.payScaleVM);
            var result = await _unitOfWork.PayScales.Update(date);
            return result;
        }
    }
}
