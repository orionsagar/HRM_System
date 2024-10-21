using Application.Tasks.Commands.CMainModule;
using AutoMapper;
using Domains.Models;
using MediatR;
using Persistence.DAL;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Handlers.HMainModule
{
    public class CreateMainModuleCommandHandler : IRequestHandler<CreateMainModuleCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateMainModuleCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateMainModuleCommand request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.MainModules.Add(request.MainModule);
            return result;
        }
    }
}
