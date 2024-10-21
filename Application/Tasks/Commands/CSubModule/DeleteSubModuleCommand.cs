using AutoMapper;
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
    public class DeleteSubModuleCommand : IRequest<int>
    {
        public int SubModuleId { get; set; }
    }

    public class DeleteSubModuleCommandHandler : IRequestHandler<DeleteSubModuleCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DeleteSubModuleCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<int> Handle(DeleteSubModuleCommand request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.SubModule.Delete(request.SubModuleId);
            return result;
        }
    }
}
