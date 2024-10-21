using AutoMapper;
using Domains.Models;
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
    public class GetSubModuleQuery : IRequest<List<SubModule>>
    {
        public int RoleId { get; set; }
    }
    public class GetSubModuleQueryHandler : IRequestHandler<GetSubModuleQuery, List<SubModule>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetSubModuleQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<SubModule>> Handle(GetSubModuleQuery request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.UserAccess.GetSubModule(request.RoleId);
            return _mapper.Map<List<SubModule>>(result);
        }
    }
}
