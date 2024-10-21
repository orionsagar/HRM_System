using Application.Tasks.Queries.QMainModule;
using AutoMapper;
using Domains.Models;
using MediatR;
using Persistence.DAL;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Handlers.HMainModule
{
    public class GetAllMainModuleQueryHandler : IRequestHandler<GetAllMainModuleQuery, List<MainModule>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllMainModuleQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<MainModule>> Handle(GetAllMainModuleQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.MainModules.GetAll();
            return _mapper.Map<List<MainModule>>(result.ToList());
        }
    }
}
