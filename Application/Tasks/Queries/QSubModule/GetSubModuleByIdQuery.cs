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

namespace Application.Tasks.Queries.QSubModule
{
    public class GetSubModuleByIdQuery : IRequest<SubModule>
    {
        public int SubModuleId { get; set; }
    }
    public class GetSubModuleByIdQueryHandler : IRequestHandler<GetSubModuleByIdQuery, SubModule>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetSubModuleByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<SubModule> Handle(GetSubModuleByIdQuery request, CancellationToken cancellationToken)
        {
            var data = await _unitOfWork.SubModule.GetById(request.SubModuleId);
            //var result = _mapper.Map<SubModule>(data);
            return data;
        }
    }
}
