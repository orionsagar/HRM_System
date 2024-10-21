using Application.Tasks.Queries.QMainModule;
using AutoMapper;
using Domains.Models;
using MediatR;
using Persistence.DAL;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Handlers.HMainModule
{
    public class GetMainModuleByIdQueryHandler : IRequestHandler<GetMainModuleByIdQuery, MainModule>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetMainModuleByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<MainModule> Handle(GetMainModuleByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.MainModules.GetById(request.ModuleID);
            return _mapper.Map<MainModule>(result);
        }
    }
}
