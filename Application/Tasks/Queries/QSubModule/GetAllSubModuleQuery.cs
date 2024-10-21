using AutoMapper;
using Domains.ViewModels;
using MediatR;
using Persistence.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Queries.QSubModule
{
    public class GetAllSubModuleQuery : IRequest<List<SubModuleVM>>
    {
    }
    public class GetAllSubModuleQueryHandler : IRequestHandler<GetAllSubModuleQuery, List<SubModuleVM>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllSubModuleQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<SubModuleVM>> Handle(GetAllSubModuleQuery request, CancellationToken cancellationToken)
        {
            var data = await _unitOfWork.SubModule.GetAll();
            var result = _mapper.Map<List<SubModuleVM>>(data);
            return result;
        }
    }
}
