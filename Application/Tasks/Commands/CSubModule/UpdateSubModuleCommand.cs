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
    public class UpdateSubModuleCommand : IRequest<int>
    {
        public SubModule SubModule { get; set; }
    }
    public class UpdateSubModuleCommandHandler : IRequestHandler<UpdateSubModuleCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public UpdateSubModuleCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<int> Handle(UpdateSubModuleCommand request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.SubModule.Update(request.SubModule);
            return result;
        }
    }
}
