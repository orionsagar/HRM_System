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

namespace Application.Tasks.Commands.CSubModule
{
    public class CreateSubModuleCommand : IRequest<int>
    {
        public SubModule SubModule { get; set; }
    }
    public class CreateSubModuleCommandHandler : IRequestHandler<CreateSubModuleCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateSubModuleCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateSubModuleCommand request, CancellationToken cancellationToken)
        {
            var data = _mapper.Map<SubModule>(request.SubModule);
            var result = await _unitOfWork.SubModule.Add(data);
            return result;
        }
    }
}
